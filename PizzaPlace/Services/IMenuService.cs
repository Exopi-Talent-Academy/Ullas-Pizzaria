namespace PizzaPlace.Services;

public interface IMenuService
{
    Task<Menu> GetMenuAsync(DateTimeOffset menuDate);
}
