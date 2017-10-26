using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Query;

namespace Fornax.Net.Analysis
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Analyzer
    {
        protected SearchMode modeOfsearch;
        internal virtual Explanation Rule => GetExplanation(this);
        internal virtual QueryType TypeOfQuery { get; private set; }

        internal static QueryMode GetMode(Analyzer anna) {
            if (anna is AdvancedAnalyzer) return QueryMode.Advanced;
            else if (anna is StandardAnalyzer) return QueryMode.Standard;
            else return QueryMode.Simple;
        }

        private Explanation GetExplanation(Analyzer anal) {
            return new Explanation(GetMode(anal), modeOfsearch);
        }
        
    }
}
