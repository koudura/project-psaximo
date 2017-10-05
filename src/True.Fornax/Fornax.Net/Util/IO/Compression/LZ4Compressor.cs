/***
* Copyright (c) 2017 Koudura Ninci @True.Inc
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*
**/

using System;
using System.IO;
using System.Text;
using LZ4;


namespace Fornax.Net.Util.IO.Compression
{

    /// <summary>
    /// Utility class for compression using the LZ4 lossless compression algorithm.
    /// </summary>
    public static class LZ4Compressor
    {
        internal static bool IsSafeLoad { get; private set; }

        /// <summary>
        /// Compresses the <paramref name="inputFile"/> and writes to
        /// <paramref name="outputFile"/>.
        /// </summary>
        /// <param name="inputFile">The input file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="HighCompression">if set to <c>true</c> [high compression].</param>
        /// <exception cref="ArgumentNullException">inputFile</exception>
        /// <exception cref="InvalidDataException">Extension</exception>
        public static void CompressWriteFile(FileInfo inputFile, FileInfo outputFile, bool HighCompression = false) {
            if (inputFile == null || !inputFile.Exists) {
                throw new ArgumentNullException(nameof(inputFile));
            }

            //if (!inputFile.Extension.Equals(FileFormat.Txt.GetString())) 
            //throw new InvalidDataException($"Not a valid Text File : {nameof(inputFile.Extension)} is now .txt");

            LZ4StreamFlags lz4Mode;

            if (HighCompression) lz4Mode = LZ4StreamFlags.HighCompression;
            else lz4Mode = LZ4StreamFlags.Default;

            using (var filestream = new FileStream(outputFile.FullName, FileMode.Create)) {
                using (var lz4stream = new LZ4Stream(filestream, LZ4StreamMode.Compress, lz4Mode)) {
                    using (var writer = new StreamWriter(lz4stream)) {
                        writer.Write(ReadString(inputFile));
                    }
                }
            }
        }

        /// <summary>
        /// Compresses the <paramref name="inputFile"/> and writes
        /// to <paramref name="outputFile"/>.
        /// </summary>
        /// <param name="inputFile">The input file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="HighCompression">if set to <c>true</c> [high compression].</param>
        public static void CompressWriteFile(string inputFile, string outputFile, bool HighCompression = false) {
            CompressWriteFile(new FileInfo(inputFile), new FileInfo(outputFile), HighCompression);
        }

        /// <summary>
        /// Decompresses and reads text file.
        /// </summary>
        /// <param name="compressedInputFile">The compressed input file.</param>
        /// <returns></returns>
        public static string DecompressReadTxtFile(string compressedInputFile) {
            return DecompressReadTxtFile(new FileInfo(compressedInputFile));
        }

        /// <summary>
        /// Decompresses and reads text file.
        /// </summary>
        /// <param name="zipFile">The zip file.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">zipFile</exception>
        public static string DecompressReadTxtFile(FileInfo zipFile) {
            if (zipFile == null || !zipFile.Exists) {
                throw new ArgumentNullException(nameof(zipFile));
            }
            StringBuilder output = new StringBuilder();

            using (var filestream = new FileStream(zipFile.FullName, FileMode.Open)) {
                using (var lz4stream = new LZ4Stream(filestream, LZ4StreamMode.Decompress)) {
                    using (var reader = new StreamReader(lz4stream)) {
                        string line;
                        while ((line = reader.ReadLine()) != null) {
                            output.AppendLine(line);
                        }
                    }
                }
            }
            return output.ToString();
        }

        internal static string ReadString(FileInfo file) {
            try {
                return File.ReadAllText(file.FullName);
            } catch (IOException) { return null; }
        }

        static LZ4Compressor() {
            if (FornaxAssembly.TryResolveLZ4()) {
                IsSafeLoad = true;
            }
        }
    }
}
