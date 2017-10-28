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
    /// Safe words filter providers for fornax.net
    /// </summary>
    /// <seealso cref="Fornax.Net.Analysis.Filters.Filter" />
    [Serializable]
    public class SafetyFilter : Filter
    {
        static FornaxLanguage fornaxLanguage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafetyFilter"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="text">The text.</param>
        public SafetyFilter(FornaxLanguage language, string text) : base(text) {
            Contract.Requires(language != null);
            fornaxLanguage = language;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafetyFilter"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public SafetyFilter(string text) : this(FornaxLanguage.English, text) {

        }

        private static Vocabulary Vocabs => ConfigFactory.GetVocabulary(fornaxLanguage);

        /// <summary>
        /// Filters the specified collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public override IEnumerable<string> Accepts(IEnumerable<string> collection) {
            foreach (var word in collection) {
                if (!IsUnsafeWord(word)) {
                    yield return word;
                }
            }
        }

        /// <summary>
        /// Filters the tokenized by <paramref name="delimiters" /> tewxt.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns></returns>
        public override IEnumerable<string> Accepts(string text, char[] delimiters) {
            var tokenizer = new StringTokenizer(text, new string(delimiters));
            while (tokenizer.HasMoreTokens()) {
                var token = tokenizer.CurrentToken;
                if (!IsUnsafeWord(token)) {
                    yield return token;
                }
            }
        }

        /// <summary>
        /// Determines whether word is unsafe or a blacklisted word or not.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>
        ///   <c>true</c> if word is unsafe; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUnsafeWord(string word) {
            return Vocabs.BadWords.Contains(word.ToLower());
        }

        /// <summary>
        /// Acceptses the specified delimiters.
        /// </summary>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns></returns>
        public override string Accepts(char[] delimiters) {
            StringBuilder output = new StringBuilder();
            var tokenizer = new StringTokenizer(text, new string(delimiters));
            while (tokenizer.HasMoreTokens()) {
                var token = tokenizer.CurrentToken;
                if (!IsUnsafeWord(token)) {
                    output.Append(token + " ");
                }
            }
            return output.ToString().Trim();
        }

        /// <summary>
        /// Filters the specified collection with a language specific rule.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// A filtered enumerable collection of input <paramref name="collection" />.
        /// </returns>
        public override IEnumerable<string> Accepts(IEnumerable<string> collection, FornaxLanguage language) {
            var badWords = ConfigFactory.GetVocabulary(language).BadWords;
            foreach (var item in collection) {
                if (!badWords.Contains(item)) {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Filters the specified Tokenstream by using default language specified bad words.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        public override TokenStream Accepts(TokenStream tokens) {
            IList<Token> newtokenns = new List<Token>();
            while (tokens.MoveNext()) {
                var now = tokens.Current;
                if (!IsUnsafeWord(now.Value)) {
                    newtokenns.Add(now);
                }
            }
            tokens.Reset();
            return new TokenStream(newtokenns);
        }
    }
}
