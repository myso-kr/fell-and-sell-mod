namespace FellAndSell.Mod.Autoplay;

// Thread-local native scan scope, including native Update and nested TryPickup.
internal static class PickupScope
{
    [ThreadStatic] private static int _depth;
    internal static void Enter() => _depth++;
    internal static void Exit() => _depth = Math.Max(0, _depth - 1);
    internal static bool Option(bool original, bool enabled, bool safe) => original || (_depth > 0 && enabled && safe);
}
