using FellAndSell.Mod.Autoplay;
using FellAndSell.Mod.Guide;
using Xunit;

namespace FellAndSell.Mod.Tests;

public sealed class SearchTests
{
    [Fact]
    public void ValidProjectionBeyondOldHeightThresholdIsAccepted()
    {
        var result = Search.Find(new(0, 1.7f, 0), new(5, 1.7f, 0),
            point => point with { Y = 0 }, (from, to) => new([from, to], "complete"));
        Assert.True(result.Complete);
    }
    [Fact]
    public void NearbyConnectedEndpointBeatsDisconnectedExactPoint()
    {
        var result = Search.Find(default, new(5, 0, 0), point => point,
            (from, to) => to.X == 5 && to.Z == 0 ? new([], "invalid") : new([from, to], "complete"));
        Assert.True(result.Complete);
        Assert.NotEqual(new Point(5, 0, 0), result.End);
    }
    [Fact]
    public void CompleteRouteBeatsShortPartialRoute()
    {
        var result = Search.Find(default, new(5, 0, 0), point => point,
            (from, to) => to.X == 5 && to.Z == 0 ? new([from, new(1, 0, 0)], "partial") : new([from, to], "complete"));
        Assert.True(result.Complete);
    }
    [Fact]
    public void UnrelatedStoreyIsRejected()
    {
        var result = Search.Find(default, new(5, 0, 0), point => point.X == 0 ? point : point with { Y = 5 },
            (from, to) => throw new Exception("Wrong storey must not be queried"));
        Assert.Equal("targetMissing", result.Status);
    }
    [Fact]
    public void MissingSourceNeverCalculatesAPath()
    {
        var result = Search.Find(default, new(5, 0, 0), _ => null,
            (from, to) => throw new Exception("Missing source"));
        Assert.Equal("sourceMissing", result.Status);
    }
    [Fact]
    public void DisconnectedMeshRemainsUnreachable()
    {
        var result = Search.Find(default, new(5, 0, 0), point => point, (from, to) => new([], "invalid"));
        Assert.False(result.Complete);
        Assert.Equal("invalid", result.Status);
    }
    [Fact]
    public void MapClickUsesDestinationRoomElevation()
    {
        var map = new FellAndSell.Mod.Map.Snapshot(1, new(1, 1, 1), [new(10, 10, 10, 10, "room", -8)], []);
        var point = FellAndSell.Mod.Map.Height.Resolve(new(15, 2, 15), map);
        Assert.Equal(-8, point.Y);
    }
}
