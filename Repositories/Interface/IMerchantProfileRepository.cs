using Dtos.MerchantDtos;
using Models;
using Models.Merchants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IMerchantProfileRepository
    {
        Task<PharmacyMerchantProfile> GetPharmacyMerchantProfile(int id);
        Task<ServiceResponse<PharmacyMerchantProfileDto>> CreatePharmacyMerchantProfile(PharmacyMerchantProfileDto pharmacyMerchantProfileDto);
        Task<ServiceResponse<string>> UpdatePharmacyMerchantProfile(int id, PharmacyMerchantProfile pharmacyMerchantProfile);
        Task<RestaurantMerchantProfile> GetRestaurantMerchantProfile(int id);
        Task<ServiceResponse<RestaurantMerchantProfileDto>> CreateRestaurantMerchantProfile(RestaurantMerchantProfileDto restaurantMerchantProfileDto);
        Task<ServiceResponse<string>> UpdateRestaurantMerchantProfile(int id, RestaurantMerchantProfile restaurantMerchantProfile);
    }
}
