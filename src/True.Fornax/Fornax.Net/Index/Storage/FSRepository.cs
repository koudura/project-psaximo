// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-05-2017
// ***********************************************************************
// <copyright file="FSRepository.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
////// ***********************************************************************
////// Assembly         : Fornax.Net
////// Author           : Koudura Mazou
////// Created          : 10-29-2017
//////
////// Last Modified By : Koudura Mazou
////// Last Modified On : 10-31-2017
////// ***********************************************************************
////// <copyright file="FSRepository.cs" company="True.Inc">
/////***
////* Copyright (c) 2017 Koudura Ninci @True.Inc
////*
////* Permission is hereby granted, free of charge, to any person obtaining a copy
////* of this software and associated documentation files (the "Software"), to deal
////* in the Software without restriction, including without limitation the rights
////* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
////* copies of the Software, and to permit persons to whom the Software is
////* furnished to do so, subject to the following conditions:
////* 
////* The above copyright notice and this permission notice shall be included in all
////* copies or substantial portions of the Software.
////* 
////* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
////* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
////* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
////* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
////* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
////* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
////* SOFTWARE.
////*
////**/
////// </copyright>
////// <summary></summary>
////// ***********************************************************************


using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Fornax.Net.Document;
using Fornax.Net.Util.IO;
using Fornax.Net.Util.IO.Readers;
using Fornax.Net.Util.IO.Writers;
using Fornax.Net.Util.Security.Cryptography;
using ProtoBuf;

using Cst = Fornax.Net.Util.Constants;
using FSDocument = Fornax.Net.Document.FSDocument;

/// <summary>
/// The Storage namespace.
/// </summary>
namespace Fornax.Net.Index.Storage
{
    /// <summary>
    /// Files System Repository.
    /// </summary>
    /// <seealso cref="Fornax.Net.Index.Storage.Repository" />
    /// <seealso cref="Repository" />
    /// <seealso cref="java.io.Serializable.__Interface" />
    /// <seealso cref="Repository" />
    [Serializable, ProtoContract]
    public sealed class FSRepository : Repository, java.io.Serializable.__Interface
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FSRepository" /> class.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="extractionProtocol">The extraction protocol.</param>
        /// <exception cref="ArgumentNullException">files</exception>
        internal FSRepository(FileInfo[] files, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            Contract.Requires(files != null);
            if (files == null) throw new ArgumentNullException(nameof(files));

            _files = GetFiles(files, config.Formats);
            InstantiateAll(config, Path.Combine(config.WorkingDirectory.FullName, "_" + Cst.ExtRepoFile), extractionProtocol, InitCorpus());

            Task.WaitAll(CreateorUpdate());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FSRepository" /> class.
        /// </summary>
        /// <param name="filewrappers">The filewrappers.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="extractionProtocol">The extraction protocol.</param>
        /// <exception cref="ArgumentNullException">filewrappers</exception>
        internal FSRepository(FileWrapper[] filewrappers, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            Contract.Requires(filewrappers != null);
            if (filewrappers == null) throw new ArgumentNullException(nameof(filewrappers));

            _files = GetFiles(filewrappers, config.Formats);
            InstantiateAll(config, Path.Combine(config.WorkingDirectory.FullName, "_" + Cst.ExtRepoFile), extractionProtocol, InitCorpus());

            Task.WaitAll(CreateorUpdate());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FSRepository" /> class.
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="extractionProtocol">The extraction protocol.</param>
        /// <exception cref="ArgumentNullException">fileNames</exception>
        internal FSRepository(string[] fileNames, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            Contract.Requires(fileNames != null);
            if (fileNames == null) throw new ArgumentNullException(nameof(fileNames));

            _files = GetFiles(fileNames, config.Formats);
            InstantiateAll(config, Path.Combine(config.WorkingDirectory.FullName, "_" + Cst.ExtRepoFile), extractionProtocol, InitCorpus());

            Task.WaitAll(CreateorUpdate());
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="FSRepository" /> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="extractionProtocol">The extraction protocol.</param>
        /// <exception cref="ArgumentNullException">directory</exception>
        internal FSRepository(DirectoryInfo directory, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            Contract.Requires(directory != null);
            if (directory == null) throw new ArgumentNullException(nameof(directory));

            _files = GetFiles(directory, config.Formats);
            InstantiateAll(config, Path.Combine(config.WorkingDirectory.FullName, "_" + Cst.ExtRepoFile), extractionProtocol, InitCorpus());

            Task.WaitAll(CreateorUpdate());
        }

        private FSRepository()
        {
        }

        /// <summary>
        /// Gets the corpus representing the collection of document id's to
        /// files.
        /// </summary>
        /// <value>The corpora.</value>
        public override Corpus Corpus => _corpus;

        /// <summary>
        /// Gets the corpora of documents in the repository.
        /// </summary>
        /// <value>The corpora.</value>
        internal override IEnumerable<IDocument> Corpora => EnumerateDocuments();

        /// <summary>
        /// Gets the configuration of the repository.
        /// </summary>
        /// <value>The configuration.</value>
        /// <exception cref="NotImplementedException"></exception>
        public override Configuration Configuration => _config;

        /// <summary>
        /// Gets the repository file.
        /// </summary>
        /// <value>The repository file.</value>
        internal override FileInfo RepositoryFile => base.RepositoryFile;

        /// <summary>
        /// Gets the snippets.
        /// </summary>
        /// <value>The snippets.</value>
        public override SnippetsFile Snippets => _snipets;

        #region workers

        private void InstantiateAll(Configuration config, string filename, Extractor ext, Corpus corp)
        {
            _config = config;
            _repofilename = filename;
            _protocol = ext;
            _corpus = corp;
            _snipets = new SnippetsFile();
        }

        /// <summary>
        /// Gets the documents in the repository.
        /// </summary>
        /// <returns>IEnumerable&lt;IDocument&gt;.</returns>
        /// <value>The documents.</value>
        internal override IEnumerable<IDocument> EnumerateDocuments()
        {
            return InitDocuments(_protocol).Result;
        }

        private async Task<IEnumerable<FSDocument>> InitDocuments(Extractor extProt)
        {
            var set = await Task.Factory.StartNew(() => YieldDocuments(extProt));
            Task.WaitAll(CreateorUpdate());
            return set;
        }

        private IList<FSDocument> YieldDocuments(Extractor ext)
        {
            IList<FSDocument> acquired = new List<FSDocument>();
            foreach (var file in _files)
            {
                var doc = new FSDocument(file, _config.Tokenizer, _config.Language, ext);
                _snipets.Add(doc.ID, doc.Capture);
                acquired.Add(doc);
            }
            return acquired;
        }

        #region files enumertors
        private static IList<FileInfo> GetFiles(DirectoryInfo directory, FileFormat[] formats)
        {
            IList<FileInfo> gotten = new List<FileInfo>();
            var files = from ext in formats
                        let sext = ext.GetString()
                        from t in directory.EnumerateFiles(("*" + sext), SearchOption.AllDirectories)
                        select t;
            foreach (var file in files) gotten.Add(file);
            return gotten;
        }

        private static IList<FileInfo> GetFiles(FileInfo[] files, FileFormat[] formats)
        {
            IList<FileInfo> filss = new List<FileInfo>();
            var fls = from ext in formats
                      let sext = ext.GetString()
                      from file in files
                      where file.Extension == sext
                      select file;
            foreach (var file in fls) filss.Add(file);
            return filss;
        }

        private static IList<FileInfo> GetFiles(FileWrapper[] filewrappers, FileFormat[] formats)
        {
            IList<FileInfo> file_s = new List<FileInfo>();
            foreach (var wrapper in filewrappers)
            {
                var pass = wrapper.Parse();
                var file = pass.AsFile; var dir = pass.AsDirectory;
                if (file == null)
                {
                    foreach (var item in formats)
                    {
                        var fs = item.GetString();
                        foreach (var f in dir.EnumerateFiles(("*" + fs), SearchOption.AllDirectories))
                        {
                            file_s.Add(f);
                        }
                    }
                }
                else if (dir == null)
                {
                    foreach (var item in formats)
                    {
                        var fs = item.GetString();
                        if (file.Extension == fs)
                            file_s.Add(file);
                    }
                }
            }
            return file_s;
        }

        private static IList<FileInfo> GetFiles(string[] fileNames, FileFormat[] formats)
        {
            IList<FileInfo> files = new List<FileInfo>();
            foreach (var filename in fileNames)
            {
                var file = new FileInfo(filename);
                if (file.Exists) files.Add(file);
            }
            return GetFiles(files.ToArray(), formats);
        }
        #endregion

        #region repo handles

        private Corpus InitCorpus()
        {
            Corpus corp = new Corpus();
            foreach (var item in _files)
            {
                corp.Add(new KeyValuePair<ulong, string>(Adler32.Compute(item.FullName), item.FullName));
            }
            return corp;
        }

        internal async Task CreateorUpdate()
        {
            await FornaxWriter.WriteAsync<Repository>(this, _repofilename);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return RepositoryFile.FullName;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is Repository)
            {
                return ((Repository)(obj)).RepositoryFile.FullName == RepositoryFile.FullName;
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (int)Adler32.Compute(ToString());
        }
        #endregion

        #endregion
    }
}
