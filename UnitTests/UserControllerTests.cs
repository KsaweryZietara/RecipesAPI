using System.Diagnostics;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecipesAPI.Api.Controllers;
using RecipesAPI.Api.Data;
using RecipesAPI.Api.Dtos;
using RecipesAPI.Api.Models;
using Xunit;

namespace RecipesAPI.UnitTests {

    public class UserControllerTests{
        private readonly Mock<IAppRepo> respositoryStub = new Mock<IAppRepo>();

        private readonly ActivitySource activitySourceStub = new ActivitySource("test");

        [Fact]
        public async Task CreateUserAsync_CreatedUserIsNull_ReturnsBadRequest(){
            //Arrange
            respositoryStub.Setup(repo => repo.CreateUserAsync(It.IsAny<RegisterDto>()))
                            .ReturnsAsync((User)null);

            var controller = new UserController(respositoryStub.Object, activitySourceStub);

            //Act
            var result = await controller.CreateUserAsync(new RegisterDto());

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }   

        [Fact]
        public async Task CreateUserAsync_CreatedUserIsValid_ReturnsOk(){
            //Arrange
            respositoryStub.Setup(repo => repo.CreateUserAsync(It.IsAny<RegisterDto>()))
                            .ReturnsAsync(new User(){
                                ApiKey = "GWlffWBdf6y3gibvzbZl",
                                Name = "TestName",
                                EmailAddress = "TestEmailAddress",
                                Password = "TestPassword"
                            });

            var controller = new UserController(respositoryStub.Object, activitySourceStub);

            //Act
            var result = await controller.CreateUserAsync(new RegisterDto());

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetUserKeyAsync_UserKeyIsNull_ReturnsNotFound(){
            //Arrange
            respositoryStub.Setup(repo => repo.GetUserKeyAsync(It.IsAny<LoginDto>()))
                            .ReturnsAsync((string)null);
            
            var controller = new UserController(respositoryStub.Object, activitySourceStub);
            
            //Act
            var result = await controller.GetUserKeyAsync(new LoginDto());

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetUserKeyAsync_UserKeyIsValid_ReturnsOk(){
            //Arrange
            respositoryStub.Setup(repo => repo.GetUserKeyAsync(It.IsAny<LoginDto>()))
                            .ReturnsAsync("GWlffWBdf6y3gibvzbZl");
            
            var controller = new UserController(respositoryStub.Object, activitySourceStub);
            
            //Act
            var result = await controller.GetUserKeyAsync(new LoginDto());

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}