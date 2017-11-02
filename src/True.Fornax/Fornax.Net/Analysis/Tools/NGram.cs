// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-30-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-31-2017
// ***********************************************************************
// <copyright file="Ngram.cs" company="True.Inc">
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
using System.Text.RegularExpressions;

using Fornax.Net.Index.Common;
using Fornax.Net.Util.Collections;
using Fornax.Net.Util.Security.Cryptography;

namespace Fornax.Net.Analysis.Tools
{
    /// <summary>
    /// N-Gram representation of string text.
    /// </summary>
    /// <seealso cref="IGram" />
    /// <seealso cref="java.io.Serializable.__Interface" />
    [Serializable]
    [Progress("Ngram", true, Documented = true, Tested = true)]
    public sealed class Ngram : IGram, java.io.Serializable.__Interface
    {
        private readonly string _text;
        private readonly string[] _grams;
        private static uint _size;
        private static int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ngram" /> class.
        /// Creates a n-gram collection of a specific size(unsigned-integer).
        /// The [NgramModel] represents the n-gram model of specification.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="size">The size of the n-gram.</param>
        /// <param name="model">The model specification of the n-gram.</param>
        /// <param name="Isbounded">if set to <c>true</c> A boundary marker [#] is set to the trailing and end bounds of the input
        /// string which reflects in the n-gram.(i.e #text# =&gt; {#t, te, ex, xt, t#}.)</param>
        /// <exception cref="ArgumentNullException">text</exception>
        public Ngram(string text, uint size, NgramModel model, bool Isbounded = false)
        {
            Contract.Requires(text != null);

            _text = text ?? throw new ArgumentNullException(nameof(text));
            _size = size;
            _grams = ConstructGrams(text, model, Isbounded);
            _count = _grams.Length;
        }

        private static string[] ConstructGrams(string text, NgramModel model, bool isbounded)
        {
            if (isbounded) { return ConstructGrams("#" + text + "#", model); }
            return ConstructGrams(text, model);
        }

        private static string[] ConstructGrams(string v, NgramModel model)
        {
            switch (model)
            {
                case NgramModel.Character:
                    return ConstructGrams(v.ToCharArray(), true);
                default: return ConstructGrams(Regex.Split(v, "\\s+"), false);
            }
        }

        private static string[] ConstructGrams<T>(T[] d_str, bool isChar)
        {
            int n = d_str.Length;
            _size = (_size > n) ? (uint)n : _size;

            var captures = new HashSet<string>();
            for (int i = 0; i < n - _size + 1; i++)
            {
                var ctor = new StringBuilder();
                for (int j = i; j < i + _size; j++)
                {
                    ctor.AppendFormat(((isChar) ? "{0}" : "{0} "), d_str[j]);
                }
                captures.Add(ctor.ToString().TrimEnd());
            }
            return captures.ToArray();
        }

        /// <summary>
        /// Gets the n-grams of a specific text.
        /// </summary>
        /// <value>The grams.</value>
        public IEnumerable<string> Grams => _grams;

        /// <summary>
        /// Gets the size of the n-gram.
        /// where n = Size.
        /// </summary>
        /// <value>The size.</value>
        public uint Size => _size;

        /// <summary>
        /// Gets the raw untokenized text of the n-gram.
        /// </summary>
        /// <value>The text.</value>
        public string Text => _text;

        /// <summary>
        /// Gets the number of produced n-grams.
        /// </summary>
        /// <value>The count.</value>
        public int Count => _count;

        /// <summary>
        /// Returns a <see cref="string" /> that represents this NGram
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this NGram.</returns>
        public override string ToString()
        {
            return Collections.ToString(Grams);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this Ngram.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return (obj is Ngram) ? Collections.Equals(((Ngram)(obj)).Grams, Grams) : false;
        }

        /// <summary>
        /// Returns a hash code/finger-print for this N-Gram.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (int)Adler32.Compute(ToString());
        }
    }
}
