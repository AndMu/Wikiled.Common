using System.Collections.Concurrent;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(2, table.Count);
            var newTable = table.Compact();
            ClassicAssert.AreEqual(2, newTable.Count);
        }

        [Test]
        public void GetAddItem()
        {
            ConcurrentDictionary<string, string> table = new ConcurrentDictionary<string, string>();
            var result = table.TryGetAddItem("Test", "Test", out string value);
            ClassicAssert.IsTrue(result);
            ClassicAssert.AreEqual("Test", value);
            result = table.TryGetAddItem("Test", "Test1", out value);
            ClassicAssert.IsTrue(result);
            ClassicAssert.AreEqual("Test", value);
        }
    }
}
