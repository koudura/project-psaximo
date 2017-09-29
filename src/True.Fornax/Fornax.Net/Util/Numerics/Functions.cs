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
    /// Math operations utility functions
    /// </summary>
    public sealed class Functions
    {

        internal Functions() { }

        /// <summary>
        /// Returns <c>x &lt;= 0 ? 0 : Math.Floor(Math.Log(x) / Math.Log(base))</c>. 
        /// </summary>
        /// <param name="x">The number.</param>
        /// <param name="base"> Must be <c>&gt; 1</c>.</param>
        /// <returns>an <see cref="int"/> value of Log(<paramref name="x"/>,<paramref name="base"/>)."/></returns>
        /// <exception cref="ArgumentException">base must be > 1</exception>
        public int Log(long x, int @base) {
            if (@base <= 1) {
                throw new ArgumentException("base must be > 1");
            }
            int ret = 0;
            while (x >= @base) {
                x /= @base;
                ret++;
            }
            return ret;
        }

        /// <summary>
        /// Calculates logarithm in a given <paramref name="base" /> with doubles.
        /// </summary>
        /// <param name="base">The base.</param>
        /// <param name="x">The number.</param>
        /// <returns></returns>
        public double Log(double x, double @base) {
            return Math.Log(x) / Math.Log(@base);
        }

        /// <summary>
        /// Return the greatest common divisor of <paramref name="a"/> and <paramref name="b"/>,
        /// consistently with <c>System.Numerics.BigInteger.GreatestCommonDivisor(System.Numerics.BigInteger, System.Numerics.BigInteger)</c>.
        /// </summary>
        /// <param name="a">the first-lower number.</param>
        /// <param name="b">the second-higher number.</param>
        /// <returns> the greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.</returns>
        /// ///<remarks> 
        ///<para/><b>NOTE</b>: A greatest common divisor must be positive, but
        /// <c>2^64</c> cannot be expressed as a <see cref="long"/> although it
        /// is the GCD of <see cref="long.MinValue"/> and <c>0</c> and the GCD of
        /// <see cref="long.MinValue"/> and <see cref="long.MinValue"/>. So in these 2 cases,
        /// and only them, this method will return <see cref="long.MinValue"/>.
        /// see http://en.wikipedia.org/wiki/Binary_GCD_algorithm#Iterative_version_in_C.2B.2B_using_ctz_.28count_trailing_zeros.29
        /// </remarks> 
        public long Gcd(long a, long b) {
            a = a < 0 ? -a : a;
            b = b < 0 ? -b : b;
            if (a == 0) {
                return b;
            } else if (b == 0) {
                return a;
            }
            int commonTrailingZeros = Number.NumberOfTrailingZeros(a | b);
            a = (long)((ulong)a >> Number.NumberOfTrailingZeros(a));
            while (true) {
                b = (long)((ulong)b >> Number.NumberOfTrailingZeros(b));
                if (a == b) {
                    break;
                } else if (a > b || a == long.MinValue) {
                    long tmp = a;
                    a = b;
                    b = tmp;
                }
                if (a == 1) {
                    break;
                }
                b -= a;
            }
            return a << commonTrailingZeros;
        }

        /// <summary>
        /// Calculates inverse hyperbolic sine of a <see cref="double" /> value.
        /// <para />
        /// Special cases:
        /// <list type="bullet"><item><description>If the argument is NaN, then the result is NaN.</description></item><item><description>If the argument is zero, then the result is a zero with the same sign as the argument.</description></item><item><description>If the argument is infinite, then the result is infinity with the same sign as the argument.</description></item></list>
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>the inverse hyperbolic sine of <paramref name="a"/>.</returns>
        public double Asinh(double a) {
            double sign;
            if (BitConverter.DoubleToInt64Bits(a) < 0) {
                a = Math.Abs(a);
                sign = -1.0d;
            } else {
                sign = 1.0d;
            }

            return sign * Math.Log(Math.Sqrt(a * a + 1.0d) + a);
        }

        /// <summary>
        /// Calculates inverse hyperbolic cosine of a <see cref="double" /> value.
        /// <para />
        /// Special cases:
        /// <list type="bullet"><item><description>If the argument is NaN, then the result is NaN.</description></item><item><description>If the argument is +1, then the result is a zero.</description></item><item><description>If the argument is positive infinity, then the result is positive infinity.</description></item><item><description>If the argument is less than 1, then the result is NaN.</description></item></list>
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>the inverse hyperbolic cosine of <paramref name="a"/> .</returns>
        public double Acosh(double a) {
            return Math.Log(Math.Sqrt(a * a - 1.0d) + a);
        }

        /// <summary>
        /// Calculates inverse hyperbolic tangent of a <see cref="double" /> value.
        /// <para />
        /// Special cases:
        /// <list type="bullet"><item><description>If the argument is NaN, then the result is NaN.</description></item><item><description>If the argument is zero, then the result is a zero with the same sign as the argument.</description></item><item><description>If the argument is +1, then the result is positive infinity.</description></item><item><description>If the argument is -1, then the result is negative infinity.</description></item><item><description>If the argument's absolute value is greater than 1, then the result is NaN.</description></item></list>
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>the inverse hyperbolic tangent of <paramref name="a"/> .</returns>
        public double Atanh(double a) {
            double mult;
            if (BitConverter.DoubleToInt64Bits(a) < 0) {
                a = Math.Abs(a);
                mult = -0.5d;
            } else {
                mult = 0.5d;
            }
            return mult * Math.Log((1.0d + a) / (1.0d - a));
        }

    }


}
