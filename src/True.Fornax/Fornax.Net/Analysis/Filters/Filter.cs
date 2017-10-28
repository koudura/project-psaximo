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
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Util.System;

namespace Fornax.Net.Analysis.Filters
{
    /// <summary>
    /// Fornax filters provider class.
    /// </summary>
    [Serializable]
    public abstract class Filter
    {

        protected string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        protected Filter(string text) {
            this.text = text;
        }

        protected Filter() { text = string.Empty; }

        /// <summary>
        /// Filter the specified collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public abstract IEnumerable<string> Accepts(IEnumerable<string> collection);

        /// <summary>
        /// Filters the tokenized by <paramref name="delimiters"/> tewxt.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns></returns>
        public abstract IEnumerable<string> Accepts(string text, char[] delimiters);

        /// <summary>
        /// Filters the specified collection with a language specific rule.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="language">The language.</param>
        /// <returns>A filtered enumerable collection of input <paramref name="collection"/>.</returns>
        public abstract IEnumerable<string> Accepts(IEnumerable<string> collection, FornaxLanguage language);

        /// <summary>
        /// Acceptses the specified delimiters.
        /// </summary>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns></returns>
        public abstract string Accepts(char[] delimiters);

        public abstract TokenStream Accepts(TokenStream tokens);

        /// <summary>
        /// Filters the specified input collection with a specified input
        /// filter set.
        /// </summary>
        /// <param name="inputCollection">The input collection.</param>
        /// <param name="filterSet">The filter set.</param>
        /// <returns>A filtered enumerable collection.</returns>
        public static IEnumerable<string> Accepts(IEnumerable<string> inputCollection, ISet<string> filterSet) {
            foreach (var item in inputCollection) {
                if (!filterSet.Contains(item)) {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return base.ToString();
        }
    }
}
