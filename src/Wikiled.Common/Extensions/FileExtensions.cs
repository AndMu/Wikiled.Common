using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Wikiled.Common.Helpers;

namespace Wikiled.Common.Extensions
{
    public static class FileExtensions
    {
        public static void Backup(this string file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var location = Path.GetDirectoryName(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var allFiles = FileManager.FindFilesByMask(location, $"{fileName}.*");
            int current = 0;
            foreach (var currentFile in allFiles)
            {
                var extension = Path.GetExtension(currentFile);
                if (extension?.Length > 0 &&
                    int.TryParse(extension.Substring(1),
                                 NumberStyles.Any,
                                 CultureInfo.DefaultThreadCurrentUICulture,
                                 out var number) &&
                    number > current)
                {
                    current = number;
                }
            }

            if (File.Exists(file))
            {
                var newPath = Path.Combine(location, $"{fileName}.{current + 1}");
                File.Move(file, newPath);
            }
        }
    }
}
