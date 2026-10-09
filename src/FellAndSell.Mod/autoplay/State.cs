namespace FellAndSell.Mod.Autoplay;

// Reads game state once per update; decisions use the plain Snapshot.
internal static class State
{
    internal static object? Player, Input, Pickup;
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
        if (Player == null || Input == null || stats == null) return default;
        return new(true, (bool)Reflect.Get(stats, "IsDead")!, (bool)Reflect.Get(stats, "IsInCombat")!,
            panel || (bool)Reflect.Get(Input, "IsInputsBlocked")! || (bool)Anchors.Static("Il2Cpp.FirstPersonController", "IsPlayerBlocked")!,
            Pickup != null && (bool)Reflect.Call(Pickup, "IsInventoryOpen")!, Convert.ToSingle(Anchors.Static("UnityEngine.Time", "timeScale")) <= 0,
            (bool)Anchors.Static("UnityEngine.Application", "isFocused")!, Convert.ToSingle(Reflect.Get(stats, "CurrentHealth")),
            Position(Reflect.Get(Reflect.Get(Player, "transform")!, "position")!));
    }
}
