using MelonLoader;

namespace FellAndSell.Mod;

internal static class Config
{
    internal static bool Ready;
    private static MelonPreferences_Category _category = null!;
    private static MelonPreferences_Entry<bool> _pickup = null!, _map = null!, _guide = null!, _enemies = null!;
    private static MelonPreferences_Entry<float> _radius = null!;
    internal static bool Pickup { get => _pickup.Value; set { _pickup.Value = value; Save(); } }
    internal static bool Map { get => _map.Value; set { _map.Value = value; Save(); } }
    internal static bool Guide { get => _guide.Value; set { _guide.Value = value; Save(); } }
    internal static bool Enemies { get => _enemies.Value; set { _enemies.Value = value; Save(); } }
    internal static float Radius => Math.Clamp(_radius.Value, 1f, 4f);
    internal static void Load()
    {
        _category = MelonPreferences.CreateCategory("FellAndSell", "Fell & Sell Mod");
        _pickup = _category.CreateEntry("NearbyPickup", false, description: "Extend native eligible nearby pickup; no distant pursuit.");
        _map = _category.CreateEntry("MapOverlay", false, description: "Read-only map of the generated dungeon. Does not reveal saved exploration.");
        _guide = _category.CreateEntry("RouteGuide", false, description: "NavMesh route display. Automatic movement starts manually each time.");
        _enemies = _category.CreateEntry("EnemyMarkers", false, description: "Show loaded enemies on the mod map.");
        _radius = _category.CreateEntry("PickupRadius", 3f, description: "Nearby pickup radius in metres, clamped to 1–4.");
        Ready = true;
    }
    private static void Save() => _category.SaveToFile();
}
