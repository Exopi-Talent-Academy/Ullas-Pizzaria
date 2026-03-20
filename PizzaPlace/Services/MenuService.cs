namespace PizzaPlace.Services;

public class MenuService(IMenuOrchestrator menuOrchestrator) : IMenuService
{
    public Menu GetMenu(DateTimeOffset menuDate)
    {
        return menuOrchestrator.ChooseMenuTitle(menuDate.Hour, menuDate.Minute)
            ?? throw new InvalidOperationException("No menu found");
    }
}
 