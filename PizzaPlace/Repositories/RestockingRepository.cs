using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories
{
    public class RestockingRepository : IRestockingRepository
    {
        public ComparableList<StockDto> ExternalWarehouse {  get; set; } =
            [
                new(StockType.Bacon, 100),
                new(StockType.Tomatoes, 100),
                new(StockType.Chocolate, 100),
                new(StockType.BellPeppers, 100)
            ];
        public ComparableList<StockDto> GetStocks(ComparableList<StockDto> stocksToGet)
        {
            ComparableList<StockDto> orderedStocks = new ComparableList<StockDto>();
            foreach (var stock in stocksToGet) 
            {               
                var amountToGet = 50 - stock.Amount;
                var wareHouseStock = ExternalWarehouse.FirstOrDefault(ew => ew.StockType == stock.StockType) ?? new StockDto(stock.StockType, 0);
                if (wareHouseStock.Amount - amountToGet < 0)
                {
                    ExternalWarehouse.Remove(stock);
                    ExternalWarehouse.Add(new StockDto(stock.StockType, 0));
                    var newAmountStock = stock.Amount + wareHouseStock.Amount;
                    orderedStocks.Add(new StockDto(stock.StockType, newAmountStock));
                    throw new InvalidOperationException($"Order problems. New Amount only: {newAmountStock}");
                }
                ExternalWarehouse.Remove(stock);
                var newAmountWareHouseStock = wareHouseStock.Amount - amountToGet;
                ExternalWarehouse.Add(new StockDto(stock.StockType, newAmountWareHouseStock));
                orderedStocks.Add(new StockDto(stock.StockType, 50));   
            }
            return orderedStocks;
        }
    }
}
