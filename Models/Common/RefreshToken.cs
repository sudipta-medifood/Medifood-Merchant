using Microsoft.EntityFrameworkCore;
using Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Merchants
{
    public abstract class RefreshToken : ITrackable      //abstract class
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
