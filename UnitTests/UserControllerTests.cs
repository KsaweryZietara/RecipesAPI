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
        [Fact]
        public async Task CreateUserAsync_CreatedUserIsNull_ReturnsBadRequest(){
            //Arrange
            var respositoryStub = new Mock<IAppRepo>();
            respositoryStub.Setup(repo => repo.CreateUserAsync(It.IsAny<RegisterDto>()))
                            .ReturnsAsync((User)null);
            
            var activitySourceStub = new ActivitySource("test");

            var controller = new UserController(respositoryStub.Object, activitySourceStub);

            //Act
            var result = await controller.CreateUserAsync(new RegisterDto());

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }   

        [Fact]
        public async Task CreateUserAsync_CreatedUserIsValid_ReturnsOk(){
            //Arrange
            var respositoryStub = new Mock<IAppRepo>();
            respositoryStub.Setup(repo => repo.CreateUserAsync(It.IsAny<RegisterDto>()))
                            .ReturnsAsync(new User(){
                                ApiKey = "GWlffWBdf6y3gibvzbZl",
                                Name = "TestName",
                                EmailAddress = "TestEmailAddress",
                                Password = "TestPassword"
                            });
            
            var activitySourceStub = new ActivitySource("test");

            var controller = new UserController(respositoryStub.Object, activitySourceStub);

            //Act
            var result = await controller.CreateUserAsync(new RegisterDto());

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }  
    }
}