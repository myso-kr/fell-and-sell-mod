namespace FellAndSell.Mod.Panel;

internal readonly record struct Hit(float X, float Y, float Width, float Height)
{
    internal bool Contains(float x, float y) => x >= X && x < X + Width && y >= Y && y < Y + Height;
}

// Screen-pixel input matches the owned TMP Canvas, independent of IMGUI state.
internal sealed class Pointer
{
    private int _pressed = -1;
    internal void Reset() => _pressed = -1;
    internal int Step(float x, float y, bool down, bool up, bool visible, IReadOnlyList<Hit> buttons)
    {
        if (!visible) { Reset(); return -1; }
        if (down)
        {
            _pressed = -1;
            for (var index = 0; index < buttons.Count; index++)
                if (buttons[index].Contains(x, y)) { _pressed = index; break; }
        }
        if (!up) return -1;
        var pressed = _pressed; Reset();
        return pressed >= 0 && pressed < buttons.Count && buttons[pressed].Contains(x, y) ? pressed : -1;
    }
}
