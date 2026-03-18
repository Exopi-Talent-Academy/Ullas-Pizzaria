using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;
public class StockService(IStockRepository stockRepository, IRecipeService recipeService) : IStockService
{
    private async Task<ComparableList<PizzaRecipeDto>> GetrecipeDtos(PizzaOrder order) => await recipeService.GetPizzaRecipes(order);
    public Task<bool> HasInsufficientStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        throw new NotImplementedException("Sufficient stock must be checked.");
    }

    public async Task<ComparableList<StockDto>> GetStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        ComparableList<(PizzaRecipeDto type, int pizzaAmount)> recipeTypeAndAmountsInOrder = new ComparableList<(PizzaRecipeDto type, int pizzaAmount)>();
         
        foreach (var orderItem in order.RequestedOrder)
        {
            var recipeDtoForPizzaType= recipeDtos.FirstOrDefault(rDto => rDto.RecipeType == orderItem.PizzaType);
            var pizzaAmont = orderItem.Amount;

            recipeTypeAndAmountsInOrder.Add((recipeDtoForPizzaType, pizzaAmont));
        }

        List<(StockType stockType, int stockAmount)> stocksAndTheirAmounts = new List<(StockType stockType, int stockAmount)>();

        foreach (var item in recipeTypeAndAmountsInOrder)
        {
            foreach(var stockDto in item.type.Ingredients) 
            {
                var existing = stocksAndTheirAmounts.FirstOrDefault(s => s.stockType == stockDto.StockType);
                if (existing == default)
                {
                    stocksAndTheirAmounts.Add((stockDto.StockType, (stockDto.Amount * item.pizzaAmount)));
                }
                else
                {
                    stocksAndTheirAmounts.Remove(existing);                    
                    stocksAndTheirAmounts.Add((existing.stockType, (existing.stockAmount + (stockDto.Amount * item.pizzaAmount))));
                }
            }      
        }

        return stocksAndTheirAmounts.Select(s => new StockDto(s.stockType, s.stockAmount)).ToComparableList();
       
    }
}
