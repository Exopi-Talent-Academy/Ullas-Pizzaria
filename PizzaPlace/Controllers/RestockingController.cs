using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/restocking")]
public class RestockingController(IStockService stockService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Restock([FromBody] ComparableList<StockDto> stocksToReorder)
    {
        try
        {
            var result = await stockService.Restock(stocksToReorder);
            return Ok(result);
        }
        catch (InvalidOperationException ex) 
        {
           return BadRequest(ex.Message);  
        }
    }
}
