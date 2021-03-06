using Dtos.MerchantDtos;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Merchants;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class MerchantProfileRepository : IMerchantProfileRepository
    {
        private readonly DataContext _context;

        public MerchantProfileRepository(DataContext context)
        {
            this._context = context;
        }

        // get pharmacy merchant profile using id
        public async Task<PharmacyMerchantProfile> GetPharmacyMerchantProfile(int id)
        {
            PharmacyMerchantProfile pharmacyMerchantProfile = await _context.PharmacyMerchantProfiles.
                FirstOrDefaultAsync(x => x.Id == id);
            return pharmacyMerchantProfile;
        }

        // create pharmacy merchant profile
        public async Task<ServiceResponse<PharmacyMerchantProfileDto>> CreatePharmacyMerchantProfile(PharmacyMerchantProfileDto pharmacyMerchantProfileDto)
        {
            ServiceResponse<PharmacyMerchantProfileDto> response = new ServiceResponse<PharmacyMerchantProfileDto>() { Data = new PharmacyMerchantProfileDto() };
            MerchantPharmacy pharmacyMerchant = await _context.MerchantPharmacys.FirstOrDefaultAsync(x => x.Email == pharmacyMerchantProfileDto.Email.ToLower());
            if (pharmacyMerchant != null)
            {
                var pharmacyMerchantProfile = new PharmacyMerchantProfile()
                {
                    FirstName = pharmacyMerchantProfileDto.FirstName,
                    LastName = pharmacyMerchantProfileDto.LastName,
                    Phone = pharmacyMerchantProfileDto.Phone,
                    Email = pharmacyMerchantProfileDto.Email,
                    Address = pharmacyMerchantProfileDto.Address,
                    MerchantPaymentType = pharmacyMerchantProfileDto.MerchantPaymentType,
                    PaymentPeriod = pharmacyMerchantProfileDto.PaymentPeriod,
                    PharmacyName = pharmacyMerchantProfileDto.PharmacyName,
                    DrugLicenseNumber = pharmacyMerchantProfileDto.DrugLicenseNumber,
                    TradeLicenseNumber = pharmacyMerchantProfileDto.TradeLicenseNumber,
                    NidNumber = pharmacyMerchantProfileDto.NidNumber,
                    CreationDate = pharmacyMerchant.CreatedAt,
                    ApprovedDate = DateTime.UtcNow,
                    LoginStatus = pharmacyMerchantProfileDto.LoginStatus,
                    PharmacyMerchantId = pharmacyMerchant.Id
                };

                await _context.PharmacyMerchantProfiles.AddAsync(pharmacyMerchantProfile);
                await _context.SaveChangesAsync();
                response.Data = new PharmacyMerchantProfileDto
                {
                    FirstName = pharmacyMerchantProfile.FirstName,
                    LastName = pharmacyMerchantProfile.LastName,
                    Phone = pharmacyMerchantProfile.Phone,
                    Email = pharmacyMerchantProfile.Email,
                    Address = pharmacyMerchantProfile.Address,
                    MerchantPaymentType = pharmacyMerchantProfile.MerchantPaymentType,
                    PaymentPeriod = pharmacyMerchantProfile.PaymentPeriod,
                    PharmacyName = pharmacyMerchantProfile.PharmacyName,
                    DrugLicenseNumber = pharmacyMerchantProfile.DrugLicenseNumber,
                    TradeLicenseNumber = pharmacyMerchantProfile.TradeLicenseNumber,
                    NidNumber = pharmacyMerchantProfile.NidNumber,
                    ApprovedDate = pharmacyMerchantProfile.ApprovedDate,
                    LoginStatus = pharmacyMerchantProfile.LoginStatus,
                    PharmacyMerchantId = pharmacyMerchantProfile.Id
                };
                response.Message = "Pharmacy merchant profile created successfully!";
            }
            else
            {
                response.Success = false;
                response.Data = new PharmacyMerchantProfileDto { };
                response.Message = "Pharmacy merchant profile create failed!";
            }
            return response;
        }

        // update pharmacy merchant profile
        public async Task<ServiceResponse<string>> UpdatePharmacyMerchantProfile(int id, PharmacyMerchantProfile pharmacyMerchantProfile)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var pharmacymerchantprofile = await _context.PharmacyMerchantProfiles.FirstOrDefaultAsync(x => x.Id == id);
            if (pharmacymerchantprofile != null)
            {
                pharmacymerchantprofile.FirstName = pharmacyMerchantProfile.FirstName;
                pharmacymerchantprofile.LastName = pharmacyMerchantProfile.LastName;
                pharmacymerchantprofile.Phone = pharmacyMerchantProfile.Phone;
                pharmacymerchantprofile.Email = pharmacyMerchantProfile.Email;
                pharmacymerchantprofile.Address = pharmacyMerchantProfile.Address;
                pharmacymerchantprofile.MerchantPaymentType = pharmacyMerchantProfile.MerchantPaymentType;
                pharmacymerchantprofile.PaymentPeriod = pharmacyMerchantProfile.PaymentPeriod;
                pharmacymerchantprofile.PharmacyName = pharmacyMerchantProfile.PharmacyName;
                pharmacymerchantprofile.DrugLicenseNumber = pharmacyMerchantProfile.DrugLicenseNumber;
                pharmacymerchantprofile.TradeLicenseNumber = pharmacyMerchantProfile.TradeLicenseNumber;
                pharmacymerchantprofile.NidNumber = pharmacyMerchantProfile.NidNumber;
                pharmacymerchantprofile.PharmacyRatting = pharmacyMerchantProfile.PharmacyRatting;
                pharmacymerchantprofile.CreationDate = pharmacyMerchantProfile.CreationDate;
                pharmacymerchantprofile.ApprovedDate = pharmacyMerchantProfile.ApprovedDate;
                pharmacymerchantprofile.BlockDate = pharmacyMerchantProfile.BlockDate;
                pharmacymerchantprofile.SuspendedDate = pharmacyMerchantProfile.SuspendedDate;
                pharmacymerchantprofile.LoginStatus = pharmacyMerchantProfile.LoginStatus;
                pharmacymerchantprofile.PharmacyMerchantId = pharmacyMerchantProfile.PharmacyMerchantId;
                _context.PharmacyMerchantProfiles.Update(pharmacymerchantprofile);
                await _context.SaveChangesAsync();
                response.Message = "Profile data updated successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "No profile created for merchant!";
            }

            return response;
        }

        // get restaurant merchant profile using id
        public async Task<RestaurantMerchantProfile> GetRestaurantMerchantProfile(int id)
        {
            RestaurantMerchantProfile restaurantMerchantProfile = await _context.RestaurantMerchantProfiles.FirstOrDefaultAsync(x => x.Id == id);
            return restaurantMerchantProfile;
        }

        // create restaurant merchant profile
        public async Task<ServiceResponse<RestaurantMerchantProfileDto>> CreateRestaurantMerchantProfile(RestaurantMerchantProfileDto restaurantMerchantProfileDto)
        {
            ServiceResponse<RestaurantMerchantProfileDto> response = new ServiceResponse<RestaurantMerchantProfileDto>() { Data = new RestaurantMerchantProfileDto() };
            MerchantRestaurant restaurantMerchant = await _context.MerchantRestaurants.FirstOrDefaultAsync(x => x.Email == restaurantMerchantProfileDto.Email.ToLower());
            if (restaurantMerchant != null)
            {
                var restaurantMerchantProfile = new RestaurantMerchantProfile()
                {
                    FirstName = restaurantMerchantProfileDto.FirstName,
                    LastName = restaurantMerchantProfileDto.LastName,
                    Phone = restaurantMerchantProfileDto.Phone,
                    Email = restaurantMerchantProfileDto.Email,
                    Address = restaurantMerchantProfileDto.Address,
                    MerchantPaymentType = restaurantMerchantProfileDto.MerchantPaymentType,
                    PaymentPeriod = restaurantMerchantProfileDto.PaymentPeriod,
                    RestaurantName = restaurantMerchantProfileDto.RestaurantName,
                    TradeLicenseNumber = restaurantMerchantProfileDto.TradeLicenseNumber,
                    NidNumber = restaurantMerchantProfileDto.NidNumber,
                    CreationDate = restaurantMerchant.CreatedAt,
                    ApprovedDate = DateTime.UtcNow,
                    LoginStatus = restaurantMerchantProfileDto.LoginStatus,
                    RestaurantMerchantId = restaurantMerchant.Id
                };

                await _context.RestaurantMerchantProfiles.AddAsync(restaurantMerchantProfile);
                await _context.SaveChangesAsync();
                response.Data = new RestaurantMerchantProfileDto
                {
                    FirstName = restaurantMerchantProfile.FirstName,
                    LastName = restaurantMerchantProfile.LastName,
                    Phone = restaurantMerchantProfile.Phone,
                    Email = restaurantMerchantProfile.Email,
                    Address = restaurantMerchantProfile.Address,
                    MerchantPaymentType = restaurantMerchantProfile.MerchantPaymentType,
                    PaymentPeriod = restaurantMerchantProfile.PaymentPeriod,
                    RestaurantName = restaurantMerchantProfile.RestaurantName,
                    TradeLicenseNumber = restaurantMerchantProfile.TradeLicenseNumber,
                    NidNumber = restaurantMerchantProfile.NidNumber,
                    ApprovedDate = restaurantMerchantProfile.ApprovedDate,
                    LoginStatus = restaurantMerchantProfile.LoginStatus,
                    RestaurantMerchantId = restaurantMerchantProfile.Id
                };
                response.Message = "Restaurant merchant profile created successfully!";
            }
            else
            {
                response.Success = false;
                response.Data = new RestaurantMerchantProfileDto { };
                response.Message = "Restaurant merchant profile create failed!";
            }
            return response;
        }

        //update restaurant merchant profile
        public async Task<ServiceResponse<string>> UpdateRestaurantMerchantProfile(int id, RestaurantMerchantProfile restaurantMerchantProfile)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var restaurantmerchantprofile = await _context.RestaurantMerchantProfiles.FirstOrDefaultAsync(x => x.Id == id);
            if (restaurantmerchantprofile != null)
            {
                restaurantmerchantprofile.FirstName = restaurantMerchantProfile.FirstName;
                restaurantmerchantprofile.LastName = restaurantMerchantProfile.LastName;
                restaurantmerchantprofile.Phone = restaurantMerchantProfile.Phone;
                restaurantmerchantprofile.Email = restaurantMerchantProfile.Email;
                restaurantmerchantprofile.Address = restaurantMerchantProfile.Address;
                restaurantmerchantprofile.MerchantPaymentType = restaurantMerchantProfile.MerchantPaymentType;
                restaurantmerchantprofile.PaymentPeriod = restaurantMerchantProfile.PaymentPeriod;
                restaurantmerchantprofile.RestaurantName = restaurantMerchantProfile.RestaurantName;
                restaurantmerchantprofile.TradeLicenseNumber = restaurantMerchantProfile.TradeLicenseNumber;
                restaurantmerchantprofile.NidNumber = restaurantMerchantProfile.NidNumber;
                restaurantmerchantprofile.RestaurantRatting = restaurantMerchantProfile.RestaurantRatting;
                restaurantmerchantprofile.CreationDate = restaurantMerchantProfile.CreationDate;
                restaurantmerchantprofile.ApprovedDate = restaurantMerchantProfile.ApprovedDate;
                restaurantmerchantprofile.BlockDate = restaurantMerchantProfile.BlockDate;
                restaurantmerchantprofile.SuspendedDate = restaurantMerchantProfile.SuspendedDate;
                restaurantmerchantprofile.LoginStatus = restaurantMerchantProfile.LoginStatus;
                restaurantmerchantprofile.RestaurantMerchantId = restaurantMerchantProfile.RestaurantMerchantId;
                _context.RestaurantMerchantProfiles.Update(restaurantmerchantprofile);
                await _context.SaveChangesAsync();
                response.Message = "Profile data updated successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "No profile created for merchant!";
            }

            return response;
        }
    }
}
