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
        [HttpPost]
        [Route("api/token/verify")]
        public IHttpActionResult GET_TokenVerification()
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
                return BadRequest("invalid"); // Status code 400 Bad Request
            }
        }


        [HttpGet]
        [Route("api/user/tasks")]
        public ResponseModel<List<GET_AllTasksResponse>> GET_Tasks([FromUri] string state)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    ResponseModel<List<GET_AllTasksResponse>> res = service.GET_Tasks(identity, state);
                    return res;

                }
                else
                {
                    return new ResponseModel<List<GET_AllTasksResponse>>
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect"
                    };
                }
            }
            else
            {
                return new ResponseModel<List<GET_AllTasksResponse>>
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect"
                };
            }
        }

        [HttpGet]
        [Route("api/user/tasks")]
        public ResponseModel<GET_AllTasksResponse> GET_Task_Single([FromUri] int taskID)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    ResponseModel<GET_AllTasksResponse> res = service.GET_Task_Single(identity, taskID);
                    return res;

                }
                else
                {
                    return new ResponseModel<GET_AllTasksResponse>
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect"
                    };
                }
            }
            else
            {
                return new ResponseModel<GET_AllTasksResponse>
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect"
                };
            }
        }


        [HttpPost]
        [Route("api/user/tasks/create")]
        public GenericResponseModel SET_Task()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    GenericResponseModel res = service.SET_Task(identity);
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

        [HttpGet]
        [Route("api/user/tasks/remove")]
        public GenericResponseModel DELETE_Tasks([FromUri] int taskID)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    GenericResponseModel res = service.DELETE_Tasks(identity, taskID);
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

        //[HttpPost]
        //[Route("api/user/update-card")]
        //public GenericResponseModel UPDATE_Tasks()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var identity = User.Identity as ClaimsIdentity;
        //        if (identity != null)
        //        {
        //            UserServices service = new UserServices();
        //            GenericResponseModel res = service.UPDATE_Task(identity);
        //            return res;

        //        }
        //        else
        //        {
        //            return new GenericResponseModel
        //            {
        //                response = null,
        //                msg = "Please login with your account credentials.",
        //                status = "Redirect"
        //            };
        //        }
        //    }
        //    else
        //    {
        //        return new GenericResponseModel
        //        {
        //            response = null,
        //            msg = "Please login with your account credentials.",
        //            status = "Redirect"
        //        };
        //    }
        //}

        [HttpPost]
        [Route("api/user/tasks/update-status")]
        public GenericResponseModel UPDATE_Task_Status([FromBody] UpdateTaskRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    GenericResponseModel res = service.UPDATE_Task_Status(identity, request.taskID, request.status);
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

        [HttpPost]
        [Route("api/user/tasks/update-color")]
        public GenericResponseModel UPDATE_Task_Color([FromBody] UpdateTaskColor request)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    GenericResponseModel res = service.UPDATE_Task_Color(identity, request.taskID, request.color);
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


        [HttpGet]
        [Route("api/user/tasks/")]
        public ResponseModel<List<GET_AllTasksResponse>> SEARCH_Tasks([FromUri] string searchTerm, string status)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    UserServices service = new UserServices();
                    ResponseModel<List<GET_AllTasksResponse>> res = service.SEARCH_Tasks(identity, searchTerm, status);
                    return res;

                }
                else
                {
                    return new ResponseModel<List<GET_AllTasksResponse>>
                    {
                        response = null,
                        msg = "Please login with your account credentials.",
                        status = "Redirect"
                    };
                }
            }
            else
            {
                return new ResponseModel<List<GET_AllTasksResponse>>
                {
                    response = null,
                    msg = "Please login with your account credentials.",
                    status = "Redirect"
                };
            }
        }

    }
}
