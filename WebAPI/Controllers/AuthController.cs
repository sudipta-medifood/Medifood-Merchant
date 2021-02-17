using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos.MerchantDtos;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAuthRepository _authRepo;
        private readonly TokenSettings _tokenSettings;

        public AuthController(IAuthRepository authoRepo, IOptions<TokenSettings> tokenSettings)
        {
            this._authRepo = authoRepo;
            this._tokenSettings = tokenSettings.Value;
        }

        //register pharmacy merchant
        [HttpPost("registerpharmacymerchant")]
        public async Task<IActionResult> RegisterPharmacyMerchant(PharmacyMerchantDto registerRequest)
        {
            ServiceResponse<int> registerResponse = await _authRepo.RegisterPharmacyMerchant(
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

        // register restaurant merchant
        [HttpPost("registerrestaurantmerchant")]
        public async Task<IActionResult> RegisterRestaurantMerchant(RestaurantMerchantDto registerRequest)
        {
            ServiceResponse<int> registerResponse = await _authRepo.RegisterRestaurantMerchant(
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

        // login merchant
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            ServiceResponse<LoginResponse> response = await _authRepo.Login(user.email, user.password);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            SetTokenToCookie(response.Data.RefreshToken);
            return Ok(response);
        }

        // forgot password merchant
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            ServiceResponse<string> forgotPasswordResponse = await _authRepo.ForgotPassword(forgotPassword.Email);
            if (!forgotPasswordResponse.Success)
            {
                return BadRequest(forgotPasswordResponse);
            }
            return Ok(forgotPasswordResponse);
        }

        // reset password merchant
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            ServiceResponse<string> resetPasswordResponse = await _authRepo.ResetPassword(resetPassword.Token, resetPassword.Password);
            if (!resetPasswordResponse.Success)
            {
                return BadRequest(resetPasswordResponse);
            }
            return Ok(resetPasswordResponse);
        }

        // create cookie to store token on merchant login
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
