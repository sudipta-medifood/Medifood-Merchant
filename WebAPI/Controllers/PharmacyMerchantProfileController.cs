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

namespace WebAPI.Controllers
{
    public class PharmacyMerchantProfileController : BaseApiController
    {
        private readonly IMerchantProfileRepository _merchantProfileRepo;

        public PharmacyMerchantProfileController(IMerchantProfileRepository merchantProfileRepo)
        {
            this._merchantProfileRepo = merchantProfileRepo;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PharmacyMerchantProfile>> GetPharmacyMerchantProfile(int id)
        {
            return await _merchantProfileRepo.GetPharmacyMerchantProfile(id);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePharmacyMerchantProfile(PharmacyMerchantProfileDto pharmacyMerchant)
        {
            ServiceResponse<PharmacyMerchantProfileDto> merchantProfileResponse = await _merchantProfileRepo.CreatePharmacyMerchantProfile(pharmacyMerchant);

            if (!merchantProfileResponse.Success)
            {
                return BadRequest(merchantProfileResponse);
            }
            return Ok(merchantProfileResponse);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePharmacyMerchantProfile(int id, PharmacyMerchantProfile pharmacyMerchant)
        {
            ServiceResponse<string> updateProfileResponse = await _merchantProfileRepo.UpdatePharmacyMerchantProfile(id, pharmacyMerchant);
            if (!updateProfileResponse.Success)
            {
                return BadRequest();
            }
            return Ok(updateProfileResponse);
        }
    }
}
