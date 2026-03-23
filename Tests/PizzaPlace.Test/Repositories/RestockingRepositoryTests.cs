using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Repositories
{
  

    [TestClass]
    public class RestockingRepositoryTests
    {
        ComparableList<StockDto> externalWarehouse =
            [
                new(StockType.Bacon, 100),
                new(StockType.Anchovies, 100),
                new(StockType.Chocolate, 100),
                new(StockType.BellPeppers, 2)
            ];

        [TestMethod]
        [DataRow(StockType.Tomatoes, 1, StockType.Bacon, 4, StockType.BellPeppers, 3)]
        public async Task TestGetStocks(StockType stockType1, int stockType1Amount, StockType stockType2, int stockType2Amount, StockType stockType3, int stockType3Amount)
        
        {
            // Arrange
            ComparableList<StockDto> stocksToReorder = [
                new(stockType1, stockType1Amount),
                new(stockType2, stockType2Amount),
                new(stockType3, stockType3Amount)
                ];

            RestockingRepository repo = new RestockingRepository();

            // Act
            var result = await repo.GetStocksAsync(stocksToReorder);

            // Assert
            Assert.IsInstanceOfType<ComparableList<StockDto>>(result);
            Assert.AreEqual(50, result.First().Amount);
            Assert.AreEqual(50, result.Skip(1).First().Amount);
            Assert.AreEqual(50, result.Skip(2).First().Amount);
        }

        [TestMethod]
        [DataRow(StockType.Tomatoes, 1, StockType.Bacon, 4, StockType.BellPeppers, 3)]
        public async Task TestGetStocks_ExceptionCases(StockType stockType1, int stockType1Amount, StockType stockType2, int stockType2Amount, StockType stockType3, int stockType3Amount)

        {
            // Arrange
            ComparableList<StockDto> stocksToReorder = [
                new(stockType1, stockType1Amount),
                new(stockType2, stockType2Amount),
                new(stockType3, stockType3Amount)
                ];

            RestockingRepository repo = new RestockingRepository();
            repo.ExternalWarehouse = [
                new(StockType.Bacon, 10),
                new(StockType.Tomatoes, 10),
                new(StockType.Chocolate, 10),
                new(StockType.BellPeppers, 10)
            ];

            // Act And Assert
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await repo.GetStocksAsync(stocksToReorder));
            Assert.IsTrue(ex.Message.Contains("Order problems"));
            

        }

    }
}
