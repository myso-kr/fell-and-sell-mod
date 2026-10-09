using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal static class Ground
{
    internal static Point Position(Point fallback)
    {
        var controller = State.Player == null ? null : Reflect.Get(State.Player, "CharacterController");
        if (!Alive.Is(controller)) return fallback;
        var min = State.Position(Reflect.Get(Reflect.Get(controller!, "bounds")!, "min")!);
        return fallback with { Y = min.Y + .05f };
    }
}
