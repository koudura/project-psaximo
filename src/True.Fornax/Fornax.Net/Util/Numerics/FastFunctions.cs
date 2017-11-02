// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-29-2017
// ***********************************************************************
// <copyright file="FastFunctions.cs" company="True.inc">
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

// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

/// <summary>
/// The Numerics namespace.
/// </summary>
namespace Fornax.Net.Util.Numerics
{
    /// <summary>
    /// Math functions that trade off accuracy for speed.
    /// </summary>
    public sealed class FastFunctions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FastFunctions"/> class.
        /// </summary>
        internal FastFunctions() { }

        /// <summary>
        /// Returns the distance in kilometers between two points
        /// specified in decimal degrees (latitude/longitude).
        /// </summary>
        /// <param name="lat1">Latitude of the first point.</param>
        /// <param name="lon1">Longitude of the first point.</param>
        /// <param name="lat2">Latitude of the second point.</param>
        /// <param name="lon2">Longitude of the second point.</param>
        /// <returns>distance in kilometers.</returns>
        public double Haversin(double lat1, double lon1, double lat2, double lon2) {
            double x1 = lat1 * TO_RADIANS;
            double x2 = lat2 * TO_RADIANS;
            double h1 = 1 - Cos(x1 - x2);
            double h2 = 1 - Cos((lon1 - lon2) * TO_RADIANS);
            double h = (h1 + Cos(x1) * Cos(x2) * h2) / 2;

            double avgLat = (x1 + x2) / 2d;
            double diameter = EarthDiameter(avgLat);

            return diameter * Asin(Math.Min(1, Math.Sqrt(h)));
        }

        /// <summary>
        /// Returns the trigonometric cosine of an angle.
        /// <para />
        /// Error is around 1E-15.
        /// <para />
        /// Special cases:
        /// <list type="bullet"><item><description>If the argument is <see cref="double.NaN" /> or an infinity, then the result is <see cref="double.NaN" />.</description></item></list>
        /// </summary>
        /// <param name="a">An angle, in radians.</param>
        /// <returns>The cosine of the argument.</returns>
        /// <seealso cref="Math.Cos(double)" />
        public double Cos(double a) {
            if (a < 0.0) {
                a = -a;
            }
            if (a > SIN_COS_MAX_VALUE_FOR_INT_MODULO) {
                return Math.Cos(a);
            }

            int index = (int)(a * SIN_COS_INDEXER + 0.5);
            double delta = (a - index * SIN_COS_DELTA_HI) - index * SIN_COS_DELTA_LO;
            
            /**
             ** Making sure index is within tables range.
             ** Last value of each table is the same than first, so we ignore it (tabs size minus one) for modulo.
            ***/
            index &= (SIN_COS_TABS_SIZE - 2);
            double indexCos = cosTab[index];
            double indexSin = sinTab[index];
            return indexCos + delta * (-indexSin + delta * (-indexCos * ONE_DIV_F2 + delta * (indexSin * ONE_DIV_F3 + delta * indexCos * ONE_DIV_F4)));
        }

        /// <summary>
        /// Returns the arc sine of a value.
        /// <para />
        /// The returned angle is in the range <i>-pi</i>/2 through <i>pi</i>/2.
        /// Error is around 1E-7.
        /// <para />
        /// Special cases:
        /// <list type="bullet"><item><description>If the argument is <see cref="double.NaN" /> or its absolute value is greater than 1, then the result is <see cref="double.NaN" />.</description></item></list>
        /// </summary>
        /// <param name="a">the value whose arc sine is to be returned.</param>
        /// <returns>arc sine of the argument</returns>
        /// <seealso cref="Math.Asin(double)" />
        public double Asin(double a) {
            bool negateResult;
            if (a < 0.0) {
                a = -a;
                negateResult = true;
            } else {
                negateResult = false;
            }
            if (a <= ASIN_MAX_VALUE_FOR_TABS) {
                int index = (int)(a * ASIN_INDEXER + 0.5);
                double delta = a - index * ASIN_DELTA;
                double result = asinTab[index] + delta * (asinDer1DivF1Tab[index] + delta * (asinDer2DivF2Tab[index] + delta * (asinDer3DivF3Tab[index] + delta * asinDer4DivF4Tab[index])));
                return negateResult ? -result : result;
            } else {
                // this part is derived from fdlibm.
                if (a < 1.0) {
                    double t = (1.0 - a) * 0.5;
                    double p = t * (ASIN_PS0 + t * (ASIN_PS1 + t * (ASIN_PS2 + t * (ASIN_PS3 + t * (ASIN_PS4 + t * ASIN_PS5)))));
                    double q = 1.0 + t * (ASIN_QS1 + t * (ASIN_QS2 + t * (ASIN_QS3 + t * ASIN_QS4)));
                    double s = Math.Sqrt(t);
                    double z = s + s * (p / q);
                    double result = ASIN_PIO2_HI - ((z + z) - ASIN_PIO2_LO);
                    return negateResult ? -result : result;
                } else {
                    if (a == 1.0) {
                        return negateResult ? -Math.PI / 2 : Math.PI / 2;
                    } else {
                        return double.NaN;
                    }
                }
            }
        }

        /// <summary>
        /// Return an approximate value of the diameter of the earth at the given latitude, in kilometers.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>System.Double.</returns>
        public double EarthDiameter(double latitude) {
            if (double.IsNaN(latitude))
                return double.NaN;
            int index = (int)(Math.Abs(latitude) * RADIUS_INDEXER + 0.5) % earthDiameterPerLatitude.Length;
            return earthDiameterPerLatitude[index];
        }


        #region fields

        private static readonly double TO_RADIANS = Math.PI / 180D;
        private const double ONE_DIV_F2 = 1 / 2.0;

        private const double ONE_DIV_F3 = 1 / 6.0;
        private const double ONE_DIV_F4 = 1 / 24.0;

        private static readonly double PIO2_HI = BitConverter.Int64BitsToDouble(0x3FF921FB54400000L); // 1.57079632673412561417e+00 first 33 bits of pi/2
        private static readonly double PIO2_LO = BitConverter.Int64BitsToDouble(0x3DD0B4611A626331L); // 6.07710050650619224932e-11 pi/2 - PIO2_HI
        private static readonly double TWOPI_HI = 4 * PIO2_HI;
        private static readonly double TWOPI_LO = 4 * PIO2_LO;
        private static readonly int SIN_COS_TABS_SIZE = (1 << 11) + 1;
        private static readonly double SIN_COS_DELTA_HI = TWOPI_HI / (SIN_COS_TABS_SIZE - 1);
        private static readonly double SIN_COS_DELTA_LO = TWOPI_LO / (SIN_COS_TABS_SIZE - 1);
        private static readonly double SIN_COS_INDEXER = 1 / (SIN_COS_DELTA_HI + SIN_COS_DELTA_LO);
        private static readonly double[] sinTab = new double[SIN_COS_TABS_SIZE];
        private static readonly double[] cosTab = new double[SIN_COS_TABS_SIZE];
        private static readonly double ASIN_MAX_VALUE_FOR_TABS = Math.Sin(73.0.ToRadians());

        private static readonly int ASIN_TABS_SIZE = (1 << 13) + 1;
        private static readonly double ASIN_DELTA = ASIN_MAX_VALUE_FOR_TABS / (ASIN_TABS_SIZE - 1);
        private static readonly double ASIN_INDEXER = 1 / ASIN_DELTA;
        private static readonly double[] asinTab = new double[ASIN_TABS_SIZE];
        private static readonly double[] asinDer1DivF1Tab = new double[ASIN_TABS_SIZE];
        private static readonly double[] asinDer2DivF2Tab = new double[ASIN_TABS_SIZE];
        private static readonly double[] asinDer3DivF3Tab = new double[ASIN_TABS_SIZE];
        private static readonly double[] asinDer4DivF4Tab = new double[ASIN_TABS_SIZE];

        private static readonly double ASIN_PIO2_HI = BitConverter.Int64BitsToDouble(0x3FF921FB54442D18L); // 1.57079632679489655800e+00
        private static readonly double ASIN_PIO2_LO = BitConverter.Int64BitsToDouble(0x3C91A62633145C07L); // 6.12323399573676603587e-17
        private static readonly double ASIN_PS0 = BitConverter.Int64BitsToDouble(0x3fc5555555555555L); //  1.66666666666666657415e-01
        private static readonly double ASIN_PS1 = BitConverter.Int64BitsToDouble(unchecked((long)0xbfd4d61203eb6f7dL)); // -3.25565818622400915405e-01
        private static readonly double ASIN_PS2 = BitConverter.Int64BitsToDouble(0x3fc9c1550e884455L); //  2.01212532134862925881e-01
        private static readonly double ASIN_PS3 = BitConverter.Int64BitsToDouble(unchecked((long)0xbfa48228b5688f3bL)); // -4.00555345006794114027e-02
        private static readonly double ASIN_PS4 = BitConverter.Int64BitsToDouble(0x3f49efe07501b288L); //  7.91534994289814532176e-04
        private static readonly double ASIN_PS5 = BitConverter.Int64BitsToDouble(0x3f023de10dfdf709L); //  3.47933107596021167570e-05
        private static readonly double ASIN_QS1 = BitConverter.Int64BitsToDouble(unchecked((long)0xc0033a271c8a2d4bL)); // -2.40339491173441421878e+00
        private static readonly double ASIN_QS2 = BitConverter.Int64BitsToDouble(0x40002ae59c598ac8L); //  2.02094576023350569471e+00
        private static readonly double ASIN_QS3 = BitConverter.Int64BitsToDouble(unchecked((long)0xbfe6066c1b8d0159L)); // -6.88283971605453293030e-01
        private static readonly double ASIN_QS4 = BitConverter.Int64BitsToDouble(0x3fb3b8c5b12e9282L); //  7.70381505559019352791e-02

        private static readonly int RADIUS_TABS_SIZE = (1 << 10) + 1;
        private static readonly double RADIUS_DELTA = (Math.PI / 2d) / (RADIUS_TABS_SIZE - 1);
        private static readonly double RADIUS_INDEXER = 1d / RADIUS_DELTA;
        private static readonly double[] earthDiameterPerLatitude = new double[RADIUS_TABS_SIZE];

        /// <summary>
        /// Max abs value for fast modulo, above which we use regular angle normalization.
        /// this value must be (Integer.MAX_VALUE / SIN_COS_INDEXER), to stay in range of int type.
        /// The higher it is, the higher the error, but also the faster it is for lower values.
        /// If you set it to ((Integer.MAX_VALUE / SIN_COS_INDEXER) * 0.99), worse accuracy on double range is about 1e-10.
        /// </summary>
        internal static readonly double SIN_COS_MAX_VALUE_FOR_INT_MODULO = ((int.MaxValue >> 9) / SIN_COS_INDEXER) * 0.99;
        #endregion


        /// <summary>
        /// Initializes the <see cref="FastFunctions"/> clook up tables.
        /// </summary>
        static FastFunctions() {

            int SIN_COS_PI_INDEX = (SIN_COS_TABS_SIZE - 1) / 2;
            int SIN_COS_PI_MUL_2_INDEX = 2 * SIN_COS_PI_INDEX;
            int SIN_COS_PI_MUL_0_5_INDEX = SIN_COS_PI_INDEX / 2;
            int SIN_COS_PI_MUL_1_5_INDEX = 3 * SIN_COS_PI_INDEX / 2;
            for (int i = 0; i < SIN_COS_TABS_SIZE; i++) {

                // angle: in [0,2*PI].
                double angle = i * SIN_COS_DELTA_HI + i * SIN_COS_DELTA_LO;
                double sinAngle = Math.Sin(angle);
                double cosAngle = Math.Cos(angle);

                // For indexes corresponding to null cosine or sine, we make sure the value is zero
                // and not an epsilon. this allows for a much better accuracy for results close to zero.
                if (i == SIN_COS_PI_INDEX) {
                    sinAngle = 0.0;
                } else if (i == SIN_COS_PI_MUL_2_INDEX) {
                    sinAngle = 0.0;
                } else if (i == SIN_COS_PI_MUL_0_5_INDEX) {
                    cosAngle = 0.0;
                } else if (i == SIN_COS_PI_MUL_1_5_INDEX) {
                    cosAngle = 0.0;
                }
                sinTab[i] = sinAngle;
                cosTab[i] = cosAngle;
            }

            for (int i = 0; i < ASIN_TABS_SIZE; i++) {

                double x = i * ASIN_DELTA;
                asinTab[i] = Math.Asin(x);
                double oneMinusXSqInv = 1.0 / (1 - x * x);
                double oneMinusXSqInv0_5 = Math.Sqrt(oneMinusXSqInv);
                double oneMinusXSqInv1_5 = oneMinusXSqInv0_5 * oneMinusXSqInv;
                double oneMinusXSqInv2_5 = oneMinusXSqInv1_5 * oneMinusXSqInv;
                double oneMinusXSqInv3_5 = oneMinusXSqInv2_5 * oneMinusXSqInv;
                asinDer1DivF1Tab[i] = oneMinusXSqInv0_5;
                asinDer2DivF2Tab[i] = (x * oneMinusXSqInv1_5) * ONE_DIV_F2;
                asinDer3DivF3Tab[i] = ((1 + 2 * x * x) * oneMinusXSqInv2_5) * ONE_DIV_F3;
                asinDer4DivF4Tab[i] = ((5 + 2 * x * (2 + x * (5 - 2 * x))) * oneMinusXSqInv3_5) * ONE_DIV_F4;
            }

            // WGS84 earth-ellipsoid major (a) and minor (b) radius
            const double a = 6378137; // [m]
            const double b = 6356752.31420; // [m]
            double a2 = a * a;
            double b2 = b * b;

            earthDiameterPerLatitude[0] = 2 * a / 1000d;
            earthDiameterPerLatitude[RADIUS_TABS_SIZE - 1] = 2 * b / 1000d;
            // earth radius
            for (int i = 1; i < RADIUS_TABS_SIZE - 1; i++) {
                double lat = Math.PI * i / (2d * RADIUS_TABS_SIZE - 1);
                double one = Math.Pow(a2 * Math.Cos(lat), 2);
                double two = Math.Pow(b2 * Math.Sin(lat), 2);
                double three = Math.Pow(a * Math.Cos(lat), 2);
                double four = Math.Pow(b * Math.Sin(lat), 2);

                double radius = Math.Sqrt((one + two) / (three + four));
                earthDiameterPerLatitude[i] = 2 * radius / 1000d;
            }

        }
    }
}
