namespace FellAndSell.Mod.Map;

internal static class Window
{
    internal static bool Open => Anchors.Instance("DungeonMapWindow") is { } window
        && (bool)Reflect.Get(window, "IsMapOpen")!;
}
