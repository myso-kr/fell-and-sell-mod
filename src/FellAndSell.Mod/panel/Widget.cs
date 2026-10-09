using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Panel;

internal static class Widget
{
    internal static bool Visible;
    internal static bool StartRequested;
    private static readonly Pointer Pointer = new();
    private static readonly Hit[] Buttons = [new(32, 65, 350, 28), new(32, 100, 350, 28),
        new(32, 135, 350, 28), new(32, 170, 350, 28), new(32, 270, 170, 28), new(212, 270, 170, 28)];
    private static void TogglePanel()
    {
        Visible = !Visible;
        Pointer.Reset();
        Guide.Move.Stop();
        Log.Guard("panel-input", () => Exec.PanelInput(Visible));
    }
    internal static bool Key(string name)
    {
        var key = Enum.Parse(Anchors.Type("UnityEngine.KeyCode"), name);
        return (bool)Reflect.Method(Anchors.Type("UnityEngine.Input"), "GetKeyDown", key.GetType()).Invoke(null, [key])!;
    }
    internal static void Tick()
    {
        if (Key("F8")) TogglePanel();
        if (Key("F9")) { Config.Map = !Config.Map; }
        if (Key("F10")) { if (Guide.Move.Active) Guide.Move.Stop(); else RequestStart(); }
        if (Key("Escape")) { Guide.Move.Stop(); if (Visible) TogglePanel(); }
        if (!Visible) { Pointer.Reset(); return; }
        var mouse = Anchors.Static("UnityEngine.Input", "mousePosition")!;
        var x = Convert.ToSingle(Reflect.Get(mouse, "x"));
        var y = Convert.ToSingle(Anchors.Static("UnityEngine.Screen", "height")) - Convert.ToSingle(Reflect.Get(mouse, "y"));
        bool Mouse(string method) => (bool)Reflect.Method(Anchors.Type("UnityEngine.Input"), method, typeof(int)).Invoke(null, [0])!;
        switch (Pointer.Step(x, y, Mouse("GetMouseButtonDown"), Mouse("GetMouseButtonUp"), Visible, Buttons))
        {
            case 0: Config.Pickup = !Config.Pickup; break;
            case 1: Config.Map = !Config.Map; break;
            case 2: Config.Enemies = !Config.Enemies; break;
            case 3: Config.Guide = !Config.Guide; break;
            case 4: RequestStart(); break;
            case 5: Guide.Move.Stop(); break;
        }
    }
    internal static void Draw()
    {
        if (!Visible) return;
        Gui.Box(20, 20, 380, 450);
        Gui.Label(32, 30, 350, Text.Get("title"));
        DrawButton(0, Toggle("pickup", Config.Pickup));
        DrawButton(1, Toggle("map", Config.Map));
        DrawButton(2, Toggle("enemies", Config.Enemies));
        DrawButton(3, Toggle("guide", Config.Guide));
        Gui.Label(32, 205, 350, Text.Get("route") + ": " + Text.Get(Guide.Route.Status));
        Gui.Label(32, 235, 350, Text.Get("move") + ": " + Text.Get(Guide.Move.Follower.Status.ToString()));
        DrawButton(4, Text.Get("start"));
        DrawButton(5, Text.Get("stop"));
        Gui.Label(32, 305, 350, Text.Get("keys"));
        Gui.Label(32, 335, 350, Text.Get("achievements"));
        var failed = new[] { "state", "pickup", "pickup-sight", "map", "enemies", "route", "movement", "hooks" }
            .Where(name => !Log.Available(name));
        Gui.Label(32, 365, 350, Supervisor.Current.Ready ? Text.Get("ready") : Text.Get("waiting"));
        Gui.Label(32, 395, 350, string.Join(", ", failed));
    }
    private static string Toggle(string key, bool enabled) => Text.Get(key) + ": " + Text.Get(enabled ? "on" : "off");
    private static void DrawButton(int index, string text)
    {
        var button = Buttons[index];
        Gui.Button(button.X, button.Y, button.Width, text);
    }
    private static void RequestStart()
    {
        if (Visible) TogglePanel();
        Log.Guard("map-close", Exec.CloseMap);
        StartRequested = true;
    }
}
