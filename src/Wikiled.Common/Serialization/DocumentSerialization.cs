using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using Wikiled.Common.Helpers;

namespace Wikiled.Common.Serialization
{
    public static class DocumentSerialization
    {
        private static int counter;

        public static void SaveSafe(this XDocument doc, string fileName)
        {
            var value = Interlocked.Increment(ref counter);
            string oldFileTmp = fileName + "." + value + ".old";
            string newFileTmp = fileName + "." + value + ".new";
            if (File.Exists(oldFileTmp))
            {
                File.Delete(oldFileTmp);
            }

            if (File.Exists(newFileTmp))
            {
                File.Delete(newFileTmp);
            }

            if (File.Exists(fileName))
            {
                File.Move(fileName, oldFileTmp);
            }

            try
            {
                using (var stream = new FileStream(newFileTmp, FileMode.CreateNew, FileAccess.Write))
                {
                    using (var memory = new MemoryStream())
                    {
                        doc.Save(memory);
                        memory.ZipStream(stream);
                    }
                }
                
                File.Move(newFileTmp, fileName);
            }
            catch (Exception)
            {
                if (File.Exists(oldFileTmp))
                {
                    File.Move(oldFileTmp, fileName);
                }

                throw;
            }

            if (File.Exists(oldFileTmp))
            {
                File.Delete(oldFileTmp);
            }
        }
    }
}
