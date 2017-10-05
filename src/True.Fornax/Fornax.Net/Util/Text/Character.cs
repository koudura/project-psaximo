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
using System.Collections.Generic;

using Num = Fornax.Net.Util.Numerics.Number;

namespace Fornax.Net.Util.Text
{
    /// <summary>
    /// The <see cref="Character"/> class wraps a value of a primitive type <see cref="(char)"/> in an
    /// object. <para></para>
    /// This class also provides methods for determining a characters category and Manipulating characters.
    /// </summary>
    [Progress("Character",false,Documented = false,Tested = false)]
    public class Character
    {
        #region Character Fields & Consts
        private const char Null = '\0';
        private const char Zero = '0';
        private const char A = 'a';

        /// <summary>
        /// The maximum radix available for conversion to and from strings.
        /// </summary>
        public const int MaxRadix = 36;
        /// <summary>
        /// The minimum radix available for conversion to and from strings.
        /// </summary>
        public const int MinRadix = 2;
        /// <summary>
        /// The constant value of this field is the smallest value of type <see cref="(char)"/>,
        /// <code>'\u005Cu0000'</code>.
        /// </summary>
        public const char MinValue = '\u0000';
        /// <summary>
        /// The constant value of this field is the largest value of type <see cref="(char)"/>,
        /// <code>'\u005CuFFFF'</code>.
        /// </summary>
        public const char MaxValue = '\uFFFF';

        /// <summary>
        /// The error flag
        /// </summary>
        internal const uint ERROR = 0xFFFFFFFFu;

        /// <summary>
        /// The maximum value of a Unicode code point.
        /// constant = (<code>U+10FFFF</code>).
        /// </summary>
        public const int MaxCodePoint = 0x10FFFF;
        /// <summary>
        /// The minimum value of a Unicode code point.
        /// constant = (<code>U+0000</code>).
        /// </summary>
        public const int MinCodePoint = 0x000000;

        /// <summary>
        /// The minimum value of a Unicode low-surrogate code unit.
        /// in the UTF-16 encoding, constant = <code>('u\u005CuDC00')</code>.
        /// <para></para>Low-Surrogate is also known as <c>Trailing-Surrogate</c>.
        /// </summary>
        public const char MinLowSurrogate = '\uDC00';
        /// <summary>
        /// The maximum value of a Unicode low-surrogate code unit.
        /// in the UTF-16 encoding, constant = <code>('u\u005CuDFFF')</code>.
        /// <para></para>Low-Surrogate is also known as <c>Trailing-Surrogate</c>.
        /// </summary>
        public const char MaxLowSurrogate = '\uDFFF';

        /// <summary>
        /// The minimum value of a Unicode high-surrogate code unit.
        /// in the UTF-16 encoding, constant = <code>('u\u005CuD800')</code>.
        /// <para></para>High-Surrogate is also known as <c>Leading-Surrogate</c>.
        /// </summary>
        public const char MinHighSurrogate = '\uD800';
        /// <summary>
        /// The maximum value of a Unicode high-surrogate code unit.
        /// in the UTF-16 encoding, constant = <code>('u\u005CuD8FF')</code>.
        /// <para></para>High Surrogate is also known as <c>Leading-Surrogate</c>.
        /// </summary>
        public const char MaxHighSurrogate = '\uDBFF';

        /// <summary>
        /// The maximum value of a Unicode low-surrogate code unit.
        /// in the UTF-16 encoding, constant = <code>('u\u005CuDFFF')</code>.
        /// <para></para>Low-Surrogate is also known as <c>Trailing-Surrogate</c>.
        /// </summary>
        public const char MaxSurrogate = MaxLowSurrogate;
        /// <summary>
        /// The minimum value of a Unicode high-surrogate code unit.
        /// in the UTF-16 encoding, constant = <code>('u\u005CuD800')</code>.
        /// <para></para>High-Surrogate is also known as <c>Leading-Surrogate</c>.
        /// </summary>
        public const char MinSurrogate = MinHighSurrogate;

        /// <summary>
        /// The minimum value of a Unicode supplementary code point.
        /// constant = (<code>U+10000</code>).
        /// </summary>
        public const int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
        /// <summary>
        /// The digit keys
        /// </summary>
        private static readonly string digitKeys = "0Aa\u0660\u06f0\u0966\u09e6\u0a66\u0ae6\u0b66\u0be7\u0c66\u0ce6\u0d66\u0e50\u0ed0\u0f20\u1040\u1369\u17e0\u1810\uff10\uff21\uff41";
        /// <summary>
        /// The digit values
        /// </summary>
        private static readonly char[] digitValues = "90Z7zW\u0669\u0660\u06f9\u06f0\u096f\u0966\u09ef\u09e6\u0a6f\u0a66\u0aef\u0ae6\u0b6f\u0b66\u0bef\u0be6\u0c6f\u0c66\u0cef\u0ce6\u0d6f\u0d66\u0e59\u0e50\u0ed9\u0ed0\u0f29\u0f20\u1049\u1040\u1371\u1368\u17e9\u17e0\u1819\u1810\uff19\uff10\uff3a\uff17\uff5a\uff37"
            .ToCharArray();
        #endregion


        #region non statics
        private char value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Character"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Character(char value) {
            this.value = value;
        }

        public char CharValue() => this.value;
        #endregion


        private static class CharacterCache
        {
            internal static readonly Character[] cache = new Character[127 + 1];
            static CharacterCache() {
                for (int i = 0; i < cache.Length; i++) {
                    cache[i] = new Character((char)i);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="Character"/> instance representing the specified <see cref="char"/> <paramref name="c"/>.
        /// <para>
        /// This method should generally be used in preference to the constructor <see cref="Character(char)"/>,<b></b>
        /// as this method yields better space and time performance by caching frequently requested values.
        /// </para>
        /// </summary>
        /// <param name="c">The char value c.</param>
        /// <returns>A <see cref="Character"/> object of <paramref name="c"/>.</returns>
        public static Character ValueOf(char c) {
            if (c <= 127) {
                return CharacterCache.cache[c];
            }
            return new Character(c);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents a specified character.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents <paramref name="c"/>.
        /// </returns>
        public static string ToString(char c) {
            return char.ToString(c);
        }

        /// <summary>
        /// Determines whether the specified code point is a valid Unicode code point value.
        /// </summary>
        /// <param name="codePoint"> The Unicode code point to be tested.</param>
        /// <returns>if the specified code point value 
        /// is between <see cref="MinCodePoint"/> and <see cref="MaxCodePoint"/> inclusive, otherwise, false.</returns>
        public static bool IsValidCodePoint(int codePoint) {
            int plane = Num.URShift(codePoint, 16);
            return plane < (Num.URShift(MaxCodePoint + 1, 16));
        }

        /// <summary>
        /// Detern=mines whether the specified character is in the <c>Basic MultiLingual Plane.</c>
        /// </summary>
        /// <param name="codepoint"> the character (unicode code point) to be tested.</param>
        /// <returns>if the specified code point value 
        /// is between <see cref="MinCodePoint"/> and <see cref="MaxCodePoint"/> inclusive, otherwise, false.</returns>
        public static bool IsBmpCodePoint(int codepoint) => Num.URShift(codepoint, 16) == 0;

        /// <summary>
        /// Determines whether the specified character (Unicode code point) is in the supplementary character range.
        /// </summary>
        /// <param name="codepoint"> the character (Unicode code point) to be tested.</param>
        /// <returns>if character is in supplementary range; false otherwise.</returns>
        public static bool IsSupplementaryCodePoint(int codepoint) {
            return (codepoint >= MIN_SUPPLEMENTARY_CODE_POINT && codepoint < (MaxCodePoint + 1));
        }

        /// <summary>
        /// determines the number of <code>char</code> values needed to represent the specified character (Unicode code point).
        /// </summary>
        /// <param name="codepoint">the Unicode code point to be tested.</param>
        /// <returns>2 if the character is a valid supplementary character; 1 otherwise.</returns>
        public static int CharCount(int codepoint) => codepoint >= MIN_SUPPLEMENTARY_CODE_POINT ? 2 : 1;

        /// <summary>
        /// Coverts the specified surrogate pair to its supplementary code point value.
        /// <para>Caller must validate if is Surrogate pair by <see cref="char.IsSurrogatePair(char, char)"/>.</para>
        /// </summary>
        /// <param name="high">the high surrogate code unit. </param>
        /// <param name="low">the low surrogate code unit. </param>
        /// <returns>the supplemenatary code point composed from the specified pair.</returns>
        public static int ToCodePoint(char high, char low) {
            return ((high << 10) + low) + (MIN_SUPPLEMENTARY_CODE_POINT - (MinHighSurrogate << 10) - MinLowSurrogate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int CodePointAt(char[] a, int index) {
            return CodePointAt(a, index, a.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="index"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal static int CodePointAt(char[] a, int index, int limit) {
            if ((index >= limit) || (limit < 0) || (limit > a.Length)) throw new IndexOutOfRangeException(nameof(index) +"and/or" + nameof(limit));
            return CodePointAtImpl(a, index, limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="index"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal static int CodePointAtImpl(char[] a, int index, int limit) {
            char c1 = a[index];
            if (char.IsHighSurrogate(c1) && ++index < limit) {
                char c2 = a[index];
                if (char.IsLowSurrogate(c2)) return ToCodePoint(c1, c2);
            }
            return c1;
        }

        /// <summary>
        /// Codes the point before.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static int CodePointBefore(char[] a, int index) {
            return CodePointBeforeImpl(a, index, 0);
        }

        /// <summary>
        /// Codes the point before.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="index">The index.</param>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">index</exception>
        public static int CodePointBefore(char[] a, int index, int start) {
            if(index <= start || start< 0 || start >= a.Length) {
                throw new IndexOutOfRangeException(nameof(index));
            }
            return CodePointBeforeImpl(a, index, start);
        }

        static int CodePointBeforeImpl(char[] a, int index, int start) {
            char c2 = a[--index];
            if(Char.IsLowSurrogate(c2) && index > start) {
                char c1 = a[--index];
                if (Char.IsHighSurrogate(c1)) {
                    return ToCodePoint(c1, c2);
                }
            }
            return c2;
        }


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


        /// <summary>
        /// To the chars.
        /// </summary>
        /// <param name="codePoint">The code point.</param>
        /// <param name="dst">The DST.</param>
        /// <param name="dstIndex">Index of the DST.</param>
        /// <returns></returns>
        public static int ToChars(int codePoint, char[] dst, int dstIndex) {
            var converted = Unicode.ToCharArray(new[] { codePoint }, 0, 1);

            Array.Copy(converted, 0, dst, dstIndex, converted.Length);
            return converted.Length;
        }

        /// <summary>
        /// To the chars.
        /// </summary>
        /// <param name="codePoint">The code point.</param>
        /// <returns></returns>
        public static char[] ToChars(int codePoint) {
            return Unicode.ToCharArray(new[] { codePoint }, 0, 1);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public static int GetHashCode(char value) => value;

        #region overrides
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            var character = obj as Character;
            return character != null &&
                   this.value == character.value;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => GetHashCode(this.value);

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            char[] buffer = { this.value };
            return new string(buffer);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="character1">The character1.</param>
        /// <param name="character2">The character2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Character character1, Character character2) {
            return EqualityComparer<Character>.Default.Equals(character1, character2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="character1">The character1.</param>
        /// <param name="character2">The character2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Character character1, Character character2) {
            return !(character1 == character2);
        }
        #endregion
    }
}
