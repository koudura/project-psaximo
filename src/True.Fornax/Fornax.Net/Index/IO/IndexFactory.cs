// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Habuto Koudura
// Created          : 09-25-2017
//
// Last Modified By : Habuto Koudura
// Last Modified On : 10-29-2017
// ***********************************************************************
// <copyright file="IndexFactory.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using Fornax.Net.Util.Linq;

/// <summary>
/// The IO namespace.
/// </summary>
namespace Fornax.Net.Index.IO
{
    /// <summary>
    /// Class IndexFactory.
    /// </summary>
    public class IndexFactory
    {
        /// <summary>
        /// The inverted index
        /// </summary>
        private InvertedFile invertedIndex;
        /// <summary>
        /// The corpus
        /// </summary>
        private Corpus corpus;
        /// <summary>
        /// The file
        /// </summary>
        private readonly FileInfo file;
        /// <summary>
        /// The configuration
        /// </summary>
        private Configuration _config;


        /// <summary>
        /// Initializes a new instance of the <see cref="IndexFactory"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="ArgumentNullException">file</exception>
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
        /// <returns>Task&lt;Corpus&gt;.</returns>
        public static async Task<Corpus> GetCorpus(FileInfo file) {
            return await FornaxWriter.ReadAsync<Corpus>(file);
        }

        /// <summary>
        /// Gets the inverted file from disk.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Task&lt;InvertedFile&gt;.</returns>
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
        /// <exception cref="ArgumentNullException">index
        /// or
        /// config</exception>
        public static void Add(ulong DocID, TokenStream stream, InvertedFile index, Configuration config) {
            Contract.Requires(stream != null && index != null);
            if (stream == null || index == null) throw new ArgumentNullException(nameof(index) + " || " + nameof(stream));
            if (config == null) throw new ArgumentNullException(nameof(config));

            long pos = 0;

            while (stream.MoveNext()) {
                var token = stream.Current;
                var term = new Term(token, config.Language);

                if (!index.TryGetValue(term, out Postings posting)) {
                    index.Add(term, new Postings(term, DocID, pos));
                } else {
                    if (!posting.TryGetValue(DocID, out TermVector vector)) {
                        posting.Add(DocID, new TermVector(term, pos));
                    } else vector[term].Value.Add(pos);
                }
            }
        }

        /// <summary>
        /// add as an asynchronous operation.
        /// </summary>
        /// <param name="DocID">The document identifier.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="index">The index.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Task.</returns>
        public async static Task AddAsync(ulong DocID, TokenStream stream, InvertedFile index, Configuration config) {
            await Task.Factory.StartNew(() => Add(DocID, stream, index, config));
        }
        /// <summary>
        /// add as an asynchronous operation.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="index">The index.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Task.</returns>
        public async static Task AddAsync(IDocument document, InvertedFile index, Configuration config) {
            await Task.Factory.StartNew(function: async () => await AddAsync(document.ID, document.Tokens, index, config));
        }
        /// <summary>
        /// add as an asynchronous operation.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public async static Task AddAsync(Repository repository, InvertedFile index) {
            await Task.Factory.StartNew(() => Add(repository, index));
        }

        /// <summary>
        /// Adds the specified document to an existing inverted index.
        /// </summary>
        /// <param name="document">The document to be added.</param>
        /// <param name="index">The existing inverted index to be updated.</param>
        /// <param name="config">The configuration of this runtime.</param>
        public static void Add(IDocument document, InvertedFile index, Configuration config) {
            Contract.Requires(document != null && index != null && config != null);

            Add(document.ID, document.Tokens, index, config);
        }

        /// <summary>
        /// Adds the specified repository to the given inverted index.
        /// </summary>
        /// <param name="repository">The existing repository to add to index.</param>
        /// <param name="index">The index to be Updated.</param>
        public static void Add(Repository repository, InvertedFile index) {
            foreach (var doc in repository.Documents) {
                Add(doc, index, repository.Configuration);
            }
        }

        /// <summary>
        /// Removes the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentNullException">
        /// index
        /// or
        /// document
        /// </exception>
        public static void Remove(IDocument document, InvertedFile index) {
            Contract.Requires(document != null && index != null);
            if (index == null) throw new ArgumentNullException(nameof(index));
            if (document == null) throw new ArgumentNullException(nameof(document));

            Remove(document.ID, index);
        }
        /// <summary>
        /// Removes the specified document identifier.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentNullException">index</exception>
        public static void Remove(ulong DocId, InvertedFile index) {
            Contract.Requires(index != null);
            if (index == null) throw new ArgumentNullException(nameof(index));

            foreach (var pair in index) {
                foreach (Postings post in pair.Value) {
                    if (post.ContainsKey(DocId)) {
                        post.Remove(DocId);
                        if (post.Count <= 0) {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// remove as an asynchronous operation.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public static async Task RemoveAsync(IDocument document, InvertedFile index) {
            await Task.Factory.StartNew(async () => await RemoveAsync(document.ID, index));
        }

        /// <summary>
        /// remove as an asynchronous operation.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public static async Task RemoveAsync(ulong DocId, InvertedFile index) {
            await Task.Factory.StartNew(() => Remove(DocId, index));
        }

        /// <summary>
        /// Merges the specified first index.
        /// </summary>
        /// <param name="firstIndex">The first index.</param>
        /// <param name="secondIndex">Index of the second.</param>
        public static void Merge(InvertedFile firstIndex, InvertedFile secondIndex) {

        }

        /// <summary>
        /// Merges the specified postings list.
        /// </summary>
        /// <param name="postingsList">The postings list.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentNullException">
        /// index
        /// or
        /// postingsList
        /// </exception>
        [Obsolete("Probably a hacky code.")]
        public static void Merge(Postings postingsList, InvertedFile index) {
            Contract.Requires(postingsList != null && index != null);
            if (index == null) throw new ArgumentNullException(nameof(index));
            if (postingsList == null) throw new ArgumentNullException(nameof(postingsList));

            foreach (KeyValuePair<ulong, TermVector> post in postingsList) {
                var id = post.Key;
                foreach (var vector in post.Value) {
                    var term = vector.Key;
                    if (index.TryGetValue(term, out Postings postIndex)) {
                        if (postIndex.TryGetValue(id, out TermVector indexVector)) {
                            indexVector.AddAll(post.Value);
                        } else {
                            postIndex.Add(id, post.Value);
                        }
                    } else {
                        index.Add(term, postingsList);
                        break;
                    }
                }
            }
        }

    }
}


