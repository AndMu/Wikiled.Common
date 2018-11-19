using System.IO;
using System.Linq;
using NUnit.Framework;
using Wikiled.Common.Extensions;
using Wikiled.Common.Helpers;

namespace Wikiled.Common.Tests.Extensions
{
    [TestFixture]
    public class FileExtensionsTests
    {
        [SetUp]
        public void SetUp()
        {
            var files = FileManager.FindFilesByMask(TestContext.CurrentContext.TestDirectory, "Test.*");
            foreach (var file in files)
            {
                File.Delete(file);
            }

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "test.csv"), "Data");
        }

        [Test]
        public void Backup()
        {
            var total = FileManager.FindFilesByMask(TestContext.CurrentContext.TestDirectory, "Test.*").Count();
            Assert.AreEqual(1, total);
            
            Path.Combine(TestContext.CurrentContext.TestDirectory, "Test.csv").Backup();
            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "test.csv"), "Data");
            total = FileManager.FindFilesByMask(TestContext.CurrentContext.TestDirectory, "Test.*").Count();
            Assert.AreEqual(2, total);
            
            Path.Combine(TestContext.CurrentContext.TestDirectory, "Test.csv").Backup();
            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "test.csv"), "Data");
            total = FileManager.FindFilesByMask(TestContext.CurrentContext.TestDirectory, "Test.*").Count();
            Assert.AreEqual(3, total);
        }
    }
}
