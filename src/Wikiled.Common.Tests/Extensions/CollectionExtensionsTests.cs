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
            var suffled = data.Shuffle(new Random(32));
            Assert.AreEqual(6, suffled.Count());
        }
    }
}