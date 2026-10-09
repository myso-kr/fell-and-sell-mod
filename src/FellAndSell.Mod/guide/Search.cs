using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal sealed record Result(Point[] Corners, string Status, Point? Start = null, Point? End = null)
{
    internal bool Complete => Status == "complete" && Corners.Length >= 2;
}

// Bounded endpoint projection and path ranking; only the native mesh connects rooms.
internal static class Search
{
    internal const float Radius = 2.25f;
    internal static Result Find(Point start, Point goal, Func<Point, Point?> sample, Func<Point, Point, Result> calculate)
    {
        var source = sample(start);
        if (source == null || source.Value.Distance(start) > Radius + .01f) return new([], "sourceMissing");
        Result? best = null;
        var bestScore = float.MaxValue;
        var seen = new List<Point>();
        foreach (var offset in new (float X, float Z)[] { (0, 0), (1, 0), (-1, 0), (0, 1), (0, -1), (.7f, .7f), (.7f, -.7f), (-.7f, .7f), (-.7f, -.7f) })
        {
            var candidate = sample(goal with { X = goal.X + offset.X, Z = goal.Z + offset.Z });
            if (candidate == null || MathF.Abs(candidate.Value.Y - goal.Y) > Radius + .01f
                || candidate.Value.Horizontal(goal) > 3 || seen.Any(value => value.Distance(candidate.Value) < .1f)) continue;
            seen.Add(candidate.Value);
            var route = calculate(source.Value, candidate.Value) with { Start = source, End = candidate };
            if (route.Corners.Length < 2 || route.Status is not ("complete" or "partial")) continue;
            var length = 0f;
            for (var index = 1; index < route.Corners.Length; index++) length += route.Corners[index - 1].Distance(route.Corners[index]);
            // Reachability first, then endpoint accuracy, then actual route length.
            var score = (route.Complete ? 0 : 1000000) + candidate.Value.Distance(goal) * 1000 + length;
            if (score >= bestScore) continue;
            best = route; bestScore = score;
        }
        return best ?? new([], seen.Count == 0 ? "targetMissing" : "invalid", source);
    }
}
