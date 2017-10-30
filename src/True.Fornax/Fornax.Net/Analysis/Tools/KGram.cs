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

using Fornax.Net.Index.Common;
using Fornax.Net.Util.Collections;
using Fornax.Net.Util.Security.Cryptography;
using Token = Fornax.Net.Analysis.Tokenization.Token;

namespace Fornax.Net.Analysis.Tools
{
    /// <summary>
    /// K-gram representation of a word.
    /// </summary>
    /// <seealso cref="Fornax.Net.Index.Common.IGrammable" />
    public sealed class KGram : IGrammable
    {

        private readonly string word;
        private int size;

        /// <summary>
        /// Initializes a new instance of the <see cref="NGram"/> class.
        /// Creates a n-gram collection of size 2.
        /// </summary>
        /// <param name="text">The text.</param>
        public KGram(string word) : this(word, 2) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="NGram" /> class.
        /// Creates a n-gram collection of size 2.
        /// </summary>
        /// <param name="token">The token.</param>
        public KGram(Token token) : this(token.Value ?? throw new ArgumentNullException(nameof(token)), 2) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NGram"/> class.
        /// Creates a n-gram collection of (<paramref name="size"/>).
        /// </summary>
        /// <param name="word">The text.</param>
        /// <param name="size">The size.</param>
        /// <exception cref="ArgumentNullException">text</exception>
        public KGram(string word, uint size) {
            Contract.Requires(word != null);
            this.word = word ?? throw new ArgumentNullException(nameof(word));
            this.size = (int)size;
        }

        public KGram(Token token, uint size) : this(token.Value ?? throw new ArgumentNullException(nameof(token)), size) { }

        IList<string> GetGrams() {
            List<string> list = new List<string>();
                string pterm = "$" + word + "$";

                int n = pterm.Length;

                if (size > n) {
                    throw new ArgumentOutOfRangeException("k");
                }

                for (int i = 0; i < n - size + 1; i++) {
                    list.Add(pterm.Substring(i, size));
                }
            return list;
        }

        /// <summary>
        /// Gets the K-grams of a specific text.
        /// </summary>
        /// <value>
        /// The grams.
        /// </value>
        public IEnumerable<string> Grams => GetGrams();

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>The word.</value>
        public string Word => word;

        /// <summary>
        /// Gets or sets the size of the grams.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Size { get => size; set => size = value; }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            return (obj is KGram) ? Collections.Equals(((KGram)(obj)).Grams, Grams) : false;
        }

        /// <summary>
        /// Returns a hash code/finger-print for this K-gram.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return (int)Adler32.Compute(word);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this KGram.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this KGram.
        /// </returns>
        public override string ToString() {
            return Collections.ToString(Grams);
        }
    }
}
