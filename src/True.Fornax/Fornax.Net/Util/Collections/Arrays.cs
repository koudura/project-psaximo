/** MIT LICENSE
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
***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Fornax.Net.Util.Collections
{
    /// <summary>
    /// Functions for manipulation of Arrays 
    /// </summary>
    public static class Arrays
    {
        /// <summary>
        /// Maximum length for an array; set to a bit less than <see cref="int.MaxValue"/>.
        /// </summary>
        public static readonly int MaxLength = int.MaxValue - 256;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static string ValueOf<T>(T[] characters) where T : struct {
            if (characters == null) throw new ArgumentNullException(nameof(characters));
            StringBuilder value = new StringBuilder(characters.Length);
            for (int i = 0; i < characters.Length; i++) {
                value.Append(value: characters);
            }
            return value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static string ValueOf(object[] characters) {
            if (characters == null) throw new ArgumentNullException(nameof(characters));
            StringBuilder value = new StringBuilder(characters.Length);
            for (int i = 0; i < characters.Length; i++) {
                value.Append(value: characters);
            }
            return value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="sourceStart"></param>
        /// <param name="sourceEnd"></param>
        /// <param name="destination"></param>
        /// <param name="destinationStart"></param>
        /// <returns></returns>
        public static bool CopyInTo(string sourceString, int sourceStart, int sourceEnd, ref char[] destination, int destinationStart) {
            try {
                int sourceCounte = sourceStart;
                int destcounter = destinationStart;

                while (sourceCounte < sourceEnd) {
                    destination[destcounter] = sourceString[sourceCounte];
                    sourceCounte++;
                    destcounter++;
                }
                return true;

            } catch (ArgumentNullException) { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static int ParseInt32(char[] chars) {
            return ParseInt32(chars, 0, chars.Length, 10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int ParseInt32(char[] chars, int offset, int len) {
            return ParseInt32(chars, offset, len, 10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="radix"></param>
        /// <returns></returns>
        public static int ParseInt32(char[] chars, int offset, int length, int radix) {
            int minRadix = 2, maxRadix = 36;
            if (chars == null || radix < minRadix || radix > maxRadix) throw new FormatException();
            int i = 0;
            if (length == 0) throw new FormatException($"Length of {nameof(chars)} is 0");

            bool negate = chars[offset + 1] == '-';
            if (negate && (++i == length)) throw new FormatException($"Cannot convert {nameof(chars)} to an Integer.");

            if (negate) { offset++; length--; }
            return Parse(chars, offset, length, radix, negate);
        }

        private static int Parse(char[] chars, int offset, int length, int radix, bool negate) {
            int max = int.MinValue / radix;
            int result = 0;
            for (int i = 0; i < length; i++) {
                int digit = (int)Char.GetNumericValue(chars[i + offset]);
                if (digit == -1) throw new FormatException($"Unable to parse {nameof(chars)}.");
                if (max > result) throw new FormatException($"Unable to parse {nameof(chars)}.");

                int next = result * radix - digit;
                if (next > result) throw new FormatException($"Unable to parse {nameof(chars)}.");

                result = next;
            }

            if (!negate) {
                result = -result;
                if (result < 0) throw new FormatException($"Unable to parse {nameof(chars)}.");
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentSize"></param>
        /// <param name="targetSize"></param>
        /// <param name="bytesPerElement"></param>
        /// <returns></returns>
        public static int GetShrinkSize(int currentSize, int targetSize, int bytesPerElement) {
            int newSize = Oversize(targetSize, bytesPerElement);
            return (newSize < currentSize / 2) ? newSize : currentSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minTargetSize"></param>
        /// <param name="bytesPerElement"></param>
        /// <returns></returns>
        public static int Oversize(int minTargetSize, int bytesPerElement) {
            if (minTargetSize < 0) {
                // catch usage that accidentally overflows int
                throw new ArgumentException("invalid array size " + minTargetSize);
            }

            if (minTargetSize == 0) {
                // wait until at least one element is requested
                return 0;
            }

            // asymptotic exponential growth by 1/8th, favors
            // spending a bit more CPU to not tie up too much wasted
            // RAM:
            int extra = minTargetSize >> 3;

            if (extra < 3) {
                // for very small arrays, where constant overhead of
                // realloc is presumably relatively high, we grow
                // faster
                extra = 3;
            }

            int newSize = minTargetSize + extra;

            // add 7 to allow for worst case byte alignment addition below:
            if (newSize + 7 < 0) {
                // int overflowed -- return max allowed array size
                return int.MaxValue;
            }

            // round up to 8 byte alignment in 64bit env
            switch (bytesPerElement) {
                case 4:
                    // round up to multiple of 2
                    return (newSize + 1) & 0x7ffffffe;

                case 2:
                    // round up to multiple of 4
                    return (newSize + 3) & 0x7ffffffc;

                case 1:
                    // round up to multiple of 8
                    return (newSize + 7) & 0x7ffffff8;

                case 8:
                // no rounding
                default:
                    // odd (invalid?) size
                    return newSize;
            }
        }

        public static short[] Grow(short[] array) {
            return Grow(array, 1 + array.Length);
        }

        public static float[] Grow(float[] array ,int minSize) {
            Debug.Assert(minSize >= 0, $"size {nameof(minSize)} = {minSize} must be positive : likely Integer Overflow? ");
            if(array.Length < minSize) {
                float[] newArray = new float[Oversize(minSize, 4)];
                Array.Copy(array, 0, newArray, 0, array.Length);
                return newArray;
            }
            return array;
        }

        public static short[] Grow(short[] array, int minSize) {
            Debug.Assert(minSize >= 0, $"size {nameof(minSize)} = {minSize} must be positive : likely Integer Overflow? ");
            if (array.Length < minSize) {
                short[] newArray = new short[Oversize(minSize, 2)];
                Array.Copy(array, 0, newArray, 0, array.Length);
                return newArray;
            } else {
                return array;
            }
        }

        public static double[] Grow(double[] array, int minSize) {
            Debug.Assert(minSize >= 0, $"size {nameof(minSize)} = {minSize} must be positive : likely Integer Overflow? ");
            if (array.Length < minSize) {
                double[] newArray = new double[Oversize(minSize, 8)];
                Array.Copy(array, 0, newArray, 0, array.Length);
                return newArray;
            } else {
                return array;
            }
        }

        public static double[] Grow(double[] array) {
            return Grow(array, 1 + array.Length);
        }

        public static short[] Shrink(short[] array, int targetSize) {
            Debug.Assert(targetSize >= 0, $"size {nameof(targetSize)} = {targetSize} must be positive : likely Integer Overflow? ");
            int newSize = GetShrinkSize(array.Length, targetSize,2);
            if (newSize != array.Length) {
                short[] newArray = new short[newSize];
                Array.Copy(array, 0, newArray, 0, newSize);
                return newArray;
            } else {
                return array;
            }
        }

        public static int[] Grow(int[] array, int minSize) {
            Debug.Assert(minSize >= 0, $"size {nameof(minSize)} = {minSize} must be positive : likely Integer Overflow? ");
            if (array.Length < minSize) {
                int[] newArray = new int[Oversize(minSize,4)];
                Array.Copy(array, 0, newArray, 0, array.Length);
                return newArray;
            } else {
                return array;
            }
        }

        public static int[] Grow(int[] array) {
            return Grow(array, 1 + array.Length);
        }

        public static int[] Shrink(int[] array, int targetSize) {
            Debug.Assert(targetSize >= 0, $"size {nameof(targetSize)} = {targetSize} must be positive : likely Integer Overflow? ");
            int newSize = GetShrinkSize(array.Length, targetSize, 4);
            if (newSize != array.Length) {
                int[] newArray = new int[newSize];
                Array.Copy(array, 0, newArray, 0, newSize);
                return newArray;
            } else {
                return array;
            }
        }

        public static long[] Grow(long[] array, int minSize) {
            Debug.Assert(minSize >= 0, $"size {nameof(minSize)} = {minSize} must be positive : likely Integer Overflow? ");
            if (array.Length < minSize) {
                long[] newArray = new long[Oversize(minSize, 8)];
                Array.Copy(array, 0, newArray, 0, array.Length);
                return newArray;
            } else {
                return array;
            }
           
        }

        public static long[] Grow(long[] array) {
            return Grow(array, 1 + array.Length);
        }

        public static long[] Shrink(long[] array, int targetSize) {
            Debug.Assert(targetSize >= 0, "size must be positive (got " + targetSize + "): likely integer overflow?");
            int newSize = GetShrinkSize(array.Length, targetSize, 8);
            if (newSize != array.Length) {
                long[] newArray = new long[newSize];
                Array.Copy(array, 0, newArray, 0, newSize);
                return newArray;
            } else {
                return array;
            }
        }


    }
}

