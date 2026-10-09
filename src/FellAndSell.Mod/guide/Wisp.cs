using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal readonly record struct Spark(Point Position, float Opacity);

// Arc-length animation stays on the route through turns and stair segments.
internal static class Wisp
{
    internal static Spark Sample(Point[] path, Point player, float seconds)
    {
        if (path.Length < 2) return default;
        var lengths = new float[path.Length];
        var nearest = float.MaxValue;
        var origin = 0f;
        for (var index = 1; index < path.Length; index++)
        {
            var a = path[index - 1]; var b = path[index];
            var length = a.Distance(b);
            lengths[index] = lengths[index - 1] + length;
            if (length < .001f) continue;
            var t = Math.Clamp(((player.X - a.X) * (b.X - a.X) + (player.Y - a.Y) * (b.Y - a.Y)
                + (player.Z - a.Z) * (b.Z - a.Z)) / (length * length), 0, 1);
            var distance = player.Distance(Lerp(a, b, t));
            if (distance >= nearest) continue;
            nearest = distance; origin = lengths[index - 1] + length * t;
        }
        var remaining = lengths[^1] - origin;
        if (remaining < .05f) return new(path[^1], 0);
        var span = Math.Min(14, remaining);
        var progress = Math.Max(0, seconds) * 2.8f % span;
        var distanceAlong = origin + progress;
        for (var index = 1; index < path.Length; index++)
        {
            if (distanceAlong > lengths[index]) continue;
            var length = lengths[index] - lengths[index - 1];
            var position = Lerp(path[index - 1], path[index], length < .001f ? 0 : (distanceAlong - lengths[index - 1]) / length);
            // Fade at each loop boundary so returning to the player never flashes.
            var opacity = Math.Clamp(progress / .8f, 0, 1) * Math.Clamp((span - progress) / .8f, 0, 1);
            return new(position, opacity);
        }
        return new(path[^1], 0);
    }
    internal static float Fade(float distance, bool blocked) => blocked ? 0 : 1 - Math.Clamp((distance - 8) / 18, 0, 1);
    internal static float Smooth(float current, float target, float delta) => current + (target - current) * (1 - MathF.Exp(-8 * Math.Max(0, delta)));
    private static Point Lerp(Point a, Point b, float t) => new(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t);
}
