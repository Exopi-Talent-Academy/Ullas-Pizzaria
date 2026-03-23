using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Services;
using System;

namespace PizzaPlace.Controllers;

[Route("api/menu")]
public class MenuController(TimeProvider timeProvider, IMenuService menuService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        try
        {
            return Ok(await menuService.GetMenuAsync(timeProvider.GetUtcNow()));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }

    }
}
