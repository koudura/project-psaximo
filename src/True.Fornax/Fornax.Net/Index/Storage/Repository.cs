﻿// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-31-2017
// ***********************************************************************
// <copyright file="Repository.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************



using System;
using System.Collections.Generic;
using System.IO;

using Fornax.Net.Document;
using Fornax.Net.Util;
using Fornax.Net.Util.IO;
using Fornax.Net.Util.IO.Readers;
using Fornax.Net.Util.IO.Writers;
using ProtoBuf;

namespace Fornax.Net.Index.Storage
{
    /// <summary>
    /// Represents a collection of files which queries are tob performed against.
    /// </summary>
    /// <seealso cref="java.io.Serializable.__Interface" />
    [Serializable, ProtoContract]
    public abstract class Repository : java.io.Serializable.__Interface
    {
        protected static IList<FileInfo> _files;
        protected static Configuration _config;
        protected string _repofilename;
        protected Corpus _corpus;
        protected internal Extractor _protocol;

        /// <summary>
        /// Creates the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="type">The type.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Repository.</returns>
        public static Repository Create(FileInfo[] files, RepositoryType type, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            if (type == RepositoryType.Local) return new FSRepository(files, config, extractionProtocol);
            return new NETRepository(files, config);
        }

        /// <summary>
        /// Creates the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="type">The type.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Repository.</returns>
        public static Repository Create(string[] files, RepositoryType type, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            if (type == RepositoryType.Local) return new FSRepository(files, config, extractionProtocol);
            return new NETRepository(files, config);
        }

        /// <summary>
        /// Creates the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="type">The type.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Repository.</returns>
        public static Repository Create(FileWrapper[] files, RepositoryType type, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            if (type == RepositoryType.Local) return new FSRepository(files, config, extractionProtocol);
            return new NETRepository(files, config);
        }

        /// <summary>
        /// Creates the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="type">The type.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Repository.</returns>
        public static Repository Create(DirectoryInfo directory, RepositoryType type, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            if (type == RepositoryType.Local) return new FSRepository(directory, config, extractionProtocol);
            return new NETRepository(directory, config);
        }

        /// <summary>
        /// Opens the specified repo file.
        /// </summary>
        /// <param name="repoFile">The repo file.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="FileLoadException">repoFile</exception>
        public static bool TryOpen(FileInfo repoFile, out Repository repository)
        {
            repository = null;
            if (IsValidRepoFile(repoFile)) return false;
            try
            {
                repository = FornaxWriter.Read<FSRepository>(repoFile);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        private static bool IsValidRepoFile(FileInfo repoFile)
        {
            return repoFile.Extension.Equals(Constants.ExtRepoFile);
        }
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public abstract Configuration Configuration { get; }
        /// <summary>
        /// Gets the corpora.
        /// </summary>
        /// <value>The corpora.</value>
        public abstract Corpus Corpora { get; }
        /// <summary>
        /// Enumerates the documents.
        /// </summary>
        /// <param name="extractor">The extractor.</param>
        /// <returns>IEnumerable&lt;IDocument&gt;.</returns>
        internal abstract IEnumerable<IDocument> EnumerateDocuments();

        /// <summary>
        /// Gets the repository file.
        /// </summary>
        /// <value>The repository file.</value>
        internal virtual FileInfo RepositoryFile => new FileInfo(_repofilename);

        internal virtual Extractor Protocol => _protocol;
    }
}
