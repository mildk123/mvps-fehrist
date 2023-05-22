using fehrist.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fehrist.Models.API_Models.Response.User
{
    public class GET_AllTasksResponse
    {
        public int taskID { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string color { get; set; }
        public List<ImagesModel> imageList { get; set; }
    }
}