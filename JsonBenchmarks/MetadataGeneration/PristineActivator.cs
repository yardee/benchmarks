using System;
using System.Reflection;

namespace Gmc.Cloud.Infrastructure.Utils;
#nullable enable

public static class PristineActivator
{
    public static T CreatePristineObject<T>() => typeof(T).CreatePristineObject<T>();

    public static T CreatePristineObject<T>(this Type type) =>
        type.GetDefaultConstructor() != null
            ? (T)Activator.CreateInstance(type, true)!
            : (T)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(type);
    
    public static ConstructorInfo? GetDefaultConstructor(this Type instanceType)
    {
        return instanceType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null, Type.EmptyTypes, null);
    }
}
