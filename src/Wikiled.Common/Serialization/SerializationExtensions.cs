using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Wikiled.Common.Serialization
{
    public static class SerializationExtension
    {        
        public static byte[] GetArrayBin<T>(this T data)
        {
            if (data == null)
            {
                return new byte[] { };
            }

            // instantiate a MemoryStream and a new instance of our class           
            using (var stream = new MemoryStream())
            {
                // create a new BinaryFormatter instance
                var formatter = new BinaryFormatter();
                // serialize the class into the MemoryStream
                formatter.Serialize(stream, data);
                stream.Seek(0, 0);
                return stream.ToArray();
            }
        }

        public static byte[] DCSerialize<T>(this T data) where T : class
        {
            if (data == null)
            {
                return new byte[] { };
            }

            // instantiate a MemoryStream and a new instance of our class           
            using (var stream = new MemoryStream())
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(T));
                ser.WriteObject(stream, data);
                stream.Seek(0, 0);
                return stream.ToArray();
            }
        }

        public static T DCDeserialize<T>(this byte[] array) where T : class
        {
            if (array == null ||
                array.Length == 0)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                stream.Write(array, 0, array.Length);
                stream.Seek(0, 0);
                DataContractSerializer ser = new DataContractSerializer(typeof(T));
                return (T)ser.ReadObject(stream);
            }
        }

        public static T GetObjectBin<T>(this byte[] array)
        {
            return (T)GetObjectBin(array);
        }

        public static object GetObjectBin(this byte[] array)
        {
            if (array == null ||
                array.Length == 0)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                stream.Write(array, 0, array.Length);
                stream.Seek(0, 0);
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }

        public static void GetObjectData<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, string name, SerializationInfo info)
        {
            var items = dictionary.Select(item => item).ToArray();
            info.AddValue(name, items);
        }
    }
}
