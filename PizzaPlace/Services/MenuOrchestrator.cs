namespace PizzaPlace.Services
{
    public class MenuOrchestrator : IMenuOrchestrator
    {
        private List<Menu> menus = new List<Menu>() {
            new Menu("StandardMenu", new ComparableList<MenuItem>()),
            new Menu("LunchMenu", new ComparableList<MenuItem>())
        };



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
