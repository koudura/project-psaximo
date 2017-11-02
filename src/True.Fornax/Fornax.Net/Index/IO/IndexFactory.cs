// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Habuto Koudura
// Created          : 09-25-2017
//
// Last Modified By : Habuto Koudura
// Last Modified On : 11-01-2017
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
using System.Threading.Tasks;

using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Document;
using Fornax.Net.Index.Storage;
using Fornax.Net.Util.IO.Writers;
using Fornax.Net.Util.Linq;
using Fornax.Net.Util;

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
        /// Initializes a new instance of the <see cref="IndexFactory" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="ArgumentNullException">file</exception>
        public IndexFactory(FileInfo file, Configuration config)
        {
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
        public static async Task<Corpus> GetCorpus(FileInfo file)
        {
            return await FornaxWriter.BufferReadAsync<Corpus>(file);
        }

        /// <summary>
        /// Gets the inverted file from disk.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Task&lt;InvertedFile&gt;.</returns>
        public static async Task<InvertedFile> GetInvertedFile(FileInfo file)
        {
            return await FornaxWriter.BufferReadAsync<InvertedFile>(file);
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
        public static void Add(ulong DocID, TokenStream stream, InvertedFile index, Configuration config)
        {
            Contract.Requires(stream != null && index != null);
            if (stream == null || index == null) throw new ArgumentNullException(nameof(index) + " || " + nameof(stream));
            if (config == null) throw new ArgumentNullException(nameof(config));


            while (stream.MoveNext())
            {
                var token = stream.Current;
                var term = new Term(token, config.Language);

                if (!index.TryGetValue(term, out Postings posting))
                {
                    index.Add(term, new Postings(term, DocID, token.Start));
                }
                else
                {
                    if (!posting.TryGetValue(DocID, out TermVector vector))
                    {
                        posting.Add(DocID, new TermVector(term, token.Start));
                    }
                    else vector[term].Value.Add(token.Start);
                }
            }
        }

        /// <summary>
        /// Add or the updates the specified inverted index with a given vector.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="vector">The vector.</param>
        /// <param name="index">The index.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="ArgumentNullException">index
        /// or
        /// config</exception>
        public static void AddorUpdate(ulong DocId, TermVector vector, InvertedFile index, Configuration config)
        {
            Contract.Requires(vector != null && index != null);
            if (vector == null || index == null) throw new ArgumentNullException(nameof(index) + " || " + nameof(vector));
            if (config == null) throw new ArgumentNullException(nameof(config));

            foreach (var v in vector)
            {
                if (index.TryGetValue(v.Key, out Postings posting))
                {
                    if (posting.TryGetValue(DocId, out TermVector docVector))
                    {
                        posting[DocId] = vector;
                    }
                    else
                    {
                        posting.Add(DocId, vector);
                    }
                }
                else
                {
                    index.Add(v.Key, new Postings(DocId, vector));
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
        public async static Task AddAsync(ulong DocID, TermVector stream, InvertedFile index, Configuration config)
        {
            await Task.Factory.StartNew(() => AddorUpdate(DocID, stream, index, config));
        }

        /// <summary>
        /// add as an asynchronous operation.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="index">The index.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Task.</returns>
        public async static Task AddAsync(IDocument document, InvertedFile index, Configuration config)
        {
            await Task.Factory.StartNew(function: async () => await AddAsync(document.ID, document.Terms, index, config));
        }

        /// <summary>
        /// add as an asynchronous operation.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public async static Task AddAsync(Repository repository, InvertedFile index)
        {
            await Task.Factory.StartNew(() => Add(repository, index));
        }

        /// <summary>
        /// Adds the specified document to an existing inverted index.
        /// </summary>
        /// <param name="document">The document to be added.</param>
        /// <param name="index">The existing inverted index to be updated.</param>
        /// <param name="config">The configuration of this runtime.</param>
        public static void Add(IDocument document, InvertedFile index, Configuration config)
        {
            Contract.Requires(document != null && index != null && config != null);
            AddorUpdate(document.ID, document.Terms, index, config);
        }

        /// <summary>
        /// Adds the specified repository to the given inverted index.
        /// </summary>
        /// <param name="repository">The existing repository to add to index.</param>
        /// <param name="index">The index to be Updated.</param>
        public static void Add(Repository repository, InvertedFile index)
        {
            foreach (var doc in repository.EnumerateDocuments())
            {
                Add(doc, index, repository.Configuration);
            }
        }


        #region deletion

        /// <summary>
        /// Removes the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentNullException">index
        /// or
        /// document</exception>
        public static void Delete(IDocument document, InvertedFile index)
        {
            Contract.Requires(document != null && index != null);
            if (index == null) throw new ArgumentNullException(nameof(index));
            if (document == null) throw new ArgumentNullException(nameof(document));

            Delete(document.ID, index);
        }

        /// <summary>
        /// Removes the specified document identifier.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="index">The index.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">index</exception>
        public static bool Delete(ulong DocId, InvertedFile index)
        {
            Contract.Requires(index != null);
            if (index == null) throw new ArgumentNullException(nameof(index));
            bool found = false;

            foreach (var _termpost in index)
            {
                if (_termpost.Value.ContainsKey(DocId))
                {
                    _termpost.Value.Remove(DocId);
                    found = true;
                    if (_termpost.Value.Count <= 0)
                        index.Remove(_termpost.Key);
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// remove as an asynchronous operation.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public static async Task DeleteAsync(IDocument document, InvertedFile index)
        {
            await Task.Factory.StartNew(async () => await DeleteAsync(document.ID, index));
        }

        /// <summary>
        /// remove as an asynchronous operation.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public static async Task DeleteAsync(ulong DocId, InvertedFile index)
        {
            await Task.Factory.StartNew(() => Delete(DocId, index));
        }

        #endregion


        #region merging
        /// <summary>
        /// Updates the current inverted file with the new file, i.e the <paramref name="newIndex" /> is merged
        /// into the currentIndex given.
        /// </summary>
        /// <param name="currentIndex">Existing current inverted file.</param>
        /// <param name="newIndex">new inverted file to be merged.</param>
        /// <exception cref="ArgumentNullException">An index deemed null...is not acceptable</exception>
        public static void Update(ref InvertedFile currentIndex, InvertedFile newIndex)
        {
            Contract.Requires(currentIndex != null && newIndex != null);
            if (currentIndex == null || newIndex == null) throw new ArgumentNullException("An index deemed null...is not acceptable");

            foreach (var _termpost in newIndex)
            {
                if (currentIndex.TryGetValue(_termpost.Key, out Postings _currpost))
                {
                    foreach (var _newpost in _termpost.Value)
                    {
                        if (_currpost.ContainsKey(_newpost.Key))
                        {
                            _currpost[_newpost.Key] = _newpost.Value;
                        }
                        else
                        {
                            _currpost.Add(_newpost.Key, _newpost.Value);
                        }
                    }
                }
                else
                {
                    currentIndex.Add(_termpost.Key, _termpost.Value);
                }
            }
        }

        /// <summary>
        /// Merges the specified postings list.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="postingsList">The postings list.</param>
        /// <exception cref="ArgumentNullException">index
        /// or
        /// postingsList</exception>
        [Obsolete("Probably a hacky code.", true)]
        public static void Update(ref InvertedFile index, Postings postingsList)
        {
            Contract.Requires(postingsList != null && index != null);
            if (index == null) throw new ArgumentNullException(nameof(index));
            if (postingsList == null) throw new ArgumentNullException(nameof(postingsList));

            foreach (var _postDoc in postingsList.Values)
            {
                foreach (var _tv in _postDoc)
                {
                    if (index.ContainsKey(_tv.Key))
                    {
                        index[_tv.Key] = postingsList;
                    }
                    else
                    {
                        index.Add(_tv.Key,postingsList);
                    }
                }
            }



            foreach (KeyValuePair<ulong, TermVector> post in postingsList)
            {
                //var id = post.Key;
                //foreach (var vector in post.Value)
                //{
                //    var term = vector.Key;
                //    if (index.TryGetValue(term, out Postings postIndex))
                //    {
                //        if (postIndex.TryGetValue(id, out TermVector indexVector))
                //        {
                //            indexVector.AddAll(post.Value);
                //        }
                //        else
                //        {
                //            postIndex.Add(id, post.Value);
                //        }
                //    }
                //    else
                //    {
                //        index.Add(term, postingsList);
                //        break;
                //    }
                //}
            }
        }
        #endregion

        /// <summary>
        /// Saves the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="index">The index.</param>
        public static void Save(Configuration config, InvertedFile index)
        {
            var inxFile = new FileInfo(Path.Combine(config.WorkingDirectory.FullName, "_.inx"));
            FornaxWriter.Write(index, inxFile);
        }

        /// <summary>
        /// save as an asynchronous operation.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public static async Task SaveAsync(Configuration config, InvertedFile index)
        {
            var inxFile = new FileInfo(Path.Combine(config.WorkingDirectory.FullName, "_.inx"));
            await FornaxWriter.WriteAsync(index, inxFile);
        }

        /// <summary>
        /// Loads the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>System.ValueTuple.InvertedFile.Repository.</returns>
        public static (InvertedFile Index, Repository Repository) Load(Configuration config)
        {
            var inx = FornaxWriter.Read<InvertedFile>(Path.Combine(config.WorkingDirectory.FullName, "_.inx"));
            var repo = FornaxWriter.Read<Repository>(Path.Combine(config.WorkingDirectory.FullName, "_.repx"));
            return (inx, repo);
        }

        /// <summary>
        /// load as an asynchronous operation.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>Task&lt;System.ValueTuple.InvertedFile.Repository&gt;.</returns>
        public static async Task<(InvertedFile Index, Repository Repository)> LoadAsync(Configuration config)
        {
            var inx = await FornaxWriter.ReadAsync<InvertedFile>(Path.Combine(config.WorkingDirectory.FullName, "_.inx"));
            var repo = await FornaxWriter.ReadAsync<Repository>(Path.Combine(config.WorkingDirectory.FullName, "_.repx"));
            return (inx, repo);
        }

        /// <summary>
        /// Loads the specified configuration identifier.
        /// </summary>
        /// <param name="configID">The configuration identifier.</param>
        /// <returns>System.ValueTuple.InvertedFile.Repository.</returns>
        public static (InvertedFile Index, Repository Repository) Load(string configID)
        {
            var str = string.Format(@"User[{0}].config", configID);
            var inx = FornaxWriter.Read<InvertedFile>(Path.Combine(Constants.BaseDirectory.FullName, str, "_.inx"));
            var repo = FornaxWriter.Read<Repository>(Path.Combine(Constants.BaseDirectory.FullName, str, "_.repx"));
            return (inx, repo);
        }


    }
}


