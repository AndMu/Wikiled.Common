using System;
using System.Reflection;
using Wikiled.Common.Arguments;

namespace Wikiled.Common.Reflection
{
    public static class ReflectionHelper
    {
        public static bool IsSubclassOfGeneric(this Type toCheck, Type generic)
        {
            var genericInfo = generic.GetTypeInfo();
            Guard.IsValid(() => genericInfo, genericInfo, info => info.IsGenericType && info.GenericTypeArguments.Length == 0, "Invalid generic definition. Should be similar to Dictionary<,>");
            while (toCheck != null && toCheck != typeof(object))
            {
                var current = toCheck.GetTypeInfo().IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == current)
                {
                    return true;
                }

                toCheck = toCheck.GetTypeInfo().BaseType;
            }

            return false;
        }
    }
}
