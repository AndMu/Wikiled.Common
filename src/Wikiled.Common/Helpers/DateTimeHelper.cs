using System;

namespace Wikiled.Common.Helpers
{
    public static class DateTimeHelper
    {
        private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            return epoch.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        public static long ToUnixTime(this DateTime data)
        {
            return Convert.ToInt64((data - epoch).TotalSeconds);
        }
    }
}
