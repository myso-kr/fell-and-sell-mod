using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal sealed class Target
{
    internal Point? Goal { get; private set; }
    private bool _dismissed;
    internal bool NeedsDefault => Goal == null && !_dismissed;
    internal void Reset() { Goal = null; _dismissed = false; }
    internal void Select(Point goal) { Goal = goal; _dismissed = false; }
    internal void Cancel() { Goal = null; _dismissed = true; }
    internal void Default(Point? goal) { if (Goal == null && !_dismissed) Goal = goal; }
}
