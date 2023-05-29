using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fehrist.Models.API_Models
{
    public class ResponseModel<T> where T : new()
    {
        public string status { get; set; }
        public string msg { get; set; }
        public T response { get; set; }

        public ResponseModel()
        {
            response = new T();
        }
    }
}