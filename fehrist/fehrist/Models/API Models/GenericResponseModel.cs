using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fehrist.Models.API_Models
{
    public class GenericResponseModel
    {
        public string status { get; set; }
        public string msg { get; set; }
        public string response { get; set; }
    }
}