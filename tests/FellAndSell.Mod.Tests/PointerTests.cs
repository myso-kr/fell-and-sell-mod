using FellAndSell.Mod.Panel;
using Xunit;

namespace FellAndSell.Mod.Tests;

public sealed class PointerTests
{
    private static readonly Hit[] Buttons = [new(32, 135, 350, 28), new(32, 170, 350, 28)];
    [Theory]
    [InlineData(145, 0)]
    [InlineData(180, 1)]
    public void LowerOptionsReceiveExactlyOneClick(float y, int expected)
    {
        var pointer = new Pointer();
        Assert.Equal(-1, pointer.Step(100, y, true, false, true, Buttons));
        Assert.Equal(expected, pointer.Step(100, y, false, true, true, Buttons));
        Assert.Equal(-1, pointer.Step(100, y, false, true, true, Buttons));
    }
    [Fact]
    public void ReleasingOnAnotherButtonDoesNotToggleEither()
    {
        var pointer = new Pointer();
        pointer.Step(100, 145, true, false, true, Buttons);
        Assert.Equal(-1, pointer.Step(100, 180, false, true, true, Buttons));
    }
    [Fact]
    public void HidingPanelCancelsHeldClick()
    {
        var pointer = new Pointer();
        pointer.Step(100, 145, true, false, true, Buttons);
        pointer.Step(100, 145, false, false, false, Buttons);
        Assert.Equal(-1, pointer.Step(100, 145, false, true, true, Buttons));
    }
}
