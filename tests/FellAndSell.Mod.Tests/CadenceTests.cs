using FellAndSell.Mod.Autoplay;
using Xunit;

namespace FellAndSell.Mod.Tests;

public sealed class CadenceTests
{
    [Fact]
    public void NativeAndModScanShareOneCadence()
    {
        var cadence = new Cadence();
        Assert.True(cadence.Take(1000, 200, true));
        Assert.False(cadence.Take(1000, 200, true));
        Assert.False(cadence.Take(1100, 200, true));
        Assert.True(cadence.Take(1200, 200, true));
    }
    [Fact]
    public void UnsafeStateCannotScanOrConsumeNextSlot()
    {
        var cadence = new Cadence();
        Assert.False(cadence.Take(1000, 200, false));
        Assert.True(cadence.Take(1001, 200, true));
    }
    [Fact]
    public void SceneResetDoesNotKeepOldCooldown()
    {
        var cadence = new Cadence();
        cadence.Take(1000, 1000, true);
        cadence.Reset();
        Assert.True(cadence.Take(1010, 200, true));
    }
}
