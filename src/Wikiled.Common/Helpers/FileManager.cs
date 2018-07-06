using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Wikiled.Common.Helpers
{
    public static class FileManager
    {
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

            var extensions = new HashSet<string>(extensionsList, StringComparer.OrdinalIgnoreCase);
            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                                             .Where(s => extensions.Any(ext => ext == Path.GetExtension(s))))
            {
                yield return file;
            }
        }
    }
}
