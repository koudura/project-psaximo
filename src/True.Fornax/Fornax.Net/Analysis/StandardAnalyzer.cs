using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Search;

namespace Fornax.Net.Analysis
{
    class StandardAnalyzer : Analyzer
    {

        public StandardAnalyzer(string query, SearchMode modeOfsearch, Expand expansionRule)
            : base(query, modeOfsearch, expansionRule)
        {

        }
    }
}
