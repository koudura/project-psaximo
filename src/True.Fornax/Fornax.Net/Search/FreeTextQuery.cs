using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Analysis;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Document;
using Fornax.Net.Index;

namespace Fornax.Net.Search
{
    public class FreeTextQuery : FornaxQuery
    {

        public FreeTextQuery(string query, Configuration config)
            : this(new SimpleAnalyzer(query, SearchMode.Free, Expand.Default), config)
        {
        }

        public FreeTextQuery(SimpleAnalyzer analyzer, Configuration config)
            : base(analyzer, config)
        {
            _tokens = GetQueryTokens();
            terms = FSDocument.GetTerms(_tokens, config.Language);
        }

        private TokenStream GetQueryTokens()
        {
            var tokenizer = FSDocument.GetTok(config.Tokenizer, analyzer.Query);
            return tokenizer.GetTokens();
        }
    }
}
