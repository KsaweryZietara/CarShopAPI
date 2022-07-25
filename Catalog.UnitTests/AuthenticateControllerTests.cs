using System;
using CarShopAPI.Api.Controllers;
using CarShopAPI.Api.Data;
using CarShopAPI.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using FluentAssertions;
using CarShopAPI.Api.Dtos;
using System.Security.Claims;

namespace Catalog.UnitTests{

    public class AuthenticateControllerTests{
        [Fact]
        public async Task RegisterUserAsync_UserModelIsNull_ReturnsBadRequest(){     
            //Arrange
            var repositoryStub = new Mock<IAppRepo>(); 
            repositoryStub.Setup(repo => repo.CreateUserAsync(It.IsAny<UserModel>()))
                            .ReturnsAsync((UserModel)null); 

            var configurationStub = new Mock<IConfiguration>();

            var controller = new AuthenticateController(repositoryStub.Object, configurationStub.Object);

            //Act
            var result = await controller.RegisterUserAsync(new UserModel()); 

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task RegisterUserAsync_UserModelIsValid_ReturnsOk(){     
            //Arrange
            var repositoryStub = new Mock<IAppRepo>(); 
            repositoryStub.Setup(repo => repo.CreateUserAsync(It.IsAny<UserModel>()))
                            .ReturnsAsync(new UserModel()); 

            repositoryStub.Setup(repo => repo.SaveChangesAsync())
                            .ReturnsAsync(true);

            var configurationStub = new Mock<IConfiguration>();

            var controller = new AuthenticateController(repositoryStub.Object, configurationStub.Object);

            //Act
            var result = await controller.RegisterUserAsync(new UserModel(){
                Id = 1,
                Username = "test",
                Password = "test",
                EmailAddress = "test",
                Role = "buyer"
            }); 

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RegisterUserAsync_UserModelWithInvalidRole_ReturnsBadResult(){     
            //Arrange
            var repositoryStub = new Mock<IAppRepo>(); 
            repositoryStub.Setup(repo => repo.CreateUserAsync(It.IsAny<UserModel>()))
                            .ReturnsAsync(new UserModel()); 

            repositoryStub.Setup(repo => repo.SaveChangesAsync())
                            .ReturnsAsync(true);

            var configurationStub = new Mock<IConfiguration>();

            var controller = new AuthenticateController(repositoryStub.Object, configurationStub.Object);

            //Act
            var result = await controller.RegisterUserAsync(new UserModel(){
                Id = 1,
                Username = "test",
                Password = "test",
                EmailAddress = "test",
                Role = "InvalidRole"
            }); 

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void LoginUser_UserModelIsNull_ReturnsUnauthorized(){     
            //Arrange
            var repositoryStub = new Mock<IAppRepo>(); 
            repositoryStub.Setup(repo => repo.FindUser(It.IsAny<LoginModel>()))
                            .Returns((UserModel)null); 

            var configurationStub = new Mock<IConfiguration>();

            var controller = new AuthenticateController(repositoryStub.Object, configurationStub.Object);

            //Act
            var result = controller.LoginUser(new LoginModel()); 

            //Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void LoginUser_UserModelIsValid_ReturnsOk(){     
            //Arrange
            var repositoryStub = new Mock<IAppRepo>(); 
            
            repositoryStub.Setup(repo => repo.FindUser(It.IsAny<LoginModel>()))
                            .Returns(new UserModel(){
                                Id = 1,
                                Username = "test",
                                Password = "test",
                                EmailAddress = "test",
                                Role = "buyer"
                            }); 

            var configurationStub = new Mock<IConfiguration>();

            configurationStub.SetupGet(p => p[It.IsAny<string>()]).Returns("testtesttesttesttesttesttesttest");

            var controller = new AuthenticateController(repositoryStub.Object, configurationStub.Object);

            //Act
            var result = controller.LoginUser(new LoginModel()); 

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}