using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fehrist.Models.API_Models.Response.User
{
    public class RegistrationResponse
    {
        public object token;

        public int? roleID { get; set; }
        public string roleName { get; set; }
        public int accountID { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string status { get; set; }
    }
}