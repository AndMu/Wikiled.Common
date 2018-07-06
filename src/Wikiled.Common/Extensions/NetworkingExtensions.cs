﻿using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Wikiled.Common.Extensions
{
    public static class NetworkingExtensions
    {
        /// <summary>
        /// Converts a string representing a host name or address to its <see cref="IPAddress"/> representation, 
        /// optionally opting to return a IpV6 address (defaults to IpV4)
        /// </summary>
        /// <param name="hostNameOrAddress">Host name or address to convert into an <see cref="IPAddress"/></param>
        /// <param name="favorIpV6">When <code>true</code> will return an IpV6 address whenever available, otherwise 
        /// returns an IpV4 address instead.</param>
        /// <returns>The <see cref="IPAddress"/> represented by <paramref name="hostNameOrAddress"/> in either IpV4 or
        /// IpV6 (when available) format depending on <paramref name="favorIpV6"/></returns>
        public static IPAddress ToIpAddress(this string hostNameOrAddress, bool favorIpV6 = false)
        {
            if (string.IsNullOrWhiteSpace(hostNameOrAddress))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(hostNameOrAddress));
            }

            var favoredFamily = favorIpV6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
            var addrs = Dns.GetHostAddresses(hostNameOrAddress);
            return addrs.FirstOrDefault(addr => addr.AddressFamily == favoredFamily) ?? addrs.FirstOrDefault();
        }
    }
}
