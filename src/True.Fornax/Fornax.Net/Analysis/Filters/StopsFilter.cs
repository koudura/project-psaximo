// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-01-2017
// ***********************************************************************
// <copyright file="StopsFilter.cs" company="True.inc">
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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Util.System;
using Fornax.Net.Util.Text;

namespace Fornax.Net.Analysis.Filters
{
    /// <summary>
    /// Filter provider for removal of stop words from a collection of text.
    /// </summary>
    /// <seealso cref="Filter" />
    [Serializable]
    public sealed class StopsFilter : Filter
    {
        private static FornaxLanguage _language;
        private static Vocabulary _vocabs;

        /// <summary>
        /// Initializes a new instance of the <see cref="StopsFilter" /> class with an input
        /// text to be filtered.
        /// </summary>
        /// <param name="text">The text.</param>
        public StopsFilter(string text) : base(text) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StopsFilter" /> class with
        /// an input text to be filtered and language of the text, by which fornax removes the
        /// stops from the text.
        /// </summary>
        /// <param name="lang">The language rule used to extract stops from text collection.</param>
        /// <param name="text">The text to be filtered.</param>
        /// <exception cref="ArgumentNullException">language</exception>
        public StopsFilter(FornaxLanguage lang, string text) : base(text) {
            Contract.Requires(lang != null);
            _language = lang ?? throw new ArgumentNullException(nameof(lang));
            _vocabs = ConfigFactory.GetVocabulary(lang);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StopsFilter" /> class
        /// which serves as an initiator to handle input collection of text by underlying metod calls.
        /// </summary>
        /// <param name="language">The language rule used to extract stops from text collection.</param>
        public StopsFilter(FornaxLanguage language) : this(language, string.Empty) { }

        /// <summary>
        /// Uses the given language rule to remove stop words from the input text-collection.
        /// </summary>
        /// <param name="collection">The collection of string-text to be filtered.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public override IEnumerable<string> Accepts(IEnumerable<string> collection) {
            Contract.Requires(collection != null);
            foreach (var item in collection) {
                if (!IsStop(item)) {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Removes all stop words from the input text after tokenizing the text by using the
        /// input delimiter characters.
        /// </summary>
        /// <param name="text">The solid text input to be tokenized an filtered.</param>
        /// <param name="delimiters">The delimiters to be used for tokenization.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
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
        /// Determines whether the specified word is stop word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>
        ///   <c>true</c> if the specified word is stop; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStop(string word) {
            return _vocabs.StopWords.Contains(word);
        }

        /// <summary>
        /// Filters the curent state input text by tokenizing the text using
        /// the input delimiter characters.
        /// </summary>
        /// <param name="delimiters">The delimiters to be used for tokenization.</param>
        /// <returns>System.String representtion of the de-stopped text.</returns>
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
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() {
            return base.ToString();
        }

        /// <summary>
        /// Filters out the stop words from the collection using <paramref name="language"/> rule.
        /// </summary>
        /// <param name="collection">The collection to be filtered.</param>
        /// <param name="language">The language rule used for filtering.</param>
        /// <returns>A filtered enumerable collection of input <paramref name="collection" />.</returns>
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
        /// <returns>filtered TokenStream.</returns>
        public override TokenStream Accepts(TokenStream tokens) {
            IList<Token> newtokenns = new List<Token>();
            while (tokens.MoveNext()) {
                var now = tokens.Current;
                if (!IsStop(now.Value.ToLower()) && now != null) {
                    newtokenns.Add(now);
                }
            }
            return new TokenStream(newtokenns);
        }
    }
}
