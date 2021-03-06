﻿using Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Merchants
{
    public class RestaurantMerchantProfile : ITrackable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MerchantPaymentType { get; set; }
        public string PaymentPeriod { get; set; }
        public string RestaurantName { get; set; }
        public string TradeLicenseNumber { get; set; }
        public string NidNumber { get; set; }
        public string RestaurantRatting { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime? BlockDate { get; set; }
        public DateTime? SuspendedDate { get; set; }
        public int LoginStatus { get; set; }
        public int RestaurantMerchantId { get; set; }
        public MerchantRestaurant MerchantRestaurant { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
