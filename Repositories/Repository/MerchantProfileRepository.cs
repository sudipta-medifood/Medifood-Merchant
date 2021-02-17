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
                    ApprovedDate = DateTime.Now,
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
                response.Data = new PharmacyMerchantProfileDto { };
                response.Message = "No user found!";
            }
            return response;
        }

        // update pharmacy merchant profile
        public async Task<ServiceResponse<string>> UpdatePharmacyMerchantProfile(int id, PharmacyMerchantProfile pharmacyMerchantProfile)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var pharmacyMerchant = await _context.PharmacyMerchantProfiles.FirstOrDefaultAsync(x => x.Id == id);
            pharmacyMerchant.FirstName = pharmacyMerchantProfile.FirstName;
            pharmacyMerchant.LastName = pharmacyMerchantProfile.LastName;
            pharmacyMerchant.Phone = pharmacyMerchantProfile.Phone;
            pharmacyMerchant.Email = pharmacyMerchantProfile.Email;
            pharmacyMerchant.Address = pharmacyMerchantProfile.Address;
            pharmacyMerchant.MerchantPaymentType = pharmacyMerchantProfile.MerchantPaymentType;
            pharmacyMerchant.PaymentPeriod = pharmacyMerchantProfile.PaymentPeriod;
            pharmacyMerchant.PharmacyName = pharmacyMerchantProfile.PharmacyName;
            pharmacyMerchant.DrugLicenseNumber = pharmacyMerchantProfile.DrugLicenseNumber;
            pharmacyMerchant.TradeLicenseNumber = pharmacyMerchantProfile.TradeLicenseNumber;
            pharmacyMerchant.NidNumber = pharmacyMerchantProfile.NidNumber;
            pharmacyMerchant.PharmacyRatting = pharmacyMerchantProfile.PharmacyRatting;
            pharmacyMerchant.CreationDate = pharmacyMerchantProfile.CreationDate;
            pharmacyMerchant.ApprovedDate = pharmacyMerchantProfile.ApprovedDate;
            pharmacyMerchant.BlockDate = pharmacyMerchantProfile.BlockDate;
            pharmacyMerchant.SuspendedDate = pharmacyMerchantProfile.SuspendedDate;
            pharmacyMerchant.LoginStatus = pharmacyMerchantProfile.LoginStatus;
            pharmacyMerchant.PharmacyMerchantId = pharmacyMerchantProfile.PharmacyMerchantId;
            _context.PharmacyMerchantProfiles.Update(pharmacyMerchant);
            await _context.SaveChangesAsync();
            response.Message = "Profile data updated successfully";
            return response;
        }
    }
}
