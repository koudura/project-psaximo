using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Analysis.Tools;

namespace Fornax.Net.Index.Common
{
    public static class GramFactory
    {
        /// <summary>
        /// Gets the index of the k gram.
        /// </summary>
        /// <param name="tokenStream">The token stream.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">tokenStream</exception>
        public static GramIndex GetKGramIndex(TokenStream tokenStream, uint size) {
            if (tokenStream == null) throw new ArgumentNullException(nameof(tokenStream));
            GramIndex index = new GramIndex();

            while (tokenStream.MoveNext()) {
                var token = tokenStream.Current;
                var kgrams = new KGram(token, size).Grams;
                foreach (var gram in kgrams) {
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
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">words</exception>
        public static GramIndex GetKGramIndex(IEnumerable<string> words, uint size) {
            if (words == null) throw new ArgumentNullException(nameof(words));
            GramIndex index = new GramIndex();

            foreach (var word in words) {
                var kgrams = new KGram(word, size).Grams;
                foreach (var gram in kgrams) {
                    Add(gram, word, ref index);
                }
            }
            return index;
        }


        private static void Add(string key, string value, ref GramIndex index) {
            if (index.ContainsKey(key)) {
                index[key].Add(value);
            } else {
                index.Add(key, new HashSet<string> { { value } });
            }
        }
    }
}
