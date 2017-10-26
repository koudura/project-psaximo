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
using System.Text.RegularExpressions;

using Fornax.Net.Analysis.Tools;
using Fornax.Net.Util.Text;
using ProtoBuf;

using IGrammable = Fornax.Net.Index.Common.IGrammable;
using Adler = Fornax.Net.Util.Security.Cryptography.Adler32;

namespace Fornax.Net.Analysis.Tokenization
{
    /// <summary>
    /// Represents a token in a filed of text, stores the attributes and position of a token in a field.
    /// position of string can be used later in highlighting of token in field.
    /// </summary>
    [Serializable, ProtoContract]
    [Progress("Token", false, Documented = true, Tested = false)]
    public class Token
    {
        private readonly string value;
        private int startoffset;
        private int endoffset;
        private int length;
        private TokenAttribute type;
        private ulong flags;

        /// <summary>
        /// Gets the value of the token.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [ProtoMember(1)]
        internal string Value => value;

        /// <summary>
        /// Gets the end positional index of token in field.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        [ProtoMember(2)]
        public int End => endoffset;

        /// <summary>
        /// Gets the start positional index of token in field.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        [ProtoMember(3)]
        public int Start => startoffset;

        /// <summary>
        /// Gets the type attribute of the token.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [ProtoMember(4)]
        public TokenAttribute Type => type;

        /// <summary>
        /// Gets the flags hash of the token.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        [ProtoMember(5)]
        public ulong Flag => flags;

        /// <summary>
        /// Gets the length of the token.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [ProtoMember(6)]
        public int Length => Value.Length;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="fullText">The full text.</param>
        /// <param name="type">The type attribute of the token</param>
        public Token(int offset, string fullText, TokenAttribute type = TokenAttribute.Unknown) : this(offset, ToEnd(offset, fullText), fullText, type) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="start">The start index.</param>
        /// <param name="end">The end index inclusive.</param>
        /// <param name="fullText">The full text.</param>
        /// <param name="type">The type.</param>
        public Token(int start, int end, string fullText, TokenAttribute type = TokenAttribute.Unknown) {
            startoffset = start;
            endoffset = end;
            value = GetValue(fullText);
            flags = Adler.Compute(value);
            this.type = (type.Equals(TokenAttribute.Unknown)) ? DetectType(value) : type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end index inclusive</param>
        /// <param name="text">The text.</param>
        /// <param name="fullText">The full text.</param>
        /// <param name="type">The type.</param>
        internal Token(int start, int end, string text, string fullText, TokenAttribute type = TokenAttribute.Unknown) {
            startoffset = start;
            endoffset = end;
            value = text;
            flags = Adler.Compute(value);
            this.type = (type.Equals(TokenAttribute.Unknown)) ? DetectType(value) : type;
        }

        private TokenAttribute DetectType(string value) {
            TokenAttribute attrib = TokenAttribute.Unknown;
            if (Regex.IsMatch(value, @"[\s\S]", RegexOptions.Compiled) && value.Length == 1) {
                attrib = TokenAttribute.Character;
            }
            if (Regex.IsMatch(value, @"[\d]+[.]?[\d]+", RegexOptions.Compiled)) {
                attrib = TokenAttribute.Number;
            }
            if (Regex.IsMatch(value, @"[\w]+@[\w]+[.][\w]+", RegexOptions.Compiled)) {
                attrib = TokenAttribute.Email;
            }
            if (DateTime.TryParse(value, out DateTime res)) {
                attrib = TokenAttribute.Date;
            }
            if (value.IsWord() || Regex.IsMatch(value, @"[\w]+", RegexOptions.Compiled)) {
                attrib = TokenAttribute.Word;
            }
            if (Regex.IsMatch(value, @"[A-Z0-9]+[.][A-Z0-9]+[.][A-Z0-9]+", RegexOptions.Compiled)) {
                attrib = TokenAttribute.Acronym;
            }
            if (Regex.IsMatch(value, @"[\W]+", RegexOptions.Compiled)) {
                attrib = TokenAttribute.Operator;
            }
            if (Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute)) {
                attrib = TokenAttribute.Link;
            }
            return attrib;
        }

        private string GetValue(string fullText) {
            int flen = fullText.Length;
            return fullText.Substring(startoffset, flen - endoffset);
        }

        private static int ToEnd(int offset, string fullText) {
            int count = offset;
            for (int i = offset; i < fullText.Length; i++) {
                if (Char.IsWhiteSpace(fullText[i])) {
                    return i - 1;
                }
            }
            return fullText.Length;
        }

        /// <summary>
        /// Gets the K/N gram of the token.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public IGrammable GetGram(uint size) {
            IGrammable grammer;
            if (value.Contains(" ")) {
                grammer = new NGram(value, size);
            }else {
                grammer = new KGram(value, size);
            }
            return grammer;
        }

        /// <summary>
        /// Returns a <see cref="string" /> representation of this <see cref="Token"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this <see cref="Token"/> instance.
        /// </returns>
        public override string ToString() {
            return Value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this token.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this token.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this token; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj == this) return true;
            return (obj is Token) ? ((Token)(obj)).value == value : false;
        }

        /// <summary>
        /// Returns a hash code for this token.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return value.GetHashCode();
        }
    }
}
