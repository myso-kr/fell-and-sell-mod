namespace FellAndSell.Mod.Map;

internal static class Overlay
{
    private static readonly Layer Map = new(), Hud = new();
    private static long _next;
    internal static void Tick()
    {
        var manager = Anchors.Instance("DungeonMinimapManager");
        if (manager == null || Read.Current == null) { Reset(); return; }
        Select.Tick(manager);
        if (Environment.TickCount64 < _next) return;
        _next = Environment.TickCount64 + 100;
        Map.Tick(Anchors.Instance("DungeonMapWindow"), manager);
        Hud.Tick(Anchors.Instance("DungeonMinimapHUD"), manager);
    }
    internal static void Reset() { Map.Reset(); Hud.Reset(); }
}
