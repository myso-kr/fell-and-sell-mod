using System.Text.Json;

namespace FellAndSell.Mod.Panel;

internal static class Text
{
    private static Dictionary<string, string> _strings = [];
    internal static void Load() => _strings = JsonSerializer.Deserialize<Dictionary<string, string>>(
        File.ReadAllText(Path.Combine(I18n.Catalog.Root, "locale", "ko", "mod-ui.json")))
        ?? throw new InvalidDataException("Mod UI catalog must be an object");
    internal static string Get(string key) => _strings.GetValueOrDefault(key, key);
    internal static IEnumerable<char> Glyphs => _strings.Values.SelectMany(value => value).Distinct();
}
