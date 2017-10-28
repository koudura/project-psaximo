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

using Fornax.Net.Analysis;
using Fornax.Net.Index.Storage;


namespace Fornax.Net.Query
{
    /// <summary>
    /// Query Representation class.
    /// </summary>
    /// <seealso cref="FornaxQuery" />
    public sealed class Query : FornaxQuery
    {
        private static QueryType type  { get; set; }

        internal Query(string query, Analyzer analyzer, Repository repo, Expand expansion) : base (query,analyzer,repo,expansion) {
        }

        /// <summary>
        /// Creates the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="searchMode">The search mode.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="expansion">The expansion.</param>
        /// <returns></returns>
        public static FornaxQuery Create(string query, QueryMode queryMode, SearchMode searchMode, Repository repository, Expand expansion = Expand.Default) {
            return Create(query, GetRelativeAnalyzer(queryMode, searchMode), repository, expansion);
        }

        /// <summary>
        /// Creates the simple.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="expansion">The expansion.</param>
        /// <returns></returns>
        public static FornaxQuery CreateSimple(string query, Repository repository, Expand expansion = Expand.Default) {
            return Create(query, new SimpleAnalyzer(SearchMode.Free), repository, expansion);
        }

        /// <summary>
        /// Creates the standard.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="expansion">The expansion.</param>
        /// <returns></returns>
        public static FornaxQuery CreateStandard(string query, Repository repository, Expand expansion = Expand.Default) {
            return Create(query, new StandardAnalyzer(SearchMode.Free), repository, expansion);
        }

        /// <summary>
        /// Creates the advanced.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="expansion">The expansion.</param>
        /// <returns></returns>
        public static FornaxQuery CreateAdvanced(string query, Repository repository, Expand expansion = Expand.Default) {
            return Create(query, new AdvancedAnalyzer(SearchMode.Free), repository, expansion);

        }

        /// <summary>
        /// Creates the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="queryAnalyzer">The query analyzer.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="expansion">The expansion.</param>
        /// <returns></returns>
        public static FornaxQuery Create(string query, Analyzer queryAnalyzer, Repository repository, Expand expansion = Expand.Default) {
            type = queryAnalyzer.TypeOfQuery;
            return new Query(query, queryAnalyzer, repository, expansion);
        }

        private static Analyzer GetRelativeAnalyzer(QueryMode mmode, SearchMode modeOfsearch) {
            switch (mmode) {
                case QueryMode.Advanced:
                    return new AdvancedAnalyzer(modeOfsearch);
                case QueryMode.Standard:
                    return new StandardAnalyzer(modeOfsearch);
                case QueryMode.Simple:
                    return new SimpleAnalyzer(modeOfsearch);
                default:
                    return null;
            }
        }
        private QueryType getQueryType(Analyzer an) {
            return an.TypeOfQuery;
        }

    }
}