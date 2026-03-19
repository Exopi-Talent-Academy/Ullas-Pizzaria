using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;
public class StockService(IStockRepository stockRepository, IRecipeService recipeService) : IStockService
{
    private async Task<ComparableList<PizzaRecipeDto>> GetrecipeDtos(PizzaOrder order) => 
        await recipeService.GetPizzaRecipes(order);
    public async Task<bool> HasInsufficientStock(PizzaOrder order, 
        ComparableList<PizzaRecipeDto> recipeDtos)
    {
        bool result = false;    
        var requiredStock = await CalculateRequiredStock(order, recipeDtos);
        var allStock = await GetAllStock();
        foreach (var required in requiredStock)
        {
            foreach (var allItem in allStock)
            {
                if (required.StockType == allItem.StockType)
                {
                    var comparison = allItem.Amount - required.Amount;
                    result = comparison < 0 ? false : true;
                    if (result == false)
                        break;
                }
            }
        } return result;
    }

    public async Task<ComparableList<StockDto>> GetAllStock() 
    {
        return await stockRepository.GetAllStock();
    }

    public async Task<ComparableList<StockDto>> CalculateRequiredStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        ComparableList<(PizzaRecipeDto type, int pizzaAmount)> recipeTypeAndAmountsForOrder = new ComparableList<(PizzaRecipeDto type, int pizzaAmount)>();
         
        foreach (var orderItem in order.RequestedOrder)
        {
            var recipeDtoForPizzaType= recipeDtos.FirstOrDefault(rDto => rDto.RecipeType == orderItem.PizzaType);
            var pizzaAmont = orderItem.Amount;

            recipeTypeAndAmountsForOrder.Add((recipeDtoForPizzaType, pizzaAmont));
        }

        List<(StockType stockType, int stockAmount)> stocksAndTheirAmounts = new List<(StockType stockType, int stockAmount)>();

        foreach (var item in recipeTypeAndAmountsForOrder)
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
