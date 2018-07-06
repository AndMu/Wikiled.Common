using System;
using System.IO;

namespace Wikiled.Common.Helpers
{
    public class FileRelativeCopyItem
    {
        private readonly string targetDirectory;

        public FileRelativeCopyItem(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(source));
            }

            if (string.IsNullOrEmpty(target))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(target));
            }

            FileSource = source;
            FileTarget = target;
            targetDirectory = Path.GetDirectoryName(FileTarget);
            FileName = Path.GetFileName(FileSource);
        }

        public string Content { get; set; }

        public string FileName { get; }

        public string FileSource { get; }

        public string FileTarget { get; }

        public override string ToString()
        {
            return FileName;
        }

        public void Copy()
        {
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            if (string.IsNullOrEmpty(Content))
            {
                File.Copy(FileSource, FileTarget);
            }
            else
            {
                File.WriteAllText(FileTarget, Content);
            }
        }

        public void Read()
        {
            if (string.IsNullOrEmpty(Content))
            {
                Content = File.ReadAllText(FileSource);
            }
        }
    }
}
