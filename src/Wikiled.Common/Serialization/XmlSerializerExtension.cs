using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Wikiled.Common.Helpers;

namespace Wikiled.Common.Serialization
{
    public static class XmlSerializerExtension
    {
        private static readonly ConcurrentDictionary<string, XmlSerializer> serializerCache =
            new ConcurrentDictionary<string, XmlSerializer>();

        /// <summary>
        ///     Remove illegal XML characters from a string.
        /// </summary>
        public static string SanitizeXmlString(this string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(xml));
            }

            StringBuilder buffer = new StringBuilder(xml.Length);
            foreach (char character in xml)
            {
                if (IsLegalXmlChar(character, false))
                {
                    buffer.Append(character);
                }
            }

            return buffer.ToString();
        }

        public static XElement SerializeAsXElement<T>(this T instance, string rootName = null)
        {
            return CreateOverrider<T>(rootName).SerializeAsXElement(instance);
        }

        public static XElement SerializeAsXElement(this XmlSerializer serializer, object instance)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            XElement element = serializer.XmlSerialize(instance).Root;
            element?.Remove();
            return element;
        }

        public static T XmlDeserialize<T>(this string xml, string rootName = null)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(xml));
            }

            return XDocument.Parse(xml).XmlDeserialize<T>(rootName);
        }

        public static T XmlDeserialize<T>(this FileInfo file, Encoding encoding, string rootName = null)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using (var stream = file.OpenRead())
            {
                return stream.IsGZipHeader()
                           ? XDocument.Parse(stream.UnZipStream(encoding)).XmlDeserialize<T>(rootName)
                           : XDocument.Load(stream).XmlDeserialize<T>(rootName);
            }
        }

        public static T XmlDeserialize<T>(this XDocument document, string rootName = null)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            using (var reader = document.CreateReader())
            {
                return (T)CreateOverrider<T>(rootName).Deserialize(reader);
            }
        }

        public static object XmlDeserialize(this XDocument document, Type type, string rootName = null)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            using (var reader = document.CreateReader())
            {
                return CreateOverrider(type, rootName).Deserialize(reader);
            }
        }

        public static T XmlDeserialize<T>(this XElement element, string rootName = null)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            using (var reader = element.CreateReader())
            {
                return (T)CreateOverrider<T>(rootName).Deserialize(reader);
            }
        }

        public static T XmlDeserializeZip<T>(this byte[] data, string rootName = null)
        {
            return XDocument.Parse(data.UnZipString()).XmlDeserialize<T>(rootName);
        }

        public static T XmlDeserializeZip<T>(this byte[] data, Encoding encoding, string rootName = null)
        {
            return XDocument.Parse(data.UnZipString(encoding)).XmlDeserialize<T>(rootName);
        }

        public static void XmlSerialize<T>(this FileInfo file, T data, bool compress, string rootName = null)
        {
            if (compress)
            {
                File.WriteAllBytes(file.FullName, data.XmlSerializeZip(Encoding.ASCII, "DataItem"));
            }
            else
            {
                data.XmlSerialize("DataItem").Save(file.FullName);
            }
        }

        public static XDocument XmlSerialize<T>(this T instance, string rootName = null)
        {
            return CreateOverrider<T>(rootName).XmlSerialize(instance);
        }

        public static XDocument XmlSerialize(this XmlSerializer serializer, object instance)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            //Create our own namespaces for the output
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            //Add an empty namespace and empty value
            namespaces.Add(string.Empty, string.Empty);
            XDocument document = new XDocument();
            using (XmlWriter writer = document.CreateWriter())
            {
                serializer.Serialize(writer, instance, namespaces);
            }

            return document;
        }

        public static byte[] XmlSerializeZip<T>(this T instance, string rootName = null)
        {
            return instance.XmlSerialize(rootName).ToString().Zip();
        }

        public static byte[] XmlSerializeZip<T>(this T instance, Encoding encoding, string rootName = null)
        {
            return instance.XmlSerialize(rootName).ToString().Zip(encoding);
        }

        private static XmlSerializer CreateOverrider<T>(string rootName = null)
        {
            var type = typeof(T);
            return CreateOverrider(type, rootName);
        }

        private static XmlSerializer CreateOverrider(Type type, string rootName = null)
        {
            string name = type.Name + rootName;
            if (!serializerCache.TryGetValue(name, out XmlSerializer serializer))
            {
                if (rootName != null)
                {
                    XmlAttributes myXmlAttributes = new XmlAttributes();
                    XmlRootAttribute myXmlRootAttribute = new XmlRootAttribute
                    {
                        ElementName = rootName,
                        Namespace = string.Empty
                    };

                    myXmlAttributes.XmlRoot = myXmlRootAttribute;
                    XmlAttributeOverrides myXmlAttributeOverrides = new XmlAttributeOverrides();
                    myXmlAttributeOverrides.Add(type, myXmlAttributes);
                    serializer = new XmlSerializer(type, myXmlAttributeOverrides);
                }
                else
                {
                    serializer = new XmlSerializer(type);
                }

                serializerCache[name] = serializer;
            }

            return serializer;
        }

        /// <summary>
        ///     Whether a given character is allowed by XML 1.0.
        /// </summary>
        private static bool IsLegalXmlChar(int character, bool withSpecialSymbols)
        {
            return
                (withSpecialSymbols && (
                                           character == 0x9 /* == '\t' == 9   */||
                                           character == 0xA /* == '\n' == 10  */||
                                           character == 0xD /* == '\r' == 13  */)) ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF);
        }
    }
}
