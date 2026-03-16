namespace PizzaPlace.Services;

public class MenuService : IMenuService
{
    MenuOrchestrator _menuOrchestrator = new MenuOrchestrator();
    public Menu GetMenu(DateTimeOffset menuDate)
    {
        return _menuOrchestrator.ChooseMenuTitle(menuDate.Hour, menuDate.Minute);
    }
}
 