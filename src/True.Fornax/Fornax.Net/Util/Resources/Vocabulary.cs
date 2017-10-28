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
using Fornax.Net.Util.System;
using Fornax.Net.Util.Text;

using Collection = Fornax.Net.Util.Collections.Collections;

namespace Fornax.Net.Util.Resources
{
    /// <summary>
    /// Fornax Vocabulary Handler.
    /// </summary>
    [Serializable]
    public sealed class Vocabulary
    {
        FornaxLanguage language;
        string infix;
        private ISet<string> stopwords;
        private ISet<string> badwords;
        private ISet<string> dicts;
        private IDictionary<string, string> stemtable;


        internal Vocabulary(FornaxLanguage culture) {
            language = culture ?? throw new FornaxException($"{nameof(culture)} is not-supported.");
            infix = Infix();

            stopwords = Collection.UnmodifiableSet(Stops);
            badwords = Collection.UnmodifiableSet(Bads);
            dicts = Collection.UnmodifiableSet(Dicts);

        }

        /// <summary>
        /// Gets the dictionary. i.e a comprehensive set of words from a 
        /// atandard dictionary of the specified language.S
        /// </summary>
        /// <value>
        /// The dictionary.
        /// </value>
        public ISet<string> Dictionary => dicts;

        /// <summary>
        /// Gets the stop words. i.e a set of stop words by fornax for the specified 
        /// language.
        /// </summary>
        /// <value>
        /// The stop words.
        /// </value>
        public ISet<string> StopWords => stopwords;

        /// <summary>
        /// Gets the bad words. i.e the blacklisted words for the specified
        /// language.
        /// </summary>
        /// <value>
        /// The bad words.
        /// </value>
        public ISet<string> BadWords => badwords;

        private string Infix() {
            return (language.IsEnglish) ? "en" : "fr";
        }

        private ISet<string> Dicts() {
            ISet<string> dict = new HashSet<string>();
            if (language.IsEnglish)
                Process(dictionary.en_voc.Split(Constants.Brokers), ref dict);
            else
                Process(dictionary.fr_voc.Split(Constants.Brokers), ref dict);
           
            return dict;
        }

        private ISet<string> Bads() {
            ISet<string> bads = new HashSet<string>();
            if (language.IsEnglish)
                Process(dictionary.en_blacklist.Split(Constants.Brokers), ref bads);
            else
                return bads;

                return bads;
        }

        private  ISet<string> Stops() {
            ISet<string> stops = new HashSet<string>();
            if (language.IsEnglish) 
               Process(dictionary.en_stop.Split(Constants.Brokers),ref stops); 
            else 
               Process(dictionary.fr_stop.Split(Constants.Brokers), ref stops);
           
            return stops;
        }

        private void Process(string[] strs, ref ISet<string> set) {
            foreach (var str in strs) {
                set.Add(str.Clean(true));
            }
        }

        static Vocabulary() {

        }
    }
}
