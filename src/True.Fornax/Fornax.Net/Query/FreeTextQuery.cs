using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Analysis;
using Fornax.Net.Index.IO;
using Fornax.Net.Index.Storage;

namespace Fornax.Net.Query
{
    public class FreeTextQuery : Query
    {
        internal FreeTextQuery(string query, FornaxRepository repo,SearchMode searchMode = SearchMode.Free ,Expand expansion = Expand.NO) : base(query,new SimpleAnalyzer(searchMode), repo, expansion) {
        }
    }
}
