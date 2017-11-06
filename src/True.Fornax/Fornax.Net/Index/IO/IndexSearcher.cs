// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-06-2017
// ***********************************************************************
// <copyright file="IndexSearcher.cs" company="Microsoft">
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
using Fornax.Net.Document;
using Fornax.Net.Search;
using Fornax.Net.Util.Linq;
using Repository = Fornax.Net.Index.Storage.Repository;

/// <summary>
/// The IO namespace.
/// </summary>
namespace Fornax.Net.Index.IO
{
    /// <summary>
    /// Class IndexSearcher.
    /// </summary>
    [Serializable]
    public class IndexSearcher
    {
        private FornaxQuery _query;
        private Repository _repo;

        private ulong N;
        private uint _max;

        private InvertedFile sub_index;
        private Corpus sub_corpus;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexSearcher"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="index">The index.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="max">The maximum.</param>
        /// <exception cref="ArgumentNullException">A parameter as been deemed null</exception>
        public IndexSearcher(FornaxQuery query, InvertedFile index, Repository repository, uint max = 10)
        {
            Contract.Requires(query != null && index != null && repository != null);
            if (query == null || index == null || repository == null)
            {
                throw new ArgumentNullException("A parameter as been deemed null");
            }
            _query = query;
            _max = max;
            _repo = repository;
            var r = Retrieve(index, repository.Corpus).Result;

            sub_index = r.SubIndex;
            sub_corpus = r.SubCorpus;
            N = (ulong)repository.Corpus.Count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexSearcher"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="max">The maximum.</param>
        public IndexSearcher(FornaxQuery query, IndexFactory factory, uint max = 10)
            : this(query, factory.Index, factory.Repository, max) { }

        private async Task<(InvertedFile SubIndex, Corpus SubCorpus)> Retrieve(InvertedFile main, Corpus corpus)
        {
            var ind = new InvertedFile();
            Corpus corp = new Corpus();

            await Task.Run(() =>
            {
                foreach (var term in _query.Terms.Keys)
                {
                    if (main.TryGetValue(term, out Postings posting))
                    {
                        ind.Add(term, posting);
                        foreach (var id in posting.Keys)
                        {
                            if (!corp.ContainsKey(id) && corpus.ContainsKey(id)) corp.Add(id, corpus[id]);
                        }
                    }
                }
            });
            return (ind, corp);
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <returns>HitList.</returns>
        internal HitList GetResult()
        {
            var hits = new HitList();

            foreach (var t_p in sub_index)
            {
                foreach (var post in t_p.Value)
                {
                    if (hits.TryGetValue(post.Key, out double score))
                    {
                        score += Weight.TfxIdf((ulong)post.Value.Value.Count, N, (ulong)t_p.Value.Count);
                    }
                    else
                    {
                        hits.Add(post.Key, Weight.TfxIdf((ulong)post.Value.Value.Count, N, (ulong)t_p.Value.Count));
                    }
                }
            }
            return hits;
        }

        /// <summary>
        /// Gets the term frequency.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="post">The post.</param>
        /// <returns>System.UInt64.</returns>
        internal ulong GetTermFrequency(Term term, Postings post)
        {
            ulong count = 0;
            foreach (var item in post)
            {

            }

            return count;
        }

        /// <summary>
        /// Gets the query scores.
        /// </summary>
        /// <returns>Vector.</returns>
        internal Vector GetQueryScores()
        {
            IList<double> vec = new List<double>();
            foreach (var query_term in sub_index)
            {
                vec.Add(Weight.Idf(N, (ulong)query_term.Value.Count));
            }
            return new Vector(vec);
        }

        /// <summary>
        /// Gets the t fs.
        /// </summary>
        /// <param name="corpe">The corpe.</param>
        /// <returns>IDictionary&lt;System.UInt64, Vector&gt;.</returns>
        internal IDictionary<ulong, Vector> GetTFs(Corpus corpe)
        {
            //Dictionary<decimal, Vector> docVector = new Dictionary<decimal, Vector>();
            //for (int i = 0; i < query.Terms.Count; i++)
            //{
            //    foreach (decimal d in docList)
            //    {
            //        if (index[query.Terms[i]].ContainsKey(d))
            //        {
            //            docVector[d].Values.Add(index[query.Terms[i]][d].Count); // divide by number of terms in document
            //        }
            //        else
            //        {
            //            docVector[d].Values.Add(0);
            //        }

            //    }
            //}
            //return docVector;
            // IDictionary<ulong,Vector>

            return null;
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <param name="sortRule">The sort rule.</param>
        /// <returns>IEnumerable&lt;DocResult&gt;.</returns>
        public IEnumerable<DocResult> GetResults(SortBy sortRule)
        {
            var acquired = GetSorter(sortRule);
            acquired.AddAll(GetResults(GetResult(), sub_corpus));
            return acquired;
        }

        private IEnumerable<DocResult> GetResults(HitList hitList, Corpus corp)
        {
            foreach (var hit in hitList)
            {
                if (corp.TryGetValue(hit.Key, out string name))
                {
                    yield return new DocResult(name, hit.Value);
                }
            }
        }

        private SortedSet<DocResult> GetSorter(SortBy sortRule)
        {
            switch (sortRule)
            {
                case SortBy.Relevance:
                    return new SortedSet<DocResult>(new Ranker.ByRelevance());
                case SortBy.Modified:
                    return new SortedSet<DocResult>(new Ranker.ByModified());
                case SortBy.Date:
                    return new SortedSet<DocResult>(new Ranker.ByDate());
                case SortBy.Name:
                    return new SortedSet<DocResult>(new Ranker.ByName());
                case SortBy.Size:
                    return new SortedSet<DocResult>(new Ranker.BySize());
                default:
                    return new SortedSet<DocResult>();
            }
        }
    }
}
