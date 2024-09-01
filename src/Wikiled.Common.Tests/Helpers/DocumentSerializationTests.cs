using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.IsTrue(File.Exists(guid));

            using (var stream = new FileStream(guid, FileMode.Open))
            {
                var text = stream.UnZipStream(Encoding.UTF8);
                var result = XDocument.Parse(text);
                ClassicAssert.AreEqual(document.Root.Name, result.Root.Name);
            }
        }
    }
}
