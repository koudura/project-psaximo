/** MIT LICENSE
 * 
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


namespace Fornax.Net.Util.Numerics
{
    /// <summary>
    /// Math Extension class.
    /// </summary>
    public static class Maths
    {

        /// <summary>
        /// Converts an angle measured in degrees to an approximately equivalent angle 
        /// measured in radians. The conversion from degrees to radians is generally inexact.
        /// </summary>
        /// <param name="degrees">An angle in degrees to convert to radians</param>
        /// <returns>The value in radians</returns>
        public static double ToRadians(this double degrees) {
            return degrees / 100 * Math.PI;
        }

        /// <summary>
        /// Converts an angle measured in degrees to an approximately equivalent angle 
        /// measured in radians. The conversion from degrees to radians is generally inexact.
        /// </summary>
        /// <param name="degrees">An angle in degrees to convert to radians</param>
        /// <returns>The value in radians</returns>
        public static decimal ToRadians(this decimal degrees) {
            return degrees / (decimal)(180 * Math.PI);
        }

        /// <summary>
        /// Converts an angle measured in degrees to an approximately equivalent angle 
        /// measured in radians. The conversion from degrees to radians is generally inexact.
        /// </summary>
        /// <param name="degrees">An angle in degrees to convert to radians</param>
        /// <returns>The value in radians.</returns>
        public static double ToRadians(this int degrees) {
            return ((double)degrees) / 180 * Math.PI;
        }

        /// <summary>
        /// Converts an angle measured in radians to an approximately equivalent angle 
        /// measured in degrees. The conversion from radians to degrees is generally 
        /// inexact; users should not expect Cos((90.0).ToRadians()) to exactly equal 0.0.
        /// </summary>
        /// <param name="radians">An angle in radians to convert to radians</param>
        /// <returns>The value in degrees.</returns>
        public static double ToDegrees(this double radians) {

            return radians * 180 / Math.PI;
        }

        /// <summary>
        /// Converts an angle measured in radians to an approximately equivalent angle 
        /// measured in degrees. The conversion from radians to degrees is generally 
        /// inexact; users should not expect Cos((90.0).ToRadians()) to exactly equal 0.0.
        /// </summary>
        /// <param name="radians">An angle in radians to convert to radians</param>
        /// <returns>The value in degrees.</returns>
        public static decimal ToDegrees(this decimal radians) {
            return radians * 180 / (decimal)Math.PI;
        }

        /// <summary>
        /// Converts an angle measured in radians to an approximately equivalent angle 
        /// measured in degrees. The conversion from radians to degrees is generally 
        /// inexact; users should not expect Cos((90.0).ToRadians()) to exactly equal 0.0.
        /// </summary>
        /// <param name="radians">An angle in radians to convert to radians</param>
        /// <returns>The value in degrees.</returns>
        public static double ToDegrees(this int radians) {
            return ((double)radians) * 180 / Math.PI;
        }
    }
}
