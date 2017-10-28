using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Analysis;
using Fornax.Net.Index.Storage;

namespace Fornax.Net.Query
{
    public abstract class FornaxQuery
    {
        private string query;
        private Analyzer analyzer;
        private Repository repo;
        private Expand expansion;
        private QueryMode queryMode;

       protected FornaxQuery(string query, Analyzer analyzer, Repository repo, Expand expansion) {
            this.query = query;
            this.analyzer = analyzer;
            this.repo = repo;
            this.expansion = expansion;
            queryMode = Analyzer.GetMode(analyzer);
        }
        


    }
}
