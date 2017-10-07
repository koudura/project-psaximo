using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Analysis;
using Fornax.Net.Index.Storage;

namespace Fornax.Net.Query
{
    public sealed class BooleanQuery : Query
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanQuery"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="searchMode">The search mode.</param>
        /// <param name="repo">The repo.</param>
        /// <param name="expansion">The expansion.</param>
        internal BooleanQuery(string query,FornaxRepository repo,SearchMode searchMode = SearchMode.Free, Expand expansion = Expand.NO) 
            : base(query, new StandardAnalyzer(searchMode), repo, expansion) {
        }
    }
}
