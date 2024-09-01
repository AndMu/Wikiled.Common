using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Helpers;

namespace Wikiled.Common.Tests.Helpers
{
    [TestFixture]
    public class DirectoryRelativeCopyItemTests
    {
        private string sourceFolder;

        private string targetFolder;

        [SetUp]
        public void Setup()
        {
            sourceFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "Helpers" ,"Data");
            targetFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "Helpers" ,"Data2");
            if (Directory.Exists(targetFolder))
            {
                Directory.Delete(targetFolder, true);
            }
        }

        [Test]
        public void Copy()
        {
            FileRelativeCopyItem[] files = DirectoryRelativeCopyItem.Copy(sourceFolder, targetFolder).ToArray();
            ClassicAssert.AreEqual(2, files.Length);
            ClassicAssert.False(File.Exists(files[0].FileTarget));
            ClassicAssert.True(File.Exists(files[0].FileSource));
            files[0].Copy();
            ClassicAssert.True(File.Exists(files[0].FileTarget));
            ClassicAssert.True(File.Exists(files[0].FileSource));
            files[1].Copy();
            ClassicAssert.True(File.Exists(files[1].FileTarget));
        }

        [Test]
        public void Read()
        {
            FileRelativeCopyItem[] files = DirectoryRelativeCopyItem.Copy(sourceFolder, targetFolder).ToArray();
            ClassicAssert.AreEqual(2, files.Length);
            ClassicAssert.IsNull(files[0].Content);
            files[0].Read();
            files[0].Content = "22";
            files[0].Copy();
            var content = File.ReadAllText(files[0].FileTarget);
            ClassicAssert.AreEqual(content, files[0].Content);
        }
    }
}
