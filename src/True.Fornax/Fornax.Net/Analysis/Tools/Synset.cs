// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-29-2017
// ***********************************************************************
// <copyright file="Synset.cs" company="Microsoft">
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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

using Fornax.Net.Index.Common;
using Fornax.Net.Util.Collections;
using Fornax.Net.Util.Text;
using ProtoBuf;

namespace Fornax.Net.Analysis.Tools
{
    /// <summary>
    /// Represents a Synonym set for a given word retrieved from a synonym index.
    /// NOTE: If Synonym Index isnt provided and there be no existing index,
    /// the defaut @<see cref="SynsetFactory.Default" /> would be used.
    /// </summary>
    /// <remarks>Usage: Synset synonyms = SynsetFactory.GetSynset("word");</remarks>
    [Serializable, ProtoContract]
    [Progress("Synset", false, Documented = true, Tested = true)]
    public sealed class Synset
    {
        readonly SynsetIndex index;
        IEnumerable<string> synoyms;
        string primeWord;

        private readonly uint? MaxDepth = 64;
        private uint? depthOfexpansion;

        /// <summary>
        /// Initializes a new instance of the <see cref="Synset"/> class.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="doe">The doe.</param>
        internal Synset(string word, uint? doe = null) : this(word, SynsetFactory.Default, doe) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Synset"/> class.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="index">The index.</param>
        /// <param name="doe">The doe.</param>
        /// <exception cref="ArgumentNullException">
        /// word
        /// or
        /// index
        /// </exception>
        internal Synset(string word, SynsetIndex index, uint? doe = null) {
            Contract.Requires(word != null && index != null);

            primeWord = word.Trim() ?? throw new ArgumentNullException(nameof(word));
            this.index = index ?? throw new ArgumentNullException(nameof(index));
            depthOfexpansion = ((doe < 1) ? 1 : doe) ?? MaxDepth;
        }

        /// <summary>
        /// Gets the current prime word of the <see cref="Synset" />.
        /// </summary>
        /// <value>The word at which the synonyms would be returned.</value>
        public string Word => primeWord;

        /// <summary>
        /// Gets the synonyms of the current prime word(<see cref="Word" />).
        /// </summary>
        /// <value>The synonyms of a word.</value>
        public IEnumerable<string> Synonyms => synoyms = Get();

        private ISet<string> Get() {
            var syns = from ln in index
                       where Regex.IsMatch(ln, string.Format($"[\\W]+{primeWord}[\\W]+"))
                       select ln;

            StringTokenizer tokenizer; ISet<string> synonyms = new HashSet<string>();
            uint count = 0;
            foreach (var word in syns) {
                if (count >= depthOfexpansion) {
                    break;
                }
                tokenizer = new StringTokenizer(word, " ,()\n\t\r\f");
                while (tokenizer.HasMoreTokens()) {
                    synonyms.Add(tokenizer.CurrentToken);
                }
                count++;
            }
            return synonyms;
        }

        /// <summary>
        /// Sets the specified new word as prime word for the new synonym set, of
        /// which the synonyms would be returned.
        /// </summary>
        /// <param name="newWord">The new word.</param>
        public void Set(string newWord) {
            primeWord = newWord.Trim();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance of synonym set.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance of synonym set.</returns>
        public override string ToString() {
            return Collections.ToString(Synonyms);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this <see cref="Synset" />.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this <see cref="Synset" />.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) {
            if (obj is Synset) {
                var sn = obj as Synset;
                return Collections.Equals(sn.primeWord, primeWord) && Collections.Equals(sn.Synonyms, Synonyms);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this set of synonyms.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() {
            return Collections.GetHashCode(Synonyms);
        }
    }
}
