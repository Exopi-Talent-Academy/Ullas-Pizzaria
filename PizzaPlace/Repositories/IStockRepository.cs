using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public interface IStockRepository
{
    Task<StockDto> AddToStock(StockDto stock);
    Task<ComparableList<StockDto>> GetAllStockAsync();
    Task<StockDto> TakeStock(StockType stockType, int amount);
    Task<StockDto> GetStockDto(StockType stockType);
}
