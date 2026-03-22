using PizzaPlace.Models;

namespace PizzaPlace.Services
{
    public interface IRestockingService
    {
        Task<ComparableList<StockDto>> Restock(ComparableList<StockDto> stocksToReorder);
    }
}