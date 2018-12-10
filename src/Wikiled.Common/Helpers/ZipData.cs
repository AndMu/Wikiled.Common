using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Wikiled.Common.Extensions;

namespace Wikiled.Common.Helpers
{
    public static class ZipData
    {
        public static byte[] ZipAsTextFile(this string textToZip, string fileName = "zipped.txt")
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var demoFile = zipArchive.CreateEntry(fileName);

                    using (var entryStream = demoFile.Open())
                    {
                        using (var streamWriter = new StreamWriter(entryStream))
                        {
                            streamWriter.Write(textToZip);
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Unzip a zipped byte array into a string.
        /// </summary>
        /// <param name="zippedBuffer">The byte array to be unzipped</param>
        /// <returns>string representing the original stream</returns>
        public static string UnZipTextFile(this byte[] zippedBuffer)
        {
            using (var zippedStream = new MemoryStream(zippedBuffer))
            {
                using (var archive = new ZipArchive(zippedStream))
                {
                    var entry = archive.Entries.FirstOrDefault();

                    if (entry != null)
                    {
                        using (var unzippedEntryStream = entry.Open())
                        {
                            using (var ms = new MemoryStream())
                            {
                                unzippedEntryStream.CopyTo(ms);
                                var unzippedArray = ms.ToArray();

                                return Encoding.Default.GetString(unzippedArray);
                            }
                        }
                    }

                    return null;
                }
            }
        }

        public static byte[] Zip(this byte[] orginalData)
        {
            using (var memory = new MemoryStream())
            {
                using (var zipStream = new GZipStream(memory, CompressionMode.Compress))
                {
                    zipStream.Write(orginalData, 0, orginalData.Length);
                    zipStream.Flush();
                    zipStream.Close();
                    return memory.ToArray();
                }
            }
        }

        public static void ZipStream(this Stream data, Stream targetStream)
        {
            using (var zipStream = new GZipStream(targetStream, CompressionMode.Compress))
            {
                data.Position = 0;
                data.CopyTo(zipStream);
                zipStream.Flush();
            }
        }

        public static bool IsGZipHeader(this Stream stream)
        {
            var bytes = new byte[4];
            stream.Read(bytes, 0, 4);
            stream.Position = 0;
            return IsGZipHeader(bytes);
        }

        /// <summary>
        /// Checks the first two bytes in a GZIP file, which must be 31 and 139.
        /// </summary>
        public static bool IsGZipHeader(this byte[] arr)
        {
            return arr.Length >= 4 &&
                   arr[0] == 0x1F &&
                   arr[1] == 0x8b &&
                   arr[2] == 0x08;
            ;
        }

        public static byte[] Zip(this string text)
        {
            return text.GetBytes().Zip();
        }

        public static byte[] Zip(this string text, Encoding encoding)
        {
            return encoding.GetBytes(text).Zip();
        }

        public static string UnZipString(this byte[] data)
        {
            return data.UnZip().GetString();
        }

        public static string UnZipString(this byte[] data, Encoding encoding)
        {
            return encoding.GetString(data.UnZip());
        }

        public static byte[] UnZip(this byte[] data)
        {
            if (data == null ||
                data.Length == 0)
            {
                return new byte[] {};
            }

            using (var memory = new MemoryStream(data))
            {
                return memory.UnZipStream();
            }
        }

        public static string UnZipStream(this Stream memory, Encoding encoding)
        {
            var data = UnZipStream(memory);
            var preamble = Encoding.UTF8.GetPreamble();
            if (preamble.Length > 0)
            {
                var header = new byte[preamble.Length];
                Array.Copy(data, header, header.Length);
                if (header.SequenceEqual(preamble))
                {
                    var copy = data;
                    data = new byte[data.Length - preamble.Length];
                    Array.Copy(copy, preamble.Length, data, 0, data.Length);
                }
            }

            return encoding.GetString(data);
        }

        public static byte[] UnZipStream(this Stream memory)
        {
            var allData = new List<byte>((int)memory.Length);
            const int chunk = 1024;
            byte[] buffer = new byte[chunk];

            using (var zipStream = new GZipStream(memory, CompressionMode.Decompress))
            {
                int length;
                while ((length = zipStream.Read(buffer, 0, chunk)) != 0)
                {
                    var addData = length != chunk
                        ? buffer.Take(length)
                        : buffer;

                    allData.AddRange(addData);
                }

                zipStream.Flush();
                zipStream.Close();
            }

            return allData.ToArray();
        }
    }
}
