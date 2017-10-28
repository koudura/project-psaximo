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
using System.Text;

using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Util.Resources;
using Fornax.Net.Util.System;
using Fornax.Net.Util.Text;

namespace Fornax.Net.Analysis.Filters
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Fornax.Net.Analysis.Filters.Filter" />
    [Serializable]
    public sealed class StopsFilter : Filter
    {
        private static FornaxLanguage language;

        /// <summary>
        /// Initializes a new instance of the <see cref="StopsFilter"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public StopsFilter(string text) : base(text) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StopsFilter"/> class.
        /// FornaxLanguage
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <param name="text">The text.</param>
        public StopsFilter(FornaxLanguage lang, string text) : base(text) {
            Contract.Requires(language != null);
            language = lang;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StopsFilter"/> class.
        /// </summary>
        public StopsFilter(FornaxLanguage language) : this(language, string.Empty) { }

        private static Vocabulary Vocabs => ConfigFactory.GetVocabulary(language);

        /// <summary>
        /// Acceptses the specified collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public override IEnumerable<string> Accepts(IEnumerable<string> collection) {
            foreach (var item in collection) {
                if (!IsStop(item)) {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Acceptses the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns></returns>
        public override IEnumerable<string> Accepts(string text, char[] delimiters) {
            var tokenizer = new StringTokenizer(text, new string(delimiters));
            while (tokenizer.HasMoreTokens()) {
                var token = tokenizer.CurrentToken;
                if (!IsStop(token)) {
                    yield return token;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified word is stop.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>
        ///   <c>true</c> if the specified word is stop; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsStop(string word) {
            return Vocabs.StopWords.Contains(word);
        }

        public override string Accepts(char[] delimiters) {
            StringBuilder output = new StringBuilder();
            var tokenizer = new StringTokenizer(text, new string(delimiters));
            while (tokenizer.HasMoreTokens()) {
                var token = tokenizer.CurrentToken;
                if (!IsStop(token)) {
                    output.Append(token + " ");
                }
            }
            return output.ToString().Trim();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return base.ToString();
        }

        /// <summary>
        /// Filters out the language specific stop words from the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public override IEnumerable<string> Accepts(IEnumerable<string> collection, FornaxLanguage language) {
            var stopWords = ConfigFactory.GetVocabulary(language).StopWords;
            foreach (var item in collection) {
                if (!stopWords.Contains(item)) {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Filters out the default stop words from the input Token stream;
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        public override TokenStream Accepts(TokenStream tokens) {
            IList<Token> newtokenns = new List<Token>();
            while (tokens.MoveNext()) {
                var now = tokens.Current;
                if (!IsStop(now.Value)) {
                    newtokenns.Add(now);
                }
            }
            tokens.Reset();
            return new TokenStream(newtokenns);
        }

    }
}
