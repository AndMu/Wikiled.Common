using System;

namespace Wikiled.Common.Extensions
{
    [Flags]
    public enum ReplacementOption
    {
        None = 0,

        IgnoreCase = 1,

        WholeWord = 2
    }
}
