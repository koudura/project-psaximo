// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-31-2017
// ***********************************************************************
// <copyright file="Configuration.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using System.IO;
using System.Linq;

using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Util;
using Fornax.Net.Util.IO.Writers;
using Fornax.Net.Util.Linq;
using Fornax.Net.Util.System;
using ProtoBuf;

namespace Fornax.Net
{
    /// <summary>
    /// Holds the configuration settings for Crawler,Repository, and Query.
    /// </summary>
    /// <seealso cref="java.io.Serializable.__Interface" />
    [Serializable, ProtoContract]
    public class Configuration : java.io.Serializable.__Interface
    {

        private FetchAttribute fetch;
        private CachingMode caching;
        private FileFormat[] fileformats;
        private FornaxLanguage language;
        private Tokenizer tokenizer;

        private readonly DirectoryInfo user_config_dir;
        private readonly FileInfo user_config_file;
        private string id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="fetch">The fetch.</param>
        /// <param name="caching">The caching.</param>
        /// <param name="fileFormats">The file formats.</param>
        /// <param name="tokenizer">The tokenizer.</param>
        /// <param name="language">The language.</param>
        internal Configuration(string id, FetchAttribute fetch, CachingMode caching, FileFormat[] fileFormats, Tokenizer tokenizer, FornaxLanguage language)
        {
            ConfigFactory.CheckId(ref id);
            this.fetch = fetch;
            this.caching = caching;
            this.language = language;

            var fileformatss = new HashSet<FileFormat>(fileFormats).ToArray();
            fileformats = fileformatss;
            this.id = id;
            user_config_dir = TryCreate();
            user_config_file = CreateUserFile();
            this.tokenizer = tokenizer;

            Save();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="fetch">The fetch.</param>
        /// <param name="caching">The caching.</param>
        /// <param name="formats">The formats.</param>
        /// <param name="tokenizer">The tokenizer.</param>
        /// <param name="language">The language.</param>
        internal Configuration(string id, FetchAttribute fetch, CachingMode caching, FornaxFormat formats, Tokenizer tokenizer, FornaxLanguage language)
            : this(id, fetch, caching, formats.GetFormats(), tokenizer, language)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="tokenizer">The tokenizer.</param>
        internal Configuration(string id, Tokenizer tokenizer)
            : this(id, FetchAttribute.Weak, CachingMode.Default, FornaxFormat.Plain, tokenizer, FornaxLanguage.English) { }

        /// <summary>
        /// Gets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        internal DirectoryInfo WorkingDirectory => user_config_dir;

        /// <summary>
        /// Sets the specified <see cref="CachingMode" /> as this configs Caching mode.
        /// NOTE: this set is temporal until the <see cref="Save" /> method is called.
        /// </summary>
        /// <value>The cache mode.</value>
        /// <param name="newCachingMode"></param>
        public CachingMode CacheMode { get { return caching; } set { caching = value; } }

        /// <summary>
        /// Sets the specified <see cref="FetchAttribute" /> as this configs FetchAtrribute.
        /// NOTE: this set is temporal until the <see cref="Save" /> method is called.
        /// </summary>
        /// <value>The fetch mode.</value>
        /// <param name="newfetchAttribute"></param>
        public FetchAttribute FetchMode { get { return fetch; } set { fetch = value; } }

        /// <summary>
        /// Gets the language by which this configuration works in fornax.net.
        /// </summary>
        /// <value>The language.</value>
        public FornaxLanguage Language { get { return language; } set { language = value; } }

        /// <summary>
        /// Recalls the ID of this Configuration, should be used to call up a configuration manually.
        /// <see cref="" />
        /// </summary>
        /// <value>The identifier.</value>
        public string ID => id;

        /// <summary>
        /// Gets or sets the tokenizer.
        /// </summary>
        /// <value>The tokenizer.</value>
        public Tokenizer Tokenizer { get { return tokenizer; } set { tokenizer = value; } }

        /// <summary>
        /// Gets the formats.
        /// </summary>
        /// <value>The formats.</value>
        internal FileFormat[] Formats => fileformats;

        private DirectoryInfo TryCreate()
        {
            var str = Path.Combine(Constants.BaseDirectory.FullName, string.Format(@"User[{0}].config", id));
            return Constants.GetCurrentDirectory(str);
        }

        private FileInfo CreateUserFile()
        {
            var loc = Path.Combine(WorkingDirectory.FullName, $"_.config");
            return new FileInfo(loc);
        }

        /// <summary>
        /// Refreshes this Configuration to the default fornax.net configuration.
        /// where (FetchAttribute = Weak, CachingMode = Default, FornaxFormat = Plain)
        /// ID would be changed to id of new configuration.
        /// </summary>
        /// <returns>A new  configuration with default settings and new id;</returns>
        public Configuration Refresh(string newConfigId = null)
        {
            Directory.Delete(user_config_dir.FullName);
            return new Configuration(newConfigId, tokenizer);
        }

        /// <summary>
        /// Resets this configuration to fornax.net default.(<see cref="ConfigFactory.Default" />)
        /// NOTE: Id is maintained.
        /// </summary>
        public void ResetToDefault()
        {
            fetch = FetchAttribute.Weak;
            caching = CachingMode.Default;
            language = FornaxLanguage.English;
            fileformats = FornaxFormat.Plain.GetFormats();
            Save();
        }

        /// <summary>
        /// Adds the format.
        /// </summary>
        /// <param name="fileFormats">The file formats.</param>
        public void AddFormat(FileFormat[] fileFormats)
        {
            var set = new HashSet<FileFormat>(fileFormats);
            set.AddAll(fileFormats);
            fileformats = set.ToArray();
        }

        /// <summary>
        /// Removes the format.
        /// </summary>
        /// <param name="fileFormat">The file format.</param>
        public void RemoveFormat(FileFormat fileFormat)
        {
            var lst = new HashSet<FileFormat>(fileformats);
            lst.RemoveWhere(x => lst.Contains(fileFormat));
            fileformats = lst.ToArray();
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            FornaxWriter.Write(this, user_config_file);
        }
    }
}
