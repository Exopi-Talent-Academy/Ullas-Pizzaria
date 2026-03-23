using PizzaPlace.Models.Types;
using PizzaPlace.Models;

namespace PizzaPlace.Test.Factories
{
    public static class PizzaOvenHelperTests
    {
        public const int StandardPizzaPrepareTime = 10;
        public const int TastyPizzaPrepareTime = 15;


        public static PizzaRecipeDto GetTestStandardPizzaRecipe() =>
        new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [
                new StockDto(StockType.Dough, 1),
                new StockDto(StockType.Tomatoes, 2),
                new StockDto(StockType.GratedCheese, 1),
                new StockDto(StockType.GenericSpices, 1)
            ], StandardPizzaPrepareTime);

        public static PizzaRecipeDto GetTestTastyPizzaRecipe() =>
        new PizzaRecipeDto(PizzaRecipeType.ExtremelyTastyPizza, [
                new StockDto(StockType.FermentedDough, 1),
                new StockDto(StockType.RottenTomatoes, 2),
                new StockDto(StockType.Bacon, 1),
                new StockDto(StockType.GenericSpices, 1)
            ], TastyPizzaPrepareTime);

        public static ComparableList<StockDto> GetPlentyStock() =>
        new ComparableList<StockDto>(Enum.GetValues<StockType>().Select(type => new StockDto(type, int.MaxValue)));
    }
}
