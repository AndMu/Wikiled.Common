using System;

namespace Wikiled.Common.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            return Epoch.AddSeconds(unixTimeStamp);
        }

        public static long ToUnixTime(this DateTime data)
        {
            return Convert.ToInt64((data - Epoch).TotalSeconds);
        }

        public static DateTime FromUnixTimeMilis(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            return Epoch.AddMilliseconds(unixTimeStamp);
        }

        public static long ToUnixTimeMilis(this DateTime data)
        {
            return Convert.ToInt64((data - Epoch).TotalMilliseconds);
        }

        public static DateTime FromUnixDays(this int unixTimeStamp)
        {
            return Epoch.AddDays(unixTimeStamp);
        }

        public static int ToUnixDays(this DateTime data)
        {
            return Convert.ToInt32((data - Epoch).TotalDays);
        }
    }
}
