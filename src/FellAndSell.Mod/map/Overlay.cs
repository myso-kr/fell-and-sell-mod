using FellAndSell.Mod.Autoplay;
using FellAndSell.Mod.Panel;

namespace FellAndSell.Mod.Map;

internal static class Overlay
{
    internal static void Draw()
    {
        var map = Read.Current;
        if (!Config.Map || map == null || map.Tiles.Length == 0) return;
        var x = Math.Max(420, Convert.ToSingle(Anchors.Static("UnityEngine.Screen", "width")) - 430);
        const float y = 45, size = 380;
        Gui.Box(x - 10, y - 30, size + 20, size + 80);
        Gui.Label(x, y - 25, size, Text.Get("floor") + ": " + map.Floor + " · " + Text.Get("pinHint"));
        var minX = map.Tiles.Min(tile => tile.X) * map.Size.X;
        var minZ = map.Tiles.Min(tile => tile.Z) * map.Size.Z;
        var maxX = map.Tiles.Max(tile => tile.X + tile.Width) * map.Size.X;
        var maxZ = map.Tiles.Max(tile => tile.Z + tile.Depth) * map.Size.Z;
        var scale = size / Math.Max(1f, Math.Max(maxX - minX, maxZ - minZ));
        (float X, float Y) Project(Point point) => (x + (point.X - minX) * scale, y + size - (point.Z - minZ) * scale);
        foreach (var tile in map.Tiles)
            Gui.Fill(x + (tile.X * map.Size.X - minX) * scale, y + size - ((tile.Z + tile.Depth) * map.Size.Z - minZ) * scale,
                tile.Width * map.Size.X * scale, tile.Depth * map.Size.Z * scale, tile.Kind);
        foreach (var marker in map.Markers)
        {
            var point = Project(marker.Position);
            Gui.Fill(point.X - 3, point.Y - 3, 7, 7, marker.Kind);
            if (Widget.Visible && marker.Kind is "chest" or "endRoom" or "bossRoom")
                if (Gui.Button(point.X, point.Y, 40, marker.Kind == "chest" ? "C" : marker.Kind == "endRoom" ? "E" : "B"))
                { Guide.Move.Stop(); Guide.Route.Select(marker.Position); }
        }
        Point? previous = null;
        foreach (var corner in Guide.Route.Corners)
        {
            var point = Project(corner);
            if (previous is { } start) Gui.Line(start.X, start.Y, point.X, point.Y, Guide.Route.Complete ? "route" : "partial");
            previous = new(point.X, point.Y, 0);
        }
        var player = Project(Supervisor.Current.Position);
        Gui.Fill(player.X - 4, player.Y - 4, 9, 9, "player");
        if (Widget.Visible)
        {
            var current = Anchors.Static("UnityEngine.Event", "current");
            if (current != null && Reflect.Get(current, "type")!.ToString() == "MouseDown" && Convert.ToInt32(Reflect.Get(current, "button")) == 0)
            {
                var mouse = Reflect.Get(current, "mousePosition")!;
                var mouseX = Convert.ToSingle(Reflect.Get(mouse, "x"));
                var mouseY = Convert.ToSingle(Reflect.Get(mouse, "y"));
                if (mouseX >= x && mouseX <= x + size && mouseY >= y && mouseY <= y + size)
                {
                    Guide.Move.Stop();
                    Guide.Route.Select(new(minX + (mouseX - x) / scale, Supervisor.Current.Position.Y, minZ + (size - mouseY + y) / scale));
                    Reflect.Call(current, "Use");
                }
            }
        }
    }
}
