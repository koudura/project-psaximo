// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-03-2017
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

/// <summary>
/// The Storage namespace.
/// </summary>
namespace Fornax.Net.Index.Storage
{
    /// <summary>
    /// Represents a collection of files which queries are to be performed against.
    /// </summary>
    /// <seealso cref="java.io.Serializable.__Interface" />
    [Serializable, ProtoContract]
    public abstract class Repository : java.io.Serializable.__Interface
    {
        protected static IList<FileInfo> _files;
        protected static SnippetsFile _snipets;

        protected static Configuration _config;
        protected Corpus _corpus;

        protected string _repofilename;
        protected internal Extractor _protocol;

        /// <summary>
        /// Creates the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="type">The type.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="extractionProtocol">The extraction protocol.</param>
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
        /// <param name="extractionProtocol">The extraction protocol.</param>
        /// <returns>Repository.</returns>
        public static Repository Create(string[] files, RepositoryType type, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            if (type == RepositoryType.Local) return new FSRepository(files, config, extractionProtocol);
            return new NETRepository(files, config);
        }

        /// <summary>
        /// Creates a new Fornax repository using the specified file-wrappers.
        /// (<see cref="FileWrapper" />).
        /// </summary>
        /// <param name="files">The files in the repository.</param>
        /// <param name="type">The type of repository.</param>
        /// <param name="config">The configuration of the repository.</param>
        /// <param name="extractionProtocol">The extraction protocol to be used for file extraction.</param>
        /// <returns>Fornax Repository.</returns>
        public static Repository Create(FileWrapper[] files, RepositoryType type, Configuration config, Extractor extractionProtocol = Extractor.Default)
        {
            if (type == RepositoryType.Local) return new FSRepository(files, config, extractionProtocol);
            return new NETRepository(files, config);
        }

        /// <summary>
        /// Creates a new Fornax repository from files in a given file-system directory.
        /// (<see cref="DirectoryInfo"/>).
        /// </summary>
        /// <param name="directory">The directory from which to enumerate files in repository.</param>
        /// <param name="type">The type of repository.</param>
        /// <param name="config">The configuration of the repository.</param>
        /// <param name="extractionProtocol">The extraction protocol to be used for file extraction.</param>
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
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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
        /// Gets the corpus re
        /// </summary>
        /// <value>The corpora.</value>
        public abstract Corpus Corpus { get; }

        /// <summary>
        /// Gets the corpora of documents in the repository.
        /// </summary>
        /// <value>The corpora.</value>
       internal abstract IEnumerable<IDocument> Corpora { get; }

        /// <summary>
        /// Enumerates the documents.
        /// </summary>
        /// <returns>IEnumerable&lt;IDocument&gt;.</returns>
        internal abstract IEnumerable<IDocument> EnumerateDocuments();

        /// <summary>
        /// Gets the repository file.
        /// </summary>
        /// <value>The repository file.</value>
        internal virtual FileInfo RepositoryFile => new FileInfo(_repofilename);

        /// <summary>
        /// Gets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        internal virtual Extractor Protocol => _protocol;

        public virtual SnippetsFile Snippets => _snipets;

    }
}
