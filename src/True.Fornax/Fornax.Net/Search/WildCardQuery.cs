using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Analysis;
using Fornax.Net.Index;

namespace Fornax.Net.Search
{
    public sealed class WildCardQuery : FornaxQuery
    {
        private static TermVector _terms;

        private WildCardQuery(string query, Configuration config)
            : this(new AdvancedAnalyzer(query, SearchMode.Free, Expand.Default), config) { }

        private WildCardQuery(AdvancedAnalyzer analyzer, Configuration config)
            : base(analyzer, config)
        {

        }
    }
}
