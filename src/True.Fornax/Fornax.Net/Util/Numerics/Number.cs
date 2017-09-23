using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Util.Numerics
{
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


    }
}
