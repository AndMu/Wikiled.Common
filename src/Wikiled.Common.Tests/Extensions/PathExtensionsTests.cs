﻿using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Tests.Extensions
{
    [TestFixture]
    public class PathExtensionsTests
    {
        private string root;

        [SetUp]
        public void Setup()
        {
            root = Path.GetPathRoot(Environment.CurrentDirectory) ?? "/";
        }

        [Test]
        public void GetRelativePath()
        {
            var sep = Path.DirectorySeparatorChar;
            ClassicAssert.AreEqual($".{sep}", Path.Combine(root, $"1{sep}2{sep}3").GetRelativePath(Path.Combine(root, $"1{sep}2{sep}")));
            ClassicAssert.AreEqual($"3{sep}4", Path.Combine(root, $"1{sep}2{sep}3").GetRelativePath(Path.Combine(root, $"1{sep}2{sep}3{sep}4")));
            ClassicAssert.AreEqual("4", Path.Combine(root, $"1{sep}2{sep}3{sep}").GetRelativePath(Path.Combine(root, $"1{sep}2{sep}3{sep}4")));
            ClassicAssert.AreEqual(
                Path.Combine(root, $"1{sep}2{sep}3{sep}"),
                new DirectoryInfo(Path.Combine(Path.Combine(root, $"1{sep}2{sep}3"), Path.Combine(root, $"1{sep}2{sep}3").GetRelativePath(Path.Combine(root, $"1{sep}2{sep}")))).FullName);
            ClassicAssert.AreEqual($".{sep}", Path.Combine(root, $"1{sep}2 2{sep}3").GetRelativePath(Path.Combine(root, $"1{sep}2 2{sep}")));
        }

        [Test]
        public void InsertSubFolder()
        {
            var sep = Path.DirectorySeparatorChar;
            ClassicAssert.AreEqual(Path.Combine(root, $"1{sep}2{sep}3{sep}4"), Path.Combine(root, $"1{sep}2{sep}3").InsertSubFolder("4", 0));
            ClassicAssert.AreEqual(Path.Combine(root, $"1{sep}2{sep}4{sep}3"), Path.Combine(root, $"1{sep}2{sep}3").InsertSubFolder("4", 1));
            ClassicAssert.AreEqual(Path.Combine(root, $"1{sep}4{sep}2{sep}3"), Path.Combine(root, $"1{sep}2{sep}3").InsertSubFolder("4", 2));
            ClassicAssert.AreEqual(Path.Combine(root, $"4{sep}1{sep}2{sep}3"), Path.Combine(root, $"1{sep}2{sep}3").InsertSubFolder("4", 3));
            ClassicAssert.AreEqual(Path.Combine(root, $"4{sep}1{sep}2{sep}3"), Path.Combine(root, $"1{sep}2{sep}3").InsertSubFolder("4", 4));
        }

        [Test]
        public void RelativeInsertSubFolder()
        {
            var sep = Path.DirectorySeparatorChar;
            ClassicAssert.AreEqual($"1{sep}2{sep}3{sep}4", $"1{sep}2{sep}3".InsertSubFolder("4", 0));
            ClassicAssert.AreEqual($"1{sep}2{sep}4{sep}3", $"1{sep}2{sep}3".InsertSubFolder("4", 1));
            ClassicAssert.AreEqual($"1{sep}4{sep}2{sep}3", $"1{sep}2{sep}3".InsertSubFolder("4", 2));
            ClassicAssert.AreEqual($"4{sep}1{sep}2{sep}3", $"1{sep}2{sep}3".InsertSubFolder("4", 3));
            ClassicAssert.AreEqual($"4{sep}1{sep}2{sep}3", $"1{sep}2{sep}3".InsertSubFolder("4", 4));
        }

        [TestCase("folder/subfolder", "folder/subfolder/")]
        [TestCase("folder/subfolder/", "folder/subfolder/")]
        [TestCase("folder\\subfolder", "folder\\subfolder\\")]
        [TestCase("folder\\subfolder\\", "folder\\subfolder\\")]
        public void PathAddBackslash(string path, string expectedResult)
        {
            var result = path.PathAddBackslash();
            var expectedSeparator = expectedResult.EndsWith("/") ? "/" : "\\";
            var actualSeparator = result.EndsWith("/") ? "/" : "\\";

            Assert.That(result, Does.EndWith(actualSeparator), "The result should end with a separator");
            Assert.That(actualSeparator, Is.EqualTo(expectedSeparator), "The separator type should be preserved");
            Assert.That(result.TrimEnd('/', '\\'), Is.EqualTo(path.TrimEnd('/', '\\')), "The path (without trailing separator) should remain unchanged");
        }
    }
}