using System;
using System.IO;

namespace Wikiled.Common.Serialization
{
    public static class Serialization
    {
        public static T DeserializeFromFile<T>(string file) where T : class
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException("file");
            }

            return File.ReadAllText(file).XmlDeserialize<T>();
        }
    }
}
