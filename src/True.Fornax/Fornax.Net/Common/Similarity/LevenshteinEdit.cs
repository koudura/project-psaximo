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

namespace Fornax.Net.Common.Similarity
{
    /// <summary>
    /// Levenshtein Edit Distance class.
    /// </summary>
    /// <seealso cref="Fornax.Net.Common.Similarity.IEditDistance" />
    [Progress("LevenshteinEdit", true, Documented = true, Tested = true)]
    public sealed class LevenshteinEdit : IEditDistance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LevenshteinEdit"/> class.
        /// For calculating the edit distance between two strings using the Levenshtein edit algorithm.
        /// </summary>
        public LevenshteinEdit() { }


        /// <summary>
        /// Returns a float between 0 and 1 based on how similar the specified strings are to one another.
        /// Returning a value of 1 means the specified strings are identical and 0 means the
        /// string are absolutely different.
        /// </summary>
        /// <param name="target">The target string.</param>
        /// <param name="source">The source string.</param>
        /// <returns>
        /// a float between 0 and 1 based on how similar the specified strings are to one another.
        /// </returns>
        /// <remarks>
        /// The difference between this implementation and the previous (<see cref="Analysis.Tools.FornaxLevenshteinEdit"/>) is that, rather
        /// than creating and retaining a matrix of size s.length()+1 by t.length()+1,
        /// we maintain two single-dimensional arrays of length s.length()+1.  The first, d,
        /// is the 'current working' distance array that maintains the newest distance cost
        /// counts as we iterate through the characters of String s.Each time we increment
        /// the index of String t we are comparing, d is copied to p, the second int[].  Doing so
        /// allows us to retain the previous cost counts as required by the algorithm (taking
        /// the minimum of the cost count to the left, up one, and diagonally up and to the left
        /// of the current cost count being calculated).  (Note that the arrays aren't really
        /// copied anymore, just switched...this is clearly much better than cloning an array
        /// or doing a System.arraycopy() each time through the outer loop.)	
        /// Effectively, the difference between the two implementations is this one does not
        /// cause an out of memory condition when calculating the LD over two very large strings.
        /// </remarks>
        public float GetDistance(string target, string source) {

            char[] sa;
            int n;
            int[] p;
            int[] d;
            int[] _d;

            sa = target.ToCharArray();
            n = sa.Length;
            p = new int[n + 1];
            d = new int[n + 1];

            int m = source.Length;
            if (n == 0 || m == 0) {
                return (n == m) ? 1 : 0;
            }

            int i;
            int j;

            char t_j;

            int cost;

            for (i = 0; i <= n; i++) {
                p[i] = i;
            }

            for (j = 1; j <= m; j++) {
                t_j = source[j - 1];
                d[0] = j;

                for (i = 1; i <= n; i++) {
                    cost = sa[i - 1] == t_j ? 0 : 1;
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);
                }
                _d = p;
                p = d;
                d = _d;
            }
            return 1.0f - ((float)p[n] / Math.Max(source.Length, sa.Length));
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj == this) return true;
            return (GetType() == obj.GetType());
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return GetType().GetHashCode() * 163;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return "Levenshtein Edit Distance.";
        }
    }
}
