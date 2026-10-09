namespace FellAndSell.Mod.Autoplay;

internal static class Sight
{
    internal static bool Clear(object collider)
    {
        if (!Alive.Is(State.Player) || !Alive.Is(collider)) return false;
        var player = Reflect.Get(State.Player!, "transform")!;
        var target = Reflect.Get(collider, "transform")!;
        var origin = State.Position(Reflect.Get(player, "position")!);
        origin = origin with { Y = origin.Y + 0.75f };
        var destination = State.Position(Reflect.Get(Reflect.Get(collider, "bounds")!, "center")!);
        var length = origin.Distance(destination);
        if (length < 0.01f) return true;
        var direction = new Point((destination.X - origin.X) / length, (destination.Y - origin.Y) / length, (destination.Z - origin.Z) / length);
        var physics = System.Reflection.Assembly.Load("UnityEngine.PhysicsModule").GetType("UnityEngine.Physics")!;
        var triggers = System.Reflection.Assembly.Load("UnityEngine.PhysicsModule").GetType("UnityEngine.QueryTriggerInteraction")!;
        var hits = Reflect.Method(physics, "RaycastAll", Anchors.Type("UnityEngine.Vector3"), Anchors.Type("UnityEngine.Vector3"),
            typeof(float), typeof(int), triggers).Invoke(null, [State.Vector(origin), State.Vector(direction), length, -1, Enum.Parse(triggers, "Ignore")])!;
        foreach (var hit in ((System.Collections.IEnumerable)hits).Cast<object>())
        {
            var transform = Reflect.Get(hit, "transform");
            if (transform == null) continue;
            // Never compare scene roots: dungeon props and walls can share a root.
            if (Equals(transform, player) || (bool)Reflect.Call(transform, "IsChildOf", player)!) continue;
            if (Equals(transform, target) || (bool)Reflect.Call(transform, "IsChildOf", target)!) continue;
            return false;
        }
        return true;
    }
}
