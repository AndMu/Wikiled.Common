using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Reflection
{
    public static class ReflectionExtension
    {
        private static readonly ConcurrentDictionary<Type, Func<string, object>> ConversionMethods =
            new ConcurrentDictionary<Type, Func<string, object>>();

        private static readonly ConcurrentDictionary<Type, bool> isPrimitive = new ConcurrentDictionary<Type, bool>();

        private static readonly Dictionary<Type, BasicTypes> KnownTypes = new Dictionary<Type, BasicTypes>
                                                                          {
                                                                              {typeof(bool), BasicTypes.Bool},
                                                                              {typeof(char), BasicTypes.Char},
                                                                              {typeof(sbyte), BasicTypes.SByte},
                                                                              {typeof(byte), BasicTypes.Byte},
                                                                              {typeof(short), BasicTypes.Short},
                                                                              {typeof(ushort), BasicTypes.UShort},
                                                                              {typeof(int), BasicTypes.Int},
                                                                              {typeof(uint), BasicTypes.UInt},
                                                                              {typeof(long), BasicTypes.Long},
                                                                              {typeof(ulong), BasicTypes.ULong},
                                                                              {typeof(float), BasicTypes.Float},
                                                                              {typeof(double), BasicTypes.Double},
                                                                              {typeof(decimal), BasicTypes.Decimal},
                                                                              {typeof(string), BasicTypes.String},
                                                                              {typeof(DateTime), BasicTypes.DateTime},
                                                                              {typeof(TimeSpan), BasicTypes.TimeSpan},
                                                                              {typeof(Guid), BasicTypes.Guid},
                                                                              {typeof(Uri), BasicTypes.Uri},
                                                                              {typeof(byte[]), BasicTypes.ByteArray},
                                                                              {typeof(Type), BasicTypes.Type}
                                                                          };

        private static readonly Regex PropertyRegex = new Regex("(?<Name>[a-zA-Z][a-zA-Z0-9]*)(\\[(?<Index>[0-9]+)\\])?");

        private static readonly ConcurrentDictionary<Type, string> TypeNameTable = new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<string, Type> TypeTable = new ConcurrentDictionary<string, Type>();

        public static object CallByReflection(this object instance, string name, Type[] typeArg, object[] values)
        {
            // Just for simplicity, assume it's public etc
            MethodInfo method = instance.GetType().GetMethod(name);
            MethodInfo generic = method.MakeGenericMethod(typeArg);
            return generic.Invoke(instance, values);
        }

        public static T GetField<T>(this object instance, string name)
        {
            var item = instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException("name", name);
            }

            return (T)item.GetValue(instance);
        }

        public static T GetField<T>(this Type type, string name)
        {
            var item = type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException("name", name);
            }

            return (T)item.GetValue(null);
        }

        public static Func<string, object> GetParser(Type type)
        {
            if (!ConversionMethods.TryGetValue(type, out Func<string, object> conversion))
            {
                conversion = GetParserInternal(type);
                ConversionMethods[type] = conversion;
            }

            return conversion;
        }

        public static Dictionary<string, Tuple<string, string>> GetProperties(object source, object target)
        {
            if (source is IEnumerable)
            {
                return GetEnumerableValues(source as IEnumerable, target as IEnumerable);
            }

            return GetSimpleProperties(source, target);
        }

        public static T GetProperty<T>(this object instance, Type type, string name)
        {
            PropertyInfo propertyInfo = instance == null
                                            ? type.GetProperty(name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                                            : type.GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (propertyInfo == null)
            {
                throw new ArgumentOutOfRangeException("name", name);
            }

            return (T)propertyInfo.GetValue(instance, null);
        }

        /// <summary>
        ///     Get type description
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Description</returns>
        public static string GetTypeName<T>()
        {
            return GetTypeName(typeof(T));
        }

        /// <summary>
        ///     Get type description
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Description</returns>
        public static string GetTypeName(this Type type)
        {
            if (!TypeNameTable.TryGetValue(type, out string name))
            {
                name = $"{type.FullName},{type.Assembly.GetName().Name}";
                TypeNameTable[type] = name;
            }

            return name;
        }

        public static object InvokeMethod<T>(this T instance, string name, params object[] args)
        {
            var item = typeof(T).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException("name", name);
            }

            return item.Invoke(instance, args);
        }

        public static bool IsEnum(Type type)
        {
            return type.IsEnum;
        }

        public static bool IsNumericType(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }

                    return false;
            }

            return false;
        }

        public static bool IsPrimitive(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (!isPrimitive.TryGetValue(type, out bool result))
            {
                result = type.IsPrimitive || type == typeof(string) || type.IsNumericType();
                isPrimitive[type] = result;
            }

            return result;
        }

        /// <summary>
        ///     Resolve type from name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type ResolveType(this string typeName)
        {
            if (!TypeTable.TryGetValue(typeName, out Type type))
            {
                type = Type.GetType(typeName);
                TypeTable[typeName] = type;
            }

            return type;
        }

        public static void SetBaseField<T>(this T instance, string name, object value)
        {
            SetField(instance, typeof(T).BaseType, name, value);
        }

        public static void SetBaseProperty(this object instance, string name, object value)
        {
            if (instance == null)
            {
                return;
            }

            var item = instance.GetType().BaseType.GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException("name", name);
            }

            item.SetValue(instance, value, null);
        }

        public static void SetField<T>(this T instance, Type setType, string name, object value)
        {
            FieldInfo item = instance != null
                                 ? setType.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                 : setType.GetField(name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException("name", name);
            }

            item.SetValue(instance, value);
        }

        public static void SetField<T>(this T instance, string name, object value)
        {
            SetField(instance, typeof(T), name, value);
        }

        public static void SetProperty(string compoundProperty, object target, string value)
        {
            var propertyBits = compoundProperty.Split('.').SelectMany(
                bit =>
                {
                    Match match = PropertyRegex.Match(bit);
                    if (!match.Success)
                    {
                        throw new ArgumentOutOfRangeException("Can't resolve " + bit);
                    }

                    string propertyName = match.Groups["Name"].Value;
                    var property = new
                    {
                        PropertyName = propertyName,
                        IsIndexer = false,
                        Index = 0
                    };

                    if (match.Groups["Index"].Success)
                    {
                        var indexer = new
                        {
                            PropertyName = "Item",
                            IsIndexer = true,
                            Index = int.Parse(match.Groups["Index"].Value)
                        };
                        return new[] { property, indexer };
                    }

                    return new[] { property };
                }).ToList();

            for (int i = 0; i < propertyBits.Count - 1; i++)
            {
                var property = propertyBits[i];
                PropertyInfo propertyToGet = target.GetType().GetProperty(property.PropertyName);

                if (propertyToGet == null)
                {
                    throw new ArgumentOutOfRangeException("Can't resolve " + property.PropertyName);
                }

                if (property.IsIndexer && target.GetType().IsGenericType && target.GetType().GetGenericTypeDefinition() == typeof(List<>))
                {
                    if (property.Index >= ((IList)target).Count)
                    {
                        object indexItem = Activator.CreateInstance(target.GetType().GetGenericArguments()[0]);
                        ((IList)target).Add(indexItem);
                    }
                }

                object[] index = property.IsIndexer ? new object[] { property.Index } : null;
                object propertyValue = propertyToGet.GetValue(target, index);
                if (propertyValue == null)
                {
                    propertyValue = Activator.CreateInstance(propertyToGet.PropertyType);
                    propertyToGet.SetValue(target, propertyValue, null);
                }

                target = propertyValue;
            }

            var item = propertyBits.Last();
            PropertyInfo propertyToSet = target.GetType().GetProperty(item.PropertyName);
            if (propertyToSet == null)
            {
                throw new ArgumentOutOfRangeException("Can't resolve " + item.PropertyName);
            }

            object converted = ConvertTo(propertyToSet.PropertyType, value);
            propertyToSet.SetValue(target, converted, null);
        }

        public static void SetProperty(this object instance, string name, object value)
        {
            if (instance == null)
            {
                return;
            }

            var item = instance.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException("name", name);
            }

            item.SetValue(instance, value, null);
        }

        public static object ConvertTo(Type targetType, string value)
        {
            if (targetType == typeof(string))
            {
                return value;
            }

            if (targetType.IsPrimitive || (!targetType.IsEnum && typeof(IConvertible).IsAssignableFrom(targetType)))
            {
                return Convert.ChangeType(value, targetType);
            }

            return TypeDescriptor.GetConverter(targetType).ConvertFromString(value);
        }

        private static Dictionary<string, Tuple<string, string>> ExpectedAndActualListValues(IList expectedList, IList actualList)
        {
            Dictionary<string, Tuple<string, string>> table = new Dictionary<string, Tuple<string, string>>();
            for (int index = 0; index < Math.Max(expectedList.Count, actualList.Count); index++)
            {
                string key = $"[{index}]";
                table[key] = GetStringValuesFromListsAtIndex(expectedList, actualList, index);
            }

            return table;
        }

        private static Dictionary<string, Tuple<string, string>> GetEnumerableValues(IEnumerable expected, IEnumerable actual)
        {
            ArrayList expectedAsList = new ArrayList();
            ArrayList actualAsList = new ArrayList();
            foreach (var ex in expected)
            {
                expectedAsList.Add(ex);
            }

            foreach (var ex in actual)
            {
                actualAsList.Add(ex);
            }

            return ExpectedAndActualListValues(expectedAsList, actualAsList);
        }

        private static Func<string, object> GetParserInternal(Type type)
        {
            Type underlyingType = GetUnderlyingType(type);
            if (underlyingType != null)
            {
                type = underlyingType;
            }

            if (IsEnum(type))
            {
                return str => Enum.Parse(type, str);
            }

            switch (GetTypeCode(type))
            {
                case BasicTypes.Bool:
                    return str => bool.Parse(str);

                case BasicTypes.Char:
                    return str =>
                    {
                        if (str.Length != 1)
                        {
                            throw new FormatException("Single character expected: \"" + str + "\"");
                        }

                        return str[0];
                    };

                case BasicTypes.SByte:
                    return str => sbyte.Parse(str, NumberStyles.Integer, CultureInfo.InvariantCulture);

                case BasicTypes.Byte:
                    return str => byte.Parse(str, NumberStyles.Integer, CultureInfo.InvariantCulture);

                case BasicTypes.Short:
                    return str => short.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);

                case BasicTypes.UShort:
                    return str => ushort.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);

                case BasicTypes.Int:
                    return str => int.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);

                case BasicTypes.UInt:
                    return str => uint.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);

                case BasicTypes.Long:
                    return str => long.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);

                case BasicTypes.ULong:
                    return str => ulong.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);

                case BasicTypes.Float:
                    return str => float.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);

                case BasicTypes.Double:
                    return str => double.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);

                case BasicTypes.Decimal:
                    return str => decimal.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);

                case BasicTypes.DateTime:
                    return str => DateTime.ParseExact(str, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                case BasicTypes.String:
                    return str => str;

                case BasicTypes.TimeSpan:
                    return str => TimeSpan.Parse(str);

                case BasicTypes.Guid:
                    return str => new Guid(str);

                case BasicTypes.Uri:
                    return str => str;
            }

            throw new ArgumentOutOfRangeException("type", type.ToString());
        }

        private static Dictionary<string, Tuple<string, string>> GetSimpleProperties(object source, object target)
        {
            var sourceType = source.GetType();
            var sourceProperties = sourceType.GetProperties();
            var targetType = target.GetType();
            var targetProperties = targetType.GetProperties();

            Dictionary<string, Tuple<string, string>> table = new Dictionary<string, Tuple<string, string>>();

            var properties = from s in sourceProperties
                             from t in targetProperties
                             where s.Name == t.Name && s.PropertyType == t.PropertyType
                             select new
                             {
                                 Source = s,
                                 Target = t
                             };

            foreach (var property in properties)
            {
                object sourceValue = property.Source.GetValue(source, null);
                object targetValue = property.Target.GetValue(target, null);

                if (IsPrimitive(property.Source.PropertyType))
                {
                    table[property.Source.Name] = new Tuple<string, string>(
                        sourceValue?.ToString() ?? "NULL",
                        targetValue?.ToString() ?? "NULL");
                }
                else if (sourceValue == null)
                {
                    table[property.Source.Name] = new Tuple<string, string>("NULL", targetValue?.ToString() ?? "NULL");
                }
                else if (targetValue == null)
                {
                    table[property.Source.Name] = new Tuple<string, string>(sourceValue.ToString(), "NULL");
                }
                else
                {
                    var child = GetProperties(sourceValue, targetValue);
                    foreach (var subProperty in child)
                    {
                        table[property.Source.Name + "." + subProperty.Key] = subProperty.Value;
                    }
                }
            }

            return table;
        }

        private static Tuple<string, string> GetStringValuesFromListsAtIndex(IList expectedList, IList actualList, int index)
        {
            return new Tuple<string, string>(ListItemAsString(expectedList, index), ListItemAsString(actualList, index));
        }

        private static BasicTypes GetTypeCode(Type type)
        {
            return KnownTypes[type];
        }

        private static Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        private static string ListItemAsString(IList list, int itemIndex)
        {
            if (list.Count > itemIndex)
            {
                return list[itemIndex] == null ? "NULL" : list[itemIndex].ToString();
            }

            return "OutOfBounds";
        }
    }
}
