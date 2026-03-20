using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Services;
using System;

namespace PizzaPlace.Controllers;

[Route("api/menu")]
public class MenuController(TimeProvider timeProvider, IMenuService menuService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetMenu()
    {
        try
        {
            return Ok(menuService.GetMenu(timeProvider.GetUtcNow()));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }

    }
}
