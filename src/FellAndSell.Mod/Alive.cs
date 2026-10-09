namespace FellAndSell.Mod;

internal static class Alive
{
    internal static bool Is(object? instance) => instance != null && (bool)Reflect.Method(
        Anchors.Type("UnityEngine.Object"), "op_Implicit", Anchors.Type("UnityEngine.Object"))
        .Invoke(null, [instance])!;
}
