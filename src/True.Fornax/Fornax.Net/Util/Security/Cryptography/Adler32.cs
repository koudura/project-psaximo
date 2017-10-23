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

namespace Fornax.Net.Util.Security.Cryptography
{
    /// <summary>
    /// Adler-32 Hash Algorithm handler. 
    /// </summary>
    public static class Adler32
    {
        internal const uint ADLER_MOD = 65521;

        /// <summary>
        /// Computes the Adler-32 specified buffer context.
        /// </summary>
        /// <param name="bufferContext">The buffer context as string.</param>
        /// <returns></returns>
        public static ulong Compute(string bufferContext) {
            return Compute(bufferContext.ToCharArray());
        }

        /// <summary>
        /// Computes the Adler-32 hash code for the specified buffer context.
        /// </summary>
        /// <param name="bufferContext">The buffer context as array of characters.</param>
        /// <returns></returns>
        public static ulong Compute(char[] bufferContext) {
            uint A = 1, B = 0;

            for (int i = 0; i < bufferContext.Length; ++i) {
                A = (A + bufferContext[i]) % ADLER_MOD;
                B = (B + A) % ADLER_MOD;
            }

            return (B << 16) | A;
        }

        /// <summary>
        /// Computes the Adler-32 hash code for the specified context reader's
        /// text.
        /// </summary>
        /// <param name="bufferContextreader">The buffer contextreader.</param>
        /// <returns></returns>
        public static ulong Compute(StringReader bufferContextreader) {
            string context = bufferContextreader.ReadToEnd();
            bufferContextreader.Close();
            return Compute(context);
        }

        /// <summary>
        /// Computes the Adler-32 hash code for the specified range in string
        /// <paramref name="bufferContext"/>.
        /// </summary>
        /// <param name="bufferContext">The buffer context as string.</param>
        /// <param name="startindex">The startindex.</param>
        /// <param name="endindex">The endindex.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">startindex</exception>
        public static ulong ComputeRange(string bufferContext, int startindex, int endindex) {
            if (!(startindex >= 0 && startindex < bufferContext.Length) || !((endindex >= 0 && endindex < bufferContext.Length)))
                throw new ArgumentOutOfRangeException(nameof(startindex));
            int bound = endindex - startindex;
            return Compute(bufferContext.Substring(startindex, bound));
        }
    }
}
