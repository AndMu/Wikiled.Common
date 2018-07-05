using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wikiled.Common.Reflection;
using Wikiled.Common.Tests.Data;

namespace Wikiled.Common.Tests.Helpers
{
    [TestFixture]
    public class ReflectionHelperTests
    {
        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentException>(() => typeof(SubClass).IsSubclassOfGeneric(typeof(Dictionary<string, string>)));
            var result = typeof(SubClass).IsSubclassOfGeneric(typeof(Dictionary<,>));
            Assert.IsTrue(result);
            result = typeof(ReflectionHelperTests).IsSubclassOfGeneric(typeof(IDictionary<,>));
            Assert.IsFalse(result);
            result = typeof(SubClass2).IsSubclassOfGeneric(typeof(List<>));
            Assert.IsTrue(result);
        }
    }
}