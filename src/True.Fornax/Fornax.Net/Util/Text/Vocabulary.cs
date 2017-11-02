// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Habuto Koudura
// Created          : 10-03-2017
//
// Last Modified By : Habuto Koudura
// Last Modified On : 10-28-2017
// ***********************************************************************
// <copyright file="Vocabulary.cs" company="Microsoft">
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

using Fornax.Net.Properties;
using Fornax.Net.Util.System;

/// <summary>
/// The Resources namespace.
/// </summary>
namespace Fornax.Net.Util.Text
{
    /// <summary>
    /// Fornax Vocabulary Handler.
    /// </summary>
    [Serializable]
    public sealed class Vocabulary
    {
        /// <summary>
        /// The language
        /// </summary>
        static FornaxLanguage language;

        private static readonly ISet<string> en_stops;
        private static readonly ISet<string> en_bads;
        private static readonly ISet<string> en_dict;

        private static readonly ISet<string> fr_bads;
        private static readonly ISet<string> fr_dict;
        private static readonly ISet<string> fr_stops;



        /// <summary>
        /// Initializes a new instance of the <see cref="Vocabulary"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <exception cref="FornaxException">culture</exception>
        internal Vocabulary(FornaxLanguage culture)
        {
            Contract.Requires(culture != null);
            language = culture ?? throw new FornaxException($"{nameof(culture)} is not-supported.");

        }

        /// <summary>
        /// Gets the dictionary. i.e a comprehensive set of words from a
        /// atandard dictionary of the specified language.S
        /// </summary>
        /// <value>The dictionary.</value>
        public ISet<string> Dictionary => (language.IsEnglish) ? en_dict : fr_dict;

        /// <summary>
        /// Gets the stop words. i.e a set of stop words by fornax for the specified
        /// language.
        /// </summary>
        /// <value>The stop words.</value>
        public ISet<string> StopWords => (language.IsEnglish) ? en_stops : fr_stops;

        /// <summary>
        /// Gets the bad words. i.e the blacklisted words for the specified
        /// language.
        /// </summary>
        /// <value>The bad words.</value>
        public ISet<string> BadWords => (language.IsEnglish) ? en_bads : fr_bads;

        /// <summary>
        /// Dictses this instance.
        /// </summary>
        /// <returns>ISet&lt;System.String&gt;.</returns>
        private static ISet<string> Dicts(FornaxLanguage language)
        {
            ISet<string> dict = new HashSet<string>();
            if (language.IsEnglish)
                Process(dictionary.en_voc.Split(Constants.Brokers, StringSplitOptions.RemoveEmptyEntries), ref dict, en_stops);
            else
                Process(dictionary.fr_voc.Split(Constants.Brokers, StringSplitOptions.RemoveEmptyEntries), ref dict, fr_stops);

            return dict;
        }

        private static void Process(string[] v, ref ISet<string> dict, ISet<string> stopwords)
        {
            foreach (var str in v)
            {
                var st = str.Clean(true);
                if (!stopwords.Contains(st) && st.Length > 2)
                {
                    dict.Add(st);
                }
            }
        }

        /// <summary>
        /// Badses this instance.
        /// </summary>
        /// <returns>ISet&lt;System.String&gt;.</returns>
        private static ISet<string> Bads(FornaxLanguage language)
        {
            ISet<string> bads = new HashSet<string>();
            if (language.IsEnglish)
                Process(dictionary.en_blacklist.Split(Constants.Brokers, StringSplitOptions.RemoveEmptyEntries), ref bads);
            else
                return bads;

            return bads;
        }

        /// <summary>
        /// Stopses this instance.
        /// </summary>
        /// <returns>ISet&lt;System.String&gt;.</returns>
        private static ISet<string> Stops(FornaxLanguage language)
        {
            ISet<string> stops = new HashSet<string>();
            if (language.IsEnglish)
                Process(dictionary.en_stop.Split(Constants.Brokers, StringSplitOptions.RemoveEmptyEntries), ref stops);
            else
                Process(dictionary.fr_stop.Split(Constants.Brokers, StringSplitOptions.RemoveEmptyEntries), ref stops);

            return stops;
        }

        /// <summary>
        /// Processes the specified STRS.
        /// </summary>
        /// <param name="strs">The STRS.</param>
        /// <param name="set">The set.</param>
        private static void Process(string[] strs, ref ISet<string> set)
        {
            foreach (var str in strs)
            {
                set.Add(str.Clean(true));
            }
        }


        /// <summary>
        /// Initializes static members of the <see cref="Vocabulary"/> class.
        /// </summary>
        static Vocabulary()
        {
            en_stops = Stops(FornaxLanguage.English);
            en_bads = Bads(FornaxLanguage.English);
            en_dict = Dicts(FornaxLanguage.English);

            fr_stops = Stops(FornaxLanguage.French);
            fr_bads = Bads(FornaxLanguage.French);
            fr_dict = Dicts(FornaxLanguage.French);
        }
    }
}
