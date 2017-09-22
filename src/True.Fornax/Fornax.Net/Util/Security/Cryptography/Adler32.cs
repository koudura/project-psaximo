/** MIT LICENSE
 *
 *   Copyright (c) 2017 Koudura Ninci @True.Inc
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
 ***/


using System;
using System.IO;

namespace Fornax.Net.Util.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public static class Adler32
    {
        internal const uint ADLER_MOD = 65521;

        public static ulong Compute(string bufferContext) {
            return Compute(bufferContext.ToCharArray());
        }

        public static ulong Compute(char[] bufferContext) {
            uint A = 1, B = 0;

            for (int i = 0; i < bufferContext.Length; ++i) {
                A = (A + bufferContext[i]) % ADLER_MOD;
                B = (B + A) % ADLER_MOD;
            }

            return (B << 16) | A;
        }

        public static ulong Compute(StringReader bufferContextreader) {
            string context = bufferContextreader.ReadToEnd();
            bufferContextreader.Close();
            return Compute(context);
        }


        public static ulong ComputeRange(string bufferContext, int startindex, int endindex) {
            if (!(startindex >= 0 && startindex < bufferContext.Length) || !((endindex >= 0 && endindex < bufferContext.Length)))
                throw new ArgumentOutOfRangeException(nameof(startindex));
            int bound = endindex - startindex;
            return Compute(bufferContext.Substring(startindex, bound));
        }
    }
}
