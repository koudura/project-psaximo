using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Search;

namespace Fornax.Net.Analysis
{
    public class SimpleAnalyzer : Analyzer
    {
        QueryMode mode = QueryMode.Simple;
        internal QueryType QueryType { get; private set; }

        public SimpleAnalyzer(string query, SearchMode modeOfSearch, Expand modeOfExpansion)
            : base(query, modeOfSearch, modeOfExpansion)
        {

        }

        private void DetectQuery()
        {
            Automaton automata = new Automaton(Query);
            QueryType = automata.GetClass();
        }

    }
}
