namespace PizzaPlace.Services
{
    public interface IMenuOrchestrator
    {
        Menu? ChooseMenuTitle(int hour, int minute);
    }
}