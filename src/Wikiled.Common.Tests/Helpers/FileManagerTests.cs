using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.Greater(files.Length, 1);
            ClassicAssert.True(files.Any(item => string.Compare(Path.GetExtension(item), ".pdb", StringComparison.OrdinalIgnoreCase) == 0));
            ClassicAssert.True(files.Any(item => string.Compare(Path.GetExtension(item), ".dll", StringComparison.OrdinalIgnoreCase) == 0));
        }

        [Test]
        public void Arguments()
        {
            ClassicAssert.Throws<ArgumentException>(() => FileManager.FindFiles(null, ".dll", ".pdb").ToArray());
            ClassicAssert.Throws<ArgumentNullException>(() => FileManager.FindFiles("..", null).ToArray());
            ClassicAssert.Throws<ArgumentOutOfRangeException>(() => FileManager.FindFiles("..").ToArray());
            ClassicAssert.Throws<ArgumentOutOfRangeException>(() => FileManager.FindFiles("..", "dll").ToArray());
            ClassicAssert.Throws<ArgumentOutOfRangeException>(() => FileManager.FindFiles("..", "*.dll").ToArray());
        }
    }
}
