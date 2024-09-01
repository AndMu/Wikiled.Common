using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.IsNotNull(ipAddress);
            ClassicAssert.IsNotNull(ipv6Address);
        }
    }
}
