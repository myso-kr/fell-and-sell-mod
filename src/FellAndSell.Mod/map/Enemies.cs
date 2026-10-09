using FellAndSell.Mod.Autoplay;

namespace FellAndSell.Mod.Map;

internal static class Enemies
{
    internal static Marker[] Read(object manager)
    {
        var method = manager.GetType().GetMethods().Single(value => value.Name == "GetActiveEnemies");
        var list = Activator.CreateInstance(method.GetParameters()[0].ParameterType)!;
        try
        {
            method.Invoke(manager, [list]);
            return Sequence.Items(list).Where(Alive.Is)
                .Select(enemy => new Marker(State.Position(Reflect.Get(enemy, "position")!), "enemy")).ToArray();
        }
        finally { if (list is IDisposable disposable) disposable.Dispose(); }
    }
}
