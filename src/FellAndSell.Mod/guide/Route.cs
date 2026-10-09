using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal static class Route
{
    internal static Point[] Corners { get; private set; } = [];
    internal static string Status { get; private set; } = "idle";
    internal static bool Complete { get; private set; }
    private static readonly Target Target = new();
    internal static Point? Goal => Target.Goal;
    private static long _next;
    private static object? _path, _costs;
    private static bool _enabled;
    private static readonly Dictionary<string, string> Diagnostics = [];
    internal static void Select(Point goal) { Clear(); Target.Select(goal); _next = 0; }
    internal static void Clear() { Target.Reset(); Corners = []; Complete = false; Status = "idle"; Diagnostics.Clear(); }
    internal static void Cancel() { Clear(); Move.Stop(); _next = 0; }
    internal static void Tick(Snapshot state)
    {
        if (!Config.Guide) { _enabled = false; Complete = false; return; }
        if (!_enabled) { Target.Reset(); _enabled = true; }
        if (!state.Ready || Map.Read.Current == null) { Complete = false; return; }
        // Keep a following route immutable so replanning cannot erase stuck timing.
        // A blocked door stops the follower; the next explicit start uses a fresh path.
        if (Move.Follower.Status == Motion.Following) return;
        if (Environment.TickCount64 < _next) return;
        _next = Environment.TickCount64 + 500;
        if (Target.NeedsDefault) Target.Default(Exit.Read(state.Position));
        if (Goal == null) { Set([], "idle", false); return; }
        var feet = Ground.Position(state.Position);
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
            // Agent 0 has a retained native CalculatePath wrapper. Avoid its restored
            // QueryFilter/Span wrapper where the game's default agent suffices.
            var filtered = agent != 0;
            var calculate = filtered ? nav.GetMethods().Single(value => value.Name == "CalculatePathFilterInternal")
                : Reflect.Method(nav, "CalculatePath", vector, vector, typeof(int), pathType);
            if (filtered) _costs ??= Activator.CreateInstance(calculate.GetParameters()[5].ParameterType,
                [Enumerable.Repeat(1f, 32).ToArray()]);
            Point? Sample(Point point)
            {
                object?[] args = [State.Vector(point), Activator.CreateInstance(hit), Search.Radius, agent, 1];
                return (bool)sample.Invoke(null, args)! ? State.Position(Reflect.Get(args[1]!, "position")!) : null;
            }
            Result Calculate(Point from, Point to)
            {
                Reflect.Call(path, "ClearCorners");
                var arguments = filtered ? new object?[] { State.Vector(from), State.Vector(to), path, agent, 1, _costs }
                    : [State.Vector(from), State.Vector(to), 1, path];
                var success = (bool)calculate.Invoke(null, arguments)!;
                var status = Reflect.Get(path, "status")!.ToString()!;
                var corners = Sequence.Items(Reflect.Get(path, "corners")).Select(State.Position).ToArray();
                return new(corners, status == "PathComplete" && success ? "complete" : status == "PathPartial" ? "partial" : "invalid");
            }
            var result = Search.Find(feet, Goal.Value, Sample, Calculate);
            Diagnose("path", $"{agent}:{result.Status}:{result.Corners.Length}",
                $"agent={agent} feet={feet} goal={Goal.Value} from={result.Start} to={result.End} status={result.Status} corners={result.Corners.Length}");
            Set(result.Corners, result.Status, result.Complete);
        }
    }
    private static void Set(Point[] corners, string status, bool complete) { Corners = corners; Status = status; Complete = complete; }
    private static void Diagnose(string phase, string key, string message)
    {
        if (Diagnostics.TryGetValue(phase, out var previous) && previous == key) return;
        Diagnostics[phase] = key;
        MelonLoader.MelonLogger.Msg("route: " + message);
    }
}
