using FellAndSell.Mod.Autoplay;
using FellAndSell.Mod.Guide;
using Xunit;

namespace FellAndSell.Mod.Tests;

public sealed class FollowTests
{
    private static Snapshot Safe(Point position = default) => new(true, false, false, false, false, false, true, 100, position);
    private static Follow Started()
    {
        var follow = new Follow();
        follow.Start([new(0, 0, 0), new(5, 0, 0)], 100, 0);
        return follow;
    }

    [Fact]
    public void WalksTowardNextCornerAndArrivesWithoutTeleporting()
    {
        var follow = Started();
        Assert.Equal(new Point(1, 0, 0), follow.Step(Safe(), false, 10));
        Assert.Equal(default, follow.Step(Safe(new(5, 0, 0)), false, 20));
        Assert.Equal(Motion.Arrived, follow.Status);
    }

    [Fact]
    public void ManualMovementCancelsAndNeverRestartsItself()
    {
        var follow = Started();
        Assert.Equal(default, follow.Step(Safe(), true, 10));
        Assert.Equal(Motion.Cancelled, follow.Status);
        Assert.Equal(default, follow.Step(Safe(), false, 20));
    }

    [Theory]
    [InlineData("death")]
    [InlineData("combat")]
    [InlineData("menu")]
    [InlineData("inventory")]
    [InlineData("pause")]
    [InlineData("focus")]
    [InlineData("missing")]
    [InlineData("damage")]
    public void UnsafeStateImmediatelyReturnsControl(string reason)
    {
        var state = reason switch
        {
            "death" => Safe() with { Dead = true },
            "combat" => Safe() with { Combat = true },
            "menu" => Safe() with { Blocked = true },
            "inventory" => Safe() with { Inventory = true },
            "pause" => Safe() with { Paused = true },
            "focus" => Safe() with { Focused = false },
            "missing" => default,
            _ => Safe() with { Health = 99 }
        };
        var follow = Started();
        Assert.Equal(default, follow.Step(state, false, 10));
        Assert.Equal(Motion.Cancelled, follow.Status);
    }

    [Fact]
    public void CollisionWithoutProgressStopsWithinBudget()
    {
        var follow = Started();
        follow.Step(Safe(), false, 10);
        Assert.Equal(default, follow.Step(Safe(), false, 2600));
        Assert.Equal(Motion.Blocked, follow.Status);
    }

    [Fact]
    public void VerticalWaypointCannotBeMistakenForArrival()
    {
        var follow = new Follow();
        follow.Start([new(0, 0, 0), new(0, 4, 0)], 100, 0);
        Assert.Equal(default, follow.Step(Safe(), false, 10));
        Assert.Equal(Motion.Blocked, follow.Status);
    }

    [Fact]
    public void MissingRouteCannotStartMoving()
    {
        var follow = new Follow();
        follow.Start([], 100, 0);
        Assert.Equal(default, follow.Step(Safe(), false, 10));
        Assert.Equal(Motion.Blocked, follow.Status);
    }

    [Fact]
    public void CombatStillAllowsNearbyPickupButNotAutomaticMovement()
    {
        var state = Safe() with { Combat = true };
        Assert.True(state.CanPickup);
        Assert.False(state.CanMove);
    }
}
