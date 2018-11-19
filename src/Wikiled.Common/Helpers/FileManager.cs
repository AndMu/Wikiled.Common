using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wikiled.Common.Helpers
{
    public static class FileManager
    {
        public static Regex GenerateFileMask(string fileMask)
        {
            string pattern =
                '^' +
                Regex.Escape(fileMask.Replace(".", "__DOT__")
                                 .Replace("*", "__STAR__")
                                 .Replace("?", "__QM__"))
                    .Replace("__DOT__", "[.]")
                    .Replace("__STAR__", ".*")
                    .Replace("__QM__", ".")
                + '$';
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }

        public static IEnumerable<string> FindFilesByMask(string path, string mask)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (mask == null)
            {
                throw new ArgumentNullException(nameof(mask));
            }

            Regex fileMask = GenerateFileMask(mask);
            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                string name = Path.GetFileName(file);
                if (fileMask.IsMatch(name))
                {
                    yield return file;
                }
            }
        }

        public static IEnumerable<string> FindFiles(string path, params string[] extensionsList)
        {
            if (extensionsList == null)
            {
                throw new ArgumentNullException(nameof(extensionsList));
            }

            if (extensionsList.Length == 0 ||
                extensionsList.Any(item => string.IsNullOrEmpty(item) || item[0] != '.'))
            {
                throw new ArgumentOutOfRangeException(nameof(extensionsList));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(path));
            }

            if (extensionsList.Length == 0)
            {
                yield break;
            }

            HashSet<string> extensions = new HashSet<string>(extensionsList, StringComparer.OrdinalIgnoreCase);
            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                                             .Where(s => extensions.Contains(Path.GetExtension(s))))
            {
                yield return file;
            }
        }
    }
}
