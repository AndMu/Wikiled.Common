using System;
using System.Xml.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Serialization;
using Wikiled.Common.Tests.Data;

namespace Wikiled.Common.Tests.Helpers
{
    [TestFixture]
    public class XmlSerializerExtensionTests
    {
        private const string xml = "<x><Name>Hi</Name><Data>1</Data><Data3>3</Data3></x>";

        private const string xmlOriginal = "<MockConfiguration><Name>Hi</Name><Data>1</Data><Data3>3</Data3></MockConfiguration>";

        [Test]
        public void Deserialize()
        {
            MockConfiguration text = new MockConfiguration
                                         {
                                             Name = "Hi",
                                             Data = 1,
                                             Data3 = 3
                                         };
            XElement element = text.SerializeAsXElement("x");
            var instance = element.XmlDeserialize<MockConfiguration>("x");
            ClassicAssert.AreEqual("Hi", instance.Name);
            ClassicAssert.AreEqual(1, instance.Data);
            ClassicAssert.AreEqual(3, instance.Data3);
        }

        [Test]
        public void DeserializeNullNode()
        {
            XElement element = null;
            ClassicAssert.Throws<ArgumentNullException>(() => element.XmlDeserialize<MockConfiguration>("x"));
        }

        [Test]
        public void DeserializeText()
        {
            var instance = xml.XmlDeserialize<MockConfiguration>("x");
            ClassicAssert.AreEqual("Hi", instance.Name);
            ClassicAssert.AreEqual(1, instance.Data);
            ClassicAssert.AreEqual(3, instance.Data3);
        }

        [Test]
        public void DeserializeTextNull()
        {
            ClassicAssert.Throws<ArgumentException>(() => string.Empty.XmlDeserialize<MockConfiguration>("x"));
        }

        [Test]
        public void DeserializeTextOriginal()
        {
            var instance = xmlOriginal.XmlDeserialize<MockConfiguration>();
            ClassicAssert.AreEqual("Hi", instance.Name);
            ClassicAssert.AreEqual(1, instance.Data);
            ClassicAssert.AreEqual(3, instance.Data3);
        }

        [Test]
        public void DeserializeUnknownNode()
        {
            MockConfiguration text = new MockConfiguration
                                         {
                                             Name = "Hi",
                                             Data = 1,
                                             Data3 = 3
                                         };

            XElement element = text.SerializeAsXElement("x");
            ClassicAssert.Throws<InvalidOperationException>(() => element.XmlDeserialize<MockConfiguration>("xx"));
        }

        [Test]
        public void SanitizeXmlString()
        {
            string text = "test" + (char)1;
            string result = text.SanitizeXmlString();
            ClassicAssert.AreEqual("test", result);
        }

        [Test]
        public void SanitizeXmlStringWithEndOfLine()
        {
            string result = "test\n\t".SanitizeXmlString();
            ClassicAssert.AreEqual("test", result);
        }

        [Test]
        public void SerializeAsXElement()
        {
            MockConfiguration text = new MockConfiguration
                                         {
                                             Name = "Hi",
                                             Data = 1,
                                             Data3 = 3
                                         };
            XElement element = text.SerializeAsXElement("x");
            ClassicAssert.IsNotNull(element);
            ClassicAssert.AreEqual("x", element.Name.LocalName);
            ClassicAssert.AreEqual("Hi", element.Element("Name").Value);
            ClassicAssert.AreEqual("1", element.Element("Data").Value);
            ClassicAssert.AreEqual("3", element.Element("Data3").Value);
        }

        [Test]
        public void SerializeAsXElementNull()
        {
            MockConfiguration text = null;
            ClassicAssert.Throws<ArgumentNullException>(() => text.SerializeAsXElement("x"));
        }

        [Test]
        public void SerializeZip()
        {
            MockConfiguration text = new MockConfiguration
                                         {
                                             Name = "Hi",
                                             Data = 1,
                                             Data3 = 3
                                         };
            var instance = text.XmlSerializeZip();
            ClassicAssert.Greater(instance.Length, 0);
            var back = instance.XmlDeserializeZip<MockConfiguration>();
            ClassicAssert.AreEqual("Hi", back.Name);
            ClassicAssert.AreEqual(1, back.Data);
            ClassicAssert.AreEqual(3, back.Data3);
        }
    }
}
