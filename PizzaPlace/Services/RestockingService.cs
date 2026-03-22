using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services
{
    public class RestockingService(IRestockingRepository restockingRepo) : IRestockingService
    {
        public async Task<ComparableList<StockDto>> Restock(ComparableList<StockDto> stocksToReorder)
        {            
              return restockingRepo.GetStocks(stocksToReorder);   
        }
    }
}
