using PizzaPlace.Models;

namespace PizzaPlace.Repositories
{
    public interface IRestockingRepository
    {
        //Task<ComparableList<StockDto>> GetStocksAsync(ComparableList<StockDto> stocksToGet);
        ComparableList<StockDto> GetStocks(ComparableList<StockDto> stocksToGet);
    }
}