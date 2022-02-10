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
        public void ToUnixTime(string date, long seconds)
        {
            var result = DateTime.Parse(date).ToUnixTime();
            Assert.AreEqual(seconds, result);
        }

        [TestCase(1328140800000, "2012-02-02")]
        [TestCase(1391299200000, "2014-02-02")]
        [TestCase(0, "1970-01-01")]
        public void FromUnixTimeMilis(long seconds, string date)
        {
            var result = seconds.FromUnixTimeMilis();
            Assert.AreEqual(DateTime.Parse(date), result);
        }

        [TestCase("2012-02-02", 1328140800000)]
        [TestCase("2014-02-02", 1391299200000)]
        public void ToUnixTimeMilis(string date, long seconds)
        {
            var result = DateTime.Parse(date).ToUnixTimeMilis();
            Assert.AreEqual(seconds, result);
        }

        [TestCase(15372, "2012-02-02")]
        [TestCase(16103, "2014-02-02")]
        [TestCase(0, "1970-01-01")]
        public void FromUnixDays(int days, string date)
        {
            var result = days.FromUnixDays();
            Assert.AreEqual(DateTime.Parse(date), result);
        }

        [TestCase("2012-02-02", 15372)]
        [TestCase("2014-02-02", 16103)]
        public void ToUnixDays(string date, int seconds)
        {
            var result = DateTime.Parse(date).ToUnixDays();
            Assert.AreEqual(seconds, result);
        }
    }
}
