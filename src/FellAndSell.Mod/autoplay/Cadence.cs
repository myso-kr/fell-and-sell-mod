namespace FellAndSell.Mod.Autoplay;

internal sealed class Cadence
{
    private long _next;
    internal void Reset() => _next = 0;
    internal bool Take(long now, int interval, bool allowed)
    {
        if (!allowed || now < _next) return false;
        _next = now + Math.Clamp(interval, 100, 1000);
        return true;
    }
}
