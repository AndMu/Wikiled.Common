using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            var files = FileManager.FindFilesByMask(TestContext.CurrentContext.TestDirectory, "test.*");
            foreach (var file in files)
            {
                File.Delete(file);
            }

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "test.csv"), "Data");
        }

        [Test]
        public void Backup()
        {
            var total = FileManager.FindFilesByMask(TestContext.CurrentContext.TestDirectory, "test.*").Count();
            ClassicAssert.AreEqual(1, total);
            
            Path.Combine(TestContext.CurrentContext.TestDirectory, "test.csv").Backup();
            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "test.csv"), "Data");
            total = FileManager.FindFilesByMask(TestContext.CurrentContext.TestDirectory, "Test.*").Count();
            ClassicAssert.AreEqual(2, total);
            
            Path.Combine(TestContext.CurrentContext.TestDirectory, "test.csv").Backup();
            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "test.csv"), "Data");
            total = FileManager.FindFilesByMask(TestContext.CurrentContext.TestDirectory, "test.*").Count();
            ClassicAssert.AreEqual(3, total);
        }
    }
}
