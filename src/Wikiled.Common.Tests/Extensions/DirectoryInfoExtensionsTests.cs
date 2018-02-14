using System.IO;
using NUnit.Framework;
using Wikiled.Common.Extensions;
using Wikiled.Core.Utility.Extensions;

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
            Assert.IsFalse(directory.Exists);
            directory.EnsureDirectoryExistence();
            directory.Refresh();
            Assert.IsTrue(directory.Exists);
        }
    }
}
