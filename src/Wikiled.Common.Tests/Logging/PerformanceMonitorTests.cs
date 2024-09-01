using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Logging;

namespace Wikiled.Common.Tests.Logging
{
    [TestFixture]
    public class PerformanceMonitorTests
    {
        private PerformanceMonitor instance;

        [SetUp]
        public void Setup()
        {
            instance = CreatePerformanceMonitor();
        }

        [Test]
        public void Construct()
        {
            ClassicAssert.AreEqual("Processed: 0/9 Operations per second: 0", instance.ToString());
            for (int i = 0; i < 5; i++)
            {
                instance.ManualyCount();
                instance.Increment();
            }

            instance.Increment();
            ClassicAssert.AreEqual("Processed: 6/9 Opera", instance.ToString().Substring(0, 20));
            for (int i = 0; i < 10; i++)
            {
                instance.ManualyCount();
            }

            for (int i = 0; i < 10; i++)
            {
                instance.Increment();
            }

            ClassicAssert.AreEqual("Processed: 16/16 Ope", instance.ToString().Substring(0, 20));
        }

        private PerformanceMonitor CreatePerformanceMonitor()
        {
            return new PerformanceMonitor(9);
        }
    }
}
