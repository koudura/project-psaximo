using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fornax.Net.Util.IO.Readers
{
    public static class ZipHandler
    {

        public static void GZCompress(DirectoryInfo directory)
        {
            foreach (var _file in directory.GetFiles())
            {
                using (var ofstream = _file.OpenRead())
                {
                    if ((File.GetAttributes(_file.FullName) & FileAttributes.Hidden) !=
                            FileAttributes.Hidden & _file.Extension != ".gz")
                    {
                        using (var cmpStream = File.Create(_file.FullName + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(cmpStream, CompressionMode.Compress))
                            {
                                ofstream.CopyTo(compressionStream);
                            }
                        }
                    }
                }
            }
        }

        public static void GZDecompress(FileInfo gzFile)
        {
            using (var ofStream = gzFile.OpenRead())
            {
                string newFileName = Path.GetFileNameWithoutExtension(gzFile.FullName);
                using (var dcmStream = File.Create(newFileName))
                {
                    using (GZipStream decStream = new GZipStream(dcmStream, CompressionMode.Decompress))
                    {
                        decStream.CopyTo(dcmStream);
                    }
                }
            }
        }


        public static void Deflate() { }

        public static void Inflate() { }

        public static void Extract(string zipPath, string extractPath, params string[] extension)
        {

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    foreach (var ext in extension)
                    {
                        if (entry.FullName.EndsWith(value: ext, comparisonType: StringComparison.OrdinalIgnoreCase))
                        {
                            entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
                        }
                    }

                }
            }
        }



    }
}