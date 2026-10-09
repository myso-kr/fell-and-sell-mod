namespace FellAndSell.Mod.Autoplay;

// Reads game state once per update; decisions use the plain Snapshot.
internal static class State
{
    internal static object? Player, Input, Pickup;
    private static string? _diagnostic;
    internal static void Clear() { Player = Input = Pickup = null; _diagnostic = null; }
    internal static Point Position(object vector) => new(Convert.ToSingle(Reflect.Get(vector, "x")),
        Convert.ToSingle(Reflect.Get(vector, "y")), Convert.ToSingle(Reflect.Get(vector, "z")));
    internal static object Vector(Point point) => Anchors.Vector(point.X, point.Y, point.Z);
    internal static float Position2(object vector)
    {
        var x = Convert.ToSingle(Reflect.Get(vector, "x"));
        var y = Convert.ToSingle(Reflect.Get(vector, "y"));
        return MathF.Sqrt(x * x + y * y);
    }
    internal static Snapshot Read(bool panel)
    {
        Player = Anchors.Instance("FirstPersonController");
        Input = Anchors.Instance("PlayerInputHandler");
        Pickup = Anchors.Instance("PlayerAutoPickup");
        var stats = Anchors.Instance("PlayerStats");
        if (Player == null || Input == null || stats == null)
        {
            Diagnose($"waiting player={Player != null} input={Input != null} stats={stats != null} pickup={Pickup != null}");
            return default;
        }
        var state = new Snapshot(true, (bool)Reflect.Get(stats, "IsDead")!, (bool)Reflect.Get(stats, "IsInCombat")!,
            panel || (bool)Reflect.Get(Input, "IsInputsBlocked")! || (bool)Anchors.Static("Il2Cpp.FirstPersonController", "IsPlayerBlocked")!,
            Pickup != null && (bool)Reflect.Call(Pickup, "IsInventoryOpen")!, Convert.ToSingle(Anchors.Static("UnityEngine.Time", "timeScale")) <= 0,
            (bool)Anchors.Static("UnityEngine.Application", "isFocused")!, Convert.ToSingle(Reflect.Get(stats, "CurrentHealth")),
            Position(Reflect.Get(Reflect.Get(Player, "transform")!, "position")!));
        var key = $"panel={panel} map={Map.Window.Open} inputBlocked={Reflect.Get(Input, "IsInputsBlocked")} playerBlocked={Anchors.Static("Il2Cpp.FirstPersonController", "IsPlayerBlocked")} focused={state.Focused} paused={state.Paused} cursor={Anchors.Static("UnityEngine.Cursor", "lockState")}";
        Diagnose(key);
        return state;
    }
    private static void Diagnose(string key)
    {
        if (_diagnostic == key) return;
        _diagnostic = key;
        MelonLoader.MelonLogger.Msg("input: " + key);
    }
}
