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
using System.Text;
using System.Text.RegularExpressions;

using Fornax.Net.Util;
using Fornax.Net.Util.Text;
using ProtoBuf;

namespace Fornax.Net.Analysis.Tokenization
{
    /// <summary>
    /// Bias (Context-Sensitive) tokenizer that tokenizes a body of text with respect to the form and
    /// type of text.
    /// </summary>
    /// <seealso cref="Fornax.Net.Analysis.Tokenization.Tokenizer" />
    /// <seealso cref="Fornax.Net.Util.Text.ITokenizer" />
    [Serializable, ProtoContract]
    public sealed class BiasTokenizer : Tokenizer, ITokenizer
    {
        [ProtoMember(3)]
        StringTokenizer tokenizer;
        [ProtoMember(4)]
        internal static string Delimiters = Constants.GenOp_Brokers + Constants.QueryOP_Broker;

        /// <summary>
        /// Returns the value as the <code>NextToken</code> method, except that its declared value is
        /// <see cref="object" /> rather than <see cref="string" />.
        /// </summary>
        public override object CurrentElement => Filter(((string)tokenizer.CurrentElement).ToLower());

        /// <summary>
        /// Returns the current token from this whitespace tokenizer.
        /// </summary>
        public override string CurrentToken => Filter(tokenizer.CurrentToken.ToLower());

        /// <summary>
        /// Initializes a new instance of the <see cref="BiasTokenizer"/> class.
        /// Default delimiters are used, and delimiters are not returned as string.
        /// </summary>
        /// <param name="text">The text to be tokenized.</param>
        public BiasTokenizer(string text) : this(text, false) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiasTokenizer"/> class.
        /// Default delimiters are used.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="returnDelim">if set to <c>true</c> delimiters are returned as token.</param>
        public BiasTokenizer(string text, bool returnDelim) : this(text, Delimiters, returnDelim) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiasTokenizer"/> class.
        /// Sets the text to be tokenized to <c>string.Empty</c>
        /// Recommended reinitialization before real use.
        /// </summary>
        public BiasTokenizer() : base() {
            tokenizer = new StringTokenizer(text);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiasTokenizer"/> class.
        /// NOTE: The delimiters must be properly stated in line with the rules of 
        /// a context-sensitive grammar.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <param name="returnDelim">if set to <c>true</c> [return delimiter].</param>
        public BiasTokenizer(string text, string delimiters, bool returnDelim) : base(text, returnDelim) {
            Delimiters = delimiters;
            tokenizer = new StringTokenizer(text, Delimiters, returnDelim);
        }

        /// <summary>
        /// Calculates the number of times that this tokenizer's <code>NextToken</code> method can be called before
        /// it generates an exception. The current position is not advanced.
        /// </summary>
        /// <returns>
        /// the number of tokens remaining in the string using the current delimiter set. <seealso cref="CurrentToken" />.
        /// </returns>
        public override int CountTokens() {
            return tokenizer.CountTokens();
        }

        /// <summary>
        /// Returns the same value as the <see cref="HasMoreTokens()" /> method.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if there are more tokens; otherwise, <c>false</c>.
        /// </returns>
        public override bool HasMoreElements() {
            return tokenizer.HasMoreElements();
        }

        /// <summary>
        /// Tests if there are more tokens available from this tokenizer's string.
        /// If this method returns <c>true</c>, then a subsequent call to <see cref="NextToken" />
        /// wil successfully return a token.
        /// </summary>
        /// <returns>
        /// <c>true</c> if and only if there is at least one token in the string after the current position
        /// ; otherwise, <c>false</c>.
        /// </returns>
        public override bool HasMoreTokens() {
            return tokenizer.HasMoreTokens();
        }

        /// <summary>
        /// Gets the tokenstream of tokens.
        /// </summary>
        /// <returns>
        /// Token stream of tokens by this tokenizer.
        /// </returns>
        public override TokenStream GetTokens() {
            return new TokenStream(Tokenizer());
        }

        /// <summary>
        /// Tokenizers this instance.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Token> Tokenizer() {
            string regex = (returnDelim1) ? @"[\S]+" : @"[A-Za-z0-9_@\p{Pd}\'\.]+";
            var strbuilder = new StringBuilder();
            var tkner = new StringTokenizer(text, Delimiters, returnDelim1);
            
            while (tkner.HasMoreTokens()) {
                var str = tkner.CurrentToken;
                strbuilder.Append(Filter(str)).Append(" ");
            }
            var nestr = strbuilder.ToString().Trim();
            Console.WriteLine(nestr);

            var tokens = Regex.Matches(nestr, regex, RegexOptions.Multiline);
            foreach (Match exact in tokens) {
                int start = exact.Index;
                yield return new Token(start, exact.Length, text);
            }
        }

        /// <summary>
        /// Filters the specified WORD.
        /// </summary>
        /// <param name="wrd">The WRD.</param>
        /// <returns></returns>
        static string Filter(string wrd) {
            wrd = wrd.Trim(Constants.DocOP_Broker.ToCharArray());
            string wrrrd = Regex.Replace(wrd, @"[\-\']+", "");
            if (wrrrd.Contains(".") && !double.TryParse(wrrrd, out double n)) {
                if (!Token.IsEmail(wrrrd) || !Token.IsAcronym(wrrrd)) {
                    wrrrd = Regex.Replace(wrrrd, "[.]+", " ");
                }
            }
            return wrrrd;
        }
    }
}
