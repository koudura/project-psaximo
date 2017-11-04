// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-31-2017
// ***********************************************************************
// <copyright file="Token.cs" company="True.Inc">
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
using System.Text.RegularExpressions;

using Fornax.Net.Analysis.Tools;
using Fornax.Net.Index;
using Fornax.Net.Index.Common;
using Fornax.Net.Util.Text;
using ProtoBuf;

using Adler = Fornax.Net.Util.Security.Cryptography.Adler32;
using Language = Fornax.Net.Util.System.FornaxLanguage;

/// <summary>
/// The Tokenization namespace.
/// </summary>
namespace Fornax.Net.Analysis.Tokenization
{
    /// <summary>
    /// Represents a token in a filed of text, stores the attributes and position of a token in a field.
    /// position of string can be used later in highlighting of token in field.
    /// Note: A <see cref="Token" /> may represent more than words from text fields, but also
    /// things like dates, email addresses, urls, etc.
    /// </summary>
    [Serializable, ProtoContract]
    [Progress("Token", true, Documented = true, Tested = true)]
    public sealed class Token
    {

        private readonly string value;
        private int startoffset;
        private int endoffset;
        private TokenAttribute type;
        private ulong flags;
        private int _length;

        /// <summary>
        /// Gets the value of the token.
        /// </summary>
        /// <value>The value.</value>
        internal string Value => value;

        /// <summary>
        /// Gets the end positional index of token in field.
        /// </summary>
        /// <value>The end.</value>

        public int End => endoffset;

        /// <summary>
        /// Gets the start positional index of token in field.
        /// </summary>
        /// <value>The start.</value>

        public int Start => startoffset;

        /// <summary>
        /// Gets the type attribute of the token.
        /// </summary>
        /// <value>The type.</value>

        public TokenAttribute Type => type;

        /// <summary>
        /// Gets the flags hash of the token.
        /// </summary>
        /// <value>The flag.</value>

        public ulong Flag => flags;

        /// <summary>
        /// Gets the length of the token.
        /// </summary>
        /// <value>The length.</value>
        public int Length => _length;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token" /> class.
        /// </summary>
        /// <param name="start">The start index.</param>
        /// <param name="text">The text.</param>
        /// <param name="fullText">The full text.</param>
        /// <param name="type">The type.</param>
        public Token(int start, string text, string fullText, TokenAttribute type = TokenAttribute.Unknown)
        {
            startoffset = start;
            endoffset = start + text.Length - 1;
            value = text;
            flags = Adler.Compute(value);
            this.type = (type.Equals(TokenAttribute.Unknown)) ? DetectType(value) : type;
            _length = value.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token" /> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        /// <param name="fullText">The full text.</param>
        /// <param name="type">The type.</param>
        public Token(int start, int length, string fullText, TokenAttribute type = TokenAttribute.Unknown)
        {
            startoffset = start;
            endoffset = start + length - 1;
            value = fullText.Substring(start, length);
            flags = Adler.Compute(value);
            this.type = (type.Equals(TokenAttribute.Unknown)) ? DetectType(value) : type;
            _length = value.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        internal Token(string token, string text, TokenAttribute type = TokenAttribute.Unknown)
        {
            startoffset = text.IndexOf(token); 
            endoffset = startoffset + token.Length - 1;
            value = token;
            flags = Adler.Compute(value);
            this.type = (type.Equals(TokenAttribute.Unknown)) ? DetectType(value) : type;
            _length = value.Length;
        }

        private TokenAttribute DetectType(string value)
        {
            TokenAttribute attrib = TokenAttribute.Unknown;
            if (Regex.IsMatch(value, @"^[A-Za-z ]\b$", RegexOptions.Compiled) && value.Length == 1)
            {
                attrib = TokenAttribute.Character;
            }
            else if (Regex.IsMatch(value, @"^[\d]+\.?\d*$", RegexOptions.Compiled))
            {
                attrib = TokenAttribute.Number;
            }
            else if (IsEmail(value))
            {
                attrib = TokenAttribute.Link;
            }
            else if (DateTime.TryParse(value, out DateTime res))
            {
                attrib = TokenAttribute.Date;
            }
            else if (value.IsWord() || Regex.IsMatch(value, @"^(?!.*(.)\1)[a-zA-Z][a-zA-Z\d_-]*$", RegexOptions.Compiled))
            {
                attrib = TokenAttribute.Word;
            }
            else if (Regex.IsMatch(value, @"^[A-Z]+[.][A-Z]+[.]$", RegexOptions.Compiled))
            {
                attrib = TokenAttribute.Acronym;
            }
            else if (IsOperator(value))
            {
                attrib = TokenAttribute.Operator;
            }
            else if (Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                attrib = TokenAttribute.Path;
            }
            return attrib;
        }

        /// <summary>
        /// Determines whether the specified string is operator.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns><c>true</c> if the specified string is operator; otherwise, <c>false</c>.</returns>
        internal static bool IsOperator(string str)
        {
            foreach (var ch in str)
            {
                if (Char.IsLetterOrDigit(ch) || Char.IsWhiteSpace(ch) || Char.IsSeparator(ch) || Char.IsNumber(ch))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the specified value is acronym.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is acronym; otherwise, <c>false</c>.</returns>
        internal static bool IsAcronym(string value)
        {
            return (Regex.IsMatch(value, @"([A-Z]+[.][A-Z]+[.])|([A-Z]+[.][A-Z]+[.][A-Z]+)|([A-Z]+[.][A-Z]+[.][A-Z]+[.][A-Z])", RegexOptions.IgnorePatternWhitespace));
        }

        /// <summary>
        /// Determines whether the specified value is email.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is email; otherwise, <c>false</c>.</returns>
        internal static bool IsEmail(string value)
        {
            return ((Regex.IsMatch(value, @"(^[\w]+@[\w]+[.][a-z0-9]+$)|(^[\w]+[.][\w]+[.][a-z0-9]+$)", RegexOptions.Compiled) ||
                (Regex.IsMatch(value, @"^[\w]+[.][a-z0-9]+$", RegexOptions.Compiled))));
        }

        /// <summary>
        /// Gets the ngram.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="model">The model.</param>
        /// <param name="Isbounded">if set to <c>true</c> [isbounded].</param>
        /// <returns>IGram.</returns>
        public IGram GetNgram(uint size, NgramModel model, bool Isbounded = false)
        {
            return new Ngram(value, size, model, Isbounded);
        }

        /// <summary>
        /// Returns a <see cref="string" /> representation of this <see cref="Token" />.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this <see cref="Token" /> instance.</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this token.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this token.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this token; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            return (obj is Token) ? ((Token)(obj)).value == value : false;
        }

        /// <summary>
        /// Returns a hash code for this token.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// Converts this token to a normalized term.
        /// A term includes the rooted,lemmatized or normalized form of a token.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>Term.</returns>
        public Term AsTerm(Language language)
        {
            return new Term(this, language);
        }
    }
}
