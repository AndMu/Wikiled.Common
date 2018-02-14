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
            var ipAddress = "server".ToIpAddress();
            var ipv6Address = "server".ToIpAddress(true);
            Assert.IsNotNull(ipAddress);
            Assert.IsNotNull(ipv6Address);
        }
    }
}
