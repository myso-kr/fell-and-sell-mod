using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal static class Exit
{
    internal static Point? Read(Point player)
    {
        // The native floor transition trigger is the actual destination, not a room centre.
        var triggers = I18n.RuntimeTypes.All(Anchors.Type("Il2Cpp.NextFloorTrigger"))
            .Where(value => Alive.Is(value) && (bool)Reflect.Get(value, "isActiveAndEnabled")!)
            .Select(value => State.Position(Reflect.Get(Reflect.Get(value, "transform")!, "position")!))
            .OrderBy(position => position.Distance(player)).ToArray();
        return triggers.Length == 0 ? null : triggers[0];
    }
}
