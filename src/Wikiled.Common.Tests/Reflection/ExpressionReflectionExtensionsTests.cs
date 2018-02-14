using System;
using System.Linq;
using NUnit.Framework;
using Wikiled.Common.Extensions;
using Wikiled.Common.Tests.Data;
using Wikiled.Core.Utility.Extensions;

namespace Wikiled.Core.Utility.Tests.Extensions
{
    [TestFixture]
    public class ExpressionReflectionExtensionsTests
    {
        [Test]
        public void GetValueGetterStringNull()
        {
            TestType config = new TestType();
            var property = config.GetType().GetProperties().First(item => item.Name == "Value");
            var result = property.GetValueGetter<TestType>()(config);
            Assert.AreEqual(string.Empty, result);

            config.Value = 10;
            result = property.GetValueGetter<TestType>()(config);
            Assert.AreEqual("10", result);
        }

        [Test]
        public void GetSetValueDate()
        {
            TestType config = new TestType();
            config.Date = new DateTime(2012, 02, 03, 14, 14, 14);
            var property = config.GetType().GetProperties().First(item => item.Name == "Date");
            var result = property.GetValueGetter<TestType>()(config);
            Assert.AreEqual("03/02/2012 14:14:14", result);
        }

        [Test]
        public void GetValueGetterStringInt()
        {
            TestType config = new TestType();
            var property = config.GetType().GetProperties().First(item => item.Name == "Another");
            var result = property.GetValueGetter<TestType>()(config);
            Assert.AreEqual("0", result);

            config.Another = 10;
            result = property.GetValueGetter<TestType>()(config);
            Assert.AreEqual("10", result);
        }

        [Test]
        public void GetValueGetterString()
        {
            TestType config = new TestType();
            var property = config.GetType().GetProperties().First(item => item.Name == "Data");
            var result = property.GetValueGetter<TestType>()(config);
            Assert.AreEqual(null, result);

            config.Data = "Test";
            result = property.GetValueGetter<TestType>()(config);
            Assert.AreEqual("Test", result);
        }

        [Test]
        public void GetValueGetterStringEnum()
        {
            TestType config = new TestType();
            var property = config.GetType().GetProperties().First(item => item.Name == "Status1");
            var result = property.GetValueGetter<TestType>()(config);
            Assert.AreEqual("Bool", result);

            config.Status1 = BasicTypes.Byte;
            result = property.GetValueGetter<TestType>()(config);
            Assert.AreEqual("Byte", result);
        }

        [Test]
        public void GetValueGetterConverted()
        {
            TestType config = new TestType();
            var property = config.GetType().GetProperties().First(item => item.Name == "Another");
            property.GetValueSetter<TestType, int, string>(int.Parse)(config, "1");
            var result = property.GetValueGetter<TestType, int, string>(value => value.ToString())(config);
            Assert.AreEqual("1", result);
            Assert.AreEqual(1, config.Another);
        }

        [Test]
        public void GetValueGetterSetter()
        {
            TestType config = new TestType();
            var property = config.GetType().GetProperties().First(item => item.Name == "Data");
            property.GetValueSetter<TestType, string>()(config, "Test");
            var result = property.GetValueGetter<TestType, string>()(config);
            Assert.AreEqual("Test", result);
            Assert.AreEqual("Test", config.Data);
        }

        [Test]
        public void GetValueGetterSetterGeneric()
        {
            TestType config = new TestType();
            var property = config.GetType().GetProperties().First(item => item.Name == "Data");
            property.GetValueSetter<TestType, object>()(config, "Test");
            var result = property.GetValueGetter<TestType, object>()(config);
            Assert.AreEqual("Test", result);
            Assert.AreEqual("Test", config.Data);
        }
    }
}
