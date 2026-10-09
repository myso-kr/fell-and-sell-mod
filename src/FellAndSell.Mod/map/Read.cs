using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Map;

// Read-only boundary. Never calls RevealAll or changes discovery flags/alpha/buffers.
internal static class Read
{
    internal static Snapshot? Current { get; private set; }
    private static object? _layout;
    private static long _next;
    internal static void Clear() { Current = null; _layout = null; _next = 0; }
    internal static void Tick()
    {
        if (Environment.TickCount64 < _next) return;
        _next = Environment.TickCount64 + 200;
        var manager = Anchors.Instance("DungeonMinimapManager");
        var layout = manager == null || !(bool)Reflect.Get(manager, "HasActiveMap")!
            || !(bool)Reflect.Get(manager, "IsInDungeon")! ? null : Reflect.Get(manager, "CurrentMapData");
        if (layout == null)
        {
            if (Current != null) { Guide.Move.Stop(); Guide.Route.Clear(); }
            Current = null;
            _layout = null;
            return;
        }
        if (!Equals(layout, _layout)) { Guide.Move.Stop(); Guide.Route.Clear(); _layout = layout; }
        var floor = Convert.ToInt32(Reflect.Get(layout, "currentFloor"));
        if (Current != null && Current.Floor != floor) { Guide.Move.Stop(); Guide.Route.Clear(); }
        if (!Config.Map && !Config.Guide) { Current = null; return; }
        var tiles = new List<Tile>();
        foreach (var group in new[] { "rooms", "corridors", "stairs" })
        foreach (var element in Sequence.Items(Reflect.Get(layout, group)))
        {
            var bounds = Reflect.Get(element, "bounds")!;
            var kind = group == "rooms" ? Reflect.Get(element, "type")!.ToString()! : group;
            float? height = group == "rooms" ? State.Position(Reflect.Get(element, "worldCenter")!).Y : null;
            tiles.Add(new(Number(bounds, "x"), Number(bounds, "y"), Number(bounds, "width"), Number(bounds, "height"), kind, height));
        }
        var markers = new List<Marker>();
        foreach (var name in new[] { "startRoom", "endRoom", "bossRoom" })
        {
            var room = Reflect.Get(layout, name);
            if (room != null) markers.Add(new(State.Position(Reflect.Get(room, "worldCenter")!), name));
        }
        foreach (var chest in Sequence.Items(Reflect.Get(layout, "chests")))
            if (!(bool)Reflect.Get(chest, "isOpened")!) markers.Add(new(State.Position(Reflect.Get(chest, "worldPosition")!), "chest"));
        if (Config.Enemies && manager != null)
        {
            Log.Guard("enemies", () => markers.AddRange(Enemies.Read(manager)));
        }
        Current = new(floor, State.Position(Reflect.Get(layout, "tileSize")!), tiles.ToArray(), markers.ToArray());
    }
    private static float Number(object value, string name) => Convert.ToSingle(Reflect.Get(value, name));
}
