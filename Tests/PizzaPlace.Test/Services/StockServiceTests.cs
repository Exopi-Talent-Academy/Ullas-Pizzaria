using Moq;
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

        ComparableList<StockDto> ingredients1 = new ComparableList<StockDto>()
        {
            new StockDto(StockType.Tomatoes, 3 ),
            new StockDto(StockType.Bacon, 4)
        };
        ComparableList<StockDto> ingredients2 = new ComparableList<StockDto>()
        {
            new StockDto(StockType.Tomatoes, 3 ),
            new StockDto(StockType.Bacon, 4)
        };

        ComparableList<PizzaRecipeDto> pizzaRecipeDtos = new ComparableList<PizzaRecipeDto>() {
        new PizzaRecipeDto(PizzaRecipeType.RarePizza, ingredients1, 15, 1),
        new PizzaRecipeDto(PizzaRecipeType.EmptyPizza, ingredients2, 15, 2)
        };

        PizzaOrder order = new PizzaOrder(new ComparableList<PizzaAmount>() {
        new PizzaAmount(PizzaRecipeType.RarePizza, 2),
        new PizzaAmount(PizzaRecipeType.EmptyPizza, 3)
        });

        // Act
        ComparableList<StockDto> result = await service.CalculateRequiredStock(order, pizzaRecipeDtos);

        // Assert
        Assert.AreEqual(StockType.Tomatoes, result.First().StockType);
        Assert.AreEqual(15, result.First().Amount);
        Assert.AreEqual(StockType.Bacon, result.Skip(1).First().StockType);
        Assert.AreEqual(20, result.Skip(1).First().Amount);

    }

    [TestMethod]
    public async Task TestGetAllStock()
    {
        // Arrange
        var allStock = new ComparableList<StockDto>() {
            new StockDto(StockType.Tomatoes, 20),
            new StockDto(StockType.Bacon, 25)
        };

        var mockStockRepo = new Mock<IStockRepository>();
        mockStockRepo.Setup(m => m.GetAllStock())
            .ReturnsAsync(allStock);

        var mockRecipeService = new Mock<IRecipeService>();
        var service = new StockService(mockStockRepo.Object, mockRecipeService.Object);

        // Act
        ComparableList<StockDto> result = await service.GetAllStock();


        // Assert
        Assert.AreEqual(result.First().StockType, StockType.Tomatoes);
        Assert.AreEqual(result.First().Amount, 20);
        Assert.AreEqual(result.Skip(1).First().StockType, StockType.Bacon);
        Assert.AreEqual(result.Skip(1).First().Amount, 25);
    }



    [TestMethod]
    public async Task TestHasInsufficientStock()
    {
        // Arrange
        var allStock = new ComparableList<StockDto>() {
                new StockDto(StockType.Tomatoes, 20),
                new StockDto(StockType.Bacon, 25),
                new StockDto (StockType.BellPeppers, 30)
            };

        var mockStockRepo = new Mock<IStockRepository>();
        //mockStockRepo.Setup(m => m.GetAllStock())
        //    .ReturnsAsync(allStock);

        var mockRecipeService = new Mock<IRecipeService>();
        var service = new StockService(mockStockRepo.Object, mockRecipeService.Object);

        PizzaOrder order = new PizzaOrder(new ComparableList<PizzaAmount>() {
            new PizzaAmount(PizzaRecipeType.RarePizza, 2),
            new PizzaAmount(PizzaRecipeType.EmptyPizza, 3)
            });

        ComparableList<StockDto> ingredients1 = new ComparableList<StockDto>()
            {
                new StockDto(StockType.Tomatoes, 3 ),
                new StockDto(StockType.Bacon, 4)
            };
        ComparableList<StockDto> ingredients2 = new ComparableList<StockDto>()
            {
                new StockDto(StockType.Tomatoes, 3 ),
                new StockDto(StockType.Bacon, 4)
            };
        ComparableList<PizzaRecipeDto> pizzaRecipeDtos = new ComparableList<PizzaRecipeDto>() {
            new PizzaRecipeDto(PizzaRecipeType.RarePizza, ingredients1, 15, 1),
            new PizzaRecipeDto(PizzaRecipeType.EmptyPizza, ingredients2, 15, 2)
            };

        // Act
        bool result = await service.HasInsufficientStock(order, pizzaRecipeDtos);

        // Assert
        Assert.IsTrue(result);
    }

}
