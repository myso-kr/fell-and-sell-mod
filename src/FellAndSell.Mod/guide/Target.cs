using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal sealed class Target
{
    internal Point? Goal { get; private set; }
    internal bool NeedsDefault => Goal == null;
    internal void Reset() => Goal = null;
    internal void Select(Point goal) => Goal = goal;
    internal void Cancel() => Reset();
    internal void Default(Point? goal) { if (Goal == null) Goal = goal; }
}
