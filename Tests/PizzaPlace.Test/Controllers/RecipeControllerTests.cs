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
            
            // Act
            var result = await controller.PostRecipe(recipeDto);            
            var resultObject = (BadRequestObjectResult)result;
            var errorMessage = resultObject.Value?.ToString();

            // Assert
            if (result is BadRequestObjectResult)   
            Assert.IsTrue(errorMessage.Contains("Bad request"));
        }

        [TestMethod]
        public async Task TestPutRecipe()
        {
            // Arrange
            var serviceMock = new Mock<IRecipeService>();
            var updatedDto = new PizzaRecipeDto(PizzaRecipeType.HorseRadishPizza, new ComparableList<StockDto> { new(StockType.Tomatoes, 10) }, 20, 1);
            serviceMock.Setup(x => x.PutRecipe(It.IsAny<PizzaRecipeDto>()))
                .Returns(Task.FromResult<IActionResult>(new OkObjectResult(updatedDto)));
            var controller = GetController(serviceMock.Object);
            
            // Act
            var result = await controller.PutRecipe(updatedDto);
            var updatedResult = (OkObjectResult)result;  
            var dto = (PizzaRecipeDto)updatedResult.Value;

            // Assert
            Assert.AreEqual(20, dto.CookingTimeMinutes);
        }

        [TestMethod]
        public async Task TestPutRecipe_ExceptionCases() 
        {
            // Arrange
            var serviceMock = new Mock<IRecipeService>();
            var updatedDto = new PizzaRecipeDto(PizzaRecipeType.HorseRadishPizza, new ComparableList<StockDto> { new(StockType.Tomatoes, 10) }, 20, 1);
            serviceMock.Setup(x => x.PutRecipe(It.IsAny<PizzaRecipeDto>()))
                .Returns(Task.FromResult<IActionResult>(new BadRequestObjectResult($"Could not update {updatedDto}")));
            var controller = GetController(serviceMock.Object);

            // Act
            var result = await controller.PutRecipe(updatedDto);
            var resultObject = (BadRequestObjectResult)result;
            var message = resultObject.Value?.ToString();

            // Assert
            Assert.IsTrue(message.Contains("Could not update"));
        }
    }
}

