using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Document;
using Fornax.Net.Index.Storage;
using Fornax.Net.Util.IO.Readers;
using Fornax.Net.Util.IO.Writers;

namespace Fornax.Net.Index.IO
{
    public class IndexFactory
    {
        private InvertedFile invertedIndex;
        private Corpus corpus;
        private readonly FileInfo file;
        private Configuration _config;


        public IndexFactory(FileInfo file, Configuration config) {
            Contract.Requires(file != null);
            this.file = file ?? throw new ArgumentNullException(nameof(file));

            invertedIndex = GetInvertedFile(file).Result;
            corpus = GetCorpus(file).Result;
            _config = config;
        }

        /// <summary>
        /// Gets the corpus from disk.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static async Task<Corpus> GetCorpus(FileInfo file) {
            return await FornaxWriter.ReadAsync<Corpus>(file);
        }

        /// <summary>
        /// Gets the inverted file from disk.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static async Task<InvertedFile> GetInvertedFile(FileInfo file) {
            return await FornaxWriter.ReadAsync<InvertedFile>(file);
        }

        /// <summary>
        /// Adds the specified TokenStream to a specified inverted index through a document {given document id}.
        /// </summary>
        /// <param name="DocID">The document identifier.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="index">The index.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// index
        /// or
        /// config
        /// </exception>
        public static void Add(ulong DocID, TokenStream stream, InvertedFile index, Configuration config) {
            Contract.Requires(stream != null && index != null);
            if (stream == null || index == null) throw new ArgumentNullException(nameof(index) + " || " + nameof(stream));
            if (config == null) throw new ArgumentNullException(nameof(config));

            long pos = 0;

            while (stream.MoveNext()) {
                var token = stream.Current;
                var term = new Term(token, config.Language);
                if (index.ContainsKey(term)) {
                    if (index[term].ContainsKey(DocID)) {
                        index[term][DocID][term].Add(pos);
                    } else { index[term].Add(DocID, new TermVector(term, pos)); }
                } else {
                    index.Add(term, new Postings(term, DocID, pos));
                }
            }
        }

        /// <summary>
        /// Adds the specified document to
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="index">The index.</param>
        /// <param name="config">The configuration.</param>
        public static void Add(IDocument document, InvertedFile index, Configuration config) {
            Contract.Requires(document != null && index != null && config != null);

            Add(document.ID, document.Tokens, index, config);
        }

        public static void Add(Repository repository, InvertedFile index) {
            foreach (var doc in repository.Documents) {
                Add(doc, index, repository.Configuration);
            }
        }

       

    }
}

