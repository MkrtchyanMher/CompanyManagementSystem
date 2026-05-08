using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Application.DTO.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } 
        public DateTime ExpiresAt { get; set; }
    }
}
