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
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Fornax.Net.Document;
using Fornax.Net.Util.IO;
using Fornax.Net.Util.IO.Readers;
using Fornax.Net.Util.IO.Writers;
using ProtoBuf;

using Cst = Fornax.Net.Util.Constants;
using FSDocument = Fornax.Net.Document.FSDocument;

namespace Fornax.Net.Index.Storage
{
    /// <summary>
    /// Files System Repository.
    /// </summary>
    /// <seealso cref="Fornax.Net.Index.Storage.Repository" />
    [Serializable, ProtoContract]
    public sealed class FSRepository : Repository, java.io.Serializable.__Interface
    {
        private IEnumerable<FSDocument> documents;

        /// <summary>
        /// Initializes a new instance of the <see cref="FSRepository"/> class.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="config">The configuration.</param>
        public FSRepository(FileInfo[] files, Configuration config, Extractor extractionProtocol = Extractor.Default) {
            Contract.Requires(files != null);
            if (files == null) throw new ArgumentNullException(nameof(files));

            _files = GetFiles(files, config.Formats);
            _config = config;
            var random = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Cst.ExtRepoFile;
            _repofilename = Path.Combine(config.WorkingDirectory.FullName, random);
            _corpus = InitCorpus();
            documents = InitDocuments(extractionProtocol).Result;
            Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FSRepository"/> class.
        /// </summary>
        /// <param name="filewrappers">The filewrappers.</param>
        /// <param name="config">The configuration.</param>
        public FSRepository(FileWrapper[] filewrappers, Configuration config, Extractor extractionProtocol = Extractor.Default) {
            Contract.Requires(filewrappers != null);
            if (filewrappers == null) throw new ArgumentNullException(nameof(filewrappers));

            _files = GetFiles(filewrappers, config.Formats);
            _config = config;
            var random = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Cst.ExtRepoFile;
            _repofilename = Path.Combine(config.WorkingDirectory.FullName, random);
            _corpus = InitCorpus();
            documents = InitDocuments(extractionProtocol).Result;
            Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FSRepository"/> class.
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        /// <param name="config">The configuration.</param>
        public FSRepository(string[] fileNames, Configuration config, Extractor extractionProtocol = Extractor.Default) {
            Contract.Requires(fileNames != null);
            if (fileNames == null) throw new ArgumentNullException(nameof(fileNames));


            _files = GetFiles(fileNames, config.Formats);
            _config = config;
            var random = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Cst.ExtRepoFile;
            _repofilename = Path.Combine(config.WorkingDirectory.FullName, random);
            _corpus = InitCorpus();
            documents = InitDocuments(extractionProtocol).Result;

            Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FSRepository"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="config">The configuration.</param>
        public FSRepository(DirectoryInfo directory, Configuration config, Extractor extractionProtocol = Extractor.Default) {
            _files = GetFiles(directory, config.Formats);
            base._config = config;
            var random = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Cst.ExtRepoFile;
            _repofilename = Path.Combine(config.WorkingDirectory.FullName, random);
            _corpus = InitCorpus();
            documents = InitDocuments(extractionProtocol).Result;
            Create();
        }

        /// <summary>
        /// Gets the corpora.
        /// </summary>
        /// <value>
        /// The corpora.
        /// </value>
        public override Corpus Corpora => _corpus;
        /// <summary>
        /// Gets the documents in the repository.
        /// </summary>
        /// <value>
        /// The documents.
        /// </value>
        public override IEnumerable<IDocument> Documents => documents;
        /// <summary>
        /// Gets the configuration of the repository.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        /// <exception cref="NotImplementedException"></exception>
        public override Configuration Configuration => throw new NotImplementedException();


        #region workers
        private Corpus InitCorpus() {
            throw new NotImplementedException();
        }

        private async Task<IEnumerable<FSDocument>> InitDocuments(Extractor extProt) {
            IList<FSDocument> document = new List<FSDocument>();
            await Task.Factory.StartNew(() => {
                foreach (var file in _files) {
                    document.Add(new FSDocument(file, _config.Tokenizer, extProt));
                }
            });
            return document;
        }

        private static IEnumerable<FileInfo> GetFiles(DirectoryInfo directory, FileFormat[] formats) {
            var files = from ext in formats
                        let sext = ext.GetString()
                        from t in directory.EnumerateFiles(("*" + sext), SearchOption.AllDirectories)
                        select t;
            foreach (var file in files) yield return file;
        }

        private IEnumerable<FileInfo> GetFiles(FileInfo[] files, FileFormat[] formats) {
            var fls = from ext in formats
                      let sext = ext.GetString()
                      from file in files
                      where file.Extension == sext
                      select file;
            foreach (var file in fls) yield return file;
        }

        private IEnumerable<FileInfo> GetFiles(FileWrapper[] filewrappers, FileFormat[] formats) {

            IList<FileInfo> file_s = new List<FileInfo>();
            foreach (var wrapper in filewrappers) {
                var pass = wrapper.Parse();
                var file = pass.AsFile; var dir = pass.AsDirectory;
                if (file == null) {
                    foreach (var item in formats) {
                        var fs = item.GetString();
                        foreach (var f in dir.EnumerateFiles(("*" + fs), SearchOption.AllDirectories)) {
                            file_s.Add(f);
                        }
                    }
                } else if (dir == null) {
                    foreach (var item in formats) {
                        var fs = item.GetString();
                        if (file.Extension == fs)
                            file_s.Add(file);
                    }
                }
            }
            return file_s;
        }

        private IEnumerable<FileInfo> GetFiles(string[] fileNames, FileFormat[] formats) {
            IList<FileInfo> files = new List<FileInfo>();
            foreach (var filename in fileNames) {
                var file = new FileInfo(filename);
                if (file.Exists) files.Add(file);
            }
            return GetFiles(files.ToArray(), formats);
        }

        private void Create() {
            FornaxWriter.Write<Repository>(this, _repofilename);
        }

        #endregion
    }
}
