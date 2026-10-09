namespace FellAndSell.Mod.Panel;

// Own styles only: never mutate the game's shared GUISkin or font assignments.
internal static class Gui
{
    private static Type GUI => Anchors.Type("UnityEngine.GUI");
    private static object? _box, _fill;
    internal static void WithFont(Action draw)
    {
        Tmp.Begin();
        if (_box == null)
        {
            var skin = Anchors.Static("UnityEngine.GUI", "skin")!;
            var style = Anchors.Type("UnityEngine.GUIStyle");
            _box = Reflect.Get(skin, "box");
            _fill = Activator.CreateInstance(style)!;
            Reflect.Set(Reflect.Get(_fill, "normal")!, "background", Anchors.Static("UnityEngine.Texture2D", "whiteTexture"));
        }
        try { draw(); }
        finally { Tmp.End(); }
    }
    private static object Rect(float x, float y, float width, float height) =>
        Activator.CreateInstance(Anchors.Type("UnityEngine.Rect"), x, y, width, height)!;
    internal static bool Button(float x, float y, float width, string text)
    {
        Tmp.Label(x + 8, y + 4, width - 16, 24, text);
        return (bool)Reflect.Method(GUI, "Button", Anchors.Type("UnityEngine.Rect"), typeof(string))
            .Invoke(null, [Rect(x, y, width, 28), ""])!;
    }
    internal static void Label(float x, float y, float width, string text) => Tmp.Label(x, y, width, 28, text);
    internal static void Box(float x, float y, float width, float height) =>
        Reflect.Method(GUI, "Box", Anchors.Type("UnityEngine.Rect"), Anchors.Type("UnityEngine.GUIContent"), Anchors.Type("UnityEngine.GUIStyle"))
            .Invoke(null, [Rect(x, y, width, height), Anchors.Static("UnityEngine.GUIContent", "none"), _box]);
    internal static void Fill(float x, float y, float width, float height, string kind)
    {
        var color = Reflect.Read(Reflect.Member(GUI, "color"), null);
        var rgb = kind switch
        {
            "enemy" => (1f, 0.2f, 0.2f), "chest" => (1f, 0.8f, 0.2f),
            "route" or "player" => (0.2f, 1f, 0.65f), "partial" => (1f, 0.5f, 0.1f),
            "bossRoom" => (1f, 0.3f, 0.7f), "endRoom" => (0.3f, 0.8f, 1f),
            "stairs" => (0.8f, 0.6f, 1f), _ => (0.45f, 0.5f, 0.55f)
        };
        var property = (System.Reflection.PropertyInfo)Reflect.Member(GUI, "color")!;
        try
        {
            property.SetValue(null, Activator.CreateInstance(Anchors.Type("UnityEngine.Color"), rgb.Item1, rgb.Item2, rgb.Item3, 1f));
            // DrawTexture's restored wrapper reaches an unstripping stub in this game.
            // Native Box with our white-background style draws the same solid quad.
            Reflect.Method(GUI, "Box", Anchors.Type("UnityEngine.Rect"), Anchors.Type("UnityEngine.GUIContent"), Anchors.Type("UnityEngine.GUIStyle"))
                .Invoke(null, [Rect(x, y, Math.Max(1, width), Math.Max(1, height)), Anchors.Static("UnityEngine.GUIContent", "none"), _fill]);
        }
        finally { property.SetValue(null, color); }
    }
    internal static void Line(float x, float y, float targetX, float targetY, string kind)
    {
        var length = MathF.Sqrt((x - targetX) * (x - targetX) + (y - targetY) * (y - targetY));
        if (length < 1) return;
        var property = (System.Reflection.PropertyInfo)Reflect.Member(GUI, "matrix")!;
        var original = property.GetValue(null);
        try
        {
            var angle = MathF.Atan2(targetY - y, targetX - x) * 180f / MathF.PI;
            Reflect.Method(Anchors.Type("UnityEngine.GUIUtility"), "RotateAroundPivot", typeof(float), Anchors.Type("UnityEngine.Vector2"))
                .Invoke(null, [angle, Anchors.Vector2(x, y)]);
            Fill(x, y, length, 3, kind);
        }
        finally { property.SetValue(null, original); }
    }
}
