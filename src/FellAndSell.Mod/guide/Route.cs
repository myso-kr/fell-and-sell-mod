using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal static class Route
{
    internal static Point[] Corners { get; private set; } = [];
    internal static string Status { get; private set; } = "idle";
    internal static bool Complete { get; private set; }
    internal static Point? Goal { get; private set; }
    private static long _next;
    private static object? _path, _costs;
    internal static void Select(Point goal) { Clear(); Goal = goal; _next = 0; }
    internal static void Clear() { Goal = null; Corners = []; Complete = false; Status = "idle"; }
    internal static void Tick(Snapshot state)
    {
        if (!Config.Guide || !state.Ready || Goal == null) { Complete = false; return; }
        // Keep a following route immutable so replanning cannot erase stuck timing.
        // A blocked door stops the follower; the next explicit start uses a fresh path.
        if (Move.Follower.Status == Motion.Following) return;
        if (Environment.TickCount64 < _next) return;
        _next = Environment.TickCount64 + 500;
        var nav = Anchors.Type("UnityEngine.AI.NavMesh");
        var pathType = Anchors.Type("UnityEngine.AI.NavMeshPath");
        var path = _path ??= Activator.CreateInstance(pathType)!;
        {
            // Query using the player's baked surface agent type and walkable area mask.
            var generator = I18n.RuntimeTypes.All(Anchors.Type("Il2Cpp.CustomDungeonGenerator"))
                .FirstOrDefault(value => Alive.Is(value) && (bool)Reflect.Get(value, "isActiveAndEnabled")!);
            if (generator == null || (bool)Reflect.Get(generator, "isGenerating")!) { Set([], "notReady", false); return; }
            var surface = Reflect.Get(generator, "navMeshSurface");
            if (!Alive.Is(surface)) { Set([], "notReady", false); return; }
            var agent = Convert.ToInt32(Reflect.Get(surface!, "agentTypeID"));
            // This IL2CPP build strips the managed QueryFilter overload, but retains
            // its native backing methods. Only Walkable (area 0), never Jump links.
            var hit = Anchors.Type("UnityEngine.AI.NavMeshHit");
            var vector = Anchors.Type("UnityEngine.Vector3");
            var sample = Reflect.Method(nav, "SamplePositionFilter_Injected", vector.MakeByRefType(),
                hit.MakeByRefType(), typeof(float), typeof(int), typeof(int));
            object?[] source = [State.Vector(state.Position), Activator.CreateInstance(hit), 2f, agent, 1];
            object?[] target = [State.Vector(Goal.Value), Activator.CreateInstance(hit), 2f, agent, 1];
            if (!(bool)sample.Invoke(null, source)! || !(bool)sample.Invoke(null, target)!) { Set([], "invalid", false); return; }
            var from = Reflect.Get(source[1]!, "position")!;
            var to = Reflect.Get(target[1]!, "position")!;
            if (MathF.Abs(State.Position(to).Y - Goal.Value.Y) > 1.25f
                || MathF.Abs(State.Position(from).Y - state.Position.Y) > 1.25f) { Set([], "invalid", false); return; }
            var calculate = nav.GetMethods().Single(value => value.Name == "CalculatePathFilterInternal");
            _costs ??= Activator.CreateInstance(calculate.GetParameters()[5].ParameterType,
                [Enumerable.Repeat(1f, 32).ToArray()]);
            Reflect.Call(path, "ClearCorners");
            var success = (bool)calculate.Invoke(null, [from, to, path, agent, 1, _costs])!;
            var status = Reflect.Get(path, "status")!.ToString()!;
            var corners = ((System.Collections.IEnumerable)Reflect.Get(path, "corners")!).Cast<object>()
                .Select(State.Position).ToArray();
            Set(corners, status == "PathComplete" && success ? "complete" : status == "PathPartial" ? "partial" : "invalid",
                success && status == "PathComplete" && corners.Length >= 2);
        }
    }
    private static void Set(Point[] corners, string status, bool complete) { Corners = corners; Status = status; Complete = complete; }
}
