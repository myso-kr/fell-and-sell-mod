namespace FellAndSell.Mod.Autoplay;

// Game writes stay here. Input is leased only around the normal HandleMovement call.
internal static class Exec
{
    internal static void CloseMap()
    {
        if (Map.Window.Open) Reflect.Call(Anchors.Instance("DungeonMapWindow")!, "CloseMap");
    }
    internal static void PickupRadius(object owner, float radius) => Reflect.Set(owner, "pickupRadius", radius);
    internal static void PickupScan(object owner) => Reflect.Call(owner, "PerformProximityScan");
    private static object? _input, _original;
    private static object? _cursorLock;
    private static bool _cursorVisible;
    internal static void PanelInput(bool open)
    {
        if (open)
        {
            if (_cursorLock == null)
            {
                _cursorLock = Anchors.Static("UnityEngine.Cursor", "lockState");
                _cursorVisible = (bool)Anchors.Static("UnityEngine.Cursor", "visible")!;
            }
            Cursor("lockState", Enum.Parse(Anchors.Type("UnityEngine.CursorLockMode"), "None"));
            Cursor("visible", true);
        }
        else
        {
            try
            {
                if (_cursorLock != null) { Cursor("lockState", _cursorLock); Cursor("visible", _cursorVisible); }
            }
            finally { _cursorLock = null; }
        }
    }
    private static void Cursor(string name, object value) => ((System.Reflection.PropertyInfo)
        Reflect.Member(Anchors.Type("UnityEngine.Cursor"), name)!).SetValue(null, value);
    internal static void BeginMovement(Point direction)
    {
        var player = State.Player ?? throw new InvalidOperationException("Player unavailable");
        var input = State.Input ?? throw new InvalidOperationException("Input unavailable");
        var local = Reflect.Call(Reflect.Get(player, "transform")!, "InverseTransformDirection", State.Vector(direction))!;
        var point = State.Position(local);
        _input = input;
        _original = Reflect.Get(input, "MovementInput");
        Reflect.Set(input, "MovementInput", Anchors.Vector2(point.X, point.Z));
    }
    internal static void EndMovement()
    {
        try { if (Alive.Is(_input) && _original != null) Reflect.Set(_input!, "MovementInput", _original); }
        finally { _input = null; _original = null; }
    }
}
