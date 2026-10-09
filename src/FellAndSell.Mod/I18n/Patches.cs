using System.Reflection;
using HarmonyLib;
using MelonLoader;

namespace FellAndSell.Mod.I18n;

internal static class Patches
{
    private static bool _reportedError;
    private static int _hits;

    internal static void Apply(HarmonyLib.Harmony harmony)
    {
        var entry = RuntimeTypes.Require("UnityEngine.Localization.Tables.StringTableEntry");
        var formatted = entry.GetMethods().Single(method => method.Name == "GetLocalizedString");
        harmony.Patch(formatted, prefix: new HarmonyMethod(typeof(Patches), nameof(BeforeFormat)));
        var rawGetter = RuntimeTypes.Require("UnityEngine.Localization.Tables.TableEntry")
            .GetProperty("LocalizedValue")!.GetMethod!;
        harmony.Patch(rawGetter, postfix: new HarmonyMethod(typeof(Patches), nameof(AfterRaw)));
        MelonLogger.Msg("i18n: installed raw and formatted string hooks");
    }

    private static void BeforeFormat(object __instance)
    {
        try
        {
            if (!Catalog.TryGet(__instance, out var translation)) return;
            var data = RuntimeTypes.Get(__instance, "Data");
            if (data == null || Equals(RuntimeTypes.Get(data, "Localized"), translation)) return;
            data.GetType().GetProperty("Localized")!.SetValue(data, translation);
            // Cached SmartFormat parsing must be rebuilt after replacing the source template.
            __instance.GetType().GetProperty("m_FormatCache")?.SetValue(__instance, null);
            Hit();
        }
        catch (Exception error) { Report(error); }
    }

    private static void AfterRaw(object __instance, ref string __result)
    {
        try
        {
            if (!Catalog.TryGet(__instance, out var translation)) return;
            __result = translation;
            Hit();
        }
        catch (Exception error) { Report(error); }
    }

    private static void Hit()
    {
        if (++_hits <= 3) MelonLogger.Msg($"i18n: translation hook hit {_hits}");
    }

    private static void Report(Exception error)
    {
        if (_reportedError) return;
        _reportedError = true;
        MelonLogger.Error($"i18n hook failed: {error}");
    }
}
