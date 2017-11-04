// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-31-2017
// ***********************************************************************
// <copyright file="Soundex.cs" company="True.Inc">
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
using System.Linq;
using System.Text;

using Fornax.Net.Util.Collections;
using Fornax.Net.Util.Text;
using ProtoBuf;
using Token = Fornax.Net.Analysis.Tokenization.Token;

namespace Fornax.Net.Analysis.Tools
{
    /// <summary>
    /// Soundex Code object.
    /// Used for phonetic query matching and mapping.
    /// </summary>
    [Serializable, ProtoContract]
    [Progress("Soundex", true, Documented = true, Tested = true)]
    public sealed class Soundex : IComparable<Soundex>
    {

        private string token_word;
        private string value;

        /// <summary>
        /// Gets the word corresponding to the soundex value.
        /// </summary>
        /// <value>The word.</value>
        public string Word => token_word;

        /// <summary>
        /// Gets the soundex code of <see cref="Word" />.
        /// </summary>
        /// <value>The value.</value>
        public string Value => value = Generate();

        /// <summary>
        /// Initializes a new instance of the <see cref="Soundex"/> class.
        /// </summary>
        /// <param name="word">The word.</param>
        internal Soundex(Token word) : this(word.Value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Soundex"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <exception cref="ArgumentException">token</exception>
        internal Soundex(string token)
        {
            Contract.Requires(token != null && token.IsWord());
            if (token == null) throw new ArgumentException($"{nameof(token)} is not a valid word");

            token_word = (token.IsWord()) ? token.ToUpper() : "";
        }

        /// <summary>
        /// Generates this the soundex code for a word.
        /// </summary>
        /// <returns>the 4-char soundex code for <see cref="Word"/>.</returns>
        private string Generate()
        {
            char[] text = token_word.ToCharArray();

            //step 1 - map to soundex table
            for (int i = 1; i < text.Length; i++)
            {
                text[i] = Digit(text[i]);
            }

            //step 2- remove dualities
            StringBuilder temp_text = new StringBuilder();
            temp_text.Append(text[0]);
            for (int i = 1; i < text.Length - 1; i++)
            {
                int j = i; char rep = text[i];
                while ((j < text.Length - 1) && text[j] == text[j + 1])
                {
                    rep = text[j];
                    j++;
                }
                i = j;
                temp_text.Append(rep);
            }

            //step3- shift zeroes
            var temp = temp_text.ToString().ToCharArray();
            Arrays.RemoveAll(ref temp, '0');
            var f = new string(temp).PadRight(5, '0');


            //step4 - return substring of 1st 4 chars        
            return f.Substring(0, 4);
        }

        #region table
        private static readonly char[] vowels = new char[] { 'A', 'E', 'I', 'O', 'U', 'H', 'W', 'Y' };

        private static readonly char[] class1 = new char[] { 'B', 'F', 'P', 'V' };

        private static readonly char[] class2 = new char[] { 'C', 'G', 'J', 'K', 'Q', 'S', 'X', 'Z' };

        private static readonly char[] class3 = new char[] { 'D', 'T' };

        private static readonly char[] class4 = new char[] { 'L' };

        private static readonly char[] class5 = new char[] { 'M', 'N' };

        private static readonly char[] class6 = new char[] { 'R' };

        /// <summary>
        /// The soundex table
        /// </summary>
        internal static readonly Dictionary<char, char[]> SoundexTable = new Dictionary<char, char[]> { { '0', vowels }, { '1', class1 }, { '2', class2 }, { '3', class3 }, { '4', class4 }, { '5', class5 }, { '6', class6 } };

        #endregion

        private char Digit(char ch)
        {
            ch = Char.ToUpper(ch);
            var classes = from vals in SoundexTable
                          where vals.Value.Contains(ch)
                          select vals.Key;
            return classes.FirstOrDefault();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"Soundex[{Word} :: {Value}]";
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (obj is Soundex)
            {
                return (((Soundex)obj).value.Equals(value));
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.</returns>
        public int CompareTo(Soundex other)
        {
            return Value.CompareTo(other.Value);

        }
    }
}
