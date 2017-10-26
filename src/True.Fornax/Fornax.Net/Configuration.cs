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

namespace Fornax.Net
{
    /// <summary>
    /// Holds the configuration settings for Crawler,Repository, and Query.
    /// </summary>
    public class Configuration
    {
        private FetchAttribute fetch;
        private CachingMode caching;
        private FileFormat[] fileformats;

        internal Configuration(FetchAttribute fetch, CachingMode caching, FileFormat[] fileFormats) {
            this.fetch = fetch;
            this.caching = caching;
            fileformats = fileFormats;
        }

        internal Configuration(FetchAttribute fetch, CachingMode caching, FornaxFormat formats) : this(fetch, caching, formats.GetFormats()) {
        }

        internal Configuration() : this(FetchAttribute.Weak, CachingMode.Default, FornaxFormat.Plain) { }

        /// <summary>
        /// Resets this Configuration to the default fornax.net configuration.
        /// where (FetchAttribute = Weak, CachingMode = Default, FornaxFormat = Plain)
        /// </summary>
        /// <returns></returns>
        public Configuration Reset() {
            return new Configuration();
        }

        public void Save() {
            ConfigFactory.SaveSettings();
        }

    }
}
