using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal static class Move
{
    internal static readonly Follow Follower = new();
    internal static Point Direction;
    internal static void Start(Snapshot state)
    {
        if (Config.Guide && Patches.MovementReady && Route.Complete && state.CanMove)
            Follower.Start(Route.Corners, state.Health, Environment.TickCount64);
    }
    internal static void Stop() { Follower.Stop(); Direction = default; }
    internal static void Tick(Snapshot state)
    {
        if (!Config.Guide || !Route.Complete || !Log.Available("movement")) { Stop(); return; }
        var manual = State.Input != null && State.Position2(Reflect.Get(State.Input, "MovementInput")!) > 0.08f;
        Direction = Follower.Step(state, manual, Environment.TickCount64);
    }
    internal static bool Active => Follower.Status == Motion.Following && Direction != default;
}
