using System.Reflection;

namespace FellAndSell.Mod;

internal static class Anchors
{
    private static readonly Dictionary<string, Type> Types = [];
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
            : name == "UnityEngine.Collider" ? "UnityEngine.PhysicsModule"
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
        return Alive.Is(instance) ? instance : null;
    }
    internal static object Vector(float x, float y, float z) => Activator.CreateInstance(Type("UnityEngine.Vector3"), x, y, z)!;
    internal static object Vector2(float x, float y) => Activator.CreateInstance(Type("UnityEngine.Vector2"), x, y)!;
}
