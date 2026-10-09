namespace FellAndSell.Mod.Map;

// Owns a child of the game's map image. Its texture is a copy, never discovery state.
internal sealed class Layer
{
    private object? _parent, _root, _image, _source, _texture;
    private readonly List<(object Root, object Rect, object Image)> _marks = [];
    private int _used;
    private long _revision = -1;
    internal void Tick(object? owner, object manager)
    {
        var parent = owner == null ? null : Reflect.Get(owner, "_mapRawImage");
        if (!Alive.Is(parent)) { Reset(); return; }
        if (!Equals(parent, _parent) || !Alive.Is(_root) || !Alive.Is(_image))
        {
            Reset();
            _parent = parent;
            _root = Activator.CreateInstance(Anchors.Type("UnityEngine.GameObject"), ["FellAndSell.Mod.MapReveal"]);
            _image = _root!.GetType().GetMethods().Single(method => method.Name == "AddComponent"
                && method.IsGenericMethodDefinition && method.GetParameters().Length == 0)
                .MakeGenericMethod(Anchors.Type("UnityEngine.UI.RawImage")).Invoke(_root, null)!;
            var rect = Reflect.Get(_image, "rectTransform")!;
            Reflect.Call(rect, "SetParent", Reflect.Get(parent!, "rectTransform")!, false);
            Reflect.Set(rect, "anchorMin", Anchors.Vector2(0, 0));
            Reflect.Set(rect, "anchorMax", Anchors.Vector2(1, 1));
            Reflect.Set(rect, "offsetMin", Anchors.Vector2(0, 0));
            Reflect.Set(rect, "offsetMax", Anchors.Vector2(0, 0));
            Reflect.Set(_image, "raycastTarget", false);
        }
        Reflect.Call(_root!, "SetActive", (Config.Map || Config.Guide) && Read.Current != null);
        Reflect.Set(_image!, "enabled", Config.Map);
        if (Read.Current == null) return;
        var source = Reflect.Get(manager, "MapTexture");
        if (!Alive.Is(source)) return;
        if (!Equals(source, _source) || _revision != Read.Revision || !Alive.Is(_texture))
        {
            Destroy(_texture);
            var width = Convert.ToInt32(Reflect.Get(source!, "width"));
            var height = Convert.ToInt32(Reflect.Get(source!, "height"));
            var format = Enum.Parse(Anchors.Type("UnityEngine.TextureFormat"), "RGBA32");
            _texture = Activator.CreateInstance(Anchors.Type("UnityEngine.Texture2D"), width, height, format, false)!;
            Reflect.Call(_texture, "SetPixels32", Reflect.Get(manager, "_fullMapBuffer")!, 0);
            Reflect.Call(_texture, "Apply", false, false);
            Reflect.Set(_image!, "texture", _texture);
            _source = source;
            _revision = Read.Revision;
        }
        Reflect.Set(_image!, "uvRect", Reflect.Get(parent!, "uvRect"));
        _used = 0;
        if (Config.Map)
            foreach (var marker in Read.Current.Markers) Mark(manager, marker.Position, 7, marker.Kind == "enemy" ? (1f, .2f, .2f) : (1f, .8f, .1f));
        if (Config.Guide)
        {
            var corners = Guide.Route.Corners;
            for (var index = 1; index < corners.Length; index++)
            {
                var from = corners[index - 1]; var to = corners[index];
                var steps = Math.Clamp((int)MathF.Ceiling(from.Distance(to) * 2), 1, 128);
                for (var step = 0; step <= steps; step++)
                {
                    var t = (float)step / steps;
                    Mark(manager, new(from.X + (to.X - from.X) * t, from.Y, from.Z + (to.Z - from.Z) * t), 3,
                        Guide.Route.Complete ? (.2f, 1f, .4f) : (1f, .5f, .1f));
                }
            }
            if (Guide.Route.Goal is { } goal) Mark(manager, goal, 10, (.2f, .8f, 1f));
        }
        for (var index = _used; index < _marks.Count; index++) Reflect.Call(_marks[index].Root, "SetActive", false);
    }
    private void Mark(object manager, Autoplay.Point point, float size, (float R, float G, float B) color)
    {
        if (_used >= 2048) return;
        if (_used == _marks.Count)
        {
            var root = Activator.CreateInstance(Anchors.Type("UnityEngine.GameObject"), ["Map marker"]);
            var image = root!.GetType().GetMethods().Single(method => method.Name == "AddComponent"
                && method.IsGenericMethodDefinition && method.GetParameters().Length == 0)
                .MakeGenericMethod(Anchors.Type("UnityEngine.UI.Image")).Invoke(root, null)!;
            var rect = Reflect.Get(image, "rectTransform")!;
            Reflect.Call(rect, "SetParent", Reflect.Get(_root!, "transform")!, false);
            Reflect.Set(image, "raycastTarget", false);
            _marks.Add((root, rect, image));
        }
        var mark = _marks[_used++];
        var uv = Reflect.Call(manager, "WorldToMapNormalized", Autoplay.State.Vector(point))!;
        var source = Reflect.Get(_parent!, "uvRect")!;
        float N(object value, string name) => Convert.ToSingle(Reflect.Get(value, name));
        var x = (N(uv, "x") - N(source, "x")) / N(source, "width");
        var y = (N(uv, "y") - N(source, "y")) / N(source, "height");
        Reflect.Call(mark.Root, "SetActive", x >= 0 && x <= 1 && y >= 0 && y <= 1);
        Reflect.Set(mark.Rect, "anchorMin", Anchors.Vector2(x, y));
        Reflect.Set(mark.Rect, "anchorMax", Anchors.Vector2(x, y));
        Reflect.Set(mark.Rect, "anchoredPosition", Anchors.Vector2(0, 0));
        Reflect.Set(mark.Rect, "sizeDelta", Anchors.Vector2(size, size));
        Reflect.Set(mark.Image, "color", Activator.CreateInstance(Anchors.Type("UnityEngine.Color"), color.R, color.G, color.B, 1f));
    }
    internal void Reset()
    {
        Destroy(_root); Destroy(_texture);
        _parent = _root = _image = _source = _texture = null;
        _marks.Clear();
        _revision = -1;
    }
    private static void Destroy(object? value)
    {
        if (Alive.Is(value)) Reflect.Method(Anchors.Type("UnityEngine.Object"), "Destroy", Anchors.Type("UnityEngine.Object")).Invoke(null, [value]);
    }
}
