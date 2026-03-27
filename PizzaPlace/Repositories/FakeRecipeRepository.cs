using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public class FakeRecipeRepository : FakeDatabase<PizzaRecipeDto>, IRecipeRepository
{
    private static readonly object _lock = new();

    public Task<PizzaRecipeDto> AddRecipe(PizzaRecipeDto recipe)
    {
        lock (_lock)
        {
            if (Get(x => x.RecipeType == recipe.RecipeType).Any())
                throw new PizzaException($"Recipe already added for {recipe.RecipeType}.");

            var id = Insert(recipe);
            return Task.FromResult(new PizzaRecipeDto(recipe.RecipeType, recipe.Ingredients, recipe.CookingTimeMinutes, id));
        }
    }

    public Task DeleteRecipe(PizzaRecipeDto recipe)
    {
        var recipeItem = Get(x => x.Id == recipe.Id);
        if (recipeItem == null)
            throw new PizzaException("Recipe not found");
        Delete(recipe.Id);
        return Task.CompletedTask;
    }


        public Task<PizzaRecipeDto?> GetRecipe(PizzaRecipeType recipeType)
    {
        var recipe = Get(x => x.RecipeType == recipeType)
            .FirstOrDefault() ?? null;

        return Task.FromResult(recipe);
    }

    public void AddStandardRecipes()
    {
        if (Get(_ => true).Any())
            return;

        foreach (var recipe in GetStandardRecipes())
            AddRecipe(recipe).GetAwaiter().GetResult();

        static List<PizzaRecipeDto> GetStandardRecipes() =>
        [
            new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [
                new StockDto(StockType.Dough, 1),
                new StockDto(StockType.Tomatoes, 2),
                new StockDto(StockType.GratedCheese, 1),
                new StockDto(StockType.GenericSpices, 1)
            ], 10),
            new PizzaRecipeDto(PizzaRecipeType.ExtremelyTastyPizza, [
                new StockDto(StockType.FermentedDough, 1),
                new StockDto(StockType.Tomatoes, 1),
                new StockDto(StockType.RottenTomatoes, 1),
                new StockDto(StockType.GratedCheese, 1),
                new StockDto(StockType.UngenericSpices, 1)
            ], 10),
        ];
    }

    public async Task<PizzaRecipeDto> UpdateRecipe(PizzaRecipeDto updatedDto)
    {
        var type = updatedDto.RecipeType;
        var outdatedRecipe = await GetRecipe(type);
        return outdatedRecipe == null
            ? throw new PizzaException("Could not find that recipetype in database")
            : new PizzaRecipeDto(updatedDto.RecipeType, updatedDto.Ingredients, updatedDto.CookingTimeMinutes, updatedDto.Id);
    }
}
