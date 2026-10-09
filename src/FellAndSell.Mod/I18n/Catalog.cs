using System.Text.Json;
using MelonLoader;

namespace FellAndSell.Mod.I18n;

internal static class Catalog
{
    private static Dictionary<string, string> _strings = new(StringComparer.Ordinal);
    internal static string Root => Path.Combine(MelonLoader.Utils.MelonEnvironment.UserDataDirectory, "FellAndSell");
    internal static string HangulGlyphs => new(_strings.Values.SelectMany(text => text)
        .Where(character => character is >= '\uac00' and <= '\ud7a3' or >= '\u3130' and <= '\u318f')
        .Distinct().ToArray());

    internal static void Load()
    {
        var path = Path.Combine(Root, "locale", "ko", "strings.json");
        if (!File.Exists(path)) throw new FileNotFoundException("Korean catalog is missing", path);
        _strings = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path))
            ?? throw new InvalidDataException("Korean catalog must be an object");
        if (_strings.Any(entry => string.IsNullOrWhiteSpace(entry.Value)))
            throw new InvalidDataException("Korean translations must be non-empty strings");
        MelonLogger.Msg($"i18n: loaded {_strings.Count} Korean entries");
    }

    internal static bool TryGet(object entry, out string translation)
    {
        translation = "";
        var table = RuntimeTypes.Get(entry, "Table");
        if (table == null) return false;
        var collection = RuntimeTypes.Get(table, "TableCollectionName") as string;
        var id = RuntimeTypes.Get(entry, "KeyId");
        return _strings.TryGetValue($"{collection}/{id}", out translation!);
    }
}
