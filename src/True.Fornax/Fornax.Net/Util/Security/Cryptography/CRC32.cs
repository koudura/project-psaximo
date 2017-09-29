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
using System.Text;

namespace Fornax.Net.Util.Security.Cryptography
{
    /// <summary>
    /// A hash class for generating hashcodes based on the 
    /// Cyclic redundancy check 32-bit.
    /// </summary>
    /// <seealso cref="ICheckSum" />
    public sealed class CRC32 : ICheckSum
    {

        internal const long DefaultSeed = 0xffffffffL;
        internal const uint DefaultPolynomial = 0xedb88320;

        private static uint polynomial;
        private static long seed;
        private  readonly uint[] crcTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="CRC32"/> class.
        /// </summary>
        public CRC32() : this(DefaultPolynomial,DefaultSeed) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CRC32"/> class.
        /// </summary>
        /// <param name="_polynomial">The polynomial.</param>
        /// <param name="_seed">The seed.</param>
        public CRC32(uint _polynomial, long _seed) {
            polynomial = _polynomial;
            seed = _seed;
            crcTable =  InitializeCRCTable();
        }

        private static UInt32[] InitializeCRCTable() {
            UInt32[] crcTable = new UInt32[256];
            for (UInt32 n = 0; n < 256; n++) {
                UInt32 c = n;
                for (int k = 8; --k >= 0;) {
                    if ((c & 1) != 0)
                        c = polynomial ^ (c >> 1);
                    else
                        c = c >> 1;
                }
                crcTable[n] = c;
            }
            return crcTable;
        }

        private UInt32 crc = 0;

        /// <summary>
        /// Gets the value of the crc hash after an Update has been made. 
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public long Value => crc & seed;

        /// <summary>
        /// Resets the crc32 table
        /// </summary>
        public void Reset() {
            crc = 0;
        }

        public void Update(int buffer) {
            UInt32 c = ~crc;
            c = crcTable[(c ^ buffer) & 0xff] ^ (c >> 8);
            crc = ~c;
        }

        /// <summary>
        /// Updates the <see cref="crcTable"/> for a <see cref="CRC32"/> instance
        /// with the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public void Update(byte[] buffer) {
            Update(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Updates the <see cref="crcTable"/> for a <see cref="CRC32"/> instance
        /// with the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public void Update(char[] buffer) {
           
        }

        /// <summary>
        /// Updates the <see cref="crcTable"/> for a <see cref="CRC32"/> instance
        /// with the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public void Update(string buffer) {
            Update(Encoding.Default.GetBytes(buffer));
        }

        /// <summary>
        /// Updates the <see cref="crcTable"/> for a <see cref="CRC32"/> instance
        /// with the specified buffer from a specifed point in the buffer (<paramref name="offset"/>) , to
        /// up until <paramref name="length"/>.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        public void Update(byte[] buffer, int offset, int length) {
            UInt32 c = ~crc;
            while (--length >= 0)
                c = crcTable[(c ^ buffer[offset++]) & 0xff] ^ (c >> 8);
            crc = ~c;
        }
    }
}
