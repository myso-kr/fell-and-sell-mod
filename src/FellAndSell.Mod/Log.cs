using MelonLoader;

namespace FellAndSell.Mod;

internal static class Log
{
    private static readonly HashSet<string> Failed = [];
    internal static bool Guard(string feature, Action action)
    {
        if (Failed.Contains(feature)) return false;
        try { action(); return true; }
        catch (Exception error)
        {
            Failed.Add(feature);
            MelonLogger.Error($"{feature}: disabled for this scene: {error}");
            return false;
        }
    }
    internal static bool Available(string feature) => !Failed.Contains(feature);
    internal static void Reset() => Failed.Clear();
}
