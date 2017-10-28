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
using Fornax.Net.Util.Text;
using ProtoBuf;

namespace Fornax.Net.Analysis.Tokenization
{
    /// <summary>
    /// Text Tokenizer object for Pre/Post Processing of string text.
    /// </summary>
    /// <seealso cref="Fornax.Net.Util.Text.ITokenizer" />
    [Serializable, ProtoContract]
    public abstract class Tokenizer : ITokenizer
    {
        [ProtoMember(1)]
        protected internal string text;
        [ProtoMember(2)]
        protected internal bool returnDelim1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tokenizer"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="returnDelim1">if set to <c>true</c> [return delim1].</param>
        protected Tokenizer(string text, bool returnDelim1) : this(text) {
            this.returnDelim1 = returnDelim1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tokenizer"/> class.
        /// </summary>
        /// <param name="text">The text to be tokenized.</param>
        protected Tokenizer(string text) {
            this.text = text;
            returnDelim1 = false;
        }

        protected Tokenizer() { text = string.Empty ; }

        /// <summary>
        /// Returns the value as the <code>NextToken</code> method, except that its declared value is
        /// <see cref="object"/> rather than <see cref="string"/>.
        /// </summary>
        /// <returns>the next token in the string. <seealso cref="ITokenizer"/> , <seealso cref="CurrentToken"/></returns>
        public abstract object CurrentElement { get; }

        /// <summary>
        /// Returns the current token from this whitespace tokenizer.
        /// </summary>
        /// <returns>the current token from this whitespace tokenizer.</returns>
        public abstract string CurrentToken { get; }

        /// <summary>
        /// Calculates the number of times that this tokenizer's <code>NextToken</code> method can be called before
        /// it generates an exception. The current position is not advanced.
        /// </summary>
        /// <returns>the number of tokens remaining in the string using the current delimiter set. <seealso cref="CurrentToken"/>.</returns>
        public abstract int CountTokens();

        /// <summary>
        /// Returns the same value as the <see cref="HasMoreTokens()"/> method.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if there are more tokens; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool HasMoreElements();

        /// <summary>
        /// Tests if there are more tokens available from this tokenizer's string.
        /// If this method returns <c>true</c>, then a subsequent call to <see cref="NextToken"/> 
        /// wil successfully return a token.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if and only if there is at least one token in the string after the current position
        ///   ; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool HasMoreTokens();

        /// <summary>
        /// Gets the tokenstream of tokens.
        /// </summary>
        /// <returns>Token stream of tokens by this tokenizer.</returns>
        public abstract TokenStream GetTokens();

    }
}
