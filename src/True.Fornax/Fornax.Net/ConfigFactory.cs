using System.Collections.Generic;
using Fornax.Net.Util;
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

        internal static StringSet UserDefinedFormats => config.Default.USER_DEFINED ?? throw new FornaxException();
        #endregion


        private static Configuration SetConfiguration() {
            return null;
        }


        internal static void SaveSettings() {
            config.Default.Save();
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

        public static Configuration Create() {
            return null;
        }
    }
}
