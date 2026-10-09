using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal static class Overlay
{
    internal static void Draw()
    {
        if (!Config.Guide || Route.Corners.Length < 2 || Map.Window.Open) return;
        var camera = Anchors.Static("UnityEngine.Camera", "main");
        if (!Alive.Is(camera)) return;
        var height = Convert.ToSingle(Anchors.Static("UnityEngine.Screen", "height"));
        Point? previous = null;
        foreach (var corner in Route.Corners)
        {
            var projected = State.Position(Reflect.Call(camera!, "WorldToScreenPoint", State.Vector(corner with { Y = corner.Y + 0.15f }))!);
            if (projected.Z <= 0) { previous = null; continue; }
            projected = projected with { Y = height - projected.Y };
            if (previous is { } start) Panel.Gui.Line(start.X, start.Y, projected.X, projected.Y, Route.Complete ? "route" : "partial");
            previous = projected;
        }
    }
}
