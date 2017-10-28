


using System;
using System.Collections.Generic;
using System.IO;

using Fornax.Net.Document;
using Fornax.Net.Util;
using Fornax.Net.Util.IO;
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
        [ProtoMember(1)]
        protected IEnumerable<FileInfo> _files;
        [ProtoMember(2)]
        protected Configuration _config;
        [ProtoMember(3)]
        protected string _repofilename;
        [ProtoMember(4)]
        protected Corpus _corpus;

        /// <summary>
        /// Creates the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="type">The type.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static Repository Create(FileInfo[] files, RepositoryType type, Configuration config) {
            if (type == RepositoryType.Local) return new FSRepository(files, config);
            return new NETRepository(files, config);
        }

        /// <summary>
        /// Creates the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="type">The type.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static Repository Create(string[] files, RepositoryType type, Configuration config) {
            if (type == RepositoryType.Local) return new FSRepository(files, config);
            return new NETRepository(files, config);
        }

        /// <summary>
        /// Creates the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="type">The type.</param>
        /// <param name="formats">The formats.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static Repository Create(FileWrapper[] files, RepositoryType type, FileFormat[] formats, Configuration config) {
            if (type == RepositoryType.Local) return new FSRepository(files, config);
            return new NETRepository(files, config);
        }

        /// <summary>
        /// Creates the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="type">The type.</param>
        /// <param name="formats">The formats.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static Repository Create(DirectoryInfo directory, RepositoryType type, FileFormat[] formats, Configuration config) {
            if (type == RepositoryType.Local) return new FSRepository(directory, config);
            return new NETRepository(directory, config);
        }

        /// <summary>
        /// Opens the specified repo file.
        /// </summary>
        /// <param name="repoFile">The repo file.</param>
        /// <returns></returns>
        /// <exception cref="FileLoadException">repoFile</exception>
        public static Repository Open(FileInfo repoFile) {
            if (IsValidRepoFile(repoFile)) {
                return FornaxWriter.Read<FSRepository>(repoFile);
            } else throw new FileLoadException(nameof(repoFile));
        }

        private static bool IsValidRepoFile(FileInfo repoFile) {
            return repoFile.Extension.Equals(Constants.ExtRepoFile);
        }

        public abstract Configuration Configuration { get; }
        public abstract Corpus Corpora { get; }
        public abstract IEnumerable<IDocument> Documents { get; }
    }
}
