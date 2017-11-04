using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Search;

namespace Fornax.Net.Analysis
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Analyzer
    {
        protected SearchMode modeOfsearch;
        protected Expand expansionRule;
        protected readonly string _query;

        protected Analyzer(string query,SearchMode modeOfsearch, Expand expansionRule) {
            _query = query;
            this.modeOfsearch = modeOfsearch;
            this.expansionRule = expansionRule;
        }

        public virtual string Query => _query;

        public QueryType QueryType { get; internal set; }
    }
}
