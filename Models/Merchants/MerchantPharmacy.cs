using Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Merchants
{
    public class MerchantPharmacy : ITrackable
    {
        public int Id { get; set; }
        public string PharmacyName { get; set; }
        public string DrugLicenceNumber { get; set; }
        public string NidNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string AccountStatus { get; set; }
        public string LoginStatus { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public List<RefTokenPharmacyMerchant> RefTokenPharmacyMerchants { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefTokenPharmacyMerchants?.Find(x => x.Token == token) != null;
        }
    }
}
