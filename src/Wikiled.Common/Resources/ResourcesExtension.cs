using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Wikiled.Common.Resources
{
    /// <summary>
    ///     Resources access helper
    /// </summary>
    public static class ResourcesExtension
    {
        public static Stream GetEmbeddedFile(this Assembly assembly, string fileName)
        {
            var assemblyName = assembly.GetName().Name;
            return GetEmbeddedFile(assemblyName, fileName);
        }

        public static Stream GetEmbeddedFile(this Type type, string fileName)
        {
            var assemblyName = type.Assembly.GetName().Name;
            return GetEmbeddedFile(assemblyName, fileName);
        }

        public static XmlDocument GetEmbeddedXml(this Type type, string fileName)
        {
            using (Stream str = GetEmbeddedFile(type, fileName))
            {
                var reader = new XmlTextReader(str);
                var xml = new XmlDocument();
                xml.Load(reader);
                return xml;
            }
        }

        public static string[] LoadData(this Type type, string file)
        {
            var lines = new List<string>();
            using (var stream = new StreamReader(type.Assembly.GetEmbeddedFile(file)))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    lines.Add(line.Trim());
                }
            }

            return lines.ToArray();
        }

        public static XDocument LoadXmlData(this Type type, string fileName)
        {
            using (Stream str = GetEmbeddedFile(type, fileName))
            {
                var reader = new XmlTextReader(str);
                return XDocument.Load(reader);
            }
        }

        private static Stream GetEmbeddedFile(string assemblyName, string fileName)
        {
            Assembly assembly = Assembly.Load(assemblyName);

            var stream = assembly.GetManifestResourceStream(
                string.Format(
                    "{0}.{1}",
                    assemblyName,
                    fileName));
            if (stream == null)
            {
                throw new ResourcesException(
                    string.Format(
                        "Could not locate embedded resource '{0}' in assembly '{1}'",
                        fileName,
                        assemblyName));
            }

            return stream;
        }
    }
}
