using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Document;
using Fornax.Net.Util.IO;

namespace Fornax.Net.Index.Storage
{
    public class NETRepository : Repository
    {
        private FileWrapper[] files1;
        private string[] files2;
        private DirectoryInfo directory;

        public NETRepository(FileInfo[] files,Configuration config) {
            this._files = files;
            this._config = config;
        }

        public NETRepository(FileWrapper[] fileWrapper ,Configuration config) {
            this.files1 = fileWrapper;
            this._config = config;
        }

        public NETRepository(string[] files2,Configuration config) {
            this.files2 = files2;
            this._config = config;
        }

        public NETRepository(DirectoryInfo directory, Configuration config) {
            this.directory = directory;
            this._config = config;
        }

        public override Corpus Corpora => throw new NotImplementedException();

        public override IEnumerable<IDocument> Documents => throw new NotImplementedException();

        public override Configuration Configuration => throw new NotImplementedException();
    }
}
