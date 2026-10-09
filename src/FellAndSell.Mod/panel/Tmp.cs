namespace FellAndSell.Mod.Panel;

// Text uses the already verified TMP/Noto path. The owned overlay has no raycaster
// and never changes the game's Canvas, skin or individual font assignments.
internal static class Tmp
{
    private static object? _root;
    private static readonly List<(object GameObject, object Text)> Labels = [];
    private static int _used;
    internal static void Begin()
    {
        _used = 0;
        if (Alive.Is(_root)) { Reflect.Call(_root!, "SetActive", true); return; }
        Labels.Clear();
        _root = Activator.CreateInstance(Anchors.Type("UnityEngine.GameObject"), ["FellAndSell.Mod.Panel"]);
        var canvas = Add(_root!, Anchors.Type("UnityEngine.Canvas"));
        Reflect.Set(canvas, "renderMode", Enum.Parse(Anchors.Type("UnityEngine.RenderMode"), "ScreenSpaceOverlay"));
        Reflect.Set(canvas, "sortingOrder", 32760);
        Reflect.Set(canvas, "pixelPerfect", true);
        Reflect.Method(Anchors.Type("UnityEngine.Object"), "DontDestroyOnLoad", Anchors.Type("UnityEngine.Object")).Invoke(null, [_root]);
        I18n.FontFallback.VerifyPanelGlyphs(Text.Glyphs);
        MelonLoader.MelonLogger.Msg("panel: owned TMP Canvas and Noto font ready; game skin unchanged");
    }
    internal static void Label(float x, float y, float width, float height, string text)
    {
        if (_used == Labels.Count)
        {
            var gameObject = Activator.CreateInstance(Anchors.Type("UnityEngine.GameObject"), ["Label " + _used])!;
            var component = Add(gameObject, I18n.RuntimeTypes.Require("TMPro.TextMeshProUGUI"));
            var rect = Reflect.Get(component, "rectTransform")!;
            Reflect.Call(rect, "SetParent", Reflect.Get(_root!, "transform")!, false);
            Reflect.Set(rect, "anchorMin", Anchors.Vector2(0, 1));
            Reflect.Set(rect, "anchorMax", Anchors.Vector2(0, 1));
            Reflect.Set(rect, "pivot", Anchors.Vector2(0, 1));
            Reflect.Set(component, "font", I18n.FontFallback.PanelFont);
            Reflect.Set(component, "fontSize", 16f);
            Reflect.Set(component, "raycastTarget", false);
            Reflect.Set(component, "richText", false);
            Labels.Add((gameObject, component));
        }
        var label = Labels[_used++];
        Reflect.Call(label.GameObject, "SetActive", true);
        var transform = Reflect.Get(label.Text, "rectTransform")!;
        Reflect.Set(transform, "anchoredPosition", Anchors.Vector2(x, -y));
        Reflect.Set(transform, "sizeDelta", Anchors.Vector2(width, height));
        Reflect.Set(label.Text, "text", text);
    }
    internal static void End()
    {
        for (var index = _used; index < Labels.Count; index++) Reflect.Call(Labels[index].GameObject, "SetActive", false);
    }
    internal static void Hide()
    {
        if (Alive.Is(_root)) Reflect.Call(_root!, "SetActive", false);
    }
    private static object Add(object gameObject, Type type) => gameObject.GetType().GetMethods()
        .Single(method => method.Name == "AddComponent" && method.IsGenericMethodDefinition && method.GetParameters().Length == 0)
        .MakeGenericMethod(type).Invoke(gameObject, null)!;
}
