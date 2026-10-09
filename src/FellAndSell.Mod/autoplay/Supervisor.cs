namespace FellAndSell.Mod.Autoplay;

internal static class Supervisor
{
    internal static Snapshot Current;
    internal static void Tick()
    {
        if (!Config.Ready) return;
        Log.Guard("keys", Panel.Widget.Tick);
        if (!Config.Pickup && !Config.Map && !Config.Guide && !Panel.Widget.Visible)
        {
            Log.Guard("pickup-restore", Pickup.Restore);
            Guide.Move.Stop();
            Current = default;
            return;
        }
        if (!Log.Guard("state", () => Current = State.Read(Panel.Widget.Visible))) { Current = default; Guide.Move.Stop(); }
        if (!Log.Guard("pickup", () => Pickup.Tick(Current))) Log.Guard("pickup-restore", Pickup.Restore);
        Log.Guard("map", Map.Read.Tick);
        Log.Guard("route", () => Guide.Route.Tick(Current));
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
        if (!Panel.Widget.Visible && !(Config.Map && Map.Read.Current != null)
            && !(Config.Guide && Guide.Route.Corners.Length >= 2)) { Log.Guard("panel-hide", Panel.Tmp.Hide); return; }
        Log.Guard("gui-font", () => Panel.Gui.WithFont(() =>
        {
            Log.Guard("panel", Panel.Widget.Draw);
            Log.Guard("map-overlay", Map.Overlay.Draw);
            Log.Guard("route-overlay", Guide.Overlay.Draw);
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
        Map.Read.Clear();
        Current = default;
        State.Player = State.Input = State.Pickup = null;
        Panel.Widget.Visible = false;
        Panel.Widget.StartRequested = false;
        Log.Reset();
    }
}
