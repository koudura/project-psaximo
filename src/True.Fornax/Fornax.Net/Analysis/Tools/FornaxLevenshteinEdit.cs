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
using Fornax.Net.Common.Similarity;

namespace Fornax.Net.Analysis.Tools
{
    /// <summary>
    /// The Fornax.Net specific levenshtein edit distance class. 
    /// </summary>
    /// <seealso cref="Fornax.Net.Common.Similarity.IEditDistance" />
    public sealed class FornaxLevenshteinEdit : IEditDistance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxLevenshteinEdit"/> class.
        /// This Implementation is relatively less memory friendly that the forward impl.
        /// Fornax.Net implementation f the Levenshtein edit distance algorithm. 
        /// </summary>
        public FornaxLevenshteinEdit() {
        }

        /// <summary>
        /// Returns a float between 0 and 1 based on how similar the specified strings are to one another.  
        /// Returning a value of 1 means the specified strings are identical and 0 means the
        /// string are absolutely different.
        /// </summary>
        /// <param name="str1">The First string.</param>
        /// <param name="str2">The Second string.</param>
        /// <returns>a float between 0 and 1 based on how similar the specified strings are to one another.</returns>
        public float GetDistance(string source, string target) {
            int source_len = source.Length;
            int target_len = target.Length;
            int cost;

            if (source_len == 0 || target_len == 0) { return (source_len == target_len) ? 1 : 0; }

            Int32[,] levTable = new Int32[target_len, source_len];

            for (Int32 i = 0; i < source_len; i++) { levTable[0, i] = i; }
            for (Int32 i = 0; i < target_len; i++) { levTable[i, 0] = i; }

            char s_i; char t_j;

            for (int j = 1; j < target_len; j++) {

                t_j = target[j];

                for (int i = 1; i < source_len; i++) {

                    s_i = source[i];
                    cost = (s_i == t_j) ? 0 : 1;

                    levTable[j, i] = Min((levTable[j - 1, i] + 1), (levTable[j, i - 1] + 1), (levTable[j - 1, i - 1] + cost));
                }
            }
            float @out = 1.0f - ((float)levTable[target_len - 1, source_len - 1] / Math.Max(source.Length, target.Length));
            return @out;
        }


        private static Int32 Min(int val1, int val2, int val3) {
            return Math.Min(Math.Min(val1, val2), val3);
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
            return "Fornax.Net : [Levenshtein Edit Distance].";
        }
    }
}
