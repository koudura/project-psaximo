﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Query;

namespace Fornax.Net.Analysis
{
    class SimpleAnalyzer : Analyzer
    {
        public SimpleAnalyzer(SearchMode free) {
            base.modeOfsearch = free;
        }

        public SimpleAnalyzer() { }
    }
}
