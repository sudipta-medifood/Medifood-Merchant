using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Responses
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string UserType { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
