using System;
using System.Linq;

public static class TypeExtensions
{
    public static bool IsImplementationOf(this Type baseType, Type interfaceType)
    {
        return baseType.GetInterfaces().Any(interfaceType.Equals);
    }
}