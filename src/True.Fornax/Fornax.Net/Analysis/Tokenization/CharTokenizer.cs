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
using System.Text.RegularExpressions;

using Fornax.Net.Util.Text;
using ProtoBuf;
using Cst = Fornax.Net.Util.Constants;

namespace Fornax.Net.Analysis.Tokenization
{
    /// <summary>
    /// An explicit Character tokenizer that tokenizes a text using the common delimiters, symbols and operators.
    /// i.e ( ` ^ + = \ } { ; ] [ @ # \t \n \r \f).
    /// </summary>
    /// <seealso cref="Fornax.Net.Analysis.Tokenization.Tokenizer" />
    /// <seealso cref="Fornax.Net.Util.Text.ITokenizer" />
    [Serializable,ProtoContract]
    public sealed class CharTokenizer : Tokenizer, ITokenizer
    {
        [ProtoMember(3)]
        StringTokenizer tokenizer;
        [ProtoMember(4)]
        private static string operators = (Cst.GenOp_Brokers + Cst.QueryOP_Broker) + "@.-";

        /// <summary>
        /// Initializes a new instance of the <see cref="CharTokenizer"/> class.
        /// That does not return delimiters as tokens.
        /// </summary>
        /// <param name="text">The text to be tokenized.</param>
        public CharTokenizer(string text) : this(text, false) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharTokenizer"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="returnDelim">if set to <c>true</c> delimiters would be returned as tokens..</param>
        public CharTokenizer(string text, bool returnDelim) : base(text, returnDelim) {
            tokenizer = new StringTokenizer(text, operators, returnDelim);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharTokenizer"/> class.
        /// Sets the text to be tokenized to <c>string.Empty</c>
        /// Recommended reinitialization before real use.
        /// </summary>
        public CharTokenizer() : base() {
            new StringTokenizer(text);
        }

        /// <summary>
        /// Returns the value as the <code>NextToken</code> method, except that its declared value is
        /// <see cref="object" /> rather than <see cref="string" />.
        /// </summary>
        public override object CurrentElement => tokenizer.CurrentElement;

        /// <summary>
        /// Returns the current token from this whitespace tokenizer.
        /// </summary>
        public override string CurrentToken => tokenizer.CurrentToken;

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

        private IEnumerable<Token> Tokenize() {
            var esc_ops = string.Intern(string.Format("[^{0}]+", Regex.Escape(operators)));
            string regex = (returnDelim1) ? @"[\S]+" : @"[\w]+";
            var tokens = Regex.Matches(text, regex, RegexOptions.Compiled);
            foreach (Match exact in tokens) {
                int start = exact.Index;
                yield return new Token(start, exact.Length, text);
            }
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
            return new TokenStream(Tokenize());
        }
    }
}
