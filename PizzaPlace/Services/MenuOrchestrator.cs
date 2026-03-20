namespace PizzaPlace.Services
{
    public class MenuOrchestrator : IMenuOrchestrator
    {
        private readonly List<Menu> menus = [
            new("StandardMenu", []),
            new("LunchMenu", [])
        ];
        
        public Menu? ChooseMenuTitle(int hour, int minute)
        {
            var title = "StandardMenu";

            if (
                (hour >= 11 && hour < 14) ||
                (hour == 14 && minute == 0)
                )
                title = "LunchMenu";

            return menus.FirstOrDefault(m => m.Title == title);
        }
    }
}
