using NUnit.Framework;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Tests.Extensions
{
    [TestFixture]
    public class NetworkingExtensionsTests
    {
        [Test]
        public void ToIpAddress()
        {
            var ipAddress = "localhost".ToIpAddress();
            var ipv6Address = "localhost".ToIpAddress(true);
            Assert.IsNotNull(ipAddress);
            Assert.IsNotNull(ipv6Address);
        }
    }
}
