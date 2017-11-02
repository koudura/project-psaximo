using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Analysis;
using Fornax.Net.Analysis.Tokenization;
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


        private TokenStream _tokens;
        private IList<string> _terms;

        private TokenStream Tokens => _tokens;

        protected FornaxQuery(string query, Analyzer analyzer, Repository repo, Expand expansion) {
            this.query = query;
            this.analyzer = analyzer;
            this.repo = repo;
            this.expansion = expansion;
            queryMode = Analyzer.GetMode(analyzer);
        }

        private IList<string> Terms => _terms = ProcessTerms();

        private IList<string> ProcessTerms() {
            return query.Split().ToList();
        }
    }
}
