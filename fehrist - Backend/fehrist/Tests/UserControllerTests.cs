using fehrist.Accessors;
using fehrist.Controllers;
using fehrist.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace fehrist.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserAccessor accessor;
        [SetUp]
        public void Setup()
        {
            // Initialize the accessor before each test
            accessor = new UserAccessor();
        }

        

        [Test]
        public void TokenVerification_ReturnsValid_WhenUserIsAuthenticated()
        {
            // Arrange
            var controller = new UserController();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.AuthenticationMethod, "JWT")
            }, "mock"));

            controller.User = user;

            // Act
            var result = controller.GET_TokenVerification() as OkNegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("valid", result.Content);
        }

        [Test]
        public void TokenVerification_ReturnsInvalid_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var controller = new UserController();
            var user = new Mock<IPrincipal>();
            user.Setup(u => u.Identity.IsAuthenticated).Returns(false);

            controller.User = user.Object;

            // Act
            // Retrieve the message
            var res = controller.GET_TokenVerification() as NegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual("invalid", res.Content);
        }

       

        // SET-UPDATE TODO 
        [Test]
        public void SET_Task_NewTask_ReturnsSuccessMessage()
        {
            // Arrange
            int accID = 4;
            int taskID = 0;
            string title = "New Task";
            string desc = "Task Description";
            string status = "ADDED";
            string color = "Blue";
            string dueDate = "2023-06-30";
            string addedDate = "2023-06-04";
            List<HttpPostedFile> filesList = new List<HttpPostedFile>();
            string checkList = "[\"Item 1\", \"Item 2\"]";

            // Act
            var result = accessor.SET_Task(accID, taskID, title, desc, status, color, dueDate, addedDate, filesList, checkList);

            // Assert
            Assert.AreEqual("Successfully saved task", result);
            // Add additional assertions if needed
        }

        [Test]
        public void SET_Task_UpdateTask_ReturnsSuccessMessage()
        {
            // Arrange
            int accID = 4;
            int taskID = 2;
            string title = "Updated Task";
            string desc = "Updated Task Description";
            string status = "Completed";
            string color = "Green";
            string dueDate = "2023-06-15";
            string addedDate = "2023-06-04";
            List<HttpPostedFile> filesList = new List<HttpPostedFile>();
            string checkList = "[\"Item 1\", \"Item 2\", \"Item 3\"]";

            // Act
            var result = accessor.SET_Task(accID, taskID, title, desc, status, color, dueDate, addedDate, filesList, checkList);

            // Assert
            Assert.AreEqual("Successfully updated task", result);
            // Add additional assertions if needed
        }

        [Test]
        public void SET_Task_InvalidTask_ReturnsNull()
        {
            // Arrange
            int accID = 1;
            int taskID = 999; // Assuming an invalid task ID
            string title = "Invalid Task";
            string desc = "Invalid Task Description";
            string status = "Pending";
            string color = "Red";
            string dueDate = "2023-06-30";
            string addedDate = "2023-06-04";
            List<HttpPostedFile> filesList = new List<HttpPostedFile>();
            string checkList = "[\"Item 1\", \"Item 2\"]";

            // Act
            var result = accessor.SET_Task(accID, taskID, title, desc, status, color, dueDate, addedDate, filesList, checkList);

            // Assert
            Assert.IsNull(result);
        }

        // CHECKLIST TESTS
        [Test]
        public void DELETE_Check_ExistingCheck_ReturnsSuccessMessage()
        {
            // Arrange
            int accID = 4;
            int checkID = 6; // Assuming an existing checklist item ID

            // Act
            var result = accessor.DELETE_Check(accID, checkID);

            // Assert
            Assert.AreEqual("Successfully removed checklist", result);
            // Add additional assertions if needed
        }

        [Test]
        public void DELETE_Check_NonExistingCheck_ReturnsNotFoundMessage()
        {
            // Arrange
            int accID = 4;
            int checkID = 999; // Assuming a non-existing checklist item ID

            // Act
            var result = accessor.DELETE_Check(accID, checkID);

            // Assert
            Assert.AreEqual("Checklist not found.", result);
            // Add additional assertions if needed
        }

        // CHANGE COLOR TEST
        [Test]
        public void UPDATE_Task_Color_ExistingTask_ReturnsSuccessMessage()
        {
            // Arrange
            int taskID = 3; // Assuming an existing task ID
            int accID = 4; // Assuming an existing account ID
            string color = "red"; // Assuming a valid color value

            // Act
            var result = accessor.UPDATE_Task_Color(taskID, accID, color);

            // Assert
            Assert.AreEqual("Successfully changed color of task", result);
            // Add additional assertions if needed
        }

        [Test]
        public void UPDATE_Task_Color_NonExistingTask_ReturnsErrorMessage()
        {
            // Arrange
            int taskID = 999; // Assuming a non-existing task ID
            int accID = 4; // Assuming an existing account ID
            string color = "#FF0000"; // Assuming a valid color value

            // Act
            var result = accessor.UPDATE_Task_Color(taskID, accID, color);

            // Assert
            Assert.AreEqual("The selected task does not exist anymore.", result);
        }


        // Updte Status
        [Test]
        public void UPDATE_Task_Status_ExistingTask_ReturnsSuccessMessage()
        {
            // Arrange
            int taskID = 2; // Assuming an existing task ID
            int accID = 4; // Assuming an existing account ID
            string status = "completed"; // Assuming a valid status value

            // Act
            var result = accessor.UPDATE_Task_Status(taskID, accID, status);

            // Assert
            Assert.AreEqual("Successfully changed status of task", result);
            // Add additional assertions if needed
        }

        [Test]
        public void UPDATE_Task_Status_NonExistingTask_ReturnsErrorMessage()
        {
            // Arrange
            int taskID = 999; // Assuming a non-existing task ID
            int accID = 4; // Assuming an existing account ID
            string status = "completed"; // Assuming a valid status value

            // Act
            var result = accessor.UPDATE_Task_Status(taskID, accID, status);

            // Assert
            Assert.AreEqual("The selected task does not exist anymore.", result);
            // Add additional assertions if needed
        }

        [Test]
        public void UPDATE_Task_Status_InvalidStatus_ReturnsErrorMessage()
        {
            // Arrange
            int taskID = 2; // Assuming an existing task ID
            int accID = 4; // Assuming an existing account ID
            string status = "7777777777777777777777777777777777777777777777777777777777777777777777777777777777777"; // Assuming an invalid status value

            // Act
            var result = accessor.UPDATE_Task_Status(taskID, accID, status);

            // Assert
            Assert.AreEqual("An error occurred while updating the current task. Please log in again and refresh the caches.", result);
            // Add additional assertions if needed
        }


    }
}