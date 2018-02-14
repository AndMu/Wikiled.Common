using System.ComponentModel;

namespace Wikiled.Common.Helpers
{
    public static class Converter
    {
        public static bool TryConvert<T>(object obj, out T value)
        {
            value = default(T);
            if (obj == null)
            {
                return false;
            }

            if (obj is T)
            {
                value = (T)obj;
                return true;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (!converter.CanConvertFrom(obj.GetType()))
            {
                return false;
            }

            value = (T)converter.ConvertFrom(obj);
            return true;
        }

        public static T Convert<T>(object obj, T defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            if (obj is T)
            {
                return (T)obj;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(obj.GetType()))
            {
                return (T)converter.ConvertFrom(obj);
            }

            return defaultValue;
        }
    }
}
