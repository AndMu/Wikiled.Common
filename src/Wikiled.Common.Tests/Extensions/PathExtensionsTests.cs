using System.IO;
using NUnit.Framework;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Tests.Extensions
{
    [TestFixture]
    public class PathExtensionsTests
    {
        [Test]
        public void GetRelativePath()
        {
            Assert.AreEqual(@".\", @"c:\1\2\3".GetRelativePath(@"c:\1\2\"));
            Assert.AreEqual(@"3\4", @"c:\1\2\3".GetRelativePath(@"c:\1\2\3\4"));
            Assert.AreEqual(@"4", @"c:\1\2\3\".GetRelativePath(@"c:\1\2\3\4"));
            Assert.AreEqual(@"c:\1\2\3\", new DirectoryInfo(Path.Combine(@"c:\1\2\3", @"c:\1\2\3".GetRelativePath(@"c:\1\2\"))).FullName);
            Assert.AreEqual(@".\", @"c:\1\2 2\3".GetRelativePath(@"c:\1\2 2\"));
        }

        [Test]
        public void InsertSubFolder()
        {
            Assert.AreEqual(@"c:\1\2\3\4", @"c:\1\2\3".InsertSubFolder("4", 0));
            Assert.AreEqual(@"c:\1\2\4\3", @"c:\1\2\3".InsertSubFolder("4", 1));
            Assert.AreEqual(@"c:\1\4\2\3", @"c:\1\2\3".InsertSubFolder("4", 2));
            Assert.AreEqual(@"c:\4\1\2\3", @"c:\1\2\3".InsertSubFolder("4", 3));
            Assert.AreEqual(@"c:\4\1\2\3", @"c:\1\2\3".InsertSubFolder("4", 4));
        }

        [Test]
        public void RelativeInsertSubFolder()
        {
            Assert.AreEqual(@"1\2\3\4", @"1\2\3".InsertSubFolder("4", 0));
            Assert.AreEqual(@"1\2\4\3", @"1\2\3".InsertSubFolder("4", 1));
            Assert.AreEqual(@"1\4\2\3", @"1\2\3".InsertSubFolder("4", 2));
            Assert.AreEqual(@"4\1\2\3", @"1\2\3".InsertSubFolder("4", 3));
            Assert.AreEqual(@"4\1\2\3", @"1\2\3".InsertSubFolder("4", 4));
        }

        [TestCase("/windows/system32", "/windows/system32/")]
        [TestCase("c:/windows/system32/", "c:/windows/system32/")]
        [TestCase("/windows/system32\\", "/windows/system32\\")]
        [TestCase("\\windows\\system32\\", "\\windows\\system32\\")]
        public void PathAddBackslash(string path, string result)
        {
            Assert.AreEqual(result, path.PathAddBackslash());
        }
    }
}
