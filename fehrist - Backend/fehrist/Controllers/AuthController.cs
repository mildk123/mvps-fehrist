using fehrist.Helper;
using fehrist.Models;
using fehrist.Models.API_Models;
using fehrist.Models.API_Models.Request.User;
using fehrist.Models.API_Models.Response.User;
using fehrist.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace fehrist.Controllers
{

    public class AuthController : ApiController
    {

        private readonly UserServices service = new UserServices();
        public AuthController()
        {

        }

        public AuthController(UserServices userServices)
        {
            this.service = userServices;
        }

        [HttpPost]
        [Route("api/user/login")]
        public ResponseModel<LoginResponse> Login([FromBody] LoginRequest request)
        {
            ResponseModel<LoginResponse> res = service.Login_User(request.email, request.password);
            // Process user login request and return the response
            return res;
        }


        [HttpGet]
        [Route("api/testcall")]
        public string TESTCALL()
        {
            // This method is used for testing purposes
            return "TEST SUCCESS";
        }

        [HttpPost]
        [Route("api/user/register")]
        public ResponseModel<RegistrationResponse> Register([FromBody] RegistrationRequest request)
        {
            ResponseModel<RegistrationResponse> res = service.Register_User(request.name, request.email, request.password);
            // Process user registration request and return the response
            return res;
        }

    }
}
