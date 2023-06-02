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
            // Create a helper object to assist with user-related operations
            UserHelpers helper = new UserHelpers();

            // Create a response object to store the registration response
            ResponseModel<RegistrationResponse> response = new ResponseModel<RegistrationResponse>();

            // Check if any of the required fields (name, email, password) are empty
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                // ERROR: All Data Required
                response.status = "FAIL";
                response.msg = "Please provide your name, email, and password.";
                response.response = null;
                return response;
            }

            // Check if the email is in a valid format
            if (!helper.IsValidEmail(email))
            {
                response.status = "FAIL";
                response.msg = "Invalid email format. Please provide a valid email address.";
                response.response = null;
                return response;
            }

            // Check if the password is valid
            if (!helper.IsValidPassword(password))
            {
                response.status = "FAIL";
                response.msg = "Invalid password. It should have at least 8 characters and contain a combination of letters, digits, and special characters @$!%*#?&.";
                response.response = null;
                return response;
            }

            // Generate a hashed password for storage
            string hashedPassword = helper.GenerateHash(password);

            // Create an accessor object to interact with the user data
            UserAccessor accessor = new UserAccessor();

            // Retrieve the role ID for the "USER" role
            var roleID = accessor.GET_RoleID("USER");

            // Check if a valid role ID was obtained
            if (roleID != 0)
            {
                // Register the user's account
                var result = accessor.SET_UserAccount(name, email, hashedPassword, roleID);

                // Check if the account registration was successful
                if (result != null)
                {
                    response.status = "PASS";
                    response.msg = "Account registered successfully.";

                    // Create a token handler object for generating user tokens
                    token_handler tokenObj = new token_handler();

                    switch (result.ROLE.NAME)
                    {
                        case "USER":
                            // Create a registration response object for the user
                            RegistrationResponse userRegister = new RegistrationResponse()
                            {
                                roleID = result.ROLEID,
                                roleName = result.ROLE.NAME,
                                accountID = result.ACCOUNTID,
                                name = result.NAME,
                                email = result.EMAIL,
                                status = result.AC_STATUS
                            };

                            // Generate a token for the user
                            var userToken = tokenObj.GetUserToken(userRegister.roleID, userRegister.roleName, userRegister.accountID, userRegister.name, userRegister.email, userRegister.phone, userRegister.status);
                            userRegister.token = userToken;

                            // Set the registration response object in the response
                            response.response = userRegister;
                            return response;

                        default:
                            // Return null for unsupported roles
                            return null;
                    }
                }
                else
                {
                    response.status = "FAIL";
                    response.msg = "Email already exists. Please log in using your email.";
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
            // Extract the account ID from the claims of the authenticated user
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            // Create an accessor object to interact with the user data
            UserAccessor accessor = new UserAccessor();

            // Retrieve the list of tasks based on the account ID and state
            List<GET_AllTasksResponse> result = accessor.GET_Tasks(accID, state);

            // Create a response object to store the task retrieval response
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
            // Extract the account ID from the claims of the authenticated user
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            // Create an accessor object to interact with the user data
            UserAccessor accessor = new UserAccessor();

            // Retrieve a single task based on the account ID and task ID
            GET_AllTasksResponse result = accessor.GET_Task_Single(accID, taskID);

            // Create a response object to store the task retrieval response
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
            // Extract the account ID from the claims of the authenticated user
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            // Create an accessor object to interact with the user data
            UserAccessor accessor = new UserAccessor();

            // Retrieve the form data and files from the HTTP request
            var data = HttpContext.Current.Request.Form;
            List<HttpPostedFile> filesList = new List<HttpPostedFile>();
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                filesList.Add(HttpContext.Current.Request.Files[i]);
            }

            // Extract the task details from the form data
            var taskID = data["T_TASKID"] != "new" ? Int32.Parse(data["T_TASKID"]) : 0;
            var title = data["T_TITLE"].ToString();
            var desc = data["T_DESC"].ToString();
            var status = data["T_STATUS"].ToString();
            var color = data["T_COLOR"].ToString();
            var dueDate = data["T_DUE_DATE_TIME"].ToString();
            var addedDate = data["T_ADDED_DATE_TIME"].ToString();
            var prevImages = data["T_PREV_IMAGE"];
            var checkList = data["T_CHECKLIST"];

            // Check if there are previous images for the task
            if (prevImages != "[]" && prevImages != "0")
            {
                // Update the task details and images
                string result = accessor.UPDATE_TaskImage(accID, taskID, title, desc, status, color, dueDate, addedDate, filesList, prevImages, checkList);

                // Create a response object to store the task addition response
                GenericResponseModel response = new GenericResponseModel();
                if (result != null)
                {
                    response.status = "PASS";
                    response.msg = "Task added successfully.";
                    response.response = result;
                    return response;
                }
                else
                {
                    response.status = "FAIL";
                    response.msg = "No task found. Please try again later.";
                    response.response = null;
                    return response;
                }
            }
            else
            {
                // Add a new task with the provided details and files
                string result = accessor.SET_Task(accID, taskID, title, desc, status, color, dueDate, addedDate, filesList, checkList);

                // Create a response object to store the task addition response
                GenericResponseModel response = new GenericResponseModel();
                if (result != null)
                {
                    response.status = "PASS";
                    response.msg = "Task added successfully.";
                    response.response = result;
                    return response;
                }
                else
                {
                    response.status = "FAIL";
                    response.msg = "No task found. Please try again later.";
                    response.response = null;
                    return response;
                }
            }
        }

        public GenericResponseModel Remove_Check(ClaimsIdentity identity, int checkID)
        {
            // Extract the account ID from the claims of the authenticated user
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            // Create an accessor object to interact with the user data
            UserAccessor accessor = new UserAccessor();

            // Remove the check with the specified ID
            string result = accessor.DELETE_Check(accID, checkID);

            // Create a response object to store the check removal response
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
                response.msg = "No check found. Please try again later.";
                response.response = null;
                return response;
            }
        }

        public GenericResponseModel DELETE_Tasks(ClaimsIdentity identity, int taskID)
        {
            // Extract the account ID from the claims of the authenticated user
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            // Create an accessor object to interact with the user data
            UserAccessor accessor = new UserAccessor();

            // Delete the task with the specified ID
            string result = accessor.DELETE_Task(accID, taskID);

            // Create a response object to store the task deletion response
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

        public GenericResponseModel UPDATE_Task_Status(ClaimsIdentity identity, int taskID, string status)
        {
            // Extract the account ID from the claims of the authenticated user
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            // Create an accessor object to interact with the user data
            UserAccessor accessor = new UserAccessor();

            // Update the status of the task with the specified ID
            string result = accessor.UPDATE_Task_Status(taskID, accID, status);

            // Create a response object to store the task status update response
            GenericResponseModel response = new GenericResponseModel();
            if (result != null)
            {
                response.status = "PASS";
                response.msg = "Status updated.";
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
            // Extract the account ID from the claims of the authenticated user
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            // Create an accessor object to interact with the user data
            UserAccessor accessor = new UserAccessor();

            // Update the color of the task with the specified ID
            string result = accessor.UPDATE_Task_Color(taskID, accID, color);

            // Create a response object to store the task color update response
            GenericResponseModel response = new GenericResponseModel();
            if (result != null)
            {
                response.status = "PASS";
                response.msg = "Color updated.";
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
            // Extract the account ID from the claims of the authenticated user
            IEnumerable<Claim> claims = identity.Claims;
            int accID = Int32.Parse(claims.Where(x => x.Type == "accountID").FirstOrDefault()?.Value);

            // Create an accessor object to interact with the user data
            UserAccessor accessor = new UserAccessor();

            // Search for tasks matching the provided search title and status
            List<GET_AllTasksResponse> result = accessor.SEARCH_Tasks(accID, searchTitle, status);

            // Create a response object to store the task search response
            ResponseModel<List<GET_AllTasksResponse>> response = new ResponseModel<List<GET_AllTasksResponse>>();
            if (result != null)
            {
                if (result.Count != 0)
                {
                    response.status = "PASS";
                    response.msg = "Tasks retrieved successfully.";
                    response.response = result;
                    return response;
                }
                response.status = "FAIL";
                response.msg = "No tasks found.";
                response.response = null;
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