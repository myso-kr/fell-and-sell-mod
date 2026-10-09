using FellAndSell.Mod.Autoplay;
using FellAndSell.Mod.Guide;
using Xunit;

namespace FellAndSell.Mod.Tests;

public sealed class TargetTests
{
    [Fact]
    public void DefaultExitDoesNotReplaceManualSelection()
    {
        var target = new Target();
        target.Default(new Point(1, 0, 1));
        Assert.Equal(new Point(1, 0, 1), target.Goal);
        target.Select(new Point(2, 0, 2));
        target.Default(new Point(1, 0, 1));
        Assert.Equal(new Point(2, 0, 2), target.Goal);
    }
    [Fact]
    public void CancelSuppressesDefaultUntilFloorReset()
    {
        var target = new Target();
        target.Default(new Point(1, 0, 1));
        target.Cancel();
        target.Default(new Point(1, 0, 1));
        Assert.Null(target.Goal);
        target.Reset();
        target.Default(new Point(3, 0, 3));
        Assert.Equal(new Point(3, 0, 3), target.Goal);
    }
    [Fact]
    public void ManualSelectionAfterCancelResumesGuidance()
    {
        var target = new Target();
        target.Cancel();
        target.Select(new Point(2, 0, 2));
        Assert.Equal(new Point(2, 0, 2), target.Goal);
    }
}
