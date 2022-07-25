using System.Security.Claims;
using System.Security.Principal;
using AutoMapper;
using CarShopAPI.Api.Controllers;
using CarShopAPI.Api.Data;
using CarShopAPI.Api.Dtos;
using CarShopAPI.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Catalog.UnitTests{

    public class UserControllerTests{

        public Mock<IAppRepo> RepositoryStub { get; set; } = new Mock<IAppRepo>();

        public Mock<IMapper> MapperStub { get; set; } = new Mock<IMapper>();

        [Fact]
        public async Task NewCarAsync_CarModelIsValid_ReturnsOk(){
            //Arrange
            RepositoryStub.Setup(repo => repo.SaveChangesAsync())
                            .ReturnsAsync(true); 

            MapperStub.Setup(mapper => mapper.Map<CarModel>(It.IsAny<CreateCarModel>()))
                        .Returns(RandomCar());

            var controller = new UserController(RepositoryStub.Object, MapperStub.Object){
                ControllerContext = GetControllerContext()
            };

            //Act
            var result = await controller.NewCarAsync(new CreateCarModel()); 

            //Assert
            result.Should().BeOfType<OkObjectResult>();           
        }

        [Fact]
        public void CarsForSale_Valid_ReturnsOk(){
            //Arrange
            var cars = new[]{RandomCar(), RandomCar(), RandomCar()};
            RepositoryStub.Setup(repo => repo.CarsByUserId(It.IsAny<int>()))
                            .Returns(cars); 

            var controller = new UserController(RepositoryStub.Object, MapperStub.Object){
                ControllerContext = GetControllerContext()
            };

            //Act
            var result = controller.CarsForSale(); 

            //Assert
            var actual = (result.Result as ObjectResult).Value as IEnumerable<CarModel>;
            actual.Should().BeEquivalentTo(
                cars,
                options => options.ComparingByMembers<CarModel>());     
        }

        [Fact]
        public void Search_Valid_ReturnsOk(){
            //Arrange
            var cars = new[]{RandomCar(), RandomCar(), RandomCar()};
            RepositoryStub.Setup(repo => repo.CarsWithSettings(It.IsAny<SearchSettings>()))
                            .Returns(cars); 

            var controller = new UserController(RepositoryStub.Object, MapperStub.Object);

            //Act
            var result = controller.Search(new SearchSettings()); 

            //Assert
            var actual = (result.Result as ObjectResult).Value as IEnumerable<CarModel>;
            actual.Should().BeEquivalentTo(
                cars,
                options => options.ComparingByMembers<CarModel>());     
        }

        [Fact]
        public async Task BuyCarAsync_CarModelIsNull_ReturnsNotFound(){
            //Arrange
            RepositoryStub.Setup(repo => repo.BuyCarAsync(It.IsAny<int>(), It.IsAny<int>()))
                            .ReturnsAsync((CarModel)null); 

            var controller = new UserController(RepositoryStub.Object, MapperStub.Object){
                ControllerContext = GetControllerContext()
            };

            //Act
            var result = await controller.BuyCarAsync(new int()); 

            //Assert
            result.Should().BeOfType<NotFoundResult>();           
        }

        [Fact]
        public async Task BuyCarAsync_CarModelIsValid_ReturnsOk(){
            //Arrange
            RepositoryStub.Setup(repo => repo.BuyCarAsync(It.IsAny<int>(), It.IsAny<int>()))
                            .ReturnsAsync(RandomCar()); 

            var controller = new UserController(RepositoryStub.Object, MapperStub.Object){
                ControllerContext = GetControllerContext()
            };

            //Act
            var result = await controller.BuyCarAsync(new int()); 

            //Assert
            result.Should().BeOfType<OkObjectResult>();           
        }

        [Fact]
        public async Task DeleteUserAsync_UserModelIsValid_ReturnsOk(){
            //Arrange
            RepositoryStub.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync(new UserModel()); 

            var controller = new UserController(RepositoryStub.Object, MapperStub.Object);

            //Act
            var result = await controller.DeleteUserAsync(new int()); 

            //Assert
            result.Should().BeOfType<OkObjectResult>();           
        }

        [Fact]
        public async Task DeleteUserAsync_UserModelIsNull_ReturnsNotFound(){
            //Arrange
            RepositoryStub.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((UserModel)null); 

            var controller = new UserController(RepositoryStub.Object, MapperStub.Object);

            //Act
            var result = await controller.DeleteUserAsync(new int()); 

            //Assert
            result.Should().BeOfType<NotFoundResult>();           
        }

        [Fact]
        public async Task DeleteCarAsync_CarModelIsValid_ReturnsOk(){
            //Arrange
            RepositoryStub.Setup(repo => repo.GetCarByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync(RandomCar());

            var controller = new UserController(RepositoryStub.Object, MapperStub.Object);

            //Act
            var result = await controller.DeleteCarAsync(new int()); 

            //Assert
            result.Should().BeOfType<OkObjectResult>();           
        }

        [Fact]
        public async Task DeleteCarAsync_UserModelIsNull_ReturnsNotFound(){
            //Arrange
            RepositoryStub.Setup(repo => repo.GetCarByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((CarModel)null); 

            var controller = new UserController(RepositoryStub.Object, MapperStub.Object);

            //Act
            var result = await controller.DeleteCarAsync(new int()); 

            //Assert
            result.Should().BeOfType<NotFoundResult>();           
        }

        private CarModel RandomCar(){
            Random rand = new Random();
            var car = new CarModel(){
                Id = rand.Next(1, 100),
                SellerId = rand.Next(1, 100),
                BuyerId = rand.Next(1, 100),
                Status = Guid.NewGuid().ToString(),
                Make = Guid.NewGuid().ToString(),
                Model = Guid.NewGuid().ToString(),
                Price = rand.Next(1, 100),
                ManufactureYear = rand.Next(2000, 2022)
            };

            return car;
        }

        private ControllerContext GetControllerContext(){
            var identity = new GenericIdentity("some name", "test");
            var contextUser = new ClaimsPrincipal(identity); 
            var httpContext = new DefaultHttpContext() {
                User = contextUser
            };
            var controllerContext = new ControllerContext() {
                HttpContext = httpContext,
            };

            return controllerContext;
        }
    }
}