using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public interface IRecipeRepository
{
    Task<PizzaRecipeDto> AddRecipe(PizzaRecipeDto recipe);
    Task<PizzaRecipeDto> GetRecipe(PizzaRecipeType recipeType);
    Task<PizzaRecipeDto> UpdateRecipe(PizzaRecipeDto updatedDto);


}
