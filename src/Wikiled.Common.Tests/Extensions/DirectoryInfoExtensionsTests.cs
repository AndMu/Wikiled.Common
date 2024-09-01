using System.IO;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Tests.Extensions
{
    [TestFixture]
    public class DirectoryInfoExtensionsTests
    {
        private DirectoryInfo directory;

        [SetUp]
        public void Setup()
        {
            directory = new DirectoryInfo("TestDir");
            if (directory.Exists)
            {
                directory.Delete();
            }
        }

        [TearDown]
        public void TearDown()
        {
            directory.Refresh();
            if (directory.Exists)
            {
                directory.Delete();
            }
        }

        [Test]
        public void EnsureDirectoryExistence()
        {
            directory.Refresh();
            ClassicAssert.IsFalse(directory.Exists);
            directory.EnsureDirectoryExistence();
            directory.Refresh();
            ClassicAssert.IsTrue(directory.Exists);
        }
    }
}
