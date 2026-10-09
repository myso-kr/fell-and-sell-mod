namespace FellAndSell.Mod.Autoplay;

internal static class Pickup
{
    private static object? _owner;
    private static float _radius;
    internal static void Tick(Snapshot state)
    {
        if (!Config.Pickup || !Patches.PickupReady || !state.CanPickup || State.Pickup == null) { Restore(); return; }
        if (!Equals(_owner, State.Pickup))
        {
            Restore();
            _owner = State.Pickup;
            _radius = Convert.ToSingle(Reflect.Get(_owner, "pickupRadius"));
        }
        Exec.PickupRadius(_owner, Config.Radius);
        // The game's Update still owns cadence, death grace, filters and inventory limits.
    }
    internal static void Restore()
    {
        if (Alive.Is(_owner)) Exec.PickupRadius(_owner!, _radius);
        _owner = null;
    }
}
