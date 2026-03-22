using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Services;

public interface IStockService
{
    Task<bool> HasSufficientStockForOrder(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos);

    Task<ComparableList<StockDto>> CalculateRequiredStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos);

    Task<ComparableList<StockDto>> GetAllStockAsync();

    Task<ComparableList<StockDto>> ReturnStocksForReordering();

    Task<ComparableList<StockDto>> Restock(ComparableList<StockDto> stocksToReorder);
}
