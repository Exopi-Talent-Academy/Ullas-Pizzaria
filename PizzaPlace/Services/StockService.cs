using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;
public class StockService(IStockRepository stockRepository, IRecipeService recipeService) : IStockService
{
    public async Task<bool> HasSufficientStockForOrder(PizzaOrder order, 
        ComparableList<PizzaRecipeDto> recipeDtos)
    {
        bool result = false;    
        var requiredStock = await CalculateRequiredStock(order, recipeDtos);
        var allStock = await GetAllStockAsync();
        foreach (var required in requiredStock)
        {
            foreach (var allItem in allStock)
            {
                if (required.StockType == allItem.StockType)
                {
                    var comparison = allItem.Amount - required.Amount;
                    result = comparison > 0;
                    if (result == false)
                        break;
                }
            }
        } return result;
    }

    public async Task<ComparableList<StockDto>> GetAllStockAsync() 
    {
        return await stockRepository.GetAllStockAsync();
    }

    public async Task<ComparableList<StockDto>> CalculateRequiredStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        ComparableList<(PizzaRecipeDto type, int pizzaAmount)> recipeTypeAndAmountsForOrder = [];
         
        foreach (var orderItem in order.RequestedOrder)
        {
            var recipeDtoForPizzaType = recipeDtos.FirstOrDefault(rDto => rDto.RecipeType == orderItem.PizzaType)
                ?? throw new InvalidOperationException();
            var pizzaAmount = orderItem.Amount;

            recipeTypeAndAmountsForOrder.Add((recipeDtoForPizzaType, pizzaAmount));
        }

        List<(StockType stockType, int stockAmount)> stocksAndTheirAmounts = [];

        foreach (var (type, pizzaAmount) in recipeTypeAndAmountsForOrder)
        {
            foreach(var stockDto in type.Ingredients) 
            {
                var existing = stocksAndTheirAmounts.FirstOrDefault(s => s.stockType == stockDto.StockType);
                if (existing == default)
                {
                    stocksAndTheirAmounts.Add((stockDto.StockType, (stockDto.Amount * pizzaAmount)));
                }
                else
                {
                    stocksAndTheirAmounts.Remove(existing);                    
                    stocksAndTheirAmounts.Add((existing.stockType, existing.stockAmount + stockDto.Amount * pizzaAmount));
                }
            }      
        }
        return stocksAndTheirAmounts.Select(s => new StockDto(s.stockType, s.stockAmount)).ToComparableList();
       
    }

    public async Task<ComparableList<StockDto>> ReturnStocksForReordering()
    {
        ComparableList<StockDto> stocksToReorder = [];

        var allStocks = await GetAllStockAsync();
        foreach (var stockDto in allStocks) 
        {
            if(stockDto.Amount < 5)
                stocksToReorder.Add(stockDto);
        }
        return stocksToReorder;
    }

    public async Task<ComparableList<StockDto>> Restock(ComparableList<StockDto> stocksToReorder)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
