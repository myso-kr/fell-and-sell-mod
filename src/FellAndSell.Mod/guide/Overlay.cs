using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal static class Overlay
{
    private static readonly float[] Alpha = new float[7];
    private static readonly bool[] Blocked = new bool[7];
    private static Point? _goal;
    private static float _time;
    private static long _nextSight;
    internal static void Tick()
    {
        if (!Config.Guide || Route.Corners.Length < 2 || Map.Window.Open || Panel.Widget.Visible)
        { Glow.Hide(); return; }
        var camera = Anchors.Static("UnityEngine.Camera", "main");
        if (!Alive.Is(camera)) { Glow.Hide(); return; }
        var height = Convert.ToSingle(Anchors.Static("UnityEngine.Screen", "height"));
        var delta = Math.Clamp(Convert.ToSingle(Anchors.Static("UnityEngine.Time", "unscaledDeltaTime")), 0, .1f);
        if (_goal != Route.Goal) { _goal = Route.Goal; _time = 0; Array.Clear(Alpha); _nextSight = 0; }
        _time += delta;
        var origin = State.Position(Reflect.Get(Reflect.Get(camera!, "transform")!, "position")!);
        var refresh = Environment.TickCount64 >= _nextSight;
        if (refresh) _nextSight = Environment.TickCount64 + 125;
        for (var index = 6; index >= 0; index--)
        {
            var spark = Wisp.Sample(Route.Corners, Supervisor.Current.Position, _time - index * .12f);
            var point = spark.Position with { Y = spark.Position.Y + .65f + .08f * MathF.Sin(_time * 5 - index * .3f) };
            if (refresh) Blocked[index] = Occlusion.Blocked(origin, point);
            var projected = State.Position(Reflect.Call(camera!, "WorldToScreenPoint", State.Vector(point))!);
            var target = projected.Z <= .1f ? 0 : spark.Opacity * Wisp.Fade(origin.Distance(point), Blocked[index]);
            Alpha[index] = Wisp.Smooth(Alpha[index], target, delta);
            var size = Math.Clamp(height * .28f / Math.Max(.5f, projected.Z), 5, 90) * (1 - index * .09f);
            Glow.Show(index, projected.X, projected.Y, size, projected.Z <= .1f ? 0 : Alpha[index] * (1 - index * .13f), Route.Complete);
        }
    }
    internal static void Reset() { Glow.Reset(); _goal = null; _time = 0; _nextSight = 0; Array.Clear(Alpha); }
}
