using System.Collections.Concurrent;
using System.Collections.Generic;
using NUnit.Framework;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Tests.Extensions
{
    [TestFixture]
    public class DictionaryExtension
    {
        [Test]
        public void Compact()
        {
            Dictionary<int, int> table = new Dictionary<int, int>(1);
            table.Add(1, 1);
            table.Add(2, 1);
            Assert.AreEqual(2, table.Count);
            var newTable = table.Compact();
            Assert.AreEqual(2, newTable.Count);
        }

        [Test]
        public void GetAddItem()
        {
            ConcurrentDictionary<string, string> table = new ConcurrentDictionary<string, string>();
            var result = table.TryGetAddItem("Test", "Test", out string value);
            Assert.IsTrue(result);
            Assert.AreEqual("Test", value);
            result = table.TryGetAddItem("Test", "Test1", out value);
            Assert.IsTrue(result);
            Assert.AreEqual("Test", value);
        }
    }
}
