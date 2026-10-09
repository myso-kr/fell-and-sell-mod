using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Map;

internal static class Height
{
    internal static Point Resolve(Point point, Snapshot? map)
    {
        if (map == null) return point;
        // Use the selected room's world elevation, not the player's current storey.
        var rooms = map.Tiles.Where(tile => tile.Height.HasValue
            && point.X >= tile.X * map.Size.X && point.X <= (tile.X + tile.Width) * map.Size.X
            && point.Z >= tile.Z * map.Size.Z && point.Z <= (tile.Z + tile.Depth) * map.Size.Z)
            .OrderBy(tile => MathF.Abs(tile.Height!.Value - point.Y)).ToArray();
        return rooms.Length == 0 ? point : point with { Y = rooms[0].Height!.Value };
    }
}
