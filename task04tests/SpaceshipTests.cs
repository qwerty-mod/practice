using Xunit;
using task04; 

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Cruiser_ShouldPerformActionsWithoutExceptions()
    {
        var cruiser = new Cruiser();

        var exception1 = Record.Exception(() => cruiser.MoveForward());
        var exception2 = Record.Exception(() => cruiser.Rotate(90));
        var exception3 = Record.Exception(() => cruiser.Fire());

        Assert.Null(exception1);
        Assert.Null(exception2);
        Assert.Null(exception3);
    }

    [Fact]
    public void Fighter_ShouldPerformActionsWithoutExceptions()
    {
        var fighter = new Fighter();

        var exception1 = Record.Exception(() => fighter.MoveForward());
        var exception2 = Record.Exception(() => fighter.Rotate(-45));
        var exception3 = Record.Exception(() => fighter.Fire());

        Assert.Null(exception1);
        Assert.Null(exception2);
        Assert.Null(exception3);
    }
}

