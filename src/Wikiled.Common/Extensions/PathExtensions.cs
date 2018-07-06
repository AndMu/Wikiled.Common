using System;
using System.IO;

namespace Wikiled.Common.Extensions
{
    public static class PathExtensions
    {
        public static string GetRelativePath(this string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(source));
            }

            if (string.IsNullOrEmpty(target))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(target));
            }

            Uri uri1 = new Uri(target);
            Uri uri2 = new Uri(source);
            return uri2.MakeRelativeUri(uri1)
                .ToString()
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace("%20", " ");
        }

        public static string InsertSubFolder(this string path, string subFolder, int depth)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (depth < 0) throw new ArgumentOutOfRangeException(nameof(depth));

            var items = path.Split('\\');
            bool firstReserved = false;
            string constructedPath = string.Empty;
            if (items.Length > 0 &&
                items[0].IndexOf(":") > 0)
            {
                firstReserved = true;
                items[0] = items[0] + '\\';
                constructedPath = items[0];
            }

            int totalFolders = items.Length;
            bool added = false;
            
            for (int i = 0; i <= totalFolders; i++)
            {
                if (!added &&
                    !(firstReserved && i == 0 && totalFolders > 1))
                {
                    int current = depth - totalFolders + i;
                    if (current >= 0)
                    {
                        constructedPath = Path.Combine(constructedPath, subFolder);
                        added = true;
                    }
                }

                if (i < totalFolders &&
                    !(firstReserved && i == 0))
                {
                    constructedPath = Path.Combine(constructedPath, items[i]);
                }
            }

            return constructedPath;
        }
    }
}
