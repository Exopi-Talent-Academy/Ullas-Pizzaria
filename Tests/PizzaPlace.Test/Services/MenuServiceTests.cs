using PizzaPlace.Models.Types;
using PizzaPlace.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaPlace.Test.Services
{
    [TestClass]
    public class MenuServiceTests
    {
        [TestMethod]
        [DataRow(17, 5, "StandardMenu")]
        [DataRow(13, 0,  "LunchMenu")]
        [DataRow(11, 0, "LunchMenu")]
        [DataRow(10, 59, "StandardMenu")]
        [DataRow(14,0,  "LunchMenu")]
        [DataRow(14, 1, "StandardMenu")]
        public void TestGetMenu(int hour, int minute, string expMenuTitle)
        {
            // Arrange
            var menuOrchestrator = new MenuOrchestrator();
            var service = new MenuService(menuOrchestrator);            

            var lunchMenu = ("LunchMenu", new ComparableList<MenuItem>() {    /// Jeg er klar over, at denne menu ikke bruges... men jeg forstod på en af de først opg, at jeg skulle lave den... og nu gemmer jeg den til db implemtationen.
                new MenuItem("Pizza1", PizzaRecipeType.RarePizza, 130.00d, false),
                new MenuItem("Pizza2", PizzaRecipeType.OddPizza, 146.00d, false),
                new MenuItem("Pizza3", PizzaRecipeType.ExtremelyTastyPizza, 140.00d, false),
                new MenuItem("Pizza4", PizzaRecipeType.EmptyPizza, 170.00d, false),
                new MenuItem("Pizza5", PizzaRecipeType.HorseRadishPizza, 140.00d, true),
                new MenuItem("Pizza6", PizzaRecipeType.EmptyPizza, 140.00d, false),
                new MenuItem("Pizza7", PizzaRecipeType.OddPizza, 148.00d, false),
                new MenuItem("Pizza8", PizzaRecipeType.OddPizza, 140.00d, false),
                new MenuItem("Pizza9", PizzaRecipeType.OddPizza, 190.00d, true),
                new MenuItem("Pizza10", PizzaRecipeType.OddPizza, 180.00d, false),
                new MenuItem("Pizza11", PizzaRecipeType.OddPizza, 120.00d, false),
                new MenuItem("Pizza12", PizzaRecipeType.OddPizza, 170.00d, false),

            });
            var testStandardMenu = ("StandardMenu", new ComparableList<MenuItem>() {
                new MenuItem("PizzaA", PizzaRecipeType.RarePizza, 135.00d, false),
                new MenuItem("PizzaB", PizzaRecipeType.OddPizza, 146.00d, false),
                new MenuItem("PizzaC", PizzaRecipeType.ExtremelyTastyPizza, 140.00d, false),
                new MenuItem("PizzaD", PizzaRecipeType.EmptyPizza, 172.00d, false),
                new MenuItem("PizzaE", PizzaRecipeType.HorseRadishPizza, 140.00d, true),
                new MenuItem("PizzaF", PizzaRecipeType.EmptyPizza, 140.00d, false),
                new MenuItem("PizzaG", PizzaRecipeType.OddPizza, 148.00d, true),
                new MenuItem("PizzaH", PizzaRecipeType.OddPizza, 147.00d, false),
                new MenuItem("PizzaI", PizzaRecipeType.OddPizza, 196.00d, true),
                new MenuItem("PizzaJ", PizzaRecipeType.OddPizza, 180.00d, false),
                new MenuItem("PizzaK", PizzaRecipeType.OddPizza, 120.00d, true),
                new MenuItem("PizzaL", PizzaRecipeType.OddPizza, 173.00d, false),

            });

            // Act
            var menu = service.GetMenu(new DateTimeOffset(2026, 3, 15, hour, minute, 0, 1, TimeSpan.FromHours(1)));
            var menuTitle = menu.Title;

            // Assert
            Assert.AreEqual(expMenuTitle, menuTitle);
        }

        [TestMethod]
        public void TestGetmenu_ExceptionCases()
        {
            // Arrange
            var menuOrchestratorMock = new Mock<IMenuOrchestrator>();
            menuOrchestratorMock.Setup(x => x.ChooseMenuTitle(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((Menu?)null);

            var service = new MenuService(menuOrchestratorMock.Object);
            var menuDate = new DateTimeOffset(DateTime.Now);

            // Act and Assert
            Assert.ThrowsException<InvalidOperationException>(() => service.GetMenu(menuDate));

        }
    }

    

    

}
