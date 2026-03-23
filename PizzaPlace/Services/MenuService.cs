namespace PizzaPlace.Services;

public class MenuService(IMenuOrchestrator menuOrchestrator) : IMenuService
{
    public async Task<Menu> GetMenuAsync(DateTimeOffset menuDate)
    {
        await Task.CompletedTask;
        return menuOrchestrator.ChooseMenuTitle(menuDate.Hour, menuDate.Minute)
            ?? throw new InvalidOperationException("No menu found");
    }
}
 