using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fehrist.Models.API_Models.Request.User
{
    public class SET_TaskRequest
    {
        public int accountID { get; set; }
        public string title { get; set; }
    }
}