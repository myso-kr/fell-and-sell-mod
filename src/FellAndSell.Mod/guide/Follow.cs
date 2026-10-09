using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Guide;

internal enum Motion { Idle, Following, Arrived, Blocked, Cancelled }

// Pure progress and cancellation policy; no Unity or game references.
internal sealed class Follow
{
    internal Motion Status { get; private set; }
    private Point[] _corners = [];
    private int _index;
    private float _best = float.MaxValue, _health;
    private long _progress;
    internal void Start(Point[] corners, float health, long now)
    {
        _corners = corners;
        _index = 0;
        _best = float.MaxValue;
        _health = health;
        _progress = now;
        Status = corners.Length >= 2 ? Motion.Following : Motion.Blocked;
    }
    internal void Stop() => Status = Motion.Cancelled;
    internal Point Step(Snapshot state, bool manual, long now)
    {
        if (Status != Motion.Following) return default;
        if (!state.CanMove || manual || state.Health < _health - 0.01f) { Stop(); return default; }
        _health = state.Health;
        while (_index < _corners.Length && state.Position.Distance(_corners[_index]) < 0.65f)
        {
            _index++;
            _best = float.MaxValue;
            _progress = now;
        }
        if (_index == _corners.Length) { Status = Motion.Arrived; return default; }
        var target = _corners[_index];
        var distance = state.Position.Distance(target);
        if (distance < _best - 0.1f) { _best = distance; _progress = now; }
        if (now - _progress > 2500) { Status = Motion.Blocked; return default; }
        var horizontal = state.Position.Horizontal(target);
        if (horizontal < 0.05f) { Status = Motion.Blocked; return default; }
        return new((target.X - state.Position.X) / horizontal, 0, (target.Z - state.Position.Z) / horizontal);
    }
}
