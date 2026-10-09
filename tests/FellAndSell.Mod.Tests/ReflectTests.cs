using Xunit;

namespace FellAndSell.Mod.Tests;

public sealed class ReflectTests
{
    private class Base { public string Name => "base"; public int Field = 7; public int Run() => 3; }
    private sealed class Child : Base { public new int Name => 42; }
    [Fact]
    public void GeneratedMemberShadowingUsesMostDerivedDeclaration()
    {
        Assert.Equal(42, Reflect.Get(new Child(), "Name"));
        Assert.Equal(7, Reflect.Get(new Child(), "Field"));
        Assert.Equal(3, Reflect.Call(new Child(), "Run"));
    }
    [Fact]
    public void MissingMembersFailVisiblyInsteadOfInventingDefaults() =>
        Assert.Throws<MissingMemberException>(() => Reflect.Get(new Child(), "Gone"));
}
