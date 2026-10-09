using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal static class Occlusion
{
    internal static bool Blocked(Point origin, Point point)
    {
        var distance = origin.Distance(point);
        if (distance < .2f) return false;
        var direction = new Point((point.X - origin.X) / distance, (point.Y - origin.Y) / distance, (point.Z - origin.Z) / distance);
        var triggers = Anchors.Type("UnityEngine.QueryTriggerInteraction");
        var hits = Reflect.Method(Anchors.Type("UnityEngine.Physics"), "RaycastAll", Anchors.Type("UnityEngine.Vector3"),
            Anchors.Type("UnityEngine.Vector3"), typeof(float), typeof(int), triggers)
            .Invoke(null, [State.Vector(origin), State.Vector(direction), distance - .15f, -1, Enum.Parse(triggers, "Ignore")]);
        var player = Alive.Is(State.Player) ? Reflect.Get(State.Player!, "transform") : null;
        foreach (var hit in Sequence.Items(hits))
        {
            var transform = Reflect.Get(hit, "transform");
            if (transform == null) continue;
            if (player != null && (Equals(transform, player) || (bool)Reflect.Call(transform, "IsChildOf", player)!)) continue;
            return true;
        }
        return false;
    }
}
