using System.Collections.Generic;
using System.IO;

namespace Wikiled.Common.Helpers
{
    public static class DirectoryRelativeCopyItem
    {
        public static IEnumerable<FileRelativeCopyItem> Copy(string source, string target)
        {
            source = new DirectoryInfo(source).FullName;
            target = new DirectoryInfo(target).FullName;
            foreach (var child in Directory.GetDirectories(source))
            {
                string directory = Path.GetFileName(child);
                foreach (var item in Copy(child, Path.Combine(target, directory)))
                {
                    yield return item;
                }
            }

            foreach (var file in Directory.GetFiles(source))
            {
                string fileName = Path.GetFileName(file);
                yield return new FileRelativeCopyItem(Path.Combine(source, fileName), Path.Combine(target, fileName));
            }
        }
    }
}
