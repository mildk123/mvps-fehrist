using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using fehrist.Controllers;
using fehrist.Services;
using fehrist.Models.API_Models.Request.User;

namespace fehristTEST
{
    private UserController userController;
    private Mock<UserServices> userServiceMock;

    [SetUp]
    public void Setup()
    {
        userServiceMock = new Mock<UserServices>();
        userController = new UserController(userServiceMock.Object);
    }

    [Test]
    public void Login_ValidRequest_ReturnsLoginResponse()
    {
        // Arrange
        var request = new LoginRequest
        {
            email = "test@example.com",
            password = "password"
        };

        var expectedResponse = new ResponseModel<LoginResponse>
        {
            // Set the expected properties of the response object
            // based on the business logic in UserServices
            // For example:
            Success = true,
            Data = new LoginResponse
            {
                // Set the expected properties of the login response
            }
        };

        userServiceMock.Setup(x => x.Login_User(request.email, request.password))
            .Returns(expectedResponse);

        // Act
        var result = userController.Login(request);

        // Assert
        Assert.AreEqual(expectedResponse, result);
    }

    [Test]
    public void Register_ValidRequest_ReturnsRegistrationResponse()
    {
        // Arrange
        var request = new RegistrationRequest
        {
            name = "John Doe",
            email = "test@example.com",
            password = "password"
        };

        var expectedResponse = new ResponseModel<RegistrationResponse>
        {
            // Set the expected properties of the response object
            // based on the business logic in UserServices
            // For example:
            Success = true,
            Data = new RegistrationResponse
            {
                // Set the expected properties of the registration response
            }
        };

        userServiceMock.Setup(x => x.Register_User(request.name, request.email, request.password))
            .Returns(expectedResponse);

        // Act
        var result = userController.Register(request);

        // Assert
        Assert.AreEqual(expectedResponse, result);
    }
}
