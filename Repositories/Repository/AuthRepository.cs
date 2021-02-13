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

namespace Repositories.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly TokenSettings _tokenSettings;

        public AuthRepository(DataContext context, IOptions<TokenSettings> tokenSettings)
        {
            this._context = context;
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

            await _context.MerchantPharmacys.AddAsync(merchantPharmacy);
            await _context.SaveChangesAsync();
            registerResponse.Data = merchantPharmacy.Id;
            registerResponse.Message = "Successfully Registered. You will be confirmed via email";
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

            await _context.MerchantRestaurants.AddAsync(merchantRestaurant);
            await _context.SaveChangesAsync();
            registerResponse.Data = merchantRestaurant.Id;
            registerResponse.Message = "Successfully Registered. You will be confirmed via email";
            return registerResponse;
        }

        public async Task<ServiceResponse<Tokens>> Login(string email, string password)
        {
            MerchantPharmacy merchantPharmacy = new MerchantPharmacy();
            MerchantRestaurant merchantRestaurant = new MerchantRestaurant();
            ServiceResponse<Tokens> loginResponse = new ServiceResponse<Tokens> { Data = new Tokens()};
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

        public async Task<ServiceResponse<Tokens>> CreateLoginPharmacyMerchant(string email, string password)
        {
            MerchantPharmacy merchantPharmacy = await _context.MerchantPharmacys.
                    Include(merchantpharmacytoken => merchantpharmacytoken.RefTokenPharmacyMerchants).
                    FirstOrDefaultAsync(merchantpharmacy => merchantpharmacy.Email.Equals(email.ToLower()));
            ServiceResponse<Tokens> response = new ServiceResponse<Tokens> { Data = new Tokens() };
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

                response.Data.JwtToken = jwtToken;
                response.Data.RefreshToken = refreshToken.Token;
                response.Message = "Login Successful";
            }

            return response;
        }

        public async Task<ServiceResponse<Tokens>> CreateLoginRestaurantMerchant(string email, string password)
        {
            MerchantRestaurant merchantRestaurant = await _context.MerchantRestaurants.
                    Include(merchantrestauranttoken => merchantrestauranttoken.RefTokenRestaurantMerchants).
                    FirstOrDefaultAsync(merchantrestaurant => merchantrestaurant.Email.Equals(email.ToLower()));
            ServiceResponse<Tokens> response = new ServiceResponse<Tokens> { Data = new Tokens() };
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

                response.Data.JwtToken = jwtToken;
                response.Data.RefreshToken = refreshToken.Token;
                response.Message = "Login Successful";
            }
            return response;
        }

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

        private RefTokenPharmacyMerchant GenerateRefreshTokenPharmacyMerchant()
        {
            return new RefTokenPharmacyMerchant
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpires),
                Created = DateTime.UtcNow
            };
        }

        private RefTokenRestaurantMerchant GenerateRefreshTokenRestaurantMerchant()
        {
            return new RefTokenRestaurantMerchant
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

        private void RemoveOldRefreshTokensPharmacyMerchant(MerchantPharmacy merchantPharmacy)
        {
            merchantPharmacy.RefTokenPharmacyMerchants.RemoveAll(x =>
                !x.IsActive && 
                x.Created.AddDays(_tokenSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private void RemoveOldRefreshTokensRestaurantMerchant(MerchantRestaurant merchantRestaurant)
        {
            merchantRestaurant.RefTokenRestaurantMerchants.RemoveAll(x => 
            !x.IsActive &&
            x.Created.AddDays(_tokenSettings.RefreshTokenTTL) <= DateTime.UtcNow);
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
