using FellAndSell.Mod.Autoplay;
using Xunit;

namespace FellAndSell.Mod.Tests;

public sealed class PickupScopeTests
{
    [Fact]
    public void DisabledNativeOptionIsAllowedOnlyDuringSafeEnabledScan()
    {
        Assert.False(PickupScope.Option(false, true, true));
        PickupScope.Enter();
        try
        {
            Assert.True(PickupScope.Option(false, true, true));
            Assert.False(PickupScope.Option(false, false, true));
            Assert.False(PickupScope.Option(false, true, false));
        }
        finally { PickupScope.Exit(); }
        Assert.False(PickupScope.Option(false, true, true));
        Assert.True(PickupScope.Option(true, false, false));
    }
    [Fact]
    public void ExceptionCleanupCannotLeaveNativeOptionEnabled()
    {
        Assert.Throws<InvalidOperationException>((Action)(() =>
        {
            PickupScope.Enter();
            try { throw new InvalidOperationException(); }
            finally { PickupScope.Exit(); }
        }));
        Assert.False(PickupScope.Option(false, true, true));
    }
    [Fact]
    public void NestedScanRetainsOuterScopeUntilFinalCleanup()
    {
        PickupScope.Enter();
        PickupScope.Enter();
        PickupScope.Exit();
        try { Assert.True(PickupScope.Option(false, true, true)); }
        finally { PickupScope.Exit(); }
        Assert.False(PickupScope.Option(false, true, true));
    }
}
