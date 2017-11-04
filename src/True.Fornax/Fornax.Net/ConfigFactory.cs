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
using System.IO;

using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Properties;
using Fornax.Net.Util;
using Fornax.Net.Util.IO.Writers;
using Fornax.Net.Util.Text;
using Fornax.Net.Util.System;

using StringSet = System.Collections.Specialized.StringCollection;
using System;
using System.Threading.Tasks;

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

        /// <summary>
        /// Gets the configuration to be used for all continouos functions of fornax.net.
        /// </summary>
        /// <param name="fetchattribute">The fetchattribute.</param>
        /// <param name="cacheMode">The cache mode.</param>
        /// <param name="language">The language.</param>
        /// <param name="fileFormats">The file formats.</param>
        /// <returns></returns>
        public static Configuration GetConfiguration(string config_id, FetchAttribute fetchattribute, CachingMode cacheMode, FornaxLanguage language, Tokenizer tokenizer, params FileFormat[] fileFormats)
        {
            CheckId(ref config_id);
            if (fileFormats.Length <= 0) { fileFormats[0] = FileFormat.Txt; }
            return new Configuration(config_id, fetchattribute, cacheMode, fileFormats, tokenizer, language);
        }

        internal static void CheckId(ref string config_id)
        {
            if (string.IsNullOrEmpty(config_id) || string.IsNullOrWhiteSpace(config_id))
            {
                config_id = def_id;
            }

        }

        /// <summary>
        /// Gets the configuration to be used for all continouos functions of fornax.net.
        /// This Returns Configuration with Default settings.
        /// </summary>
        /// <returns>Configuration for fornax.net</returns>
        public static Configuration Default => GetConfiguration(@def_id, new CharTokenizer());

        private static readonly string @def_id = "default";

        static Configuration GetConfiguration(string config_id, Tokenizer tokenizer)
        {
            CheckId(ref config_id);
            return new Configuration(config_id, tokenizer);
        }

        /// <summary>
        /// Gets the configuration to be used for all continouos functions of fornax.net.
        /// </summary>
        /// <param name="fetchAttribute">The fetch attribute.</param>
        /// <param name="cachingMode">The caching mode.</param>
        /// <param name="language">The language.</param>
        /// <param name="fileFormatCategory">The file format category.</param>
        /// <returns>A Configuatio for fornax.net</returns>
        public static Configuration GetConfiguration(string config_id, FetchAttribute fetchAttribute, CachingMode cachingMode, FornaxLanguage language, Tokenizer tokenizer, FornaxFormat fileFormatCategory)
        {
            CheckId(ref config_id);
            return new Configuration(config_id, fetchAttribute, cachingMode, fileFormatCategory, tokenizer, language);
        }

        /// <summary>
        /// Opens the configuration from an existing config id.
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">ID</exception>
        /// <exception cref="FileLoadException">ID</exception>
        public static Configuration OpenConfiguration(string ID)
        {
            var pattern = new FileInfo(Path.Combine(Constants.BaseDirectory.FullName, $"User[{ID}].config", "_.config"));
            try
            {
                if (pattern.Exists)
                    return FornaxWriter.Read<Configuration>(pattern);
                else throw new FileNotFoundException($"Configuration data with {nameof(ID)} = {ID} does not exist;");
            }
            catch { throw new FileLoadException($"Configuration data withh {nameof(ID)} = {ID} is probably corrupted."); }
        }

        public static void SaveSettings()
        {
            config.Default.Save();
        }

        /// <summary>
        /// Gets the vocabulary from fornax.net. 
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static Vocabulary GetVocabulary(FornaxLanguage language)
        {
            return new Vocabulary(language);
        }

        public static bool DeleteConfiguration(Configuration config)
        {
            bool disposing = true;
            try
            {
                while (disposing)
                {
                    if (!config.WorkingDirectory.Exists) disposing = false;
                    else
                    {
                        config.Language = null;
                        config.Tokenizer = null;
                        Task.WaitAll(Task.Run(() => config.WorkingDirectory.Delete(true)));
                    }
                }
            }
            catch (IOException)
            {
                disposing = false;
            }
            return disposing;
        }

        static ConfigFactory()
        {

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
