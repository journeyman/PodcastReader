using System;
using System.Linq;
using System.Reflection;

namespace Pr.Phone8.Infrastructure
{
    public static class TypeExtensions
    {
        public static Tuple<Type, Type> GetTypeAndItsRawGenericInterfaceIfExists(this Type type, Type ifaceType)
        {
            var iface = type.GetInterfaces().FirstOrDefault(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == ifaceType);
            return iface == null ? null : new Tuple<Type, Type>(type, iface);
        }

        public static bool IsSubclassOfRawGeneric(this Type type, Type toCheck)
        {
            return null == type.GetInterfaces().SingleOrDefault(iface => iface.GetTypeInfo().IsGenericType && iface.GetGenericTypeDefinition() == toCheck);
        }
    }
}