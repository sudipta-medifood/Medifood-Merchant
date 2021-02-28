using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Merchants;
using Models.Responses;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Services;
using Services.Interface;
using Dtos.MerchantDtos;

namespace Repositories.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IMailService _mailService;
        private readonly AppSettings _appSettings;
        private readonly TokenSettings _tokenSettings;

        public AuthRepository(DataContext context, IOptions<TokenSettings> tokenSettings, IOptions<AppSettings> appSettings, IMailService mailService)
        {
            this._context = context;
            this._mailService = mailService;
            this._appSettings = appSettings.Value;
            this._tokenSettings = tokenSettings.Value;
        }

        // register merchant for pharmacy
        public async Task<ServiceResponse<int>> RegisterPharmacyMerchant(MerchantPharmacy merchantPharmacy, string password)
        {
            ServiceResponse<int> registerResponse = new ServiceResponse<int>();
            string merchantUserFlag = "pharmacy merchant";
            if (await EmailExists(merchantPharmacy.Email, merchantUserFlag))
            {
                registerResponse.Success = false;
                registerResponse.Message = "Email is already exist";
                return registerResponse;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            merchantPharmacy.PasswordHash = passwordHash;
            merchantPharmacy.PasswordSalt = passwordSalt;
            merchantPharmacy.Email = merchantPharmacy.Email.ToLower();
            merchantPharmacy.AccountStatus = 0;
            merchantPharmacy.LoginStatus = 0;

            await _context.MerchantPharmacys.AddAsync(merchantPharmacy);
            await _context.SaveChangesAsync();
            registerResponse.Data = merchantPharmacy.Id;
            registerResponse.Message = "Successfully Registered. You will be confirmed via email!";
            return registerResponse;
        }

        // register merchant for restaurant
        public async Task<ServiceResponse<int>> RegisterRestaurantMerchant(MerchantRestaurant merchantRestaurant, string password)
        {
            ServiceResponse<int> registerResponse = new ServiceResponse<int>();
            string merchantUserFlag = "restaurant merchant";
            if (await EmailExists(merchantRestaurant.Email, merchantUserFlag))
            {
                registerResponse.Success = false;
                registerResponse.Message = "Email is already exist.";
                return registerResponse;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            merchantRestaurant.PasswordHash = passwordHash;
            merchantRestaurant.PasswordSalt = passwordSalt;
            merchantRestaurant.Email = merchantRestaurant.Email.ToLower();
            merchantRestaurant.AccountStatus = 0;
            merchantRestaurant.LoginStatus = 0;

            await _context.MerchantRestaurants.AddAsync(merchantRestaurant);
            await _context.SaveChangesAsync();
            registerResponse.Data = merchantRestaurant.Id;
            registerResponse.Message = "Successfully Registered. You will be confirmed via email";
            return registerResponse;
        }

        // login merchant account
        public async Task<ServiceResponse<LoginResponse>> Login(string email, string password)
        {
            ServiceResponse<LoginResponse> loginResponse = new ServiceResponse<LoginResponse> { Data = new LoginResponse()};
            if (await EmailExists(email, "pharmacy merchant"))
            {
                loginResponse = await CreateLoginPharmacyMerchant(email, password);
            }
            else if (await EmailExists(email, "restaurant merchant"))
            {
                loginResponse = await CreateLoginRestaurantMerchant(email, password);
            }
            else
            {
                loginResponse.Success = false;
                loginResponse.Message = "Invalid Email or Password!";
            }
            return loginResponse;
        }

        // login pharmacy merchant account
        public async Task<ServiceResponse<LoginResponse>> CreateLoginPharmacyMerchant(string email, string password)
        {
            MerchantPharmacy merchantPharmacy = await _context.MerchantPharmacys.
                    Include(merchantpharmacytoken => merchantpharmacytoken.RefTokenPharmacyMerchants).
                    FirstOrDefaultAsync(merchantpharmacy => merchantpharmacy.Email.Equals(email.ToLower()));
            ServiceResponse<LoginResponse> response = new ServiceResponse<LoginResponse> { Data = new LoginResponse() };
            if (merchantPharmacy.AccountStatus == 0 && merchantPharmacy.LoginStatus == 0)
            {
                response.Success = false;
                response.Message = "Your account is under review. Wait for confirmation email !";
            }
            else if (merchantPharmacy.AccountStatus == 1 && merchantPharmacy.LoginStatus == 1)
            {
                if (!VerifyPasswordHash(password, merchantPharmacy.PasswordHash, merchantPharmacy.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Invalid Email or Password";
                }
                else
                {
                    var jwtToken = CreateTokenPharmacyMerchant(merchantPharmacy);
                    var refreshToken = GenerateRefreshTokenPharmacyMerchant();
                    merchantPharmacy.RefTokenPharmacyMerchants.Add(refreshToken);

                    RemoveOldRefreshTokensPharmacyMerchant(merchantPharmacy);
                    _context.MerchantPharmacys.Update(merchantPharmacy);
                    await _context.SaveChangesAsync();

                    response.Data.Id = merchantPharmacy.Id;
                    response.Data.JwtToken = jwtToken;
                    response.Data.RefreshToken = refreshToken.Token;
                    response.Data.Role = merchantPharmacy.Role;
                    response.Data.UserType = "Pharmacy Merchant";
                    response.Message = "Login Successful";
                }
            }
            else if(merchantPharmacy.AccountStatus == 2 && merchantPharmacy.LoginStatus == 0)
            {
                response.Success = false;
                response.Message = "Invalid Email or Password";
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid Email or Password";
            }
            return response;
        }

        // login restaurant merchant account
        public async Task<ServiceResponse<LoginResponse>> CreateLoginRestaurantMerchant(string email, string password)
        {
            MerchantRestaurant merchantRestaurant = await _context.MerchantRestaurants.
                    Include(merchantrestauranttoken => merchantrestauranttoken.RefTokenRestaurantMerchants).
                    FirstOrDefaultAsync(merchantrestaurant => merchantrestaurant.Email.Equals(email.ToLower()));
            ServiceResponse<LoginResponse> response = new ServiceResponse<LoginResponse> { Data = new LoginResponse() };
            if (merchantRestaurant.AccountStatus == 0 && merchantRestaurant.LoginStatus == 0)
            {
                response.Success = false;
                response.Message = "Your account is under review. Wait for confirmation email !";
            }
            else if (merchantRestaurant.AccountStatus == 1 && merchantRestaurant.LoginStatus == 1)
            {
                if (!VerifyPasswordHash(password, merchantRestaurant.PasswordHash, merchantRestaurant.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Invalid Email or Password";
                }
                else
                {
                    var jwtToken = CreateTokenRestaurantMerchant(merchantRestaurant);
                    var refreshToken = GenerateRefreshTokenRestaurantMerchant();
                    merchantRestaurant.RefTokenRestaurantMerchants.Add(refreshToken);

                    RemoveOldRefreshTokensRestaurantMerchant(merchantRestaurant);
                    _context.MerchantRestaurants.Update(merchantRestaurant);
                    await _context.SaveChangesAsync();

                    response.Data.Id = merchantRestaurant.Id;
                    response.Data.JwtToken = jwtToken;
                    response.Data.RefreshToken = refreshToken.Token;
                    response.Data.Role = merchantRestaurant.Role;
                    response.Data.UserType = "Restaurant Merchant";
                    response.Message = "Login Successful";
                }
            }
            else if (merchantRestaurant.AccountStatus == 2 && merchantRestaurant.LoginStatus == 0)
            {
                response.Success = false;
                response.Message = "Invalid Email or Password";
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid Email or Password";
            }
            return response;
        }

        // forgot password merchant account
        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            ServiceResponse<string> forgotPasswordResponse = new ServiceResponse<string>();
            if (await EmailExists(email, "pharmacy merchant"))
            {
                forgotPasswordResponse = await ForgotPasswordPharmacyMerchant(email);
            }
            else if (await EmailExists(email, "restaurant merchant"))
            {
                forgotPasswordResponse = await ForgotPasswordRestaurantMerchant(email);
            }
            else
            {
                forgotPasswordResponse.Success = false;
                forgotPasswordResponse.Message = "No user found on that email!";
            }
            return forgotPasswordResponse;
        }

        // forgot password pharmacy merchant account
        public async Task<ServiceResponse<string>> ForgotPasswordPharmacyMerchant(string email)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            MerchantPharmacy merchantPharmacy = await _context.MerchantPharmacys.FirstOrDefaultAsync(x => x.Email == email.ToLower());

            merchantPharmacy.ResetToken = RandomTokenString();
            merchantPharmacy.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15);

            _context.MerchantPharmacys.Update(merchantPharmacy);
            await _context.SaveChangesAsync();
            SendPasswordResetEmailPharmacyMerchant(merchantPharmacy);

            response.Message = "Email sent successfully. Please check your email!";
            return response;
        }

        // forgot password restaurant merchant account
        public async Task<ServiceResponse<string>> ForgotPasswordRestaurantMerchant(string email)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            MerchantRestaurant merchantRestaurant = await _context.MerchantRestaurants.FirstOrDefaultAsync(x => x.Email == email.ToLower());

            merchantRestaurant.ResetToken = RandomTokenString();
            merchantRestaurant.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15);

            _context.MerchantRestaurants.Update(merchantRestaurant);
            await _context.SaveChangesAsync();
            SendPasswordResetEmailRestaurantMerchant(merchantRestaurant);

            response.Message = "Email sent successfully. Please check your email.";
            return response;
        }

        //reset password merchant account
        public async Task<ServiceResponse<string>> ResetPassword(string token, string password)
        {
            ServiceResponse<string> resetPasswordResponse = new ServiceResponse<string>();
            var pharmacyMerchant = await _context.MerchantPharmacys.SingleOrDefaultAsync(pharmacymerchant =>
            pharmacymerchant.ResetToken == token &&
            pharmacymerchant.ResetTokenExpires > DateTime.UtcNow);

            var restaurantMerchant = await _context.MerchantRestaurants.SingleOrDefaultAsync(restaurantmerchant =>
            restaurantmerchant.ResetToken == token &&
            restaurantmerchant.ResetTokenExpires > DateTime.UtcNow);

            if (pharmacyMerchant != null)
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                pharmacyMerchant.PasswordHash = passwordHash;
                pharmacyMerchant.PasswordSalt = passwordSalt;
                pharmacyMerchant.PasswordReset = DateTime.UtcNow;
                pharmacyMerchant.ResetToken = null;
                pharmacyMerchant.ResetTokenExpires = null;

                _context.MerchantPharmacys.Update(pharmacyMerchant);
                await _context.SaveChangesAsync();
                resetPasswordResponse.Message = "Password reset successfully!";
            }
            else if (restaurantMerchant != null)
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                restaurantMerchant.PasswordHash = passwordHash;
                restaurantMerchant.PasswordSalt = passwordSalt;
                restaurantMerchant.PasswordReset = DateTime.UtcNow;
                restaurantMerchant.ResetToken = null;
                restaurantMerchant.ResetTokenExpires = null;

                _context.MerchantRestaurants.Update(restaurantMerchant);
                await _context.SaveChangesAsync();
                resetPasswordResponse.Message = "Password reset successfully";
            }
            else
            {
                resetPasswordResponse.Success = false;
                resetPasswordResponse.Message = "Token have been expired.";
            }
            return resetPasswordResponse;
        }

        // reset password pharmacy merchant account
        public string CreateTokenPharmacyMerchant(MerchantPharmacy merchantPharmacy)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, merchantPharmacy.Id.ToString()),
                new Claim(ClaimTypes.Name, merchantPharmacy.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_tokenSettings.TokenSecretKey)
                );

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_tokenSettings.JwtTokenExpires),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // reset password restaurant merchant account
        public string CreateTokenRestaurantMerchant(MerchantRestaurant merchantRestaurant)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, merchantRestaurant.Id.ToString()),
                new Claim(ClaimTypes.Name, merchantRestaurant.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_tokenSettings.TokenSecretKey)
                );

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_tokenSettings.JwtTokenExpires),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // generate refresh token pharmacy merchant account
        private RefTokenPharmacyMerchant GenerateRefreshTokenPharmacyMerchant()
        {
            return new RefTokenPharmacyMerchant
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpires),
                Created = DateTime.UtcNow
            };
        }

        // generate refresh token restaurant merchant account
        private RefTokenRestaurantMerchant GenerateRefreshTokenRestaurantMerchant()
        {
            return new RefTokenRestaurantMerchant
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpires),
                Created = DateTime.UtcNow
            };
        }

        // generate random token
        private string RandomTokenString()
        {
            using var rngToken = new RNGCryptoServiceProvider();
            var randomBytes = new Byte[40];
            rngToken.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        // remove old refresh token pharmacy merchant
        private void RemoveOldRefreshTokensPharmacyMerchant(MerchantPharmacy merchantPharmacy)
        {
            merchantPharmacy.RefTokenPharmacyMerchants.RemoveAll(x =>
                !x.IsActive && 
                x.Created.AddDays(_tokenSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        // remove old refresh token restaurant merchant
        private void RemoveOldRefreshTokensRestaurantMerchant(MerchantRestaurant merchantRestaurant)
        {
            merchantRestaurant.RefTokenRestaurantMerchants.RemoveAll(x => 
            !x.IsActive &&
            x.Created.AddDays(_tokenSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        // send reset password email pharmacy merchant
        private async void SendPasswordResetEmailPharmacyMerchant(MerchantPharmacy merchantPharmacy)
        {
            string message;
            var resetUrl = $"{_appSettings.AppIISExpressUrl}/api/auth/reset-password?token={merchantPharmacy.ResetToken}";
            message = $@"<p>Please click the below link to reset your password. This link will be invalid after 15 minutes:
                        <p><a href=""{resetUrl}"">{resetUrl}</a></p>";

            await _mailService.SendEmailAsync(
                toEmail: merchantPharmacy.Email,
                subject: "Reset Password",
                body: $@"<h4>Reset your password</h4>{message}");
        }
        
        // send reset password email restaurant merchant
        private async void SendPasswordResetEmailRestaurantMerchant(MerchantRestaurant merchantRestaurant)
        {
            string message;
            var resetUrl = $"{_appSettings.AppIISExpressUrl}/api/auth/reset-password?token={merchantRestaurant.ResetToken}";
            message = $@"<p>Please click the below link to reset your password. This link will be invalid after 15 minutes:
                        <p><a href=""{resetUrl}"">{resetUrl}</a></p>";

            await _mailService.SendEmailAsync(
                toEmail: merchantRestaurant.Email,
                subject: "Reset Password",
                body: $@"<h4>Reset your password</h4>{message}");
        }

        // use password to create password hash
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            };
        }

        // verify hashed password with user password
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        // check whether an email exist or not
        public async Task<bool> EmailExists(string email, string merchantUserFlag)
        {
            if (merchantUserFlag.Equals("pharmacy merchant"))
            {
                if (await _context.MerchantPharmacys.AnyAsync(merchant => merchant.Email == email.ToLower())) return true;
            }
            else if (merchantUserFlag.Equals("restaurant merchant"))
            {
                if (await _context.MerchantRestaurants.AnyAsync(merchant => merchant.Email == email.ToLower())) return true;
            }
            return false;
        }
    }
}
