using System;
using NUnit.Framework;
using Wikiled.Common.Helpers;

namespace Wikiled.Common.Tests.Helpers
{
    [TestFixture]
    public class DateTimeHelperTests
    {
        [TestCase(1328140800, "2012-02-02")]
        [TestCase(1391299200, "2014-02-02")]
        [TestCase(0, "1970-01-01")]
        public void FromUnixTime(long seconds, string date)
        {
            var result = seconds.FromUnixTime();
            Assert.AreEqual(DateTime.Parse(date), result);
        }

        [TestCase("2012-02-02", 1328140800)]
        [TestCase("2014-02-02", 1391299200)]
        public void FromUnixTime(string date, long seconds)
        {
            var result = DateTime.Parse(date).ToUnixTime();
            Assert.AreEqual(seconds, result);
        }
    }
}
