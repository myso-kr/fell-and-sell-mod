using System.Reflection;

namespace FellAndSell.Mod.I18n;

internal static class RuntimeTypes
{
    internal static Type Require(string name)
    {
        var found = AppDomain.CurrentDomain.GetAssemblies()
            .Select(assembly => assembly.GetType(name))
            .FirstOrDefault(type => type != null);
        if (found != null) return found;
        // Generated interop assemblies are loaded lazily by MelonLoader.
        var assemblyName = name.StartsWith("TMPro.", StringComparison.Ordinal)
            ? "Unity.TextMeshPro"
            : name.StartsWith("UnityEngine.Localization.", StringComparison.Ordinal)
                ? "Unity.Localization" : "UnityEngine.CoreModule";
        var loaded = Assembly.Load(assemblyName);
        return loaded.GetType(name) ?? loaded.GetType("Il2Cpp" + name)
            ?? throw new TypeLoadException(name);
    }

    internal static object? Get(object instance, string property) =>
        instance.GetType().GetProperty(property)?.GetValue(instance);

    internal static IEnumerable<object> All(Type type)
    {
        var method = Require("UnityEngine.Resources").GetMethods()
            .Single(candidate => candidate.Name == "FindObjectsOfTypeAll"
                && candidate.IsGenericMethodDefinition && candidate.GetParameters().Length == 0);
        var result = method.MakeGenericMethod(type).Invoke(null, null);
        return result is System.Collections.IEnumerable items ? items.Cast<object>() : [];
    }
}
