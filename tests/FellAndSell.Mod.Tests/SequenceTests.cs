using Xunit;

namespace FellAndSell.Mod.Tests;

public sealed class SequenceTests
{
    private sealed class InteropLike
    {
        public int Count => 2;
        public string this[int index] => index == 0 ? "room" : "stairs";
    }
    private sealed class Corrupt { public int Count => -1; }
    [Fact]
    public void NativeStyleListWithoutManagedEnumerableDoesNotLoseRooms() =>
        Assert.Equal(new object[] { "room", "stairs" }, Sequence.Items(new InteropLike()));
    [Fact]
    public void ManagedArraysAndNullRemainSupported()
    {
        Assert.Empty(Sequence.Items(null));
        Assert.Equal(new object[] { "chest" }, Sequence.Items(new object?[] { null, "chest" }));
    }
    [Fact]
    public void CorruptListFailsInsteadOfPretendingMapIsEmpty() =>
        Assert.Throws<InvalidDataException>(() => Sequence.Items(new Corrupt()).ToArray());
}
