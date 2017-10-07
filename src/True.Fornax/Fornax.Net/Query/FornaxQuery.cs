using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Query
{
    public abstract class FornaxQuery
    {
        public virtual bool IsExpandable { get; protected set;}


    }
}
