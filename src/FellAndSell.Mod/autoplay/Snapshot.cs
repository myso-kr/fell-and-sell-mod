namespace FellAndSell.Mod.Autoplay;

internal readonly record struct Point(float X, float Y, float Z)
{
    internal float Distance(Point other) => MathF.Sqrt((X - other.X) * (X - other.X)
        + (Y - other.Y) * (Y - other.Y) + (Z - other.Z) * (Z - other.Z));
    internal float Horizontal(Point other) => MathF.Sqrt((X - other.X) * (X - other.X) + (Z - other.Z) * (Z - other.Z));
}

internal readonly record struct Snapshot(bool Ready, bool Dead, bool Combat, bool Blocked,
    bool Inventory, bool Paused, bool Focused, float Health, Point Position)
{
    internal bool CanPickup => Ready && !Dead && !Blocked && !Inventory && !Paused && Focused;
    internal bool CanMove => CanPickup && !Combat;
}
