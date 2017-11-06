using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net;
using Fornax.Net.Search;

namespace corvuslib
{
    public partial class Querier
    {
        FreeTextQuery ftq;
        string _query;



        public Querier(string query, Configuration config, bool autocorrect)
        {

            _query = query;

        }

        static Querier()
        {

        }

    }
}
