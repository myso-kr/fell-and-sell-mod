using FellAndSell.Mod.Autoplay;
using FellAndSell.Mod.Guide;
using Xunit;

namespace FellAndSell.Mod.Tests;

public sealed class WispTests
{
    [Fact]
    public void LightFollowsTurnInsteadOfCuttingAcrossWall()
    {
        Point[] path = [new(0, 0, 0), new(5, 0, 0), new(5, 0, 5)];
        var spark = Wisp.Sample(path, default, 2.5f);
        Assert.Equal(new Point(5, 0, 2), spark.Position);
    }
    [Fact]
    public void LightUsesStairElevation()
    {
        var spark = Wisp.Sample([new(0, 0, 0), new(4, -3, 0)], default, 1);
        Assert.True(spark.Position.X > 0 && spark.Position.Y < 0);
        Assert.Equal(-.75f, spark.Position.Y / spark.Position.X, 3);
    }
    [Fact]
    public void FollowingStartsNearPlayerInsteadOfDungeonEntrance()
    {
        var spark = Wisp.Sample([default, new(30, 0, 0)], new(20, 0, 0), 1);
        Assert.InRange(spark.Position.X, 20, 24);
    }
    [Fact]
    public void DistanceAndOcclusionReduceOpacity()
    {
        Assert.Equal(1, Wisp.Fade(5, false));
        Assert.InRange(Wisp.Fade(17, false), .49f, .51f);
        Assert.Equal(0, Wisp.Fade(30, false));
        Assert.Equal(0, Wisp.Fade(5, true));
        Assert.InRange(Wisp.Smooth(1, 0, .1f), 0.1f, .9f);
    }
    [Fact]
    public void EmptyOrArrivedRouteDoesNotEmitLight()
    {
        Assert.Equal(0, Wisp.Sample([], default, 1).Opacity);
        Assert.Equal(0, Wisp.Sample([default, new(5, 0, 0)], new(5, 0, 0), 1).Opacity);
    }
}
