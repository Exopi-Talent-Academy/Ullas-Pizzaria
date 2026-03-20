namespace PizzaPlace.Services;

public class MenuService : IMenuService
{
    IMenuOrchestrator _menuOrchestrator = null!;

    public MenuService(IMenuOrchestrator menuOrchestrator) 
    { 
        _menuOrchestrator = menuOrchestrator;
    }
    public Menu GetMenu(DateTimeOffset menuDate)
    {
        return _menuOrchestrator.ChooseMenuTitle(menuDate.Hour, menuDate.Minute)
            ?? throw new InvalidOperationException("No menu found");
    }
}
 