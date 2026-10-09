namespace FellAndSell.Mod.Guide;

// Owned screen-space billboards use projected world positions and explicit occlusion.
internal static class Glow
{
    private static object? _root, _texture;
    private static readonly List<(object Root, object Image, object Rect)> Images = [];
    internal static void Show(int index, float x, float y, float size, float alpha, bool complete)
    {
        if (!Alive.Is(_root)) Create();
        Reflect.Call(_root!, "SetActive", true);
        while (Images.Count <= index)
        {
            var root = Activator.CreateInstance(Anchors.Type("UnityEngine.GameObject"), ["Fairy spark"]);
            var image = Add(root!, Anchors.Type("UnityEngine.UI.RawImage"));
            var rect = Reflect.Get(image, "rectTransform")!;
            Reflect.Call(rect, "SetParent", Reflect.Get(_root!, "transform")!, false);
            Reflect.Set(rect, "anchorMin", Anchors.Vector2(0, 0));
            Reflect.Set(rect, "anchorMax", Anchors.Vector2(0, 0));
            Reflect.Set(image, "raycastTarget", false);
            Reflect.Set(image, "texture", _texture);
            Images.Add((root!, image, rect));
        }
        var sprite = Images[index];
        Reflect.Call(sprite.Root, "SetActive", alpha > .002f);
        Reflect.Set(sprite.Rect, "anchoredPosition", Anchors.Vector2(x, y));
        Reflect.Set(sprite.Rect, "sizeDelta", Anchors.Vector2(size, size));
        Reflect.Set(sprite.Image, "color", Activator.CreateInstance(Anchors.Type("UnityEngine.Color"), 1f, complete ? .92f : .55f, complete ? .65f : .2f, alpha));
    }
    private static void Create()
    {
        Images.Clear();
        _root = Activator.CreateInstance(Anchors.Type("UnityEngine.GameObject"), ["FellAndSell.Mod.Fairy"]);
        var canvas = Add(_root!, Anchors.Type("UnityEngine.Canvas"));
        Reflect.Set(canvas, "renderMode", Enum.Parse(Anchors.Type("UnityEngine.RenderMode"), "ScreenSpaceOverlay"));
        Reflect.Set(canvas, "sortingOrder", 32750);
        var format = Enum.Parse(Anchors.Type("UnityEngine.TextureFormat"), "RGBA32");
        _texture = Activator.CreateInstance(Anchors.Type("UnityEngine.Texture2D"), 32, 32, format, false)!;
        var color = Anchors.Type("UnityEngine.Color32");
        var pixels = Array.CreateInstance(color, 1024);
        for (var y = 0; y < 32; y++)
        for (var x = 0; x < 32; x++)
        {
            var radius = MathF.Sqrt((x - 15.5f) * (x - 15.5f) + (y - 15.5f) * (y - 15.5f)) / 15.5f;
            var alpha = (byte)(255 * MathF.Pow(Math.Max(0, 1 - radius), 2));
            pixels.SetValue(Activator.CreateInstance(color, (byte)255, (byte)255, (byte)255, alpha), y * 32 + x);
        }
        var set = _texture.GetType().GetMethods().Single(method => method.Name == "SetPixels32" && method.GetParameters().Length == 2);
        var array = Activator.CreateInstance(set.GetParameters()[0].ParameterType, [pixels]);
        set.Invoke(_texture, [array, 0]);
        Reflect.Call(_texture, "Apply", false, false);
    }
    internal static void Hide() { if (Alive.Is(_root)) Reflect.Call(_root!, "SetActive", false); }
    internal static void Reset()
    {
        foreach (var value in new[] { _root, _texture })
            if (Alive.Is(value)) Reflect.Method(Anchors.Type("UnityEngine.Object"), "Destroy", Anchors.Type("UnityEngine.Object")).Invoke(null, [value]);
        _root = _texture = null; Images.Clear();
    }
    private static object Add(object root, Type type) => root.GetType().GetMethods()
        .Single(method => method.Name == "AddComponent" && method.IsGenericMethodDefinition && method.GetParameters().Length == 0)
        .MakeGenericMethod(type).Invoke(root, null)!;
}
