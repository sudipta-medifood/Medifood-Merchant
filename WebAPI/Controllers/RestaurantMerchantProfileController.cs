using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos.MerchantDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Merchants;
using Repositories.Interface;
using Repositories.Repository;

namespace WebAPI.Controllers
{
    public class RestaurantMerchantProfileController : BaseApiController
    {
        private readonly IMerchantProfileRepository _merchantProfileRepository;

        public RestaurantMerchantProfileController(IMerchantProfileRepository merchantProfileRepository)
        {
            this._merchantProfileRepository = merchantProfileRepository;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantMerchantProfile>> GetRestaurantMerchantProfile(int id)
        {
            return await _merchantProfileRepository.GetRestaurantMerchantProfile(id);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRestaurantMerchantProfile(RestaurantMerchantProfileDto restaurantMerchantProfile)
        {
            ServiceResponse<RestaurantMerchantProfileDto> merchantProfileResponse = await _merchantProfileRepository.CreateRestaurantMerchantProfile(restaurantMerchantProfile);
            if (!merchantProfileResponse.Success)
            {
                return BadRequest(merchantProfileResponse);
            }
            return Ok(merchantProfileResponse);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurantMerchantProfile(int id, RestaurantMerchantProfile restaurantMerchantProfile)
        {
            ServiceResponse<string> updateProfileResponse = await _merchantProfileRepository.UpdateRestaurantMerchantProfile(id, restaurantMerchantProfile);
            if (!updateProfileResponse.Success)
            {
                return BadRequest(updateProfileResponse);
            }
            return Ok(updateProfileResponse);
        }
    }
}
