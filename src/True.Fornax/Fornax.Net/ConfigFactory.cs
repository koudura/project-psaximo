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

using System.Collections.Generic;
using System.Globalization;

using Fornax.Net.Properties;
using Fornax.Net.Util;
using Fornax.Net.Util.Resources;
using Fornax.Net.Util.System;

using StringSet = System.Collections.Specialized.StringCollection;

namespace Fornax.Net
{
    /// <summary>
    /// Configuration Factory for managing and building Fonax settings.
    /// </summary>
    public static class ConfigFactory
    {

        #region dir-configs
        internal static IReadOnlyDictionary<FornaxFormat, StringSet> FornaxFormatTable { get; private set; }

        internal static CultureInfo english = config.Default.Fornax_Lang1;
        internal static CultureInfo french = config.Default.Fornax_Lang2;
        internal static CultureInfo current = config.Default.Language;

        internal static StringSet UserDefinedFormats => config.Default.USER_DEFINED ?? throw new FornaxException();

        internal static bool IsSafeSearchable = config.Default.QueryIsSafeSearch;
        internal static bool IsExpandable = config.Default.QueryIsExpand;
        internal static bool IsAutoCorrectable = config.Default.QueryAutoCorrect;
        #endregion


        public static Configuration SetConfiguration(FetchAttribute fetchattribute, CachingMode cacheMode, params FileFormat[] fileFormats) {
            if(fileFormats.Length <= 0) { fileFormats[0] = FileFormat.Txt; }
            return new Configuration(fetchattribute, cacheMode, fileFormats);
        }

        internal static void SaveSettings() {
            config.Default.Save();
        }

        /// <summary>
        /// Gets the vocabulary from fornax.net. 
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static Vocabulary GetVocabulary(FornaxLanguage language) {
            return new Vocabulary(language);
        }

        static ConfigFactory() {

            FornaxFormatTable = new Dictionary<FornaxFormat, StringSet>
        {
            {FornaxFormat.Dom, config.Default.DOM_format ?? throw new FornaxException()},
            {FornaxFormat.Email, config.Default.EMAIL_format ?? throw new FornaxException()},
            {FornaxFormat.Image, config.Default.IMAGE_format ?? throw new FornaxException()},
            {FornaxFormat.Media, config.Default.MEDIA_format ?? throw new FornaxException()},
            {FornaxFormat.Plain, config.Default.PLAIN_format ?? throw new FornaxException()},
            {FornaxFormat.Sheet, config.Default.SHEET_format ?? throw new FornaxException()},
            {FornaxFormat.Slide, config.Default.SLIDE_format ?? throw new FornaxException()},
            {FornaxFormat.Text, config.Default.TEXT_format ?? throw new FornaxException()},
            {FornaxFormat.Web, config.Default.WEB_format ?? throw new FornaxException()},
            {FornaxFormat.Zip, config.Default.ZIP_format ?? throw new FornaxException()}
        };

        }
    }
}
