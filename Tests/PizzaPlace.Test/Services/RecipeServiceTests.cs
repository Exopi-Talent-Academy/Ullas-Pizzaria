using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class RecipeServiceTests
{
    private static RecipeService GetService(Mock<IRecipeRepository> recipeRepository) =>
        new(recipeRepository.Object);

    [TestMethod]
    public async Task GetPizzaRecipes()
    {
        // Arrange
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, 1),
            new PizzaAmount(PizzaRecipeType.OddPizza, 2),
            new PizzaAmount(PizzaRecipeType.RarePizza, 20),
        ]);
        var rareRecipe = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 1);
        var oddRecipe = new PizzaRecipeDto(PizzaRecipeType.OddPizza, [new StockDto(StockType.Sulphur, 10)], 100);
        ComparableList<PizzaRecipeDto> expected = [rareRecipe, oddRecipe];

        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.RarePizza))
            .ReturnsAsync(rareRecipe);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.OddPizza))
            .ReturnsAsync(oddRecipe);

        var service = GetService(recipeRepository);

        // Act
        var actual = await service.GetPizzaRecipes(order);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task TestPostRecipe() 
    {
        // Arrange
        var dto = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 1);
        var createdDtoMock = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 1, 1);
        Mock<IRecipeRepository> recipeRepository = new Mock<IRecipeRepository>();
        recipeRepository.Setup(x => x.AddRecipe(dto))
            .ReturnsAsync(createdDtoMock); 
        var service = GetService(recipeRepository);

        // Act
        var result = await service.PostRecipe(dto);
        var createdResult = (CreatedResult)result;
        
        // Assert       
        Assert.IsNotNull(createdResult);
        if (createdResult.Value is PizzaRecipeDto createdDto) 
        {
            Assert.AreNotEqual(0, createdDto.Id);
            Assert.AreEqual(dto.RecipeType, createdDto.RecipeType);
            Assert.AreEqual(dto.Ingredients[0].StockType, createdDto.Ingredients[0].StockType);
        }
    }

    [TestMethod]
    public async Task TestPutRecipe() 
    {
        // Arrange
        var updatedDto = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 1, 1);
        var updatedDtoMock = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 2, 1);
        Mock<IRecipeRepository> recipeRepository = new Mock<IRecipeRepository>();
        recipeRepository.Setup(x => x.UpdateRecipe(updatedDto))
            .ReturnsAsync(updatedDtoMock);
        var service = GetService(recipeRepository);

        // Act
        var result = await service.PutRecipe(updatedDto);
        var resultObject = (OkObjectResult)result;
        var resultDto = (PizzaRecipeDto)resultObject.Value;

        // Assert
        Assert.IsNotNull(resultDto);
        Assert.AreEqual(2, resultDto.CookingTimeMinutes);
    }

    [TestMethod]
    public async Task TestPutRecipe_ExceptionCases()
    {
        // Arrange
        var updatedDto = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 1, 1);
        //var updatedDtoMock = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 2, 1);
        Mock<IRecipeRepository> recipeRepository = new Mock<IRecipeRepository>();
        recipeRepository.Setup(x => x.UpdateRecipe(updatedDto))
            .ThrowsAsync(new Exception("Could not update"));
        var exMessage = "Could not update";
        var service = GetService(recipeRepository);

        // Act
        var result = await service.PutRecipe(updatedDto);
        var resultObject = (BadRequestObjectResult)result;
        var message = resultObject.Value.ToString(); 
        
        // Assert
        Assert.IsNotNull(resultObject);
        Assert.AreEqual(exMessage, message);
        
    }

}
