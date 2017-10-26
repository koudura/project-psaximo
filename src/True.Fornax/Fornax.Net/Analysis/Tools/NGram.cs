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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Fornax.Net.Index.Common;
using Fornax.Net.Util.Collections;
using Fornax.Net.Util.Security.Cryptography;

namespace Fornax.Net.Analysis.Tools
{
    /// <summary>
    /// N-Gram representation of string text.
    /// </summary>
    /// <seealso cref="Fornax.Net.Index.Common.IGrammable" />
    public sealed class NGram : IGrammable
    {
        private readonly string text;
        private int size;

        /// <summary>
        /// Initializes a new instance of the <see cref="NGram"/> class.
        /// Creates a n-gram collection of size 2.
        /// </summary>
        /// <param name="text">The text.</param>
        public NGram(string text) : this(text, 2) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NGram"/> class.
        /// Creates a n-gram collection of (<paramref name="size"/>).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="size">The size.</param>
        /// <exception cref="ArgumentNullException">text</exception>
        public NGram(string text, uint size) {
            Contract.Requires(text != null);
            this.text = text ?? throw new ArgumentNullException(nameof(text));
            this.size = (int)size;
        }

        /// <summary>
        /// Gets the grams of a specific text.
        /// </summary>
        /// <value>
        /// The grams.
        /// </value>
        public IEnumerable<string> Grams => GetGrams();

        /// <summary>
        /// Gets or sets the size of the grams.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Size { get { return size; } set { size = value; } }

        ISet<string> GetGrams() {

            string[] list = Regex.Split(text, @"[\s\p{P}]+");
            List<string> result = new List<string>();

            int l = list.Count();

            if (size > l) {
                throw new ArgumentOutOfRangeException("n");
            }

            for (int i = 0; i < l - size + 1; i++) {
                StringBuilder sentence = new StringBuilder();
                for (int j = i; j < i + size; j++) {
                    sentence.AppendFormat("{0} ", list[j]);
                }
                result.Add(sentence.ToString().TrimEnd(' '));
            }
            return new HashSet<string>(result);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this NGram
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this NGram.
        /// </returns>
        public override string ToString() {
            return Collections.ToString(Grams);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this Ngram.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            return (obj is NGram) ? Collections.Equals(((NGram)(obj)).Grams, Grams) : false;
        }

        /// <summary>
        /// Returns a hash code/finger-print for this N-Gram.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return (int)Adler32.Compute(text);
        }
    }
}
