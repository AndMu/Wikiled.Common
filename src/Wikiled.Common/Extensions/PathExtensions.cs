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

        public static string PathAddBackslash(this string path)
        {
            // They're always one character but EndsWith is shorter than
            // array style access to last path character. Change this
            // if performance are a (measured) issue.
            string separator1 = Path.DirectorySeparatorChar.ToString();
            string separator2 = Path.AltDirectorySeparatorChar.ToString();

            // Trailing white spaces are always ignored but folders may have
            // leading spaces. It's unusual but it may happen. If it's an issue
            // then just replace TrimEnd() with Trim(). Tnx Paul Groke to point this out.
            path = path.TrimEnd();

            // Argument is always a directory name then if there is one
            // of allowed separators then I have nothing to do.
            if (path.EndsWith(separator1) || path.EndsWith(separator2))
            {
                return path;
            }

            // If there is the "alt" separator then I add a trailing one.
            // Note that URI format (file://drive:\path\filename.ext) is
            // not supported in most .NET I/O functions then we don't support it
            // here too. If you have to then simply revert this check:
            // if (path.Contains(separator1))
            //     return path + separator1;
            //
            // return path + separator2;
            if (path.Contains(separator2))
            {
                return path + separator2;
            }

            // If there is not an "alt" separator I add a "normal" one.
            // It means path may be with normal one or it has not any separator
            // (for example if it's just a directory name). In this case I
            // default to normal as users expect.
            return path + separator1;
        }

        public static string InsertSubFolder(this string path, string subFolder, int depth)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (depth < 0) throw new ArgumentOutOfRangeException(nameof(depth));

            var items = path.Split(Path.DirectorySeparatorChar);
            bool firstReserved = false;
            string constructedPath = string.Empty;
            if (items.Length > 0 &&
                items[0].IndexOf(":") > 0)
            {
                firstReserved = true;
                items[0] = items[0] + Path.DirectorySeparatorChar;
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
