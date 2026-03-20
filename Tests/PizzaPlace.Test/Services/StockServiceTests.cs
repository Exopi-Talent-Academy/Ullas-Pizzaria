using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class StockServiceTests
{
    [TestMethod]
    public async Task TestCalculateRequiredStock()
    {
        // Arrange
        var mockStockRepo = new Mock<IStockRepository>();
        var mockRecipeService = new Mock<IRecipeService>();
        var service = new StockService(mockStockRepo.Object, mockRecipeService.Object);

        ComparableList<StockDto> ingredients1 =
        [
            new StockDto(StockType.Tomatoes, 3 ),
            new StockDto(StockType.Bacon, 4)
        ];
        ComparableList<StockDto> ingredients2 = [
            new StockDto(StockType.Tomatoes, 3 ),
            new StockDto(StockType.Bacon, 4)
        ];

        ComparableList<PizzaRecipeDto> pizzaRecipeDtos = [
        new PizzaRecipeDto(PizzaRecipeType.RarePizza, ingredients1, 15, 1),
        new PizzaRecipeDto(PizzaRecipeType.EmptyPizza, ingredients2, 15, 2)
        ];

        PizzaOrder order = new PizzaOrder([
        new PizzaAmount(PizzaRecipeType.RarePizza, 2),
        new PizzaAmount(PizzaRecipeType.EmptyPizza, 3)
        ]);

        // Act
        ComparableList<StockDto> result = await service.CalculateRequiredStock(order, pizzaRecipeDtos);

        // Assert
        Assert.AreEqual(StockType.Tomatoes, result.First().StockType);
        Assert.AreEqual(15, result.First().Amount);
        Assert.AreEqual(StockType.Bacon, result.Skip(1).First().StockType);
        Assert.AreEqual(20, result.Skip(1).First().Amount);
    }


    [TestMethod]
    public async Task TestCalculateRequiredStock_ExceptionCases() 
    {
        // Arrange
        var mockStockRepo = new Mock<IStockRepository>();
        var mockRecipeService = new Mock<IRecipeService>();
        var service = new StockService(mockStockRepo.Object, mockRecipeService.Object);
               
        PizzaOrder order = new PizzaOrder([
        new PizzaAmount(PizzaRecipeType.RarePizza, 2)       
        ]);

        var pizzaRecipeDtos = new ComparableList<PizzaRecipeDto>();

        // Act and Assert

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            async () => await service.CalculateRequiredStock(order, pizzaRecipeDtos));
    }





    [TestMethod]
    public async Task TestGetAllStock()
    {
        // Arrange
        var allStock = new ComparableList<StockDto>() {
            new (StockType.Tomatoes, 20),
            new (StockType.Bacon, 25)
        };

        var mockStockRepo = new Mock<IStockRepository>();
        mockStockRepo.Setup(m => m.GetAllStockAsync())
            .ReturnsAsync(allStock);

        var mockRecipeService = new Mock<IRecipeService>();
        var service = new StockService(mockStockRepo.Object, mockRecipeService.Object);

        // Act
        ComparableList<StockDto> result = await service.GetAllStockAsync();

        // Assert
        Assert.AreEqual(result.First().StockType, StockType.Tomatoes);
        Assert.AreEqual(result.First().Amount, 20);
        Assert.AreEqual(result.Skip(1).First().StockType, StockType.Bacon);
        Assert.AreEqual(result.Skip(1).First().Amount, 25);
    }



    [TestMethod]
    public async Task TestHasSufficientStockForOrder()
    {
        // Arrange
        var allStock = new ComparableList<StockDto>() {
                new (StockType.Tomatoes, 20),
                new (StockType.Bacon, 25),
                new (StockType.BellPeppers, 30)
            };

        var mockStockRepo = new Mock<IStockRepository>();
        mockStockRepo.Setup(m => m.GetAllStockAsync())
            .ReturnsAsync(allStock);

        var mockRecipeService = new Mock<IRecipeService>();
        var service = new StockService(mockStockRepo.Object, mockRecipeService.Object);

        PizzaOrder order = new([
            new PizzaAmount(PizzaRecipeType.RarePizza, 2),
            new PizzaAmount(PizzaRecipeType.EmptyPizza, 3)
            ]);

        ComparableList<StockDto> ingredients1 = [
            new (StockType.Tomatoes, 3 ),
            new (StockType.Bacon, 4)
            ];
            
        ComparableList<StockDto> ingredients2 = [
                new StockDto(StockType.Tomatoes, 3 ),
                new StockDto(StockType.Bacon, 4)
            ];

        ComparableList<PizzaRecipeDto> pizzaRecipeDtos = [
            new (PizzaRecipeType.RarePizza, ingredients1, 15, 1),
            new (PizzaRecipeType.EmptyPizza, ingredients2, 15, 2)
            ];

        // Act
        bool result = await service.HasSufficientStockForOrder(order, pizzaRecipeDtos);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow(2, 6, StockType.Tomatoes, null)]
    [DataRow(6, 2, null, StockType.Bacon)]
    public async Task TestReturnStocksForReordering(int tomatoCount, int baconCount, StockType? expStockType1, StockType? expStockType2) 
    {
        // Arrange
        var stockRepoMock = new Mock<IStockRepository>();
        stockRepoMock.Setup(x => x.GetAllStockAsync())
        .ReturnsAsync(
        [
            new(StockType.Tomatoes, tomatoCount),
            new(StockType.Bacon, baconCount)
        ]);       

        var recipeServiceMock = new Mock<IRecipeService>();
        var service = new StockService(stockRepoMock.Object, recipeServiceMock.Object);

        // Act
        var result = await service.ReturnStocksForReordering();

        // Assert
       if(expStockType1.HasValue)
        Assert.IsTrue(result.Contains(expStockType1.Value));
       if (expStockType2.HasValue)
        Assert.IsTrue(result.Contains(expStockType2.Value));
    }
}
