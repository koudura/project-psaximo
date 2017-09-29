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

using Number = Fornax.Net.Util.Numerics.Number;

namespace Fornax.Net.Util.Security.Cryptography
{
    /// <summary>
    /// The Murmur Hash 3
    /// </summary>
    public static class Murmur3_x86
    {
        /// <summary>
        /// Computes the specified data and Gets the MurmurHash3 x86 for <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>returns the murmurhash of <paramref name="data"/>.</returns>
        /// Original source/tests at <a href="https://github.com/yonik/java_util/"> https://github.com/yonik/java_util/</a>."/>
        public static int Compute(byte[] data, int offset , int length , int seed) {
            const int c1 = unchecked((int)0xcc9e2d51);
            const int c2 = 0x1b873593;

            int h1 = seed;
            int roundedEnd = offset + (length & unchecked((int)0xfffffffc)); // round down to 4 byte block

            for (int i = offset; i < roundedEnd; i += 4) {
                // little endian load order
                int k1 = (((sbyte)data[i]) & 0xff) | ((((sbyte)data[i + 1]) & 0xff) << 8) | ((((sbyte)data[i + 2]) & 0xff) << 16) | (((sbyte)data[i + 3]) << 24);
                k1 *= c1;
                k1 = Number.RotateLeft(k1, 15);
                k1 *= c2;

                h1 ^= k1;
                h1 = Number.RotateLeft(h1, 13);
                h1 = h1 * 5 + unchecked((int)0xe6546b64);
            }

            // tail
            int k2 = 0;

            switch (length & 0x03) {
                case 3:
                    k2 = (((sbyte)data[roundedEnd + 2]) & 0xff) << 16;
                    // fallthrough
                    goto case 2;
                case 2:
                    k2 |= (((sbyte)data[roundedEnd + 1]) & 0xff) << 8;
                    // fallthrough
                    goto case 1;
                case 1:
                    k2 |= (((sbyte)data[roundedEnd]) & 0xff);
                    k2 *= c1;
                    k2 = Number.RotateLeft(k2, 15);
                    k2 *= c2;
                    h1 ^= k2;
                    break;
            }

            // finalization
            h1 ^= length;

            // fmix(h1);
            h1 ^= (int)((uint)h1 >> 16);
            h1 *= unchecked((int)0x85ebca6b);
            h1 ^= (int)((uint)h1 >> 13);
            h1 *= unchecked((int)0xc2b2ae35);
            h1 ^= (int)((uint)h1 >> 16);

            return h1;
        }


    }
}
