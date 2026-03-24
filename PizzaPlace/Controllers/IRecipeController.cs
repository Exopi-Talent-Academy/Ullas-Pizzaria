using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;

namespace PizzaPlace.Controllers
{
    public interface IRecipeController
    {
        
        Task<IActionResult> PostRecipe(PizzaRecipeDto dto);
        Task<IActionResult> PutRecipe(PizzaRecipeDto updatedDto);
    }
}