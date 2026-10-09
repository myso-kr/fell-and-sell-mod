using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Map;

internal static class Select
{
    private static Point? _pressed;
    internal static void Tick(object manager)
    {
        if (!Window.Open || !Config.Guide || Panel.Widget.Visible) { _pressed = null; return; }
        if ((bool)Reflect.Method(Anchors.Type("UnityEngine.Input"), "GetMouseButtonDown", typeof(int)).Invoke(null, [1])!)
        { _pressed = null; Guide.Route.Cancel(); return; }
        var mouse = State.Position(Anchors.Static("UnityEngine.Input", "mousePosition")!);
        if ((bool)Reflect.Method(Anchors.Type("UnityEngine.Input"), "GetMouseButtonDown", typeof(int)).Invoke(null, [0])!) _pressed = mouse;
        if (!(bool)Reflect.Method(Anchors.Type("UnityEngine.Input"), "GetMouseButtonUp", typeof(int)).Invoke(null, [0])!) return;
        var pressed = _pressed; _pressed = null;
        if (pressed == null || pressed.Value.Distance(mouse) > 6) return;
        var window = Anchors.Instance("DungeonMapWindow")!;
        var rect = Reflect.Get(Reflect.Get(window, "_mapRawImage")!, "rectTransform")!;
        var utility = Anchors.Type("UnityEngine.RectTransformUtility");
        var camera = Anchors.Type("UnityEngine.Camera");
        var vector = Anchors.Type("UnityEngine.Vector2");
        var screen = Anchors.Vector2(mouse.X, mouse.Y);
        if (!(bool)Reflect.Method(utility, "RectangleContainsScreenPoint", Anchors.Type("UnityEngine.RectTransform"), vector, camera)
            .Invoke(null, [Reflect.Get(window, "_viewportRect"), screen, null])!) return;
        object?[] args = [rect, screen, null, Anchors.Vector2(0, 0)];
        if (!(bool)Reflect.Method(utility, "ScreenPointToLocalPointInRectangle", Anchors.Type("UnityEngine.RectTransform"), vector, camera, vector.MakeByRefType()).Invoke(null, args)!) return;
        var bounds = Reflect.Get(rect, "rect")!;
        float N(object value, string name) => Convert.ToSingle(Reflect.Get(value, name));
        var u = (N(args[3]!, "x") - N(bounds, "xMin")) / N(bounds, "width");
        var v = (N(args[3]!, "y") - N(bounds, "yMin")) / N(bounds, "height");
        var uv = Reflect.Get(Reflect.Get(window, "_mapRawImage")!, "uvRect")!;
        u = N(uv, "x") + u * N(uv, "width"); v = N(uv, "y") + v * N(uv, "height");
        var origin = Reflect.Call(manager, "WorldToMapNormalized", Anchors.Vector(0, 0, 0))!;
        var x = Reflect.Call(manager, "WorldToMapNormalized", Anchors.Vector(1, 0, 0))!;
        var z = Reflect.Call(manager, "WorldToMapNormalized", Anchors.Vector(0, 0, 1))!;
        var a = N(x, "x") - N(origin, "x"); var b = N(z, "x") - N(origin, "x");
        var c = N(x, "y") - N(origin, "y"); var d = N(z, "y") - N(origin, "y");
        var determinant = a * d - b * c;
        if (MathF.Abs(determinant) < 0.00000001f) return;
        u -= N(origin, "x"); v -= N(origin, "y");
        var goal = new Point((u * d - b * v) / determinant, Guide.Ground.Position(Supervisor.Current.Position).Y, (a * v - u * c) / determinant);
        var nearest = Read.Current?.Markers.Where(marker => marker.Kind is "chest" or "endRoom" or "bossRoom")
            .OrderBy(marker => marker.Position.Distance(goal)).FirstOrDefault();
        if (nearest is { } marker && marker.Position.Distance(goal) < 3) goal = marker.Position;
        Guide.Move.Stop(); Guide.Route.Select(goal);
    }
}
