using Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Riders
{
    public class Rider : ITrackable
    {
        public int Id { get; set; }
        public string Zone { get; set; }
        public string VehicleType { get; set; }
        public string BikeRegistrationNumber { get; set; }
        public string DrivingLicenceNumber { get; set; }
        public string BicycleModelNumber { get; set; }
        public string NidNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName}{LastName}";
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public int AccountStatus { get; set; }
        public int LoginStatus { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
