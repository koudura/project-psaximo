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

using Fornax.Net.Util.Collections;
using Fornax.Net.Util.Numerics;

namespace Fornax.Net.Common.Tools
{
    /// <summary>
    /// The Jaro-Winkler Edit distance class.
    /// </summary>
    /// <seealso cref="IEditDistance" />
    public sealed class JaroWinklerEdit : IEditDistance
    {
        private float threshold = 0.7f;

        /// <summary>
        /// Initializes a new instance of the <see cref="JaroWinklerEdit"/> class.
        /// For calculating the edit distance between two strings using the Jaro-winkler algorithm.
        /// Advice: Use for similarity measure between short strings only.
        /// </summary>
        /// <remarks>/// See <a href="http://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance">http://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance</a></remarks>
        public JaroWinklerEdit() { }

        /// <summary>
        /// Gets or sets the threshold used to determine when Winkler bonus should be used.
        /// The default value is 0.7. Set to a negative value to get the Jaro distance.
        /// </summary>
        /// <value>
        /// The threshold value.
        /// </value>
        public float Threshold { get { return threshold; } set { threshold = value; } }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this Jaro-winkler instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this Jaro-winkler instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (this == obj)  return true;
            if (null == obj || GetType() != obj.GetType()) {
                return false;
            }
            var jwedit = obj as JaroWinklerEdit;
            return (Number.SingleToIntBits(jwedit.threshold) == Number.SingleToIntBits(threshold));
        }

        /// <summary>
        /// Returns a float between 0 and 1 based on how similar the specified strings are to one another.  
        /// Returning a value of 1 means the specified strings are identical and 0 means the
        /// string are absolutely different.
        /// </summary>
        /// <param name="str1">The First string.</param>
        /// <param name="str2">The Second string.</param>
        /// <returns>a float between 0 and 1 based on how similar the specified strings are to one another.</returns>
        public float GetDistance(string str1, string str2) {
            int[] mtp = Matches(str1, str2);
            float m = mtp[0];
            if (m == 0) {
                return 0f;
            }
            float j = ((m / str1.Length + m / str2.Length + (m - mtp[1]) / m)) / 3;
            float jw = j < Threshold ? j : j + Math.Min(0.1f, 1f / mtp[3]) * mtp[2] * (1 - j);
            return jw;
        }

        /// <summary>
        /// Returns a hash code for this instance of jaro-winkler.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return 113 * Number.SingleToIntBits(threshold) * GetType().GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return $"Jaro-Winkler Edit Distance by Threshold [{threshold}]";
        }

        private int[] Matches(string s1, string s2) {
            string max, min;
            if (s1.Length > s2.Length) {
                max = s1;
                min = s2;
            } else {
                max = s2;
                min = s1;
            }
            int range = Math.Max(max.Length / 2 - 1, 0);
            int[] matchIndexes = new int[min.Length];
            Arrays.Fill(matchIndexes, -1);
            bool[] matchFlags = new bool[max.Length];
            int matches = 0;
            for (int mi = 0; mi < min.Length; mi++) {
                char c1 = min[mi];
                for (int xi = Math.Max(mi - range, 0), xn = Math.Min(mi + range + 1, max.Length); xi < xn; xi++) {
                    if (!matchFlags[xi] && c1 == max[xi]) {
                        matchIndexes[mi] = xi;
                        matchFlags[xi] = true;
                        matches++;
                        break;
                    }
                }
            }
            char[] ms1 = new char[matches];
            char[] ms2 = new char[matches];
            for (int i = 0, si = 0; i < min.Length; i++) {
                if (matchIndexes[i] != -1) {
                    ms1[si] = min[i];
                    si++;
                }
            }
            for (int i = 0, si = 0; i < max.Length; i++) {
                if (matchFlags[i]) {
                    ms2[si] = max[i];
                    si++;
                }
            }
            int transpositions = 0;
            for (int mi = 0; mi < ms1.Length; mi++) {
                if (ms1[mi] != ms2[mi]) {
                    transpositions++;
                }
            }
            int prefix = 0;
            for (int mi = 0; mi < min.Length; mi++) {
                if (s1[mi] == s2[mi]) {
                    prefix++;
                } else {
                    break;
                }
            }
            return new int[] { matches, transpositions / 2, prefix, max.Length };
        }


    }
}
