using System;
using System.Reflection;

namespace Wikiled.Common.Reflection
{
    public static class ReflectionHelper
    {
        public static bool IsSubclassOfGeneric(this Type toCheck, Type generic)
        {
            var genericInfo = generic.GetTypeInfo();
            if (!genericInfo.IsGenericType || genericInfo.GenericTypeArguments.Length != 0)
            {
                throw new ArgumentOutOfRangeException(
                    "Invalid generic definition. Should be similar to Dictionary<,>, whithout type",
                    nameof(generic));
            }

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
