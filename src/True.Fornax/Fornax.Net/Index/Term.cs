// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-31-2017
// ***********************************************************************
// <copyright file="Term.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using System.Diagnostics.Contracts;
using Fornax.Net.Analysis.Tools;
using Fornax.Net.Common.Snowball.fr;

using Fornax.Net.Util.System;
using ProtoBuf;
using Token = Fornax.Net.Analysis.Tokenization.Token;

namespace Fornax.Net.Index
{
    /// <summary>
    /// A <see cref="Term" /> represents a word from text. This is the unit of search.  It is
    /// composed of two elements, the text of the word, as a string, and the stemmed token which is the term.
    /// <para />
    /// </summary>
    /// <seealso cref="System.IEquatable{Fornax.Net.Index.Term}" />
    /// <seealso cref="java.io.Serializable.__Interface" />
    [Serializable, ProtoContract]
    public sealed class Term : IEquatable<Term>, ICloneable, IComparable<Term>, java.io.Serializable.__Interface
    {
        [ProtoMember(1)]
        private readonly string term;
        [ProtoMember(2)]
        private readonly string token;

        private readonly Token _token;
        /// <summary>
        /// Initializes a new instance of the <see cref="Term" /> class.
        /// Specifying the language to use to convert token to term.
        /// </summary>
        /// <param name="word">The word to be stemmed.</param>
        /// <param name="language">The language.</param>
        /// <exception cref="ArgumentNullException">word</exception>
        public Term(string word, FornaxLanguage language)
        {
            Contract.Requires(word != null && language != null);
            if (word == null || language == null) throw new ArgumentNullException(nameof(word));

            token = word.ToLower();
            term = BuildTerm(language, word.ToLower());
            _token = new Token(0, word.Length, word);
        }

        private string BuildTerm(FornaxLanguage language, string word)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            if (language.IsEnglish)
            {
                var stemmer = new FornaxStemmer();
                return stemmer.StemWord(word);
            }
            else
            {
                var fstemmer = new FrenchStemmer();
                return fstemmer.Stem(word);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Term" /> class.
        /// Seciying the language to use to convert the token to stem.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="language">The language.</param>
        public Term(Token token, FornaxLanguage language) : this(token.Value, language)
        {
            _token = token;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Term)
            {
                return ((Term)(obj)).term == term;
            }
            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(Term other)
        {
            return term.Equals(other.term);
        }

        /// <summary>
        /// Returns a hash code for this term.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return term.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this term.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this term.</returns>
        public override string ToString()
        {
            return term;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.</returns>
        public int CompareTo(Term other)
        {
            return term.CompareTo(other.term);
        }

        public object Clone()
        {
            return term.Clone();
        }

        /// <summary>
        /// Gets the relative token value of the term.
        /// </summary>
        /// <value>The token.</value>
        public Token Token => _token;
    }
}
