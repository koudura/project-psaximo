/** MIT LICENSE
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
**/

using System;
using System.Collections.Generic;
using System.Text;


namespace Fornax.Net.Util.Text
{
    class Unicode
    {
       // public static readonly BytesRef BIG_TERM = new BytesRef(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }); // TODO this is unrelated here find a better place for it

        public const int UNI_SUR_HIGH_START = 0xD800;
        public const int UNI_SUR_HIGH_END = 0xDBFF;
        public const int UNI_SUR_LOW_START = 0xDC00;
        public const int UNI_SUR_LOW_END = 0xDFFF;
        public const int UNI_REPLACEMENT_CHAR = 0xFFFD;

        private const long UNI_MAX_BMP = 0x0000FFFF;

        private const long HALF_SHIFT = 10;
        private const long HALF_MASK = 0x3FFL;

        private const int SURROGATE_OFFSET = Character.MIN_SUPPLEMENTARY_CODE_POINT - (UNI_SUR_HIGH_START << (int)HALF_SHIFT) - UNI_SUR_LOW_START;


        internal static char[] ToCharArray(int[] v1, int v2, int v3) {
            
            //if (count < 0) {
            //    throw new System.ArgumentException();
            //}
            //int countThreashold = 1024; // If the number of chars exceeds this, we count them instead of allocating count * 2


            //if (count > countThreashold) {
            //    arrayLength = 0;
            //    for (int r = offset, e = offset + count; r < e; ++r) {
            //        arrayLength += codePoints[r] < 0x010000 ? 1 : 2;
            //    }
            //    if (arrayLength < 1) {
            //        arrayLength = count * 2;
            //    }
            //}
            //// Initialize our array to our exact or oversized length.
            //// It is now safe to assume we have enough space for all of the characters.

            //char[] chars = new char[arrayLength];
            //int w = 0;
            //for (int r = offset, e = offset + count; r < e; ++r) {
            //    int cp = codePoints[r];
            //    if (cp < 0 || cp > 0x10ffff) {
            //        throw new System.ArgumentException();
            //    }
            //    if (cp < 0x010000) {
            //        chars[w++] = (char)cp;
            //    } else {
            //        chars[w++] = (char)(LEAD_SURROGATE_OFFSET_ + (cp >> LEAD_SURROGATE_SHIFT_));
            //        chars[w++] = (char)(TRAIL_SURROGATE_MIN_VALUE + (cp & TRAIL_SURROGATE_MASK_));
            //    }
            //}

            // var result = new char[w];
            //   Array.Copy(chars, result, w);
            return null;
        }
    }
}
