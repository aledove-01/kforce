using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using kforceApp.Contexts;
using kforceApp.Controllers;
using kforceApp.Models;
using kforceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TkforceTest
{
    [TestClass]
    

    public class TestCar
	{

        private List<Car> _cars;

        public TestCar()
		{
            _cars = new List<Car>
            {
                new Car { Id = 1, Make = "Audi", Model = "R8", Year = 2018, Doors = 2, Color = "Red", Price = 79995 },
                new Car { Id = 2, Make = "Tesla", Model = "3", Year = 2018, Doors = 4, Color = "Black", Price = 54995 },
                new Car { Id = 3, Make = "Porsche", Model = " 911 991", Year = 2020, Doors = 2, Color = "White", Price = 155000 },
                new Car { Id = 4, Make = "Mercedes-Benz", Model = "GLE 63S", Year = 2021, Doors = 5, Color = "Blue", Price = 83995 },
                new Car { Id = 5, Make = "BMW", Model = "X6 M", Year = 2020, Doors = 5, Color = "Silver", Price = 62995 }
            };

        }

        private Mock<DbSet<Car>>  createMockCars()
        {
            

            var carSetMock = new Mock<DbSet<Car>>();
            carSetMock.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(_cars.AsQueryable().Provider);
            carSetMock.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(_cars.AsQueryable().Expression);
            carSetMock.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(_cars.AsQueryable().ElementType);
            carSetMock.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(() => _cars.AsQueryable().GetEnumerator());

            return carSetMock;
        }
        [TestMethod]
        public async Task GetCars()
        {
            var contextMock = new Mock<ICarRepository>();
            var expectedResult = new Result<IEnumerable<Car>> { Data = createMockCars().Object.ToList(), Success=true };

            var mockRepo = new Mock<ICarRepository>();
            mockRepo.Setup(repo => repo.GetCarsWhitOutIdAsync()).ReturnsAsync(expectedResult);
            var controller = new CarController(null,mockRepo.Object);

            // Act
            var result = await controller.GetCarsWhitOutId();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var actualResult = okResult.Value as IEnumerable<Car>;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult.Data.Count(), actualResult.Count());

        }

        [TestMethod]
        public async Task AddCar()
        {
            // Arrange
            var contextMock = new Mock<ICarRepository>();
            var carToAdd = _cars[0]; //new Car { Id = 4, Make = "Toyota", Model = "Camry", Year = 2020, Price = 25000 };
            var expectedResult = new Result<Car> { Data = carToAdd, Success = true };

            var mockRepo = new Mock<ICarRepository>();
            mockRepo.Setup(repo => repo.AddAsync(carToAdd)).ReturnsAsync(expectedResult);
            var controller = new CarController(null, mockRepo.Object);

            // Act
            var result = await controller.AddCar(carToAdd);

            // Assert
            var okResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(okResult);
            var actualResult = okResult.Value as Car;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(201, okResult.StatusCode);
            Assert.AreEqual(expectedResult.Data.Id, actualResult.Id);
            Assert.AreEqual(expectedResult.Data.Make, actualResult.Make);
            Assert.AreEqual(expectedResult.Data.Model, actualResult.Model);
            Assert.AreEqual(expectedResult.Data.Year, actualResult.Year);
            Assert.AreEqual(expectedResult.Data.Price, actualResult.Price);
        }

        [TestMethod]
        public async Task UpdateCar()
        {
            // Arrange
            var contextMock = new Mock<ICarRepository>();
            var carToUpdate = _cars[0];
            var expectedResult = new Result<Car> { Data = carToUpdate, Success = true };

            var mockRepo = new Mock<ICarRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(carToUpdate)).ReturnsAsync(expectedResult);
            var controller = new CarController(null, mockRepo.Object);

            // Act
            var result = await controller.UpdateCar(carToUpdate);

            // Assert
            var okResult = result as NoContentResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(204, okResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteCar()
        {
            // Arrange
            var carIdToDelete = 1;
            var expectedResult = new Result<Car> { Data = _cars[0], Success = true };

            var mockRepo = new Mock<ICarRepository>();
            mockRepo.Setup(repo => repo.DeleteAsync(carIdToDelete)).ReturnsAsync(expectedResult);
            var controller = new CarController(null, mockRepo.Object);

            // Act
            var result = await controller.DeleteCar(carIdToDelete);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var actualResult = okResult.Value as Car;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedResult.Data.Id, actualResult.Id);
            Assert.AreEqual(expectedResult.Data.Make, actualResult.Make);
            Assert.AreEqual(expectedResult.Data.Model, actualResult.Model);
            Assert.AreEqual(expectedResult.Data.Year, actualResult.Year);
            Assert.AreEqual(expectedResult.Data.Price, actualResult.Price);
        }

        [TestMethod]
        public async Task GetCar_ReturnsCorrectCar()
        {
            // Arrange
            var carId = 1;
            var expectedCar = _cars.First(c => c.Id == carId);
            var expectedResult = new Result<Car> { Data = expectedCar, Success = true };

            var mockRepo = new Mock<ICarRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(carId)).ReturnsAsync(expectedResult);
            var controller = new CarController(null, mockRepo.Object);

            // Act
            var result = await controller.GetCar(carId);

            // Assert
            var actualResult = result.Value as Car;
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedCar.Id, actualResult.Id);
            Assert.AreEqual(expectedCar.Make, actualResult.Make);
            Assert.AreEqual(expectedCar.Model, actualResult.Model);
            Assert.AreEqual(expectedCar.Year, actualResult.Year);
            Assert.AreEqual(expectedCar.Price, actualResult.Price);
        }

        [TestMethod]
        public async Task CheckPrice_ReturnsOkIfPriceInExpectedRangeSamePrice()
        {
            // Arrange
            var car = _cars[0];
            var mockRepo = new Mock<ICarRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(car.Id)).ReturnsAsync(new Result<Car> { Data = car, Success = true });
            var controller = new CarController(null, mockRepo.Object);

            // Act
            var result = await controller.CheckPrice(car.Id, car.Price, 5000); 

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task CheckPrice_ReturnsOkIfPriceInExpectedRange()
        {
            // Arrange
            var car = _cars[0];
            var mockRepo = new Mock<ICarRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(car.Id)).ReturnsAsync(new Result<Car> { Data = car, Success = true });
            var controller = new CarController(null, mockRepo.Object);

            // Act
            var result = await controller.CheckPrice(car.Id, (car.Price-4000), 5000);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task CheckPrice_ReturnsOkIfPriceNotInExpectedRange()
        {
            // Arrange
            var car = _cars[0];
            var mockRepo = new Mock<ICarRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(car.Id)).ReturnsAsync(new Result<Car> { Data = car, Success = true });
            var controller = new CarController(null, mockRepo.Object);

            // Act
            var result = await controller.CheckPrice(car.Id, (car.Price + 5001), 5000);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsNull(okResult.Value);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}

