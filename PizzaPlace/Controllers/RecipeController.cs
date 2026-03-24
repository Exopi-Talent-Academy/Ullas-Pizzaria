using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers
{
    [Route("api/recipe")]
    public class RecipeController(IRecipeService service) : ControllerBase, IRecipeController
    {
        [HttpPost]        
        public async Task<IActionResult> PostRecipe(PizzaRecipeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }
            try 
            {
                return await service.PostRecipe(dto);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }

    }
}
