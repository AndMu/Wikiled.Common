using System.IO;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Tests.Extensions
{
    [TestFixture]
    public class PathExtensionsTests
    {
        [Test]
        public void GetRelativePath()
        {
            ClassicAssert.AreEqual(@".\", @"c:\1\2\3".GetRelativePath(@"c:\1\2\"));
            ClassicAssert.AreEqual(@"3\4", @"c:\1\2\3".GetRelativePath(@"c:\1\2\3\4"));
            ClassicAssert.AreEqual(@"4", @"c:\1\2\3\".GetRelativePath(@"c:\1\2\3\4"));
            ClassicAssert.AreEqual(@"c:\1\2\3\", new DirectoryInfo(Path.Combine(@"c:\1\2\3", @"c:\1\2\3".GetRelativePath(@"c:\1\2\"))).FullName);
            ClassicAssert.AreEqual(@".\", @"c:\1\2 2\3".GetRelativePath(@"c:\1\2 2\"));
        }

        [Test]
        public void InsertSubFolder()
        {
            ClassicAssert.AreEqual(@"c:\1\2\3\4", @"c:\1\2\3".InsertSubFolder("4", 0));
            ClassicAssert.AreEqual(@"c:\1\2\4\3", @"c:\1\2\3".InsertSubFolder("4", 1));
            ClassicAssert.AreEqual(@"c:\1\4\2\3", @"c:\1\2\3".InsertSubFolder("4", 2));
            ClassicAssert.AreEqual(@"c:\4\1\2\3", @"c:\1\2\3".InsertSubFolder("4", 3));
            ClassicAssert.AreEqual(@"c:\4\1\2\3", @"c:\1\2\3".InsertSubFolder("4", 4));
        }

        [Test]
        public void RelativeInsertSubFolder()
        {
            ClassicAssert.AreEqual(@"1\2\3\4", @"1\2\3".InsertSubFolder("4", 0));
            ClassicAssert.AreEqual(@"1\2\4\3", @"1\2\3".InsertSubFolder("4", 1));
            ClassicAssert.AreEqual(@"1\4\2\3", @"1\2\3".InsertSubFolder("4", 2));
            ClassicAssert.AreEqual(@"4\1\2\3", @"1\2\3".InsertSubFolder("4", 3));
            ClassicAssert.AreEqual(@"4\1\2\3", @"1\2\3".InsertSubFolder("4", 4));
        }

        [TestCase("/windows/system32", "/windows/system32/")]
        [TestCase("c:/windows/system32/", "c:/windows/system32/")]
        [TestCase("/windows/system32\\", "/windows/system32\\")]
        [TestCase("\\windows\\system32\\", "\\windows\\system32\\")]
        public void PathAddBackslash(string path, string result)
        {
            ClassicAssert.AreEqual(result, path.PathAddBackslash());
        }
    }
}
