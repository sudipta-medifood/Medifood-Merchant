using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos.RiderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.Responses;
using Models.Riders;
using Repositories.Interfaces;
using Utilities;

namespace WebAPI.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthRepository _authRepository;
        private readonly TokenSettings _tokenSettings;

        public AuthController(IAuthRepository authRepository, IOptions<TokenSettings> tokenSettings)
        {
            this._authRepository = authRepository;
            this._tokenSettings = tokenSettings.Value;
        }

        /// <summary>
        /// Create rider account
        /// </summary>
        /// <returns>Returns the rider account data</returns>
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterRider(RiderDto riderRegisterRequest)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (riderRegisterRequest.VehicleType.ToLower() == "bike")
            {
                response = await _authRepository.RegisterRider(
                    new Rider
                    {
                        Zone = riderRegisterRequest.Zone,
                        VehicleType = riderRegisterRequest.VehicleType,
                        BikeRegistrationNumber = riderRegisterRequest.BikeRegistrationNumber,
                        DrivingLicenceNumber = riderRegisterRequest.DrivingLicenceNumber,
                        NidNumber = riderRegisterRequest.NidNumber,
                        FirstName = riderRegisterRequest.FirstName,
                        LastName = riderRegisterRequest.LastName,
                        Address = riderRegisterRequest.Address,
                        Phone = riderRegisterRequest.Phone,
                        Email = riderRegisterRequest.Email,
                        Role = riderRegisterRequest.Role
                    }, riderRegisterRequest.Password);
            }
            else if (riderRegisterRequest.VehicleType.ToLower() == "bicycle")
            {
                response = await _authRepository.RegisterRider(
                    new Rider
                    {
                        Zone = riderRegisterRequest.Zone,
                        VehicleType = riderRegisterRequest.VehicleType,
                        BicycleModelNumber = riderRegisterRequest.BicycleModelNumber,
                        NidNumber = riderRegisterRequest.NidNumber,
                        FirstName = riderRegisterRequest.FirstName,
                        LastName = riderRegisterRequest.LastName,
                        Address = riderRegisterRequest.Address,
                        Phone = riderRegisterRequest.Phone,
                        Email = riderRegisterRequest.Email,
                        Role = riderRegisterRequest.Role
                    }, riderRegisterRequest.Password);
            }
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Login rider account
        /// </summary>
        /// <returns>Returns the jwt token and refresh token</returns>
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(RiderLoginDto riderLoginDto)
        {
            ServiceResponse<LoginResponse> loginResponse = await _authRepository.Login(riderLoginDto.email, riderLoginDto.password);
            if (!loginResponse.Success)
            {
                return BadRequest(loginResponse);
            }

            SetTokenToCookie(loginResponse.Data.RefreshToken);
            return Ok(loginResponse);
        }

        /// <summary>
        /// Forgot password rider
        /// </summary>
        /// <returns>Send an email with forget password link</returns>
        // POST: api/auth/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            ServiceResponse<string> response = await _authRepository.ForgotPassword(forgotPassword.Email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Reset password with new password and confirm password
        /// </summary>
        /// <returns></returns>
        // POST: api/auth/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            ServiceResponse<string> response = await _authRepository.ResetPassword(resetPassword.Token, resetPassword.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        private void SetTokenToCookie(string token)
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
