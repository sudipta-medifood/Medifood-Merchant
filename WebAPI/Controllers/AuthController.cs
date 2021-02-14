using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos.MerchantDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Models.Merchants;
using Models.Responses;
using Repositories.Interface;
using Utilities;

namespace WebAPI.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthRepository authoRepo;
        private readonly TokenSettings _tokenSettings;

        public AuthController(IAuthRepository authoRepo, IOptions<TokenSettings> tokenSettings)
        {
            this.authoRepo = authoRepo;
            this._tokenSettings = tokenSettings.Value;
        }

        //register pharmacy merchant
        [HttpPost("registerpharmacymerchant")]
        public async Task<IActionResult> RegisterPharmacyMerchant(PharmacyMerchantDto registerRequest)
        {
            ServiceResponse<int> registerResponse = await authoRepo.RegisterPharmacyMerchant(
                new MerchantPharmacy
                {
                    PharmacyName = registerRequest.PharmacyName,
                    DrugLicenceNumber = registerRequest.DrugLicenceNumber,
                    NidNumber = registerRequest.NidNumber,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Phone = registerRequest.Phone,
                    Email = registerRequest.Email,
                    Address = registerRequest.Address,
                    Role = registerRequest.Role
                }, registerRequest.Password);

            if (!registerResponse.Success)
            {
                return BadRequest(registerResponse);
            }
            return Ok(registerResponse);
        }

        //register restaurant merchant
        [HttpPost("registerrestaurantmerchant")]
        public async Task<IActionResult> RegisterRestaurantMerchant(RestaurantMerchantDto registerRequest)
        {
            ServiceResponse<int> registerResponse = await authoRepo.RegisterRestaurantMerchant(
                new MerchantRestaurant
                {
                    RestaurantName = registerRequest.RestaurantName,
                    NidNumber = registerRequest.NidNumber,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Phone = registerRequest.Phone,
                    Email = registerRequest.Email,
                    Address = registerRequest.Address,
                    Role = registerRequest.Role
                }, registerRequest.Password);

            if (!registerResponse.Success)
            {
                return BadRequest(registerResponse);
            }
            return Ok(registerResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            ServiceResponse<Tokens> response = await authoRepo.Login(user.email, user.password);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            SetTokenToCookie(response.Data.RefreshToken);
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            ServiceResponse<string> forgotPasswordResponse = await authoRepo.ForgotPassword(forgotPassword.Email);
            if (!forgotPasswordResponse.Success)
            {
                return BadRequest(forgotPasswordResponse);
            }
            return Ok(forgotPasswordResponse);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            ServiceResponse<string> resetPasswordResponse = await authoRepo.ResetPassword(resetPassword.Token, resetPassword.Password);
            if (!resetPasswordResponse.Success)
            {
                return BadRequest(resetPasswordResponse);
            }
            return Ok(resetPasswordResponse);
        }

        public void SetTokenToCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenTTL)
            };
            Response.Cookies.Append("refreshtoken", token, cookieOptions);
        }
    }
}
