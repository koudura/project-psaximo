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
    /// The N-Gram Edit distance class.
    /// </summary>
    /// <seealso cref="IEditDistance" />
    /// <remarks>
    /// N-Gram version of edit distance based on paper by Grzegorz Kondrak,
    /// "N-gram similarity and distance". Proceedings of the Twelfth International
    /// Conference on String Processing and Information Retrieval (SPIRE 2005), pp. 115-126,
    /// Buenos Aires, Argentina, November 2005.
    /// <a href="http://www.cs.ualberta.ca/~kondrak/papers/spire05.pdf">http://www.cs.ualberta.ca/~kondrak/papers/spire05.pdf</a>
    /// This implementation uses the position-based optimization to compute partial
    /// matches of n-gram sub-strings and adds a null-character prefix of size n-1
    /// so that the first character is contained in the same number of n-grams as
    /// a middle character.  Null-character prefix matches are discounted so that
    /// strings with no matching characters will return a distance of 0.
    /// </remarks>
    /// <seealso cref="IEditDistance" />
    public class NGramEdit : IEditDistance
    {
        private int n;

        /// <summary>
        /// Initializes a new instance of the <see cref="NGramEdit"/> class.
        /// Creates an N-Gram distance measure using n-grams of the specified size.
        /// </summary>
        /// <param name="size"> The size of the n-gram to be used to compute the string distance.</param>
        public NGramEdit(int size) {
            n = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NGramEdit" /> class.
        /// Creates an N-Gram distance measure using n-grams of size 2.
        /// </summary>
        public NGramEdit() : this(2) {
        }

        /// <summary>
        /// Returns a float between 0 and 1 based on how similar the specified strings are to one another.
        /// Returning a value of 1 means the specified strings are identical and 0 means the
        /// string are absolutely different.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="target">The target string.</param>
        /// <returns>
        /// a float between 0 and 1 based on how similar the specified strings are to one another.
        /// </returns>
        public float GetDistance(string source, string target) {
            int sl = source.Length;
            int tl = target.Length;

            if (sl == 0 || tl == 0) {
                if (sl == tl) {
                    return 1;
                } else {
                    return 0;
                }
            }

            int cost = 0;
            if (sl < n || tl < n) {
                for (int i2 = 0, ni = Math.Min(sl, tl); i2 < ni; i2++) {
                    if (source[i2] == target[i2]) {
                        cost++;
                    }
                }
                return (float)cost / Math.Max(sl, tl);
            }
            char[] sa = new char[sl + n - 1];
            float[] p;
            float[] d;
            float[] _d;


            for (int i2 = 0; i2 < sa.Length; i2++) {
                if (i2 < n - 1) {
                    sa[i2] = (char)0;
                } else {
                    sa[i2] = source[i2 - n + 1];
                }
            }
            p = new float[sl + 1];
            d = new float[sl + 1];

            int i;
            int j;

            char[] t_j = new char[n];

            for (i = 0; i <= sl; i++) {
                p[i] = i;
            }

            for (j = 1; j <= tl; j++) {
                if (j < n) {
                    for (int ti = 0; ti < n - j; ti++) {
                        t_j[ti] = (char)0;
                    }
                    for (int ti = n - j; ti < n; ti++) {
                        t_j[ti] = target[ti - (n - j)];
                    }
                } else {
                    t_j = target.Substring(j - n, j - (j - n)).ToCharArray();
                }
                d[0] = j;
                for (i = 1; i <= sl; i++) {
                    cost = 0;
                    int tn = n;

                    for (int ni = 0; ni < n; ni++) {
                        if (sa[i - 1 + ni] != t_j[ni]) {
                            cost++;
                        } else if (sa[i - 1 + ni] == 0) {
                            tn--;
                        }
                    }
                    float ec = (float)cost / tn;
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + ec);
                }
                _d = p;
                p = d;
                d = _d;
            }
            return 1.0f - (p[sl] / Math.Max(tl, sl));
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance of ngram-edit.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return $"NGram Edit Distance of Size [{n}]";
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (this == obj) return true;
            if (null == obj || GetType() != obj.GetType()) return false;
            var ng = obj as NGramEdit;
            return (n == ng.n);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return 1427 * n * GetType().GetHashCode();
        }
    }
}
