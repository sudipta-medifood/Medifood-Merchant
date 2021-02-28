using Models.Responses;
using Models.Riders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> RegisterRider(Rider rider, string password);
        Task<ServiceResponse<LoginResponse>> Login(string email, string password);
        Task<ServiceResponse<string>> ForgotPassword(string email);
        Task<ServiceResponse<string>> ResetPassword(string token, string password);
        Task<bool> EmailExists(string email);
        string CreateTokenRider(Rider rider);
    }
}
