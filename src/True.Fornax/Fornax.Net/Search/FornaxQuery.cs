using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Analysis;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Index;
using Fornax.Net.Index.Storage;

namespace Fornax.Net.Search
{
    public abstract class FornaxQuery
    {
        protected readonly string query;
        protected Analyzer analyzer;
        protected readonly Configuration config;


        protected TokenStream _tokens;
        protected TermVector terms;

        private TokenStream Tokens => _tokens;

        public virtual TermVector Terms => terms;

        public virtual QueryType Type => analyzer.QueryType;

        protected FornaxQuery(Analyzer analyzer, Configuration config)
        {
            this.analyzer = analyzer;
            this.config = config;
        }


    }
}
