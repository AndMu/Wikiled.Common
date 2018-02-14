using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Wikiled.Common.Helpers;

namespace Wikiled.Common.Tests.Helpers
{
    [TestFixture]
    public class ZipDataTests
    {
        private string file;

        [SetUp]
        public void Setup()
        {
            file = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Helpers\Data.zip");
        }

        [Test]
        public void Zip()
        {
            var data = "test".Zip();
            Assert.Greater(data.Length, 0);
            var text = data.UnZipString();
            Assert.AreEqual("test", text);
        }

        [Test]
        public void Encoded()
        {
            var data = "it YesÃ¢Â?Â?you'll".Zip();
            var text = data.UnZipString();
            Assert.AreEqual("it YesÃ¢Â?Â?you'll", text);
        }

        [Test]
        public void ZipEncoding()
        {
            var data = "test".Zip(Encoding.ASCII);
            Assert.Greater(data.Length, 0);
            var text = data.UnZipString(Encoding.ASCII);
            Assert.AreEqual("test", text);
        }

        [Test]
        public void IsGZipHeader()
        {
            var data = File.ReadAllBytes(file).ToArray();
            Assert.True(ZipData.IsGZipHeader(data));
        }

        [Test]
        public void IsGZipHeaderStream()
        {
            var data = File.OpenRead(file);
            Assert.True(ZipData.IsGZipHeader(data));
        }
    }
}
