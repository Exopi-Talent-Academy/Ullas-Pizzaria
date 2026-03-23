using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services
{
    [TestClass]
    public class RestockingServiceTests
    {       
        private static IRestockingService GetService(Mock<IRestockingRepository> restockingRepoMock) =>
            new RestockingService(restockingRepoMock.Object);
                          

        [TestMethod]
        [DataRow(StockType.Tomatoes, 1, StockType.Bacon, 4)]
        [DataRow(StockType.Anchovies, 4, StockType.UngenericSpices, 4)]
        public async Task TestRestock(StockType type1, int type1Amount, StockType Type2, int type2Amount) 
        {
            // Arrange
            Mock<IRestockingRepository> restockingRepoMock = new();
            ComparableList<StockDto> orderedStocks =
                [
                    new(type1, 50),
                    new(Type2, 50)
                ];
            restockingRepoMock.Setup(x => x.GetStocksAsync(It.IsAny<ComparableList<StockDto>>()))
                .ReturnsAsync(orderedStocks);    
            var service = GetService(restockingRepoMock);

            ComparableList<StockDto> stocksToReorder =
                [
                    new(type1, type1Amount),
                    new(Type2, type2Amount)
                ];

            // Act
            var actual = await service.Restock(stocksToReorder);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(ComparableList<StockDto>));
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(50, actual.First().Amount);
        }


        [TestMethod]
        [DataRow(StockType.Tomatoes, 1, StockType.Bacon, 4)]
        [DataRow(StockType.Anchovies, 4, StockType.UngenericSpices, 4)]
        public async Task TestRestock_ExceptionCases(StockType type1, int type1Amount, StockType Type2, int type2Amount)
        {
            // Arrange
            Mock<IRestockingRepository> restockingRepoMock = new Mock<IRestockingRepository>();
            restockingRepoMock.Setup(x => x.GetStocksAsync(It.IsAny<ComparableList<StockDto>>()))
                .Throws(new InvalidOperationException("Order problems. New Amount only: 5"));

            var service = GetService(restockingRepoMock);

            ComparableList<StockDto> stocksToReorder =
                [
                    new(type1, type1Amount),
                    new(Type2, type2Amount)
                ];

            // Act and Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await service.Restock(stocksToReorder);
            });
            Assert.IsTrue(exception.Message.Contains("Order problems"));

        }    
    }
}
