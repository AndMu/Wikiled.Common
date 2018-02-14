using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;
using Wikiled.Common.Helpers;
using Wikiled.Common.Serialization;

namespace Wikiled.Common.Tests.Helpers
{
    [TestFixture]
    public class DocumentSerializationTests
    {
        [Test]
        public void SaveSafe()
        {
            var document = XDocument.Parse("<Document></Document>");
            var guid = Guid.NewGuid().ToString();
            document.SaveSafe(guid);
            Assert.IsTrue(File.Exists(guid));

            using (var stream = new FileStream(guid, FileMode.Open))
            {
                var text = stream.UnZipStream(Encoding.UTF8);
                var result = XDocument.Parse(text);
                Assert.AreEqual(document.Root.Name, result.Root.Name);
            }
        }
    }
}
