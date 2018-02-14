using System.IO;
using Wikiled.Common.Arguments;

namespace Wikiled.Common.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static bool EnsureDirectoryExistence(this DirectoryInfo directory)
        {
            Guard.NotNull(() => directory, directory);
            if (!directory.Exists)
            {
                directory.Create();
                return true;
            }

            return false;
        }

        public static void EnsureDirectoryExistence(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            new DirectoryInfo(path).EnsureDirectoryExistence();
        }

        // Copies all files from one directory to another.
        public static void CopyTo(this DirectoryInfo source, string destDirectory, bool recursive)
        {
            Guard.NotNull(() => source, source);
            Guard.NotNullOrEmpty(() => destDirectory, destDirectory);
            
            // Compile the target.
            DirectoryInfo target = new DirectoryInfo(destDirectory);
            // If the source doesn’t exist, we have to throw an exception.
            if (!source.Exists)
            {
                throw new DirectoryNotFoundException("Source directory not found: " + source.FullName);
            }

            // If the target doesn’t exist, we create it.
            if (!target.Exists)
            {
                target.Create();
            }

            // Get all files and copy them over.
            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }

            // Return if no recursive call is required.
            if (!recursive)
            {
                return;
            }

            // Do the same for all sub directories.
            foreach (DirectoryInfo directory in source.GetDirectories())
            {
                CopyTo(directory, Path.Combine(target.FullName, directory.Name), true);
            }
        }
    }
}
