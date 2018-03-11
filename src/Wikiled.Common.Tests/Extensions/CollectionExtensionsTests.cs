using System;
using System.Linq;
using NUnit.Framework;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Tests.Extensions
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void Shuffle()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6 };
            var suffled = data.Shuffle(new Random(32)).ToArray();
            var suffled2 = data.Shuffle(new Random(32)).ToArray();
            var random = new Random(32);
            var suffled3 = data.Shuffle(random).ToArray();
            Assert.AreEqual(6, suffled.Count());
            for (int i = 0; i < suffled.Length; i++)
            {
                Assert.AreEqual(suffled[i], suffled2[i]);
                Assert.AreEqual(suffled[i], suffled3[i]);
            }
        }
    }
}