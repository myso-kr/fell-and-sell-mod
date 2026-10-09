using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Panel;

internal static class Widget
{
    internal static bool Visible;
    internal static bool StartRequested;
    private static void TogglePanel()
    {
        Visible = !Visible;
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
    }
    internal static void Draw()
    {
        if (!Visible) return;
        Gui.Box(20, 20, 380, 450);
        Gui.Label(32, 30, 350, Text.Get("title"));
        if (Gui.Button(32, 65, 350, Toggle("pickup", Config.Pickup))) Config.Pickup = !Config.Pickup;
        if (Gui.Button(32, 100, 350, Toggle("map", Config.Map))) Config.Map = !Config.Map;
        if (Gui.Button(32, 135, 350, Toggle("enemies", Config.Enemies))) Config.Enemies = !Config.Enemies;
        if (Gui.Button(32, 170, 350, Toggle("guide", Config.Guide))) Config.Guide = !Config.Guide;
        Gui.Label(32, 205, 350, Text.Get("route") + ": " + Text.Get(Guide.Route.Status));
        Gui.Label(32, 235, 350, Text.Get("move") + ": " + Text.Get(Guide.Move.Follower.Status.ToString()));
        if (Gui.Button(32, 270, 170, Text.Get("start"))) RequestStart();
        if (Gui.Button(212, 270, 170, Text.Get("stop"))) Guide.Move.Stop();
        Gui.Label(32, 305, 350, Text.Get("keys"));
        Gui.Label(32, 335, 350, Text.Get("achievements"));
        var failed = new[] { "state", "pickup", "pickup-sight", "map", "enemies", "route", "movement", "hooks" }
            .Where(name => !Log.Available(name));
        Gui.Label(32, 365, 350, Supervisor.Current.Ready ? Text.Get("ready") : Text.Get("waiting"));
        Gui.Label(32, 395, 350, string.Join(", ", failed));
    }
    private static string Toggle(string key, bool enabled) => Text.Get(key) + ": " + Text.Get(enabled ? "on" : "off");
    private static void RequestStart()
    {
        if (Visible) TogglePanel();
        Log.Guard("map-close", Exec.CloseMap);
        StartRequested = true;
    }
}
