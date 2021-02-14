using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Models;
using Models.Merchants;
using Models.Responses;

namespace Repositories.Interface
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> RegisterPharmacyMerchant(MerchantPharmacy merchamtPharmacy, string password);
        Task<ServiceResponse<int>> RegisterRestaurantMerchant(MerchantRestaurant merchantRestaurant, string password);
        Task<ServiceResponse<Tokens>> Login(string email, string password);
        Task<ServiceResponse<string>> ForgotPassword(string email);
        Task<ServiceResponse<string>> ResetPassword(string token, string password);
        Task<ServiceResponse<Tokens>> CreateLoginPharmacyMerchant(string email, string password);
        Task<ServiceResponse<Tokens>> CreateLoginRestaurantMerchant(string email, string password);
        Task<ServiceResponse<string>> ForgotPasswordPharmacyMerchant(string email);
        Task<ServiceResponse<string>> ForgotPasswordRestaurantMerchant(string email);
        Task<bool> EmailExists(string email, string merchantUserFlag);
        string CreateTokenPharmacyMerchant(MerchantPharmacy merchantPharmacy);
        string CreateTokenRestaurantMerchant(MerchantRestaurant merchantRestaurant);
    }
}
