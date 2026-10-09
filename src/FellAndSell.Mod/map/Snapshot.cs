using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Map;

internal readonly record struct Tile(float X, float Z, float Width, float Depth, string Kind, float? Height = null);
internal readonly record struct Marker(Point Position, string Kind);
internal sealed record Snapshot(int Floor, Point Size, Tile[] Tiles, Marker[] Markers);
