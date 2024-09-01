using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

            if (File.Exists(file))
            {
                // delete empty file
                FileInfo info = new FileInfo(file);
                if (info.Length == 0)
                {
                    File.Delete(file);
                    return;
                }
            }
            else
            {
                return;
            }

            string location = Path.GetDirectoryName(file);
            string fileName = Path.GetFileNameWithoutExtension(file);
            IEnumerable<string> allFiles = FileManager.FindFilesByMask(location, $"{fileName}.*");
            int current = 0;
            foreach (string currentFile in allFiles)
            {
                string extension = Path.GetExtension(currentFile);
                if (extension?.Length > 0 &&
                    int.TryParse(
                        extension.Substring(1),
                        NumberStyles.Any,
                        CultureInfo.DefaultThreadCurrentUICulture,
                        out int number) &&
                    number > current)
                {
                    current = number;
                }
            }

            string newPath = Path.Combine(location, $"{fileName}.{current + 1}");
            File.Move(file, newPath);
        }
    }
}