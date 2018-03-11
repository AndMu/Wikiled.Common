using System;

namespace Wikiled.Common.Helpers
{
    public class RandomHelper
    {
        public static int Seed { get; } = Environment.TickCount;
    }
}
