namespace FellAndSell.Mod.Autoplay;

// Native callbacks only queue invalidation; owned objects are reset in OnUpdate.
internal static class Lifecycle
{
    private static string? _pending;
    private static int? _floor;
    internal static void Invalidate(string reason) => _pending = reason;
    internal static void Tick()
    {
        var dungeon = Anchors.Instance("DungeonManager");
        var floor = dungeon == null ? (int?)null : Convert.ToInt32(Reflect.Get(dungeon, "currentFloor"));
        if (_floor != null && floor != _floor) Invalidate("floor changed");
        _floor = floor;
        if (_pending == null) return;
        var reason = _pending;
        _pending = null;
        Supervisor.Reset();
        MelonLoader.MelonLogger.Msg($"lifecycle: refreshed floor={floor} reason={reason}");
    }
    internal static void Clear() { _pending = null; _floor = null; }
}
