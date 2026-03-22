using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Controllers
{
    [TestClass]
    public class RestockingControllerTests
    {       
        private static RestockingController GetController(Mock<IStockService> stockService) =>
            new RestockingController(stockService.Object);

        [TestMethod]
        [DataRow(StockType.Tomatoes, 1, StockType.Bacon, 4)]
        [DataRow(StockType.Anchovies, 4, StockType.UngenericSpices, 4)] 
        public async Task TestRestock(StockType type1, int type1Amount, StockType Type2, int type2Amount) 
        {
            // Arrange
            var stockService = new Mock<IStockService>();
            ComparableList<StockDto> orderedStocks =
                [
                    new(type1, 50),
                    new(Type2, 50)
                ];
            stockService.Setup(x =>  x.Restock(It.IsAny<ComparableList<StockDto>>()))
                .ReturnsAsync(orderedStocks);

            var controller = GetController(stockService);

            ComparableList<StockDto> stocksToReorder =
                [
                    new(type1, type1Amount),
                    new(Type2, type2Amount)
                ];

            // Act
            var actual = await controller.Restock(stocksToReorder);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<OkObjectResult>(actual);
            var okResult = actual as OkObjectResult;
            Assert.IsNotNull(okResult);
            var value = okResult.Value as ComparableList<StockDto>;
            Assert.IsNotNull(value);
            Assert.AreEqual(50, value.First().Amount);
        }

        [TestMethod]
        public async Task TestRestock_ExceptionCases() 
        { 
            // Arrange
            var mockService = new Mock<IStockService>();
            mockService.Setup(x => x.Restock(It.IsAny<ComparableList<StockDto>>()))
                .Throws(new InvalidOperationException("Order problems. New Amount only: 5"));
            var controller = GetController(mockService);
            ComparableList<StockDto> stocksToReorder =
                [
                    new(StockType.Sulphur, 2)
                ];
            

            // Act 
            var actual = await controller.Restock(stocksToReorder);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<BadRequestObjectResult>(actual);
            var badRequestResult = actual as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var message = badRequestResult.Value as string;
            Assert.IsTrue(message.Contains("Order problems"));
            
        }

    }
}
