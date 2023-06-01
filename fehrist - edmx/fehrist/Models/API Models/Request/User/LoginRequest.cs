using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fehrist.Models.API_Models.Request.User
{
    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}