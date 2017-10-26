using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Util;
using Fornax.Net.Util.IO;
using Fornax.Net.Util.IO.Writers;

namespace Fornax.Net.Index.Storage
{
    public abstract class Repository
    {

        public static Repository Create(FileInfo[] files, RepositoryType type, FileFormat[] formats, Configuration config) {
            switch (type) {
                case RepositoryType.Local: return new FSRepository(files, formats, config);
                case RepositoryType.Network: return new NETRepository(files, formats, config);
                default: return new RAMRepository(files, formats, config);
            }
        }

        public static Repository Create(string[] files, RepositoryType type, FileFormat[] formats, Configuration config) {
            switch (type) {
                case RepositoryType.Local: return new FSRepository(files, formats, config);
                case RepositoryType.Network: return new NETRepository(files, formats, config);
                default: return new RAMRepository(files, formats, config);
            }
        }

        public static Repository Create(FileWrapper[] files, RepositoryType type, FileFormat[] formats, Configuration config) {
            switch (type) {
                case RepositoryType.Local: return new FSRepository(files, formats, config);
                case RepositoryType.Network: return new NETRepository(files, formats, config);
                default: return new RAMRepository(files, formats, config);
            }
        }

        public static Repository Create(DirectoryInfo directory, RepositoryType type, FileFormat[] formats, Configuration config) {
            switch (type) {
                case RepositoryType.Local: return new FSRepository(directory, formats, config);
                case RepositoryType.Network: return new NETRepository(directory, formats, config);
                default: return new RAMRepository(directory, formats, config);
            }
        }

        public static Repository Open(FileInfo repoFile) {
            if (IsValidRepoFile(repoFile)) {
                return FornaxWriter.Read<FSRepository>(repoFile);
            } else throw new FileLoadException(nameof(repoFile));
        }

        private static bool IsValidRepoFile(FileInfo repoFile) {
            return repoFile.Extension.Equals(Constants.ExtDataFile);
        }
    }
}
