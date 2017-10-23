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
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

using Fornax.Net.Util.IO.Writers;
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
        /// Compresses the <paramref name="inputFile" /> and writes to
        /// <paramref name="outputFile" />.
        /// </summary>
        /// <param name="inputFile">The input file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="HighCompression">if set to <c>true</c> [high compression].</param>
        /// <exception cref="ArgumentNullException">inputFile</exception>
        /// <exception cref="InvalidDataException">Extension</exception>
        public static void Compress(FileInfo inputFile, FileInfo outputFile, bool HighCompression = false) {
            Contract.Requires(inputFile != null && outputFile != null);
            if (inputFile == null || !inputFile.Exists) {
                throw new ArgumentNullException(nameof(inputFile));
            }
            LZ4StreamFlags lz4Mode;

            if (HighCompression) lz4Mode = LZ4StreamFlags.HighCompression;
            else lz4Mode = LZ4StreamFlags.Default;

                using (var filestream = new FileStream(outputFile.FullName, FileMode.Create)) {
                    using (var lz4stream = new LZ4Stream(filestream, LZ4StreamMode.Compress, lz4Mode,10485760)) {
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
        public static void Compress(string inputFile, string outputFile, bool HighCompression = false) {
            Compress(new FileInfo(inputFile), new FileInfo(outputFile), HighCompression);
        }

        /// <summary>
        /// Decompresses and reads text file.
        /// </summary>
        /// <param name="compressedInputFile">The compressed input file.</param>
        /// <returns>string content of the decompressed file.</returns>
        public static string Decompress(string compressedInputFile) {
            return Decompress(new FileInfo(compressedInputFile));
        }

        /// <summary>
        /// Decompresses and reads text file.
        /// </summary>
        /// <param name="zipFile">The zip file.</param>
        /// <returns>string content of the decompressed file.</returns>
        /// <exception cref="ArgumentNullException">zipFile</exception>
        public static string Decompress(FileInfo zipFile) {
            Contract.Requires(zipFile != null);
            if (zipFile == null || !zipFile.Exists) {
                throw new ArgumentNullException(nameof(zipFile));
            }
            lock (zipFile) {
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
        }

        /// <summary>
        /// Decompresses the specified object file.
        /// </summary>
        /// <typeparam name="TObj">The type of the object.</typeparam>
        /// <param name="file">The file.</param>
        /// <returns>the instance of the object representation in file.</returns>
        /// <exception cref="ArgumentNullException">file</exception>
        public static TObj Decompress<TObj>(FileInfo file) where TObj : new() {
            Contract.Requires(file != null);
            if (file == null || !file.Exists) throw new ArgumentNullException(nameof(file));
            
            lock (file) {
                using (var filestream = new FileStream(file.FullName, FileMode.Open)) {
                    using (var lz4stream = new LZ4Stream(filestream, LZ4StreamMode.Decompress, LZ4StreamFlags.IsolateInnerStream)) {
                        return FornaxWriter.Read<TObj>(lz4stream);
                    }
                }
            }
        }

        internal static string ReadString(FileInfo file) {
            Contract.Requires(file != null);
            try {
                return File.ReadAllText(file.FullName);
            } catch (IOException) { return null; }
        }

        internal static byte[] ReadByte(FileInfo file) {
            Contract.Requires(file != null);
            try {
                return File.ReadAllBytes(file.FullName);
            } catch (IOException) {
                return null;
            }
        }

        static LZ4Compressor() {
            if (FornaxAssembly.TryResolveLZ4()) {
                IsSafeLoad = true;
            }
        }
    }
}