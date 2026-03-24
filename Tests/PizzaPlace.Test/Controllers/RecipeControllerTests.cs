using PizzaPlace;
using PizzaPlace.Controllers;
using PizzaPlace.Models.Types;
using PizzaPlace.Models;
using PizzaPlace.Services;
using Microsoft.AspNetCore.Mvc;

namespace PizzaPlace.Test.Controllers
{
    [TestClass]
    public class RecipeControllerTests
    {
        private static IRecipeController GetController(IRecipeService service) => new RecipeController(service);


        [TestMethod]
        public async Task TestPostRecipe() 
        {
            // Arrange
            var serviceMock = new Mock<IRecipeService>();
            var controller = GetController(serviceMock.Object);
            var recipeDto = new PizzaRecipeDto(PizzaRecipeType.HorseRadishPizza, new ComparableList<StockDto> { new(StockType.Tomatoes, 10) }, 15);

            // Act
            var result = await controller.PostRecipe(recipeDto);
            var createdResult = (CreatedResult)result;

            // Assert
            if (result is CreatedResult { Value: PizzaRecipeDto dto })        
            Assert.AreEqual(PizzaRecipeType.HorseRadishPizza, dto.RecipeType);   
        }

        [TestMethod]
        public async Task TestPostRecipe_ExcceptionCases()
        {
            // Arrange
            var serviceMock = new Mock<IRecipeService>();
            var recipeDto = new PizzaRecipeDto(PizzaRecipeType.HorseRadishPizza, new ComparableList<StockDto> { new(StockType.Tomatoes, 10) }, 15);
            serviceMock.Setup(x => x.PostRecipe(recipeDto))
                .Returns(Task.FromResult<IActionResult>(new BadRequestObjectResult("Bad request")));
            var controller = GetController(serviceMock.Object);
            var recipeDto2 = new PizzaRecipeDto(PizzaRecipeType.HorseRadishPizza, new ComparableList<StockDto> { new(StockType.Tomatoes, 10) }, 15);

            // Act
            var result = await controller.PostRecipe(recipeDto);            
            var resultObject = (BadRequestObjectResult)result;
            var errorMessage = resultObject.Value?.ToString();

            // Assert
            if (result is BadRequestObjectResult)   
            Assert.IsTrue(errorMessage.Contains("Bad request"));
        }
    }
}

