namespace FellAndSell.Mod.Autoplay;

internal static class Supervisor
{
    internal static Snapshot Current;
    internal static void Late()
    {
        // The native controller locks the cursor during Update. Keep the owned
        // panel cursor available afterwards without falsifying game block state.
        if (Panel.Widget.Visible) Log.Guard("panel-input", () => Exec.PanelInput(true));
    }
    internal static void Tick()
    {
        if (!Config.Ready) return;
        Log.Guard("lifecycle", Lifecycle.Tick);
        Log.Guard("keys", Panel.Widget.Tick);
        if (!Config.Pickup && !Config.Map && !Config.Guide && !Panel.Widget.Visible)
        {
            Log.Guard("pickup-restore", Pickup.Restore);
            Guide.Move.Stop();
            Log.Guard("map-overlay", Map.Overlay.Reset);
            Log.Guard("fairy-hide", Guide.Glow.Hide);
            Current = default;
            return;
        }
        if (!Log.Guard("state", () => Current = State.Read(Panel.Widget.Visible))) { Current = default; Guide.Move.Stop(); }
        if (!Log.Guard("pickup", () => Pickup.Tick(Current))) Log.Guard("pickup-restore", Pickup.Restore);
        Log.Guard("map", Map.Read.Tick);
        Log.Guard("map-overlay", Map.Overlay.Tick);
        Log.Guard("route", () => Guide.Route.Tick(Current));
        if (!Log.Guard("fairy", Guide.Overlay.Tick)) Log.Guard("fairy-hide", Guide.Glow.Hide);
        if (Panel.Widget.StartRequested)
        {
            Panel.Widget.StartRequested = false;
            Guide.Move.Start(Current);
        }
        if (!Log.Guard("movement", () => Guide.Move.Tick(Current))) Guide.Move.Stop();
    }
    internal static void Draw()
    {
        if (!Config.Ready) return;
        if (!Panel.Widget.Visible) { Log.Guard("panel-hide", Panel.Tmp.Hide); return; }
        Log.Guard("gui-font", () => Panel.Gui.WithFont(() =>
        {
            Log.Guard("panel", Panel.Widget.Draw);
        }));
    }
    internal static void Reset()
    {
        Guide.Move.Stop();
        Log.Guard("panel-hide", Panel.Tmp.Hide);
        Log.Guard("panel-input-restore", () => Exec.PanelInput(false));
        Log.Guard("movement-restore", Exec.EndMovement);
        Log.Guard("pickup-restore", Pickup.Restore);
        Guide.Route.Clear();
        Log.Guard("fairy-reset", Guide.Overlay.Reset);
        Map.Read.Clear();
        Log.Guard("map-overlay", Map.Overlay.Reset);
        Current = default;
        State.Clear();
        Anchors.ClearInstances();
        Panel.Widget.Visible = false;
        Panel.Widget.StartRequested = false;
        Log.Reset();
    }
}
