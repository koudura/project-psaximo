using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Document;
using Fornax.Net.Util.IO;
using Fornax.Net.Util.IO.Readers;

namespace Fornax.Net.Index.Storage
{
    public class NETRepository : Repository
    {
        private string[] files;
        private Configuration config;
        private FileInfo[] files1;
        private FileWrapper[] files2;
        private DirectoryInfo directory;

        public NETRepository(string[] files, Configuration config)
        {
            this.files = files;
            this.config = config;
        }

        public NETRepository(FileInfo[] files1, Configuration config)
        {
            this.files1 = files1;
            this.config = config;
        }

        public NETRepository(FileWrapper[] files2, Configuration config)
        {
            this.files2 = files2;
            this.config = config;
        }

        public NETRepository(DirectoryInfo directory, Configuration config)
        {
            this.directory = directory;
            this.config = config;
        }

        public override Configuration Configuration => throw new NotImplementedException();

        public override Corpus Corpus => throw new NotImplementedException();

        internal override IEnumerable<IDocument> Corpora => throw new NotImplementedException();

        public override SnippetsFile Snippets => throw new NotImplementedException();

        internal override FileInfo RepositoryFile => base.RepositoryFile;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        internal override IEnumerable<IDocument> EnumerateDocuments()
        {
            throw new NotImplementedException();
        }
    }
}
