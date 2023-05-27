using fehrist.Accessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fehrist.Helper;
using fehrist.Models.API_Models;
using fehrist.Models.API_Models.Response.User;
using System.Web.Helpers;
using System.Security.Claims;
using fehrist.Models;
using Newtonsoft.Json;

namespace fehrist.Services
{
    public class UserServices
    {
        public ResponseModel<LoginResponse> Login_User(string email, string password)
        {
            // CHECK PASSWORD MATCH FUNCTIONN
            UserHelpers helper = new UserHelpers();
            ResponseModel<LoginResponse> response = new ResponseModel<LoginResponse>();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                // ERROR: All Data Required
                response.status = "FAIL";
                response.msg = "Email/Password fields cannot be empty";
                response.response = null;
                return response;
            }

            if (!helper.IsValidEmail(email))
            {
                response.status = "FAIL";
                response.msg = "Invalid email format";
                response.response = null;
                return response;
            }


            // GENERATE HASH HERE and the call Accessors
            UserAccessor accessor = new UserAccessor();
            var hashFromDB = accessor.GET_UserHash(email);
            if (hashFromDB != null)
            {
                string storedHash = hashFromDB; 
                bool isMatch = Crypto.VerifyHashedPassword(hashFromDB, password);

                if (isMatch)
                {
                    var result = accessor.Get_UserAccount(email);
                    response.status = "PASS";
                    response.msg = "Login Succes.";
                    token_handler tokenObj = new token_handler();
                    switch (result.ROLE.NAME)
                    {
                        case "USER":
                            LoginResponse userLogin = new LoginResponse()
                            {
                                roleID = result.ROLEID,
                                roleName = result.ROLE.NAME,
                                accountID = result.ACCOUNTID,
                                name = result.NAME,
                                email = result.EMAIL,
                                phone = result.PHONE,
                                status = result.AC_STATUS
                            };
                            var userToken = tokenObj.GetUserToken(userLogin.roleID, userLogin.roleName, userLogin.accountID, userLogin.name, userLogin.email, userLogin.phone, userLogin.status);
                            userLogin.token = userToken;
                            response.response = userLogin;
                            return response;
                        default:
                            return null;
                    }
                }
                else
                {
                    response.status = "FAIL";
                    response.msg = "Invalid Email/Password. Please try again.";
                    response.response = null;
                    return response;
                }
            }
            else
            {
                response.status = "FAIL";
                response.msg = "Email address not found. Please try again.";
                response.response = null;
                return response;
            }
        }

        public ResponseModel<RegistrationResponse> Register_User(string name, string email, string password)
        {
            UserHelpers helper = new UserHelpers();
            ResponseModel<RegistrationResponse> response = new ResponseModel<RegistrationResponse>();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                // ERROR: All Data Required
                response.status = "FAIL";
                response.msg= "Name/Email/Password fields cannot be empty";
                response.response = null;
                return response;
            }

            if (!helper.IsValidEmail(email))
            {
                response.status = "FAIL";
                response.msg = "Invalid email format";
                response.response = null;
                return response;
            }

            if (!helper.IsValidPassword(password))
            {
                response.status = "FAIL";
                response.msg = "Invalid password. It should have at least 8 characters and contain a combination of letters, digits, and special characters @$!%*#?&.";
                response.response = null;
                return response;
            }

            string hashedPassword = helper.GenerateHash(password);

            // GENERATE HASH HERE and the call Accessors
            UserAccessor accessor = new UserAccessor();
            var roleID = accessor.GET_RoleID("USER");
            if (roleID != 0)
            {
                var result = accessor.SET_UserAccount(name, email, hashedPassword, roleID);
                if (result != null)
                {
                    response.status = "PASS";
                    response.msg = "Account Registered Succesfully.";
                    token_handler tokenObj = new token_handler();
                    switch (result.ROLE.NAME)
                    {
                        case "USER":
                            RegistrationResponse userRegister = new RegistrationResponse()
                            {
                               roleID = result.ROLEID,
                               roleName = result.ROLE.NAME,
                               accountID = result.ACCOUNTID,
                               name = result.NAME,
                               email = result.EMAIL,
                               phone = result.PHONE,
                               status = result.AC_STATUS
                            };
                            var userToken = tokenObj.GetUserToken(userRegister.roleID, userRegister.roleName, userRegister.accountID, userRegister.name, userRegister.email, userRegister.phone, userRegister.status);
                            userRegister.token = userToken;
                            response.response = userRegister;
                            return response;
                        default:
                            return null;
                    }
                }
                else
                {
                    response.status = "FAIL";
                    response.msg = "Email already exists. Please login using your email.";
                    response.response = null;
                    return response;
                }
            }
            else
            {
                response.status = "FAIL";
                response.msg = "Invalid Role ID. Please try again later.";
                response.response = null;
                return response;
            }
        }

        public ResponseModel<List<GET_AllTasksResponse>> GET_Tasks(ClaimsIdentity identity, string state)
        {
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            UserAccessor accessor = new UserAccessor();
            List<GET_AllTasksResponse> result = accessor.GET_Tasks(accID, state);
            ResponseModel<List<GET_AllTasksResponse>> response = new ResponseModel<List<GET_AllTasksResponse>>();
            if (result.Count != 0)
            {
                response.status = "PASS";
                response.msg = "Tasks retrieved successfully.";
                response.response = result;
                return response;
            }
            else
            {
                response.status = "FAIL";
                response.msg = "No tasks found. Please try again later.";
                response.response = null;
                return response;
            }
        }

        public ResponseModel<GET_AllTasksResponse> GET_Task_Single(ClaimsIdentity identity, int taskID)
        {
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            UserAccessor accessor = new UserAccessor();
            GET_AllTasksResponse result = accessor.GET_Task_Single(accID, taskID);
            ResponseModel<GET_AllTasksResponse> response = new ResponseModel<GET_AllTasksResponse>();
            if (result != null)
            {
                response.status = "PASS";
                response.msg = "Task retrieved successfully.";
                response.response = result;
                return response;
            }
            else
            {
                response.status = "FAIL";
                response.msg = "No task found. Please try again.";
                response.response = null;
                return response;
            }
        }

        public GenericResponseModel SET_Task(ClaimsIdentity identity)
        {
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            UserAccessor accessor = new UserAccessor();

            var data = HttpContext.Current.Request.Form;
            List<HttpPostedFile> filesList = new List<HttpPostedFile>();
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                filesList.Add(HttpContext.Current.Request.Files[i]);

            }

            var taskID = data["T_TASKID"] != "new" ? Int32.Parse(data["T_TASKID"]) : 0;
            var title = data["T_TITLE"].ToString();
            var desc = data["T_DESC"].ToString();
            var status = data["T_STATUS"].ToString();
            var color = data["T_COLOR"].ToString();
            var dueDate = data["T_DUE_DATE_TIME"].ToString();
            var addedDate = data["T_ADDED_DATE_TIME"].ToString();
            var prevImages = data["T_PREV_IMAGE"];

            if (prevImages != "[]" && prevImages != "0")
            {
                string result = accessor.UPDATE_TaskImage(accID, taskID, title, desc, status, color, dueDate, addedDate, filesList, prevImages);
                GenericResponseModel response = new GenericResponseModel();
                if (result != null)
                {
                    response.status = "PASS";
                    response.msg = "Tasks added successfully.";
                    response.response = result;
                    return response;
                }
                else
                {
                    response.status = "FAIL";
                    response.msg = "No tasks found. Please try again later.";
                    response.response = null;
                    return response;
                }
            }
            else
            {
                string result = accessor.SET_Task(accID, taskID, title, desc, status, color, dueDate, addedDate, filesList);
                GenericResponseModel response = new GenericResponseModel();
                if (result != null)
                {
                    response.status = "PASS";
                    response.msg = "Tasks added successfully.";
                    response.response = result;
                    return response;
                }
                else
                {
                    response.status = "FAIL";
                    response.msg = "No tasks found. Please try again later.";
                    response.response = null;
                    return response;
                }
            }

           
        }

        public GenericResponseModel DELETE_Tasks(ClaimsIdentity identity, int taskID)
        {
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            UserAccessor accessor = new UserAccessor();

            string result = accessor.DELETE_Task(accID, taskID);

            GenericResponseModel response = new GenericResponseModel();
            if (result != null)
            {
                response.status = "PASS";
                response.msg = result;
                response.response = result;
                return response;
            }
            else
            {
                response.status = "FAIL";
                response.msg = "No tasks found. Please try again later.";
                response.response = null;
                return response;
            }
        }

        public GenericResponseModel UPDATE_Task(ClaimsIdentity identity)
        {
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            UserAccessor accessor = new UserAccessor();

            var data = HttpContext.Current.Request.Form;
            var taskID = Int32.Parse(data["TaskID"]);
            var title = data["T_TITLE"].ToString();
            var desc = data["T_DESC"].ToString();
            var color = data["T_COLOR"].ToString();
            var dueDate = data["T_DUE_DATE_TIME"].ToString();
            var addedDate = data["T_ADDED_DATE_TIME"].ToString();

            string result = accessor.UPDATE_Task(taskID, accID, title, desc, color, dueDate);

            GenericResponseModel response = new GenericResponseModel();
            if (result != null)
            {
                response.status = "PASS";
                response.msg = "Tasks Updated successfully.";
                response.response = result;
                return response;
            }
            else
            {
                response.status = "FAIL";
                response.msg = "No tasks found. Please try again later.";
                response.response = null;
                return response;
            }
        }

        public GenericResponseModel UPDATE_Task_Status(ClaimsIdentity identity, int taskID, string status)
        {
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            UserAccessor accessor = new UserAccessor();

            string result = accessor.UPDATE_Task_Status(taskID, accID, status);

            GenericResponseModel response = new GenericResponseModel();
            if (result != null)
            {
                response.status = "PASS";
                response.msg = "Status Updated.";
                response.response = result;
                return response;
            }
            else
            {
                response.status = "FAIL";
                response.msg = "No tasks found. Please try again later.";
                response.response = null;
                return response;
            }
        }


        public GenericResponseModel UPDATE_Task_Color(ClaimsIdentity identity, int taskID, string color)
        {
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            UserAccessor accessor = new UserAccessor();

            string result = accessor.UPDATE_Task_Color(taskID, accID, color);

            GenericResponseModel response = new GenericResponseModel();
            if (result != null)
            {
                response.status = "PASS";
                response.msg = "Status Updated.";
                response.response = result;
                return response;
            }
            else
            {
                response.status = "FAIL";
                response.msg = "No tasks found. Please try again later.";
                response.response = null;
                return response;
            }
        }

        public ResponseModel<List<GET_AllTasksResponse>> SEARCH_Tasks(ClaimsIdentity identity, string searchTitle, string status)
        {
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            UserAccessor accessor = new UserAccessor();
            List<GET_AllTasksResponse> result = accessor.SEARCH_Tasks(accID, searchTitle, status);
            ResponseModel<List<GET_AllTasksResponse>> response = new ResponseModel<List<GET_AllTasksResponse>>();
            if (result.Count != 0)
            {
                response.status = "PASS";
                response.msg = "Tasks retrieved successfully.";
                response.response = result;
                return response;
            }
            else
            {
                response.status = "FAIL";
                response.msg = "No tasks found.";
                response.response = null;
                return response;
            }
        }


    }
}