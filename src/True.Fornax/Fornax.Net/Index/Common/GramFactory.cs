// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-05-2017
// ***********************************************************************
// <copyright file="GramFactory.cs" company="Microsoft">
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
using Fornax.Net.Analysis.Tools;
using Fornax.Net.Util;
using Fornax.Net.Util.IO.Writers;
using Fornax.Net.Util.System;

/// <summary>
/// The Common namespace.
/// </summary>
namespace Fornax.Net.Index.Common
{

    /// <summary>
    /// GramFactory for hndling K-Grams, N--gram and Vgrams of a text,
    /// GramIndex generation and retrieval.
    /// </summary>
    public static class GramFactory
    {
        private static readonly DirectoryInfo fornaxGrams = Constants.GetCurrentDirectory("grams");
        private static readonly string bigramPath = Path.Combine(Constants.BaseDirectory.FullName, "grams", "_02.gmx");
        private static readonly string trigramPath = Path.Combine(Constants.BaseDirectory.FullName, "grams", "_03.gmx");
        private static readonly string quadrogramPath = Path.Combine(Constants.BaseDirectory.FullName, "grams", "_04.gmx");

        private static GramIndex bigram;
        private static GramIndex quadgram;
        private static GramIndex trigram;

        /// <summary>
        /// Gets the default bi gram.
        /// </summary>
        /// <value>The default bi gram.</value>
        public static GramIndex Default_BiGram => bigram ?? (bigram = ReadGram(bigramPath));
      
        /// <summary>
        /// Gets the default tri gram.
        /// </summary>
        /// <value>The default tri gram.</value>
        public static GramIndex Default_TriGram => trigram ?? (trigram = ReadGram(trigramPath));
     
        /// <summary>
        /// Gets the default quadro gram.
        /// </summary>
        /// <value>The default quadro gram.</value>
        public static GramIndex Default_QuadroGram => quadgram ?? (quadgram = ReadGram(quadrogramPath));

        /// <summary>
        /// Reads the gram.
        /// </summary>
        /// <param name="pathTogramfile">The path togramfile.</param>
        /// <returns>GramIndex.</returns>
        public static GramIndex ReadGram(string pathTogramfile)
        {
            return FornaxWriter.ReadAsync<GramIndex>(pathTogramfile).Result;
        }

        /// <summary>
        /// Initializes the default grams.
        /// </summary>
        /// <param name="bound">The bound.</param>
        public static void InitDefaultGrams(uint bound)
        {
            bound = (bound < 2) ? 2 : (bound > 4) ? 4 : bound;

            for (uint i = 2; i <= bound; i++)
            {
                var gram = GetNgramIndex(ConfigFactory.GetVocabulary(FornaxLanguage.English).Dictionary, i, NgramModel.Character);
                string path = (i == 2) ? bigramPath : (i == 3) ? trigramPath : quadrogramPath;
                Task.WaitAll(FornaxWriter.WriteAsync(gram, path));
            }
          
            
        }

        /// <summary>
        /// Gets the index of the k gram.
        /// </summary>
        /// <param name="tokenStream">The token stream.</param>
        /// <param name="size">The size.</param>
        /// <param name="model">The model.</param>
        /// <returns>GramIndex.</returns>
        /// <exception cref="ArgumentNullException">tokenStream</exception>
        public static GramIndex GetNgramIndex(TokenStream tokenStream, uint size, NgramModel model)
        {
            if (tokenStream == null) throw new ArgumentNullException(nameof(tokenStream));
            GramIndex index = new GramIndex();

            while (tokenStream.MoveNext())
            {
                var token = tokenStream.Current;
                var kgrams = new Ngram(token.Value, size, model, true).Grams;
                foreach (var gram in kgrams)
                {
                    Add(gram, token.Value, ref index);
                }
            }
            tokenStream.Reset();
            return index;
        }

        /// <summary>
        /// Gets the index of the k gram.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <param name="size">The size.</param>
        /// <param name="model">The model.</param>
        /// <returns>GramIndex.</returns>
        /// <exception cref="ArgumentNullException">words</exception>
        public static GramIndex GetNgramIndex(IEnumerable<string> words, uint size, NgramModel model)
        {
            if (words == null) throw new ArgumentNullException(nameof(words));
            GramIndex index = new GramIndex();
            foreach (var word in words)
            {
                var kgrams = new Ngram(word, size, model, true).Grams;
                foreach (var gram in kgrams)
                {
                    Add(gram, word, ref index);
                }
            }
            return index;
        }

        private static void Add(string key, string value, ref GramIndex index)
        {
            if (index.TryGetValue(key, out SortedSet<string> vals))
            {
                vals.Add(value);
            }
            else
            {
                index.Add(key, new SortedSet<string>() { { value } });
            }
        }

        /// <summary>
        /// Retrieves a sub index of the given k-gram from a similar Gram-Index.
        /// The "similar" gram-index is determined by the size of the grams in the index.
        /// e.g In other to avoid retrieval mismatch: <br>
        /// k-gram of size (2) should be the input into a gram-index of gram-size (2).
        /// </br>
        /// </summary>
        /// <param name="ngram">The kgram.</param>
        /// <param name="mainIndex">Index of the main.</param>
        /// <returns>GramIndex of all grams in <paramref name="ngram" />.</returns>
        public static GramIndex SubGramIndex(Ngram ngram, GramIndex mainIndex)
        {
            var _sub = new GramIndex();
            foreach (var gram in ngram.Grams)
            {
                if (mainIndex.TryGetValue(gram, out SortedSet<string> _gramSet))
                {
                    _sub.Add(gram, _gramSet);
                }
            }
            return _sub;
        }

        /// <summary>
        /// Merges two Gram Index of similar Gram-size.
        /// </summary>
        /// <param name="firstIndex">The first index.</param>
        /// <param name="secondIndex">Index of the second.</param>
        /// <returns>GramIndex.</returns>
        public static GramIndex Merge(GramIndex firstIndex, GramIndex secondIndex)
        {
            var _out = new GramIndex();

            foreach (var item in firstIndex)
            {
                if (!_out.Contains(item))
                {
                    _out.Add(item);
                }
            }
            foreach (var item in secondIndex)
            {
                if (!_out.Contains(item))
                {
                    _out.Add(item);
                }
            }
            return _out;
        }


        /// <summary>
        /// Returns the Intersection of all the k-gram/ngram postings of the index.
        /// </summary>
        /// <param name="index">The index to be shrunk</param>
        /// <returns>A Set of All similar words in the index.</returns>
        public static IEnumerable<string> IntersectOf(GramIndex index)
        {
            IEnumerable<string> set = new SortedSet<string>();
            int n = index.Count; var posts = index.Values.ToList();
            for (int i = 0; i < n - 1; i++)
            {
                var intr = posts[i].Intersect(posts[i + 1]);
                set = set.Union(intr);
            }

            return set;
        }


        /// <summary>
        /// Writes the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="indexFile">The index file.</param>
        /// <exception cref="ArgumentNullException">indexFile</exception>
        public static void Write(GramIndex index, FileInfo indexFile)
        {
            Contract.Requires(index != null && indexFile != null);
            if (indexFile == null) throw new ArgumentNullException(nameof(indexFile));

        }

        /// <summary>
        /// Reads the specified index file.
        /// </summary>
        /// <param name="indexFile">The index file.</param>
        /// <param name="AsAsync">if set to <c>true</c> [as asynchronous].</param>
        /// <returns>GramIndex.</returns>
        public static GramIndex[] Read(FileInfo indexFile, bool AsAsync)
        {
            Contract.Requires(indexFile != null);
            GramIndex[] g = null;
            if (AsAsync)
            {
                g = FornaxWriter.BufferReadAsync<GramIndex>(indexFile).Result;
            }
            else
            {
                g = FornaxWriter.BufferRead<GramIndex>(indexFile);
            }
            Contract.Ensures(g != null);
            return g;
        }

        static GramFactory()
        {
         
        }

    }
}
