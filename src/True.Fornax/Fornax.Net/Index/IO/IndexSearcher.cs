using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Search;

namespace Fornax.Net.Index.IO
{
    public class IndexSearcher
    {
        InvertedFile _index;
        FornaxQuery _query;

        public IndexSearcher(InvertedFile index, FornaxQuery query)
        {
            _index = index;
            _query = query;
        }


        public 

    }
}
