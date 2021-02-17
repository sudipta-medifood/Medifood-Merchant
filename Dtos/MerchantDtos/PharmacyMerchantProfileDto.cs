using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.MerchantDtos
{
    public class PharmacyMerchantProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MerchantPaymentType { get; set; }
        public string PaymentPeriod { get; set; }
        public string PharmacyName { get; set; }
        public string DrugLicenseNumber { get; set; }
        public string TradeLicenseNumber { get; set; }
        public string NidNumber { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int LoginStatus { get; set; }
        public int PharmacyMerchantId { get; set; }
    }
}
