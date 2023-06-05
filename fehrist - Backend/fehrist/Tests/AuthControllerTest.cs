using System;
using NUnit.Framework;
using Moq;
using fehrist.Controllers;
using fehrist.Services;
using fehrist.Models.API_Models.Request.User;
using fehrist.Models.API_Models;
using fehrist.Models.API_Models.Response.User;
using fehrist.Accessors;
using fehrist.Helper;
using fehrist.Models;

namespace fehrist.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        // LOGIN TEST CASES
        [Test]
        public void Login_ValidCredentials_ReturnsSuccessResponse()
        {
            // Arrange
            var mockUserAccessor = new Mock<UserAccessor>();
            var mockTokenHandler = new Mock<token_handler>();
            var mockHelper = new Mock<UserHelpers>();

            var userServices = new UserServices(mockUserAccessor.Object);
            var controller = new AuthController(userServices);


            var loginRequest = new LoginRequest
            {
                email = "test@example.com",
                password = "Test@1234"
            };

            // Set up mock dependencies
            mockUserAccessor.Setup(accessor => accessor.GET_UserHash(It.IsAny<string>())).Returns("AAIW9eCvgp+a2xGHX8TEzNuhWETYuso6eueTmiwHnu0U0+6pHwGYGPkZKBdO1jPUbg==");
            mockUserAccessor.Setup(accessor => accessor.Get_UserAccount(It.IsAny<string>())).Returns(new ACCOUNT
            {
                ROLE = new ROLE { NAME = "USER" },
                ROLEID = 1,
                ACCOUNTID = 1,
                NAME = "John Doe",
                EMAIL = "john@example.com",
                AC_STATUS = "Active"
            });

            // Act
            var response = controller.Login(loginRequest);

            // Assert
            Assert.AreEqual("PASS", response.status);
            Assert.AreEqual("Login Success.", response.msg);
            Assert.AreEqual(200, response.code);
            Assert.IsNotNull(response.response);
            Assert.AreEqual(1, response.response.roleID);
            Assert.AreEqual("USER", response.response.roleName);
            Assert.AreEqual(1, response.response.accountID);
            Assert.AreEqual("John Doe", response.response.name);
            Assert.AreEqual("john@example.com", response.response.email);
            Assert.AreEqual("Active", response.response.status);
        }

        [Test]
        public void Login_InvalidCredentials_ReturnsInvalid()
        {
            // Arrange
            var userServices = new UserServices();
            var controller = new AuthController(userServices);


            var loginRequest = new LoginRequest
            {
                email = "test@example.com",
                password = "WrongPass@"
            };

            // Act
            var response = controller.Login(loginRequest);

            // Assert
            Assert.AreEqual("FAIL", response.status);
            Assert.AreEqual("Invalid Email/Password. Please try again.", response.msg);
            Assert.AreEqual(401, response.code);
            Assert.IsNull(response.response);
        }

        [Test]
        public void Login_InvalidCredentials_ReturnsNotFound()
        {
            // Arrange
            var userServices = new UserServices();
            var controller = new AuthController(userServices);


            var loginRequest = new LoginRequest
            {
                email = "super@d.com",
                password = "Apple@123"
            };

            // Act
            var response = controller.Login(loginRequest);

            // Assert
            Assert.AreEqual("FAIL", response.status);
            Assert.AreEqual("Email address not found. Please try again.", response.msg);
            Assert.AreEqual(404, response.code);
            Assert.IsNull(response.response);
        }

        // REGISTER TEST CASES
        [Test]
        public void Register_ValidCredentials_Success()
        {
            var authController = new AuthController();
            var registrationRequest = new RegistrationRequest()
            {
                name = "TEST USER",
                email = "test@example.com",
                password = "Test@1234"
            };

            // Act
            var result = authController.Register(registrationRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("PASS", result.status);
            Assert.AreEqual("Account registered successfully.", result.msg);
            Assert.IsNotNull(result.response);
            Assert.AreEqual(2, result.response.roleID);
            Assert.AreEqual("USER", result.response.roleName);
            Assert.AreEqual("TEST USER", result.response.name);
            Assert.AreEqual("test@example.com", result.response.email);
            Assert.AreEqual("ACTIVE", result.response.status);
        }

        [Test]
        public void Register_DuplicateCredentials_Success()
        {
            var authController = new AuthController();
            var registrationRequest = new RegistrationRequest()
            {
                name = "TEST USER",
                email = "test@example.com",
                password = "Test@1234"
            };

            // Act
            var result = authController.Register(registrationRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("FAIL", result.status);
            Assert.AreEqual(409, result.code);
            Assert.AreEqual("Email already exists. Please log in using your email.", result.msg);
            Assert.IsNull(result.response);
        }

        [Test]
        public void Register_InvalidCredentials_ReturnInvalid()
        {
            var authController = new AuthController();
            var registrationRequest = new RegistrationRequest()
            {
                name = "a123asd",
                email = "",
                password = "T-"
            };

            // Act
            var result = authController.Register(registrationRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("FAIL", result.status);
            Assert.AreEqual(401, result.code);
            Assert.IsNull(result.response);
        }
    }
}
