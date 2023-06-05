using fehrist.Models;
using fehrist.Models.API_Models;
using fehrist.Models.API_Models.Request.User;
using fehrist.Models.API_Models.Response.User;
using fehrist.Services;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace fehrist.Controllers
{
    public class UserController : ApiController
    {

        // CONTROLLER TO VERIFIY JWT TOKEN 
        [HttpPost]
        [Route("api/token/verify")]
        public virtual IHttpActionResult GET_TokenVerification()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    return Ok("valid"); // Status code 200 OK
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, "expired"); // Status code 401 Unauthorized
                }
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "invalid"); // Status code 400 Unauthorized
            }
        }

        // CONTROLLER TO GET ALL TO-DO
        [HttpGet]
        [Route("api/user/tasks")]
        public virtual IHttpActionResult GET_Tasks([FromUri] string state)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    ResponseModel<List<GET_AllTasksResponse>> res = service.GET_Tasks(identity, state);
                    return Ok(res); // Status code 200 OK
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, new ResponseModel<List<GET_AllTasksResponse>>
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect",
                        code = 401
                    }); // Status code 401 Unauthorized
                }
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new ResponseModel<List<GET_AllTasksResponse>>
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect",
                    code = 401
                }); // Status code 401 Unauthorized
            }
        }

        // CONTROLLER TO GET SELECTED TO-DO DATA
        [HttpGet]
        [Route("api/user/tasks")]
        public IHttpActionResult GET_Task_Single([FromUri] int taskID)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    ResponseModel<GET_AllTasksResponse> res = service.GET_Task_Single(identity, taskID);
                    return Ok(res); // Status code 200 OK
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, new ResponseModel<GET_AllTasksResponse>
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect",
                        code = 401
                    }); // Status code 401 Unauthorized
                }
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new ResponseModel<GET_AllTasksResponse>
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect",
                    code = 401
                }); // Status code 401 Unauthorized
            }
        }

        // CONTROLLER TO SET TO-DO DATA
        [HttpPost]
        [Route("api/user/tasks/create")]
        public IHttpActionResult SET_Task()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    GenericResponseModel res = service.SET_Task(identity);
                    return Ok(res); // Status code 200 OK
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, new GenericResponseModel
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect"
                    }); // Status code 401 Unauthorized
                }
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new GenericResponseModel
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect"
                }); // Status code 401 Unauthorized
            }
        }

        // CONTROLLER TO REMOVE TO-DO
        [HttpGet]
        [Route("api/user/tasks/remove")]
        public IHttpActionResult DELETE_Tasks([FromUri] int taskID)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    GenericResponseModel res = service.DELETE_Tasks(identity, taskID);
                    return Ok(res); // Status code 200 OK
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, new GenericResponseModel
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect"
                    }); // Status code 401 Unauthorized
                }
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new GenericResponseModel
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect"
                }); // Status code 401 Unauthorized
            }
        }

        // CONTROLLER TO MARK TO-DO AS ARCHIVE,COMPLETED,DELETED OR RESTORED TO ADDED
        [HttpPost]
        [Route("api/user/tasks/update-status")]
        public IHttpActionResult UPDATE_Task_Status([FromBody] UpdateTaskRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    GenericResponseModel res = service.UPDATE_Task_Status(identity, request.taskID, request.status);
                    return Ok(res); // Status code 200 OK
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, new GenericResponseModel
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect"
                    }); // Status code 401 Unauthorized
                }
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new GenericResponseModel
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect"
                }); // Status code 401 Unauthorized
            }
        }

        // CONTROLLER TO CHANGE COLOR OF TO-DO
        [HttpPost]
        [Route("api/user/tasks/update-color")]
        public IHttpActionResult UPDATE_Task_Color([FromBody] UpdateTaskColor request)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    GenericResponseModel res = service.UPDATE_Task_Color(identity, request.taskID, request.color);
                    return Ok(res); // Status code 200 OK
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, new GenericResponseModel
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect"
                    }); // Status code 401 Unauthorized
                }
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new GenericResponseModel
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect"
                }); // Status code 401 Unauthorized
            }
        }


        // CONTROLLER TO SEARCH FOR TO-DO USING TERM OF LENGTH GREATER THAN 3
        [HttpGet]
        [Route("api/user/tasks/")]
        public IHttpActionResult SEARCH_Tasks([FromUri] string searchTerm, string status)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    ResponseModel<List<GET_AllTasksResponse>> res = service.SEARCH_Tasks(identity, searchTerm, status);
                    return Ok(res); // Status code 200 OK
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, new ResponseModel<List<GET_AllTasksResponse>>
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect"
                    }); // Status code 401 Unauthorized
                }
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new ResponseModel<List<GET_AllTasksResponse>>
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect"
                }); // Status code 401 Unauthorized
            }
        }

        // CONTROLLER TO REMOVE A CHECKLIST ITEM FROM A TO-DO
        [HttpGet]
        [Route("api/user/tasks/checks/remove")]
        public GenericResponseModel Remove_Check([FromUri] int checkID)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    GenericResponseModel res = service.Remove_Check(identity, checkID);
                    return res;
                }
                else
                {
                    return new GenericResponseModel
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect"
                    };
                }
            }
            else
            {
                return new GenericResponseModel
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect"
                };
            }
        }
    }
}
