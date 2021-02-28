using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Responses;
using Models.Riders;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utilities;

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

        public async Task<ServiceResponse<int>> RegisterRider(Rider rider, string password)
        {
            ServiceResponse<int> registerResponse = new ServiceResponse<int>();
            if (await EmailExists(rider.Email))
            {
                registerResponse.Success = false;
                registerResponse.Message = "Email is already exists!";
                return registerResponse;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            rider.PasswordHash = passwordHash;
            rider.PasswordSalt = passwordSalt;
            rider.Email = rider.Email.ToLower();
            rider.AccountStatus = 0;
            rider.LoginStatus = 0;

            await _context.Riders.AddAsync(rider);
            await _context.SaveChangesAsync();
            registerResponse.Data = rider.Id;
            registerResponse.Message = "Successfully Registered. You will be confirmed via email!";
            return registerResponse;
        }

        public async Task<ServiceResponse<LoginResponse>> Login(string email, string password)
        {
            Rider rider = await _context.Riders.
                Include(r => r.RefreshTokens)
                .FirstOrDefaultAsync(r => r.Email.Equals(email.ToLower()));
            ServiceResponse<LoginResponse> response = new ServiceResponse<LoginResponse>() { Data = new LoginResponse() };
            if (rider.AccountStatus == 0 && rider.LoginStatus == 0)
            {
                response.Success = false;
                response.Message = "Your account is under review. Wait for confirmation email!";
            }
            else if(rider.AccountStatus == 1 && rider.LoginStatus == 1)
            {
                if (!VerifyPasswordHash(password, rider.PasswordHash, rider.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Invalid Email or Password!";
                }
                else
                {
                    var jwtToken = CreateTokenRider(rider);
                    var refreshToken = GenerateRefreshTokenRider();
                    rider.RefreshTokens.Add(refreshToken);

                    RemoveOldRefreshToken(rider);
                    _context.Riders.Update(rider);
                    await _context.SaveChangesAsync();

                    response.Data.Id = rider.Id;
                    response.Data.JwtToken = jwtToken;
                    response.Data.RefreshToken = refreshToken.Token;
                    response.Data.Role = rider.Role;
                    response.Data.UserType = "Rider";
                    response.Message = "Login Successful!";
                }
            }
            else if(rider.AccountStatus == 2 && rider.LoginStatus == 0)
            {
                response.Success = false;
                response.Message = "Invalid email or password!";
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid email or password!";
            }
            return response;
        }

        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            Rider rider = await _context.Riders.FirstOrDefaultAsync(x => x.Email == email.ToLower());

            if (rider == null)
            {
                response.Success = false;
                response.Message = "No user found on that email!";
                return response;
            }

            rider.ResetToken = RandomTokenString();
            rider.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15);

            _context.Riders.Update(rider);
            await _context.SaveChangesAsync();
            SendPasswordResetEmail(rider);

            response.Message = "Email sent successfully. Please check your email!";
            return response;
        }

        public async Task<ServiceResponse<string>> ResetPassword(string token, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var rider = await _context.Riders.SingleOrDefaultAsync(r => r.ResetToken == token &&
            r.ResetTokenExpires > DateTime.UtcNow);

            if (rider == null)
            {
                response.Success = false;
                response.Message = "Token have been expired!";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            rider.PasswordHash = passwordHash;
            rider.PasswordSalt = passwordSalt;
            rider.PasswordReset = DateTime.UtcNow;
            rider.ResetToken = null;
            rider.ResetTokenExpires = null;

            _context.Riders.Update(rider);
            await _context.SaveChangesAsync();
            response.Message = "Password reset successfully!";
            return response;
        }

        public string CreateTokenRider(Rider rider)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, rider.Id.ToString()),
                new Claim(ClaimTypes.Name, rider.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_tokenSettings.TokenSecretKey));

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

        private RefreshToken GenerateRefreshTokenRider()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpires),
                Created = DateTime.UtcNow
            };
        }

        private string RandomTokenString()
        {
            using var rngToken = new RNGCryptoServiceProvider();
            var randomBytes = new Byte[40];
            rngToken.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private void RemoveOldRefreshToken(Rider rider)
        {
            rider.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(_tokenSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private async void SendPasswordResetEmail(Rider rider)
        {
            string message;
            var resetUrl = $"{_appSettings.AppIISExpressUrl}/api/auth/reset-password?token={rider.ResetToken}";
            message = $@"<p>Please click the below link to reset your password. This link will be invalid after 15 minutes:
                        <p><a href=""{resetUrl}"">{resetUrl}</a></p>";

            await _mailService.SendEmailAsync(
                toEmail: rider.Email,
                subject: "Reset Password",
                body: $@"<h4>Reset your password</h4>{message}");
        }

        public async Task<bool> EmailExists(string email)
        {
            if (await _context.Riders.AnyAsync(rider => rider.Email == email.ToLower())) return true;
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            };
        }

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
    }
}
