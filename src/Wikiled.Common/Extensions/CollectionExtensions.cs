using System;
using System.Collections.Generic;
using System.Linq;

namespace Wikiled.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
        {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                int swapIndex = random.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
    }
}
