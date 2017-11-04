// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 11-03-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-03-2017
// ***********************************************************************
// <copyright file="PerFieldTokenizer.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************


using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Fornax.Net.Util;
using Fornax.Net.Util.Text;
using ProtoBuf;

/// <summary>
/// The Tokenization namespace.
/// </summary>
namespace Fornax.Net.Analysis.Tokenization
{
    /// <summary>
    /// Class PerFieldTokenizer. This class cannot be inherited.
    /// This Tokenizer inherits the operations of the bias and numeric tokenizer.
    /// being context-sensitive , tokenization of text a much more neater than the bias tokenizer.
    /// all numbers are also treated as delimiters for tokenization.
    /// </summary>
    /// <seealso cref="Fornax.Net.Analysis.Tokenization.Tokenizer" />
    /// <seealso cref="Tokenization.Tokenizer" />
    [Serializable, ProtoContract]
    public sealed class PerFieldTokenizer : Tokenizer
    {
        [ProtoMember(1)]
        StringTokenizer tokenizer;
        /// <summary>
        /// The delimiters
        /// </summary>
        [ProtoMember(2)]
        internal static string Delimiters = " .`^+={};<,/[]#\t\n\r\f\v&|\":*?%!>[]$~()0123456789\\@";

        /// <summary>
        /// Returns the value as the <code>NextToken</code> method, except that its declared value is
        /// <see cref="object" /> rather than <see cref="string" />.
        /// </summary>
        /// <value>The current element.</value>
        public override object CurrentElement => Filter(((string)tokenizer.CurrentElement));

        /// <summary>
        /// Returns the current token from this whitespace tokenizer.
        /// </summary>
        /// <value>The current token.</value>
        public override string CurrentToken => Filter(tokenizer.CurrentToken);

        /// <summary>
        /// Initializes a new instance of the <see cref="PerFieldTokenizer" /> class.
        /// Default delimiters are used, and delimiters are not returned as string.
        /// </summary>
        /// <param name="text">The text to be tokenized.</param>
        public PerFieldTokenizer(string text) : this(text, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerFieldTokenizer" /> class.
        /// Default delimiters are used.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="returnDelim">if set to <c>true</c> delimiters are returned as token.</param>
        public PerFieldTokenizer(string text, bool returnDelim) : this(text, Delimiters, returnDelim)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerFieldTokenizer" /> class.
        /// Sets the text to be tokenized to <c>string.Empty</c>
        /// Recommended reinitialization before real use.
        /// </summary>
        public PerFieldTokenizer() : base()
        {
            tokenizer = new StringTokenizer(text);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerFieldTokenizer" /> class.
        /// NOTE: The delimiters must be properly stated in line with the rules of
        /// a context-sensitive grammar.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <param name="returnDelim">if set to <c>true</c> [return delimiter].</param>
        public PerFieldTokenizer(string text, string delimiters, bool returnDelim) : base(text, returnDelim)
        {
            Delimiters = delimiters;
            tokenizer = new StringTokenizer(text, Delimiters, returnDelim);
        }

        /// <summary>
        /// Calculates the number of times that this tokenizer's <code>NextToken</code> method can be called before
        /// it generates an exception. The current position is not advanced.
        /// </summary>
        /// <returns>the number of tokens remaining in the string using the current delimiter set. <seealso cref="CurrentToken" />.</returns>
        public override int CountTokens()
        {
            return tokenizer.CountTokens();
        }

        /// <summary>
        /// Returns the same value as the <see cref="HasMoreTokens()" /> method.
        /// </summary>
        /// <returns><c>true</c> if there are more tokens; otherwise, <c>false</c>.</returns>
        public override bool HasMoreElements()
        {
            return tokenizer.HasMoreElements();
        }

        /// <summary>
        /// Tests if there are more tokens available from this tokenizer's string.
        /// If this method returns <c>true</c>, then a subsequent call to <see cref="NextToken" />
        /// wil successfully return a token.
        /// </summary>
        /// <returns><c>true</c> if and only if there is at least one token in the string after the current position
        /// ; otherwise, <c>false</c>.</returns>
        public override bool HasMoreTokens()
        {
            return tokenizer.HasMoreTokens();
        }

        /// <summary>
        /// Gets the tokenstream of tokens .
        /// </summary>
        /// <returns>Token stream of tokens by this tokenizer.</returns>
        public override TokenStream GetTokens()
        {
            return new TokenStream(Tokenizer());
        }

        /// <summary>
        /// Tokenizes this instance.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Token> Tokenizer()
        {
            string regex = (returnDelim1) ? @"[\S]+" : @"[A-Za-z_]+";
            var strbuilder = new StringBuilder();
            var tkner = new PerFieldTokenizer(text, Delimiters, returnDelim1);

            while (tkner.HasMoreTokens())
            {
                var str = tkner.CurrentToken;
                strbuilder.Append(str).Append(" ");
            }
            var nestr = strbuilder.ToString().Trim();

            var tokens = Regex.Matches(nestr, regex, RegexOptions.Compiled);
            foreach (Match exact in tokens)
            {
                int start = exact.Index;
                yield return new Token(start, exact.Length, nestr);
            }
        }

        /// <summary>
        /// Filters the specified WORD.
        /// </summary>
        /// <param name="wrd">The WRD.</param>
        /// <returns></returns>
        static string Filter(string wrd)
        {
            wrd = wrd.Trim(Constants.DocOP_Broker.ToCharArray());
            string wrrrd = Regex.Replace(wrd, @"[\-\']+", "");
            return wrrrd;
        }
    }
}
