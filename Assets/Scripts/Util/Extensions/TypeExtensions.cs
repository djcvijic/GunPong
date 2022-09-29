using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsImplementationOf(this Type baseType, Type interfaceType)
        {
            return baseType.GetInterfaces().Any(interfaceType.Equals);
        }

        public static IEnumerable<Type> GetImplementingClasses(this Type interfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
        }
    }
}