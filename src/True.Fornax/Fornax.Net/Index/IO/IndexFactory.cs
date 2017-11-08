////// ***********************************************************************
////// Assembly         : Fornax.Net
////// Author           : Habuto Koudura
////// Created          : 09-25-2017
//////
////// Last Modified By : Habuto Koudura
////// Last Modified On : 11-01-2017
////// ***********************************************************************
////// <copyright file="IndexFactory.cs" company="True.Inc">
/////***
////* Copyright (c) 2017 Koudura Ninci @True.Inc
////*
////* Permission is hereby granted, free of charge, to any person obtaining a copy
////* of this software and associated documentation files (the "Software"), to deal
////* in the Software without restriction, including without limitation the rights
////* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
////* copies of the Software, and to permit persons to whom the Software is
////* furnished to do so, subject to the following conditions:
////* 
////* The above copyright notice and this permission notice shall be included in all
////* copies or substantial portions of the Software.
////* 
////* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
////* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
////* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
////* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
////* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
////* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
////* SOFTWARE.
////*
////**/
////// </copyright>
////// <summary></summary>
////// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;

using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Document;
using Fornax.Net.Index.Storage;
using Fornax.Net.Search;
using Fornax.Net.Util;
using Fornax.Net.Util.IO.Writers;
using Fornax.Net.Util.Linq;
using Fornax.Net.Util.Security.Cryptography;

/// <summary>
/// The IO namespace.
/// </summary>
namespace Fornax.Net.Index.IO
{
    /// <summary>
    /// Class IndexFactory.
    /// </summary>
    [Serializable]
    public class IndexFactory
    {
        /// <summary>
        /// The inverted index
        /// </summary>
        /// <value>The index.</value>
        public InvertedFile Index { get; private set; }
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public Repository Repository { get; private set; }
        /// <summary>
        /// The configuration
        /// </summary>
        /// <value>The configuration.</value>
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexFactory" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="ArgumentNullException">file</exception>
        public IndexFactory(Configuration config)
        {
            Contract.Requires(config != null);
            Configuration = config ?? throw new ArgumentNullException(nameof(config));

            var load = LoadAsync(config).Result;

            Repository = load.Repository;
            Index = load.Index;
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
                    index.Add(term, new Postings(DocID, token.Start));
                }
                else
                {
                    if (!posting.TryGetValue(DocID, out Vector vector))
                    {
                        posting.Add(DocID, new Vector(token.Start));
                    }
                    else vector.Value.Add(token.Start);
                }
            }
        }

        /// <summary>
        /// Add or the updates the specified inverted index with a given vector.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="vector">The document term-vector to be updated to the index</param>
        /// <param name="index">The index to be updated.</param>
        /// <param name="config">The configuration in scope.</param>
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
                    if (posting.TryGetValue(DocId, out Vector docVector))
                    {
                        posting[DocId] = vector[v.Key];
                    }
                    else
                    {
                        posting.Add(DocId, vector[v.Key]);
                    }
                }
                else
                {
                    index.Add(v.Key, new Postings(DocId, vector[v.Key]));
                }
            }
        }

        /// <summary>
        /// Asynchronously adds or the updates the specified inverted index with a given vector.
        /// </summary>
        /// <param name="DocID">The document identifier.</param>
        /// <param name="vector">The document term-vector to be updated to the index</param>
        /// <param name="index">The index.</param>
        /// <param name="config">The configuration in scope.</param>
        /// <returns>Task.</returns>
        public async static Task AddAsync(ulong DocID, TermVector vector, InvertedFile index, Configuration config)
        {
            await Task.Factory.StartNew(() => AddorUpdate(DocID, vector, index, config));
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
            var corp = repository.Corpora;
            foreach (var doc in corp)
            {
                Add(doc, index, repository.Configuration);
            }
        }

        #region deletion
        /// <summary>
        /// Deletes the specified document from a specified index.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="index">The index.</param>
        /// <param name="repository">The repository of the index.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">index
        /// or
        /// document</exception>
        public static bool Delete(FileInfo document,InvertedFile index, Repository repository)
        {
            Contract.Requires(document != null && index != null && repository != null);
            if (index == null) throw new ArgumentNullException(nameof(index));
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            return Delete(Adler32.Compute(document.FullName), index, repository);
        }

        /// <summary>
        /// Deletes the specified document from a specified index.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentNullException">index
        /// or
        /// document</exception>
        public static void Delete(IDocument document, InvertedFile index, Repository repository)
        {
            Contract.Requires(document != null && index != null);
            if (index == null) throw new ArgumentNullException(nameof(index));
            if (document == null) throw new ArgumentNullException(nameof(document));

            Delete(document.ID, index, repository);
        }

        /// <summary>
        /// Removes the specified document identifier.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="index">The index.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">index</exception>
        public static bool Delete(ulong DocId, InvertedFile index, Repository repo)
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
            if (repo.Corpus.ContainsKey(DocId))
            {
                repo.Corpus.Remove(DocId);
            }
            return found;
        }

        /// <summary>
        /// Asynchronously deletes a document from a specified index.
        /// </summary>
        /// <param name="document">The document to be deleted from index.</param>
        /// <param name="index">The index to be updated.</param>
        /// <returns>Task.</returns>
        public static async Task<bool> DeleteAsync(IDocument document, InvertedFile index, Repository repository)
        {
            return await Task.Factory.StartNew(() => Delete(document.ID, index, repository));
        }

        /// <summary>
        /// Asynchronously deletes a document with specified ID from a specified index.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public static async Task<bool> DeleteAsync(ulong DocId, InvertedFile index, Repository repo)
        {
            return await Task.Factory.StartNew(() => Delete(DocId, index, repo));
        }

        /// <summary>
        /// Wipes the specified document from the specified user config repository
        /// , corpus and index. If no index or repository is found in configuration 
        ///  no wiping occurs has there is nothing found to wipe.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="config">The configuration which holds all repository and index of current working user.</param>
        public static bool Wipe(IDocument document, Configuration config)
        {
            if (document == null || config == null) throw new ArgumentNullException("null values are disallowed.");
            if (config.WorkingDirectory.Exists)
            {
                if (IndexRepoExists(Path.Combine(config.WorkingDirectory.FullName, "_.inx"), Path.Combine(config.WorkingDirectory.FullName, "_.repx")))
                {
                    var load = LoadAsync(config).Result;
                    var repository = load.Repository;
                    var index = load.Index;
                    return DeleteAsync(document, index, repository).Result;
                }
            }
            return false;
        }
        #endregion

        #region update
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
                Update(ref currentIndex, _termpost.Value, _termpost.Key);
            }
        }

        /// <summary>
        /// Merges the specified postings list into the given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="postingsList">The postings list.</param>
        /// <exception cref="ArgumentNullException">index
        /// or
        /// postingsList</exception>
        public static void Update(ref InvertedFile index, Postings postingsList, Term term)
        {
            Contract.Requires(postingsList != null && index != null);

            if (index.TryGetValue(term, out Postings ind_post))
            {
                foreach (var p_id in postingsList)
                {
                    if (ind_post.TryGetValue(p_id.Key, out Vector vect))
                    {
                        ind_post[p_id.Key] = p_id.Value;
                    }
                    else
                    {
                        ind_post.Add(p_id.Key, p_id.Value);
                    }
                }
            }
            else
            {
                index.Add(term, postingsList);
            }

        }

        /// <summary>
        /// Updates the specified currrent repository with a new repository.
        /// </summary>
        /// <param name="currrentRepo">The currrent repo.</param>
        /// <param name="newRepo">The new repo.</param>
        /// <exception cref="ArgumentNullException">
        /// currrentRepo
        /// or
        /// newRepo
        /// </exception>
        public static void Update(ref Repository currrentRepo, Repository newRepo)
        {
            if (currrentRepo == null) throw new ArgumentNullException(nameof(currrentRepo));
            if (newRepo == null) throw new ArgumentNullException(nameof(newRepo));

            foreach (var nrp in newRepo.Corpus)
            {

                if (currrentRepo.Corpus.TryGetValue(nrp.Key, out string value))
                {
                    value = nrp.Value;
                }
                else
                {
                    currrentRepo.Corpus.Add(nrp.Key, nrp.Value);
                }
            }
            foreach (var snips in newRepo.Snippets)
            {
                if(currrentRepo.Snippets.TryGetValue(snips.Key, out Snippet snip))
                {
                    snip = snips.Value;
                }
                else
                {
                    currrentRepo.Snippets.Add(snips.Key, snips.Value);
                }
            }
        }
        #endregion

        internal static bool IndexRepoExists(string indexPath, string repoPath)
        {
            return File.Exists(repoPath) && File.Exists(indexPath);
        }

        /// <summary>
        /// Saves the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="index">The index.</param>
        public static void Save(Configuration config, InvertedFile index, Repository repo)
        {
            var inxFile = new FileInfo(Path.Combine(config.WorkingDirectory.FullName, "_.inx"));
            FornaxWriter.Write(repo, repo.RepositoryFile);
            FornaxWriter.Write(index, inxFile);
        }


        /// <summary>
        /// save as an asynchronous operation.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public static async Task SaveAsync(Configuration config, InvertedFile index, Repository repo)
        {
            var inxFile = new FileInfo(Path.Combine(config.WorkingDirectory.FullName, "_.inx"));
            await FornaxWriter.WriteAsync(repo, repo.RepositoryFile);
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
        /// Asynchronously loads the index and repository of the specified configuration.
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
        /// Loads the index and repository of the specified configuration using the config ID.
        /// </summary>
        /// <param name="configID">The configuration identifier.</param>
        /// <returns>System.ValueTuple.InvertedFile.Repository.</returns>
        public static (InvertedFile Index, Repository Repository) Load(string configID)
        {
            var str = string.Format(@"User[{0}].config", configID);
            var dir_str = Path.Combine(Constants.BaseDirectory.FullName, str);
            if (Directory.Exists(dir_str))
            {
                var inx = FornaxWriter.Read<InvertedFile>(Path.Combine(dir_str, "_.inx"));
                var repo = FornaxWriter.Read<Repository>(Path.Combine(dir_str, "_.repx"));
                return (inx, repo);
            }
            return (null, null);
        }

    }
}


