namespace FellAndSell.Mod;

// Interop lists expose Count/get_Item without implementing managed IEnumerable.
internal static class Sequence
{
    internal static IEnumerable<object> Items(object? value)
    {
        if (value == null) yield break;
        if (value is System.Collections.IEnumerable sequence)
        {
            foreach (var item in sequence) if (item != null) yield return item;
            yield break;
        }
        var count = Convert.ToInt32(Reflect.Get(value, "Count"));
        if (count is < 0 or > 10000) throw new InvalidDataException("Unexpected interop list size");
        for (var index = 0; index < count; index++)
        {
            var item = Reflect.Call(value, "get_Item", index);
            if (item != null) yield return item;
        }
    }
}
