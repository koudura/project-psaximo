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


        private static Configuration SetConfiguration() {
            return null;
        }

        internal static void SaveSettings() {
            config.Default.Save();
        }

        public static Configuration Create() {
            return null;
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
