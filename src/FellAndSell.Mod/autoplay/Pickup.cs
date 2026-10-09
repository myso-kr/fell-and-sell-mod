namespace FellAndSell.Mod.Autoplay;

internal static class Pickup
{
    private static object? _owner;
    private static float _radius;
    private static readonly Cadence Scan = new();
    private static long _request;
    private static string? _diagnostic;
    private static long _report;
    private static int _attempts, _visible;
    internal static void Tick(Snapshot state)
    {
        if (!Config.Pickup || !Patches.PickupReady || !state.CanPickup || State.Pickup == null) { Restore(); return; }
        if (!Equals(_owner, State.Pickup))
        {
            Restore();
            _owner = State.Pickup;
            _radius = Convert.ToSingle(Reflect.Get(_owner, "pickupRadius"));
            Scan.Reset(); _request = 0;
        }
        Exec.PickupRadius(_owner, Config.Radius);
        // Use the native scan, including when its Update isn't scheduled/enabled.
        // A shared scan-prefix cadence prevents duplicate native/mod scans.
        if (Environment.TickCount64 < _request) return;
        _request = Environment.TickCount64 + 100;
        Exec.PickupScan(_owner);
        if (Environment.TickCount64 >= _report)
        {
            _report = Environment.TickCount64 + 3000;
            var count = Sequence.Items(Reflect.Get(_owner, "_hitColliders")).Count(Alive.Is);
            var options = Anchors.Instance("OptionsManager");
            MelonLoader.MelonLogger.Msg($"pickup: scan candidates={count} checks={_attempts} visible={_visible} componentEnabled={Reflect.Get(_owner, "enabled")} nativeOption={(options == null ? null : Reflect.Get(options, "IsAutoPickupEnabled"))}");
            _attempts = _visible = 0;
        }
    }
    internal static void Record(bool visible) { _attempts++; if (visible) _visible++; }
    internal static bool BeforeScan(object owner)
    {
        if (!Equals(owner, State.Pickup)) return false;
        var now = Environment.TickCount64;
        var time = Convert.ToSingle(Anchors.Static("UnityEngine.Time", "time"));
        var dead = (bool)Reflect.Call(owner, "IsPlayerDead")!;
        var inventory = (bool)Reflect.Call(owner, "IsInventoryOpen")!;
        var paused = time < Convert.ToSingle(Reflect.Get(owner, "_pauseAutoPickupUntil"));
        var allowed = Supervisor.Current.CanPickup && !dead && !inventory && !paused;
        var interval = (int)(Convert.ToSingle(Reflect.Get(owner, "scanInterval")) * 1000);
        var key = $"allowed={allowed} dead={dead} inventory={inventory} grace={paused}";
        if (_diagnostic != key)
        {
            _diagnostic = key;
            MelonLoader.MelonLogger.Msg("pickup: " + key + $" radius={Config.Radius}");
        }
        return Scan.Take(now, interval, allowed);
    }
    internal static void Restore()
    {
        if (Alive.Is(_owner)) Exec.PickupRadius(_owner!, _radius);
        _owner = null;
        Scan.Reset(); _request = 0;
    }
}
