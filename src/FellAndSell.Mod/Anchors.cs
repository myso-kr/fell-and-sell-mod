using System.Reflection;

namespace FellAndSell.Mod;

internal static class Anchors
{
    private static readonly Dictionary<string, Type> Types = [];
    private static readonly Dictionary<string, (object? Owner, long Next)> Instances = [];
    internal static void ClearInstances() => Instances.Clear();
    internal static Type Type(string name)
    {
        if (Types.TryGetValue(name, out var cached)) return cached;
        var assembly = name.StartsWith("Il2Cpp", StringComparison.Ordinal) ? "Assembly-CSharp"
            : name.Contains("NavMesh", StringComparison.Ordinal) ? "UnityEngine.AIModule"
            : name == "UnityEngine.Input" || name == "UnityEngine.KeyCode" ? "UnityEngine.InputLegacyModule"
            : name.StartsWith("UnityEngine.GUI", StringComparison.Ordinal) || name == "UnityEngine.Event" ? "UnityEngine.IMGUIModule"
            : name == "UnityEngine.Font" ? "UnityEngine.TextRenderingModule"
            : name.StartsWith("UnityEngine.UI.", StringComparison.Ordinal) ? "UnityEngine.UI"
            : name == "UnityEngine.RectTransformUtility" || name == "UnityEngine.Canvas" || name == "UnityEngine.RenderMode" ? "UnityEngine.UIModule"
            : name == "UnityEngine.Collider" || name == "UnityEngine.Physics" || name == "UnityEngine.QueryTriggerInteraction" ? "UnityEngine.PhysicsModule"
            : "UnityEngine.CoreModule";
        var type = AppDomain.CurrentDomain.GetAssemblies().Select(value => value.GetType(name))
            .FirstOrDefault(value => value != null) ?? Assembly.Load(assembly).GetType(name)
            ?? throw new TypeLoadException(name);
        Types[name] = type;
        return type;
    }
    internal static object? Static(string type, string member) => Reflect.Read(Reflect.Member(Type(type), member), null);
    internal static object? Instance(string name)
    {
        // Read the backing field. A singleton getter is allowed to create UI/state.
        var instance = Static("Il2Cpp." + name, name == "DungeonMinimapManager" ? "_instance" : "_Instance_k__BackingField");
        if (Alive.Is(instance)) return instance;
        // During scene replacement a destroyed singleton can clear a newer
        // instance's field. Locate an existing scene component; never create one.
        if (Instances.TryGetValue(name, out var cached))
        {
            if (Alive.Is(cached.Owner) && (bool)Reflect.Get(cached.Owner!, "isActiveAndEnabled")!) return cached.Owner;
            if (Environment.TickCount64 < cached.Next) return null;
        }
        var found = I18n.RuntimeTypes.All(Type("Il2Cpp." + name)).FirstOrDefault(value =>
            Alive.Is(value) && (bool)Reflect.Get(value, "isActiveAndEnabled")!
            && (bool)Reflect.Get(Reflect.Get(Reflect.Get(value, "gameObject")!, "scene")!, "isLoaded")!);
        Instances[name] = (found, Environment.TickCount64 + 500);
        return found;
    }
    internal static object Vector(float x, float y, float z) => Activator.CreateInstance(Type("UnityEngine.Vector3"), x, y, z)!;
    internal static object Vector2(float x, float y) => Activator.CreateInstance(Type("UnityEngine.Vector2"), x, y)!;
}
