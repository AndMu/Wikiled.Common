using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.Throws<ArgumentOutOfRangeException>(() => typeof(SubClass).IsSubclassOfGeneric(typeof(Dictionary<string, string>)));
            var result = typeof(SubClass).IsSubclassOfGeneric(typeof(Dictionary<,>));
            ClassicAssert.IsTrue(result);
            result = typeof(ReflectionHelperTests).IsSubclassOfGeneric(typeof(IDictionary<,>));
            ClassicAssert.IsFalse(result);
            result = typeof(SubClass2).IsSubclassOfGeneric(typeof(List<>));
            ClassicAssert.IsTrue(result);
        }
    }
}