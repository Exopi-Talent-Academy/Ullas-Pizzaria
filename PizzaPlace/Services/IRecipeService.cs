using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;

namespace PizzaPlace.Services;

public interface IRecipeService
{
    Task<ComparableList<PizzaRecipeDto>> GetPizzaRecipes(PizzaOrder order);
    Task<IActionResult> PostRecipe(PizzaRecipeDto dto);
}
