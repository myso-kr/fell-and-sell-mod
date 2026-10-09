using System.Reflection;
using MelonLoader;

namespace FellAndSell.Mod.I18n;

internal static class FontFallback
{
    private static object? _font;
    private static bool _failed;
    private static bool _reportedText;

    internal static void Register()
    {
        if (_failed) return;
        try
        {
            var fontType = RuntimeTypes.Require("TMPro.TMP_FontAsset");
            if (_font == null)
            {
                var path = Path.Combine(Catalog.Root, "fonts", "NotoSansCJKkr-Regular.otf");
                if (!File.Exists(path)) throw new FileNotFoundException("Hangul font missing", path);
                var create = fontType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Single(method => method.Name == "CreateFontAsset"
                        && method.GetParameters().Length == 7
                        && method.GetParameters()[0].ParameterType == typeof(string));
                var renderMode = Enum.Parse(create.GetParameters()[4].ParameterType, "SDFAA");
                _font = create.Invoke(null, [path, 0, 64, 9, renderMode, 1024, 1024])
                    ?? throw new InvalidOperationException("CreateFontAsset returned null");
                fontType.GetProperty("isMultiAtlasTexturesEnabled")?.SetValue(_font, true);
                fontType.GetProperty("name")!.SetValue(_font, "Noto Sans CJK KR (Korean patch)");
                RuntimeTypes.Require("UnityEngine.Object").GetMethod("DontDestroyOnLoad")!.Invoke(null, [_font]);
                MelonLogger.Msg("font: created dynamic Noto Sans CJK KR fallback");
                var glyphs = Catalog.HangulGlyphs;
                var addCharacters = fontType.GetMethods().Single(method => method.Name == "TryAddCharacters"
                    && method.GetParameters().Length == 3
                    && method.GetParameters()[0].ParameterType == typeof(string));
                object?[] arguments = [glyphs, null, false];
                var success = (bool)addCharacters.Invoke(_font, arguments)!;
                MelonLogger.Msg($"font: verified {glyphs.Length} Hangul glyphs; missing={((string?)arguments[1])?.Length ?? 0}");
                if (!success) throw new InvalidOperationException("Font cannot render every authored Hangul glyph");
            }
            var added = 0;
            foreach (var font in RuntimeTypes.All(fontType))
            {
                if (Equals(font, _font)) continue;
                var property = fontType.GetProperty("fallbackFontAssetTable")!;
                var list = property.GetValue(font);
                if (list == null)
                {
                    list = Activator.CreateInstance(property.PropertyType)!;
                    property.SetValue(font, list);
                }
                if ((bool)list.GetType().GetMethod("Contains")!.Invoke(list, [_font])!) continue;
                list.GetType().GetMethod("Add")!.Invoke(list, [_font]);
                added++;
            }
            if (added > 0) MelonLogger.Msg($"font: fallback registered on {added} fonts");
            if (!_reportedText)
            {
                var textType = RuntimeTypes.Require("TMPro.TMP_Text");
                var count = RuntimeTypes.All(textType).Count(text =>
                    RuntimeTypes.Get(text, "text") is string value
                    && value.Any(character => character is >= '\uac00' and <= '\ud7a3'));
                if (count > 0)
                {
                    _reportedText = true;
                    MelonLogger.Msg($"i18n: found {count} Korean TMP text components");
                }
            }
        }
        catch (Exception error)
        {
            _failed = true;
            MelonLogger.Error($"font fallback failed: {error}");
        }
    }
}
