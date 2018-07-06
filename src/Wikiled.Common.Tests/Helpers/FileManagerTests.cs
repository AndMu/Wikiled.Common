using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Wikiled.Common.Helpers;

namespace Wikiled.Common.Tests.Helpers
{
    [TestFixture]
    public class FileManagerTests
    {

        [Test]
        public void FindFiles()
        {
            var files = FileManager.FindFiles("..", ".dll", ".pdb").ToArray();
            Assert.Greater(files.Length, 1);
            Assert.True(files.Any(item => string.Compare(Path.GetExtension(item), ".pdb", StringComparison.OrdinalIgnoreCase) == 0));
            Assert.True(files.Any(item => string.Compare(Path.GetExtension(item), ".dll", StringComparison.OrdinalIgnoreCase) == 0));
        }

        [Test]
        public void Arguments()
        {
            Assert.Throws<ArgumentException>(() => FileManager.FindFiles(null, ".dll", ".pdb").ToArray());
            Assert.Throws<ArgumentNullException>(() => FileManager.FindFiles("..", null).ToArray());
            Assert.Throws<ArgumentOutOfRangeException>(() => FileManager.FindFiles("..").ToArray());
            Assert.Throws<ArgumentOutOfRangeException>(() => FileManager.FindFiles("..", "dll").ToArray());
            Assert.Throws<ArgumentOutOfRangeException>(() => FileManager.FindFiles("..", "*.dll").ToArray());
        }
    }
}
