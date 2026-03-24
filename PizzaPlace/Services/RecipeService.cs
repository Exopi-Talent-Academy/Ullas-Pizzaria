using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
{
    public async Task<ComparableList<PizzaRecipeDto>> GetPizzaRecipes(PizzaOrder order)
    {
        var pizzaTypes = order.RequestedOrder
            .Select(x => x.PizzaType)
            .Distinct()
            .ToList();

        ComparableList<PizzaRecipeDto> recipes = [];
        foreach (var pizzaType in pizzaTypes)
        {
            recipes.Add(await recipeRepository.GetRecipe(pizzaType));
        }

        return recipes;
    }

    public async Task<IActionResult> PostRecipe(PizzaRecipeDto dto) 
    {
        ArgumentNullException.ThrowIfNull(dto);       
        var resultDto = await recipeRepository.AddRecipe(dto);
        return new CreatedResult($"api/recipe/{resultDto.Id}", resultDto);        
    }

    public Task<IActionResult> PutRecipe(PizzaRecipeDto updatedDto)
    {
        throw new NotImplementedException();
    }
}
