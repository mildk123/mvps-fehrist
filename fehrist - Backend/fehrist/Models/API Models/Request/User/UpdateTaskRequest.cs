using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fehrist.Models.API_Models.Request.User
{
    public class UpdateTaskRequest
    {
        public int taskID { get; set; }
        public string status { get; set; }
    }
}