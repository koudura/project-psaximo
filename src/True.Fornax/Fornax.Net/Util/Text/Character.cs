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
**/

using System;
using System.Collections.Generic;
using System.Text;


namespace Fornax.Net.Util.Text
{
    public static class Character
    {
        private const char Null = '\0';
        private const char Zero = '0';
        private const char A = 'a';

        public const int MaxRadix = 36;
        public const int MinRadix = 2;

        public const int MaxCodePoint = 0x10FFFF;
        public const int MinCodePoint = 0x000000;

        public const char MaxSurrogate = '\uDFFF';
        public const char MinSurrogate = '\uD800';

        public const char MinLowSurrogate = '\uDC00';
        public const char MaxLowSurrogate = '\uDFFF';

        public const char MinHighSurrogate = '\uD800';
        public const char MaxHighSurrogate = '\uDBFF';

        public const int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;

        private static readonly string digitKeys = "0Aa\u0660\u06f0\u0966\u09e6\u0a66\u0ae6\u0b66\u0be7\u0c66\u0ce6\u0d66\u0e50\u0ed0\u0f20\u1040\u1369\u17e0\u1810\uff10\uff21\uff41";

        private static readonly char[] digitValues = "90Z7zW\u0669\u0660\u06f9\u06f0\u096f\u0966\u09ef\u09e6\u0a6f\u0a66\u0aef\u0ae6\u0b6f\u0b66\u0bef\u0be6\u0c6f\u0c66\u0cef\u0ce6\u0d6f\u0d66\u0e59\u0e50\u0ed9\u0ed0\u0f29\u0f20\u1049\u1040\u1371\u1368\u17e9\u17e0\u1819\u1810\uff19\uff10\uff3a\uff17\uff5a\uff37"
            .ToCharArray();

        /// <summary>
        /// Search the sorted characters in the string and return the nearest index.
        /// </summary>
        /// <param name="data">The String to search.</param>
        /// <param name="c">The character to search for.</param>
        /// <returns>The nearest index.</returns>
        private static int BinarySearchRange(string data, char c) {
            char value = (char)0;
            int low = 0, mid = -1, high = data.Length - 1;
            while (low <= high) {
                mid = (low + high) >> 1;
                value = data[mid];
                if (c > value)
                    low = mid + 1;
                else if (c == value)
                    return mid;
                else
                    high = mid - 1;
            }
            return mid - (c < value ? 1 : 0);
        }

        /// <summary>
        /// Gets the Char For specified Digit.
        /// </summary>
        /// <param name="digit">The digit.</param>
        /// <param name="radix">The radix.</param>
        /// <returns></returns>
        public static char ForDigit(int digit, int radix) {
            /**
             * Check Range of digit and radix.
             */
            if (radix < MinRadix)
                return Null;
            if (radix > MaxRadix)
                return Null;
            if (digit < 0)
                return Null;
            if (digit >= radix)
                return Null;

            if (digit < 10)
                return (char)(Zero + digit);

            return (char)(A + digit - 10);
        }

        /// <summary>
        /// Convenience method to determine the value of the specified character
        /// <paramref name="c"/> in the supplied radix. The value of <paramref name="radix"/> must be
        /// between <see cref="MinRadix"/> and <see cref="MaxRadix"/>.
        /// </summary>
        /// <param name="c">The character to determine the value of.</param>
        /// <param name="radix">The radix.</param>
        /// <returns>
        /// The value of <paramref name="c"/> in <paramref name="radix"/> if <paramref name="radix"/> lies
        /// between <see cref="MinRadix"/> and <see cref="MaxRadix"/>; -1 otherwise.
        /// </returns>
        public static int Digit(char c, int radix) {
            int result = -1;
            if (radix >= MinRadix && radix <= MaxRadix) {
                if (c < 128) {
                    /**
                     * Optimized for ASCII
                     **/
                    if ('0' <= c && c <= '9') {
                        result = c - '0';
                    } else if ('a' <= c && c <= 'z') {
                        result = c - ('a' - 10);
                    } else if ('A' <= c && c <= 'Z') {
                        result = c - ('A' - 10);
                    }
                    return result < radix ? result : -1;
                }
                result = BinarySearchRange(digitKeys, c);
                if (result >= 0 && c <= digitValues[result * 2]) {
                    int value = (char)(c - digitValues[result * 2 + 1]);
                    if (value >= radix) {
                        return -1;
                    }
                    return value;
                }
            }
            return -1;
        }


        public static int CharCount(int codepoint) {
            return codepoint >= MIN_SUPPLEMENTARY_CODE_POINT ? 2 : 1;
        }

        public static int ToChars(int codePoint, char[] dst, int dstIndex) {
            var converted = Unicode.ToCharArray(new[] { codePoint }, 0, 1);

            Array.Copy(converted, 0, dst, dstIndex, converted.Length);
            return converted.Length;
        }

        public static char[] ToChars(int codePoint) {
            return Unicode.ToCharArray(new[] { codePoint }, 0, 1);
        }




    }
}
