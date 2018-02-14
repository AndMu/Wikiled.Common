using System;
using System.Xml.Linq;
using NUnit.Framework;
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
            Assert.AreEqual("Hi", instance.Name);
            Assert.AreEqual(1, instance.Data);
            Assert.AreEqual(3, instance.Data3);
        }

        [Test]
        public void DeserializeNullNode()
        {
            XElement element = null;
            Assert.Throws<ArgumentNullException>(() => element.XmlDeserialize<MockConfiguration>("x"));
        }

        [Test]
        public void DeserializeText()
        {
            var instance = xml.XmlDeserialize<MockConfiguration>("x");
            Assert.AreEqual("Hi", instance.Name);
            Assert.AreEqual(1, instance.Data);
            Assert.AreEqual(3, instance.Data3);
        }

        [Test]
        public void DeserializeTextNull()
        {
            Assert.Throws<ArgumentException>(() => string.Empty.XmlDeserialize<MockConfiguration>("x"));
        }

        [Test]
        public void DeserializeTextOriginal()
        {
            var instance = xmlOriginal.XmlDeserialize<MockConfiguration>();
            Assert.AreEqual("Hi", instance.Name);
            Assert.AreEqual(1, instance.Data);
            Assert.AreEqual(3, instance.Data3);
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
            Assert.Throws<InvalidOperationException>(() => element.XmlDeserialize<MockConfiguration>("xx"));
        }

        [Test]
        public void SanitizeXmlString()
        {
            string text = "test" + (char)1;
            string result = text.SanitizeXmlString();
            Assert.AreEqual("test", result);
        }

        [Test]
        public void SanitizeXmlStringWithEndOfLine()
        {
            string result = "test\n\t".SanitizeXmlString();
            Assert.AreEqual("test", result);
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
            Assert.IsNotNull(element);
            Assert.AreEqual("x", element.Name.LocalName);
            Assert.AreEqual("Hi", element.Element("Name").Value);
            Assert.AreEqual("1", element.Element("Data").Value);
            Assert.AreEqual("3", element.Element("Data3").Value);
        }

        [Test]
        public void SerializeAsXElementNull()
        {
            MockConfiguration text = null;
            Assert.Throws<ArgumentNullException>(() => text.SerializeAsXElement("x"));
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
            Assert.Greater(instance.Length, 0);
            var back = instance.XmlDeserializeZip<MockConfiguration>();
            Assert.AreEqual("Hi", back.Name);
            Assert.AreEqual(1, back.Data);
            Assert.AreEqual(3, back.Data3);
        }
    }
}
