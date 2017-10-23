using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Query;

namespace Fornax.Net.Analysis
{
    class AdvancedAnalyzer : Analyzer
    {
        private SearchMode modeOfsearch;

        public AdvancedAnalyzer(SearchMode modeOfsearch) {
            this.modeOfsearch = modeOfsearch;
        }



    }
}
