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


namespace Fornax.Net.Util.Text
{
    /// <summary>
    /// Simple utility class for manipulating <see cref="char"/>.
    /// </summary>
    [Progress("Unicode",false, Documented = true,Tested = false)]
    class Unicode
    {
        private const long UNI_MAX_BMP = 0x0000FFFF;

        private const long HALF_SHIFT = 10;
        private const long HALF_MASK = 0x3FFL;


        private const int LEAD_SURROGATE_SHIFT = 10;
        private const int TRAIL_SURROGATE_MASK = 0x3FF;
        private const int TRAIL_SURROGATE_MIN_VALUE = 0xDC00;
        private const int LEAD_SURROGATE_MIN_VALUE = 0xD800;
        private const int SUPPLEMENTARY_MIN_VALUE = 0x10000;

        private static readonly int LEAD_SURROGATE_OFFSET_ = LEAD_SURROGATE_MIN_VALUE - (SUPPLEMENTARY_MIN_VALUE >> LEAD_SURROGATE_SHIFT);
        
        /// <summary>
        /// Converts a specific set of Unicode code-points to their respective <see cref="char"/>[] 
        /// representation.
        /// </summary>
        /// <param name="codePoints">The set of Unicode code-points.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// count
        /// or
        /// cp
        /// </exception>
        public static char[] ToCharArray(int[] codePoints, int offset, int count) {

            if (count < 0) {
                throw new ArgumentException(nameof(count));
            }
            int countThreashold = 1024; 
            int arrayLength = count * 2;

            if (count > countThreashold) {
                arrayLength = 0;
                for (int r = offset, e = offset + count; r < e; ++r) {
                    arrayLength += codePoints[r] < 0x010000 ? 1 : 2;
                }
                if (arrayLength < 1) {
                    arrayLength = count * 2;
                }
            }
            /** 
             ** Initialize our array to our exact or oversized length.
             ** It is now safe to assume i  have enough space for all of the characters.
             **/
            char[] chars = new char[arrayLength];
            int w = 0;
            for (int r = offset, e = offset + count; r < e; ++r) {
                int cp = codePoints[r];
                if (cp < 0 || cp > 0x10ffff) {
                    throw new ArgumentException(nameof(cp));
                }
                if (cp < 0x010000) {
                    chars[w++] = (char)cp;
                } else {
                    chars[w++] = (char)(LEAD_SURROGATE_OFFSET_ + (cp >> LEAD_SURROGATE_SHIFT));
                    chars[w++] = (char)(TRAIL_SURROGATE_MIN_VALUE + (cp & TRAIL_SURROGATE_MASK));
                }
            }

            var result = new char[w];
            Array.Copy(chars, result, w);
            return result;
        }
    }
}
