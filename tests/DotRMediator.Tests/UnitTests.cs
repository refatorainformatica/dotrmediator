namespace DotRMediator.Tests;

public sealed class UnitTests
{
    [Fact]
    public void Unit_ShouldEqualAnotherInstance()
    {
        Assert.Equal(Unit.Value, default(Unit));
        Assert.True(Unit.Value.Equals(Unit.Value));
        Assert.Equal(0, Unit.Value.GetHashCode());
        Assert.Equal("()", Unit.Value.ToString());
    }
}
