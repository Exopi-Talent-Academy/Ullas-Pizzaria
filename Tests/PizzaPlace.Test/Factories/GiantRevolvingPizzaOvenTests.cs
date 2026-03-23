using PizzaPlace.Factories;
using PizzaPlace.Models;
using PizzaPlace.Pizzas;
using PizzaPlace.Test.TestExtensions;

namespace PizzaPlace.Test.Factories;

[TestClass]
public class GiantRevolvingPizzaOvenTests
{
    private static GiantRevolvingPizzaOven GetOven(TimeProvider timeProvider) => new(timeProvider);

    [TestMethod]
    [Ignore("Waiting for GiantRevolvingPizzaOven implementation")]
    public async Task PreparePizzas_OneHundredPizzas()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        ComparableList<PizzaPrepareOrder> order =
        [
            new PizzaPrepareOrder(PizzaOvenHelperTests.GetTestStandardPizzaRecipe(), 100),
         ];
        var stock = PizzaOvenHelperTests.GetPlentyStock();

        var oven = GetOven(timeProvider);
        var expectedTime = PizzaOvenHelperTests.StandardPizzaPrepareTime;
        var expectedPizzas = 100;

        // Act
        var pizzasTask = oven.PreparePizzas(order, stock);
        timeProvider.PassTimeInMinuteIntervals(expectedTime - 1);
        var firstCheck = pizzasTask.IsCompleted;
        timeProvider.PassTimeInMinuteIntervals(1);
        var secondCheck = pizzasTask.IsCompleted;

        // Assert
        Assert.IsFalse(firstCheck);
        Assert.IsTrue(secondCheck);
        var pizzas = await pizzasTask;
        Assert.AreEqual(expectedPizzas, pizzas.Count());
        Assert.IsTrue(pizzas.All(x => x is StandardPizza), "Only standard pizzas");
    }

    [TestMethod]
    [Ignore("Waiting for GiantRevolvingPizzaOven implementation")]
    public async Task PreparePizzas_241_Pizzas()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        ComparableList<PizzaPrepareOrder> order =
        [
            new PizzaPrepareOrder(PizzaOvenHelperTests.GetTestStandardPizzaRecipe(), 241),
         ];
        var stock = PizzaOvenHelperTests.GetPlentyStock();

        var oven = GetOven(timeProvider);
        var expectedTime = PizzaOvenHelperTests.StandardPizzaPrepareTime * 3;
        var expectedPizzas = 241;

        // Act
        var pizzasTask = oven.PreparePizzas(order, stock);
        timeProvider.PassTimeInMinuteIntervals(expectedTime - 1);
        var firstCheck = pizzasTask.IsCompleted;
        timeProvider.PassTimeInMinuteIntervals(1);
        var secondCheck = pizzasTask.IsCompleted;

        // Assert
        Assert.IsFalse(firstCheck);
        Assert.IsTrue(secondCheck);
        var pizzas = await pizzasTask;
        Assert.AreEqual(expectedPizzas, pizzas.Count());
        Assert.IsTrue(pizzas.All(x => x is StandardPizza), "Only standard pizzas");
    }

    [TestMethod]
    [Ignore("Waiting for GiantRevolvingPizzaOven implementation")]
    public async Task PreparePizzas_MixedPizzas()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        ComparableList<PizzaPrepareOrder> order =
        [
            new PizzaPrepareOrder(PizzaOvenHelperTests.GetTestStandardPizzaRecipe(), 1),
             new PizzaPrepareOrder(PizzaOvenHelperTests.GetTestTastyPizzaRecipe(), 1),
             new PizzaPrepareOrder(PizzaOvenHelperTests.GetTestStandardPizzaRecipe(), 1),
         ];
        var stock = PizzaOvenHelperTests.GetPlentyStock();

        var oven = GetOven(timeProvider);
        var expectedTime = PizzaOvenHelperTests.StandardPizzaPrepareTime + PizzaOvenHelperTests.TastyPizzaPrepareTime;
        var expectedPizzas = 3;

        // Act
        var pizzasTask = oven.PreparePizzas(order, stock);
        timeProvider.PassTimeInMinuteIntervals(expectedTime - 1);
        var firstCheck = pizzasTask.IsCompleted;
        timeProvider.PassTimeInMinuteIntervals(1);
        var secondCheck = pizzasTask.IsCompleted;

        // Assert
        Assert.IsFalse(firstCheck);
        Assert.IsTrue(secondCheck);
        var pizzas = await pizzasTask;
        Assert.AreEqual(expectedPizzas, pizzas.Count());
        Assert.AreEqual(2, pizzas.Count(x => x is StandardPizza));
        Assert.AreEqual(1, pizzas.Count(x => x is ExtremelyTastyPizza));
    }
}
