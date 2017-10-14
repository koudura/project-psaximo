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

using System.Globalization;

namespace Fornax.Net.Util.System
{
    /// <summary>
    /// Represents a placeholder class for fornax.net language utilities.
    /// </summary>
    public sealed class FornaxLanguage
    {
        private string lang_tag;

        private FornaxLanguage() {
        }

        private FornaxLanguage(string v) {
            lang_tag = v;
            Culture = GetCulture();
        }

        /// <summary>
        /// Gets the english language support.
        /// </summary>
        /// <value>
        /// The english language.
        /// </value>
        public static FornaxLanguage English => new FornaxLanguage("en");

        /// <summary>
        /// Gets the french language support.
        /// </summary>
        /// <value>
        /// The french language.
        /// </value>
        public static FornaxLanguage French => new FornaxLanguage("fr");

        internal CultureInfo Culture { get; private set; }
        internal string Value => lang_tag.Equals("en") ? "English" : "French";
        internal bool IsEnglish => lang_tag.Equals("en") ? true : false;

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance of langugage.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this language.
        /// </returns>
        public override string ToString() {
            return Culture.EnglishName;
        }

        private CultureInfo GetCulture() {
            return (lang_tag.Equals("en")) ? ConfigFactory.english : ConfigFactory.french;
        }

    }
}
