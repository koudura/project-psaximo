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

namespace Fornax.Net.Util.Numerics
{
    /// <summary>
    /// A Utility for Number conversions and Parsing.
    /// </summary>
    [Progress("Number",false,Documented = false,Tested = false)]
    public static class Number
    {
        /// <summary>
        /// The minimum radix available for conversion to and from strings.
        /// </summary>
        public const int MinRadix = 2;

        /// <summary>
        /// The maximum radix available for conversion to and from strings.
        /// </summary>
        public const int MaxRadix = 36;

        #region utils

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number.
        /// <para>An Unsigned Right shift is a zero fill right shift as in <code>(>>>) in java.</code></para>
        /// </summary>
        /// <param name="number">The number to operate on.</param>
        /// <param name="shift">The shift count by bits.</param>
        /// <returns>the resulting number from zero fill right shift operation.</returns>
        public static int URShift(int number, int shift) {
            return (int)((uint)number >> shift);
        }

        /// <summary>
        /// Performs an unsigned bitwise right shift with the specified number.
        /// <para>An Unsigned Right shift is a zero fill right shift as in <code>(>>>) in java.</code></para>
        /// </summary>
        /// <param name="number">The number to operate on.</param>
        /// <param name="shift">The shift count by bits.</param>
        /// <returns>the resulting number from zero fill right shift operation.</returns>
        public static long URShift(long number, int shift) {
            return (long)(((ulong)number) >> shift);
        }

        /// <summary>
        /// Determines whether the specified <see cref="uint"/> (Unsigned Integer) value is prime.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is prime; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrime(uint value) {
            uint root = (uint)Math.Sqrt(value);
            for (uint i = 2; i <= root; i++) {
                if (value % i == 0) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="ulong"/> (Unsigned long) value is prime.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is prime; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrime(ulong value) {
            ulong root = (ulong)Math.Sqrt(value);
            for (ulong i = 2; i <= root; i++) {
                if (value % i == 0) {
                    return false;
                }
            }
            return true;
        }

        #endregion


        #region Java Ports
        /// <summary>
        /// Returns an <see cref="int"/> value with at most a single one-bit, in the position of the
        /// highest-order ("leftmost") one-bit in the specified <see cref="int"/> value <paramref name="i"/>.
        /// Returns zero if <paramref name="i"/> is equal to zero.
        /// </summary>
        /// <param name="i">The value whose highest one bit is to be computed.</param>
        /// <returns> an <see cref="int"/> value with at most a ssingle one-bit, in the position of the
        ///  highest-order ("leftmost") one-bit in the specified <see cref="int"/> value <paramref name="i"/>,
        ///  zero if <paramref name="i"/> is equal to zero.
        /// </returns>
        public static int HighestOneBit(this int i) {
            i |= (i >> 1);
            i |= (i >> 2);
            i |= (i >> 4);
            i |= (i >> 8);
            i |= (i >> 16);
            return i - (URShift(i, 1));
        }

        /// <summary>
        /// Returns an <see cref="int"/> value with at most a single one-bit, in the position
        /// of the lowest-order ("rightmost") one-bit in the specified <see cref="int"/> value <paramref name="i"/>.
        /// Returns zero if <paramref name="i"/> is equal to zero.
        /// </summary>
        /// <param name="i">The value whose lowest one bit is to be computed.</param>
        /// <returns>an <see cref="int"/> value with at most a single one-bit, in the position
        /// of the lowest-order ("rightmost") one-bit in the specified <see cref="int"/> value <paramref name="i"/>,
        /// zero if <paramref name="i"/> is equal to zero.</returns>
        public static int LowestOneBit(this int i) => i & -i;

        /// <summary>
        /// Numbers the of leading zeros.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public static int NumberOfLeadingZeros(this int i) {
            if (i == 0) return 32;
            int n = 1;

            if (URShift(i, 16) == 0) { n += 16; i <<= 16; }
            if (URShift(i, 24) == 0) { n += 8; i <<= 8; }
            if (URShift(i, 28) == 0) { n += 4; i <<= 4; }
            if (URShift(i, 30) == 0) { n += 2; i <<= 2; }
            n -= URShift(i, 31);
            return n;
        }

        /// <summary>
        /// Numbers the of trailing zeroes.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public static int NumberOfTrailingZeroes(this int i) {
            if (i == 0) return 32;
            uint ui = (uint)i;
            int count = 0;
            int num;
            for (num = 0; num < 32; ++num) {
                if ((ui & 1) == 1) break;
                count++;
                ui >>= 1;
            }
            return count;
        }

        /// <summary>
        /// Bits the count.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public static int BitCount(this int num) {
            num = num - ((URShift(num, 1)) & 0x55555555);
            num = (num & 0x33333333) + ((URShift(num, 2)) & 0x33333333);
            num = (num + (URShift(num, 4))) & 0x0f0f0f0f;
            num = num + (URShift(num, 8));
            num = num + (URShift(num, 16));
            return num & 0x3f;
        }

        /// <summary>
        /// Rotates the left.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="distance">The distance.</param>
        /// <returns></returns>
        public static int RotateLeft(this int num, int distance) {
            return (num << distance) | (URShift(num, -distance));
        }

        /// <summary>
        /// Rotates the right.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="distance">The distance.</param>
        /// <returns></returns>
        public static int RotateRight(this int num, int distance) {
            return (URShift(num, distance)) | (num << -distance);
        }

        /// <summary>
        /// Reverses the specified number.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public static int Reverse(this int num) {

            num = (num & 0x55555555) << 1 | (URShift(num, 1)) & 0x55555555;
            num = (num & 0x33333333) << 2 | (URShift(num, 2)) & 0x33333333;
            num = (num & 0x0f0f0f0f) << 4 | (URShift(num, 4)) & 0x0f0f0f0f;
            num = (num << 24) | ((num & 0xff00) << 8) | ((URShift(num, 8)) & 0xff00) | (URShift(num, 24));

            return num;
        }

        /// <summary>
        /// Sigs the number.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public static int SigNum(this int num) {
            return (num >> 31) | URShift(-num, 31);
        }

        /// <summary>
        /// Reverses the bytes.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public static int ReverseBytes(this int num) {
            return (URShift(num, 24)) | ((num >> 8) & 0xFF00) | ((num << 8) & 0xFF0000) | ((num << 24));
        }

        /// <summary>
        /// To the binary string.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public static string ToBinaryString(this int i) {
            return Convert.ToString(i, 2);
        }

        /// <summary>
        /// Highests the one bit.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        public static long HighestOneBit(this long l) {
            l |= (l >> 1);
            l |= (l >> 2);
            l |= (l >> 4);
            l |= (l >> 8);
            l |= (l >> 16);
            l |= (l >> 32);
            return l - (URShift(l, 1));
        }

        /// <summary>
        /// Lowests the one bit.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        public static long LowestOneBit(this long l) => l & -l;

        /// <summary>
        /// Numbers the of leading zeros.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public static int NumberOfLeadingZeros(this long num) {
            if (num == 0) return 64;

            ulong unum = (ulong)num;
            int count = 0;
            int i;

            for (i = 0; i < 64; ++i) {
                if ((unum & 0x8000000000000000L) == 0x8000000000000000L) break;
                count++;
                unum <<= 1;
            }
            return count;
        }

        /// <summary>
        /// Numbers the of trailing zeros.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public static int NumberOfTrailingZeros(this long num) {
            if (num == 0) return 64;

            ulong unum = (ulong)num;
            int count = 0;
            int i;

            for (i = 0; i < 64; ++i) {
                if ((unum & 1L) == 1L) break;
                count++;
                unum >>= 1;
            }
            return count;
        }

        /// <summary>
        /// Bits the count.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        public static int BitCount(this long l) {
            l = l - (URShift(l, 1) & 0x5555555555555555L);
            l = (l & 0x3333333333333333L) + (URShift(l, 2) & 0x3333333333333333L);
            l = (l + URShift(l, 4)) & 0x0f0f0f0f0f0f0f0fL;
            l = l + URShift(l, 8);
            l = l + URShift(l, 16);
            l = l + URShift(l, 32);
            return (int)l & 0x7f;
        }

        /// <summary>
        /// Rotates the left.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="distance">The distance.</param>
        /// <returns></returns>
        public static long RotateLeft(this long l, int distance) {
            return (URShift(l, distance) | URShift(l, -distance));
        }

        /// <summary>
        /// Rotates the right.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="distance">The distance.</param>
        /// <returns></returns>
        public static long RotateRight(this long l, int distance) {
            return (URShift(l, distance) | l << -distance);
        }

        /// <summary>
        /// Reverses the specified l.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        public static long Reverse(this long l) {
            l = (l & 0x5555555555555555L) << 1 | URShift(l, 1) & 0x5555555555555555L;
            l = (l & 0x3333333333333333L) << 2 | URShift(l, 2) & 0x3333333333333333L;
            l = (l & 0x0f0f0f0f0f0f0f0fL) << 4 | URShift(l, 4) & 0x0f0f0f0f0f0f0f0fL;
            l = (l & 0x00ff00ff00ff00ffL) << 8 | URShift(l, 8) & 0x00ff00ff00ff00ffL;

            l = (l << 48) | ((l & 0xffff0000L) << 16) | (URShift(l, 16) & 0xffff0000L) | URShift(l, 48);
            return l;
        }

        /// <summary>
        /// Sigs the number.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        public static int SigNum(this long l) {
            return (int)((l >> 63) | URShift(-l, 63));
        }

        /// <summary>
        /// Reverses the bytes.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        public static long ReverseBytes(this long l) {
            l = (l & 0x00ff00ff00ff00ffL) << 8 | URShift(l, 8) & 0x00ff00ff00ff00ffL;
            return (l << 48) | ((l & 0xffff0000L) << 16) | (URShift(l, 16) & 0xffff0000L) | URShift(l, 48);
        }

        #endregion


        #region conversions
        /// <summary>
        /// Flips the endian.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public static char FlipEndian(char c) { 
            return (char)((c & 0xFFU) << 8 | (c & 0xFF00U) >> 8);
        }

        /// <summary>
        /// Flips the endian.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static short FlipEndian(short s) {
            return (short)((s & 0xFFU) << 8 | s & 0xFF00U >> 8);
        }

        /// <summary>
        /// Flips the endian.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public static int FlipEndian(int i) {
            return (int)((i & 0x000000FFU) << 24 | (i & 0x0000FF00U) << 8 | (i & 0x00FF0000U) >> 8 | (i & 0xFF000000U) >> 24);
        }

        /// <summary>
        /// Flips the endian.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        public static long FlipEndian(long l) {
            ulong y = (ulong)l;
            return (long)(
                (y & 0x00000000000000FFUL) << 56 | (y & 0x000000000000FF00UL) << 40 |
                (y & 0x0000000000FF0000UL) << 24 | (y & 0x00000000FF000000UL) << 8 |
                (y & 0x000000FF00000000UL) >> 8 | (y & 0x0000FF0000000000UL) >> 24 |
                (y & 0x00FF000000000000UL) >> 40 | (y & 0xFF00000000000000UL) >> 56);
        }

        /// <summary>
        /// Flips the endian.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static float FlipEndian(float f) {
            int x = SingleToIntBits(f);
            return IntBitsToSingle(FlipEndian(x));
        }

        /// <summary>
        /// Flips the endian.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns></returns>
        public static double FlipEndian(double d) {
            long x = BitConverter.DoubleToInt64Bits(d);
            return BitConverter.Int64BitsToDouble(FlipEndian(x));
        }

        /// <summary>
        /// Singles to int bits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int SingleToIntBits(float value) {
            if (float.IsNaN(value)) return 0x7fc00000;
            return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        }

        /// <summary>
        /// Ints the bits to single.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static float IntBitsToSingle(int value) {
            return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }

        /// <summary>
        /// Singles to raw int bits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int SingleToRawIntBits(float value) {
            return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        }

        /// <summary>
        /// Singles to long bits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long SingleToLongBits(float value) {
            return BitConverter.ToInt64(BitConverter.GetBytes(value), 0);
        }

        /// <summary>
        /// Doubles to raw long bits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long DoubleToRawLongBits(double value) {
            return BitConverter.DoubleToInt64Bits(value);
        }

        /// <summary>
        /// Doubles to long bits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long DoubleToLongBits(double value) {
            if (double.IsNaN(value)) return 0x7ff8000000000000L;
            return BitConverter.DoubleToInt64Bits(value);
        }
        #endregion

    }
}