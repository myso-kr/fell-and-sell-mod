using System.Reflection;

namespace FellAndSell.Mod;

// Pure lookup: generated interop properties can shadow inherited Unity members.
internal static class Reflect
{
    private const BindingFlags Declared = BindingFlags.Public | BindingFlags.NonPublic
        | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    internal static MemberInfo? Member(Type type, string name)
    {
        for (Type? owner = type; owner != null; owner = owner.BaseType)
        {
            var property = owner.GetProperty(name, Declared);
            if (property != null) return property;
            var field = owner.GetField(name, Declared);
            if (field != null) return field;
        }
        return null;
    }

    internal static object? Get(object target, string name) => Read(Member(target.GetType(), name), target);
    internal static object? Read(MemberInfo? member, object? target) => member switch
    {
        PropertyInfo property => property.GetValue(target),
        FieldInfo field => field.GetValue(target),
        _ => throw new MissingMemberException(target?.GetType().FullName, member?.Name)
    };

    internal static void Set(object target, string name, object? value)
    {
        switch (Member(target.GetType(), name))
        {
            case PropertyInfo property: property.SetValue(target, value); break;
            case FieldInfo field: field.SetValue(target, value); break;
            default: throw new MissingMemberException(target.GetType().FullName, name);
        }
    }

    internal static MethodInfo Method(Type type, string name, params Type[] arguments)
    {
        for (Type? owner = type; owner != null; owner = owner.BaseType)
        {
            var method = owner.GetMethod(name, Declared, null, arguments, null);
            if (method != null) return method;
        }
        throw new MissingMethodException(type.FullName, name);
    }

    internal static object? Call(object target, string name, params object[] arguments) =>
        Method(target.GetType(), name, arguments.Select(value => value.GetType()).ToArray()).Invoke(target, arguments);
}
