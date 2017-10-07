using Fornax.Net.Analysis;
using Fornax.Net.Index.Storage;


namespace Fornax.Net.Query
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="FornaxQuery" />
    public class Query : FornaxQuery
    {
  
        private string queryString;
        private Analyzer analyzer;
        private bool expandable;
        private QueryMode Querymode;

        private static QueryType type;

        private Query() { }

        internal Query(string query, Analyzer analyzer, FornaxRepository repo, Expand expansion) {
            this.queryString = query;
            this.analyzer = analyzer;
            this.Querymode = Analyzer.GetMode(analyzer);
            this.expandable = expansion.IsExpandable();

            base.IsExpandable = this.expandable;
        }

        internal string RawQuery =>  this.queryString;
        internal QueryMode Mode => this.Querymode;


        /// <summary>
        /// Creates the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="searchMode">The search mode.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="expansion">The expansion.</param>
        /// <returns></returns>
        public static FornaxQuery Create(string query, QueryMode queryMode, SearchMode searchMode, FornaxRepository repository, Expand expansion = Expand.NO) {       
            return Create(query, GetRelativeAnalyzer(queryMode, searchMode), repository, expansion);
        }

        /// <summary>
        /// Creates the simple.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="expansion">The expansion.</param>
        /// <returns></returns>
        public static FornaxQuery CreateSimple(string query, FornaxRepository repository, Expand expansion = Expand.NO) {
            return Create(query, new SimpleAnalyzer(SearchMode.Free), repository, expansion);
        }

        /// <summary>
        /// Creates the standard.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="expansion">The expansion.</param>
        /// <returns></returns>
        public static FornaxQuery CreateStandard(string query, FornaxRepository repository, Expand expansion = Expand.NO) {
            return Create(query, new StandardAnalyzer(SearchMode.Free), repository, expansion);
        }

        /// <summary>
        /// Creates the advanced.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="expansion">The expansion.</param>
        /// <returns></returns>
        public static FornaxQuery CreateAdvanced(string query, FornaxRepository repository, Expand expansion = Expand.NO) {
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
        public static FornaxQuery Create(string query, Analyzer queryAnalyzer, FornaxRepository repository, Expand expansion = Expand.NO) {
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