using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Knx.Common
{
    public static class TypeExtensions
    {
        public static T GetFirstCustomAttribute<T>(this Type type, bool inherit) where T : System.Attribute
        {
            return (T)type.GetTypeInfo().GetCustomAttributes(typeof(T), inherit).First();
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }
    }
}
