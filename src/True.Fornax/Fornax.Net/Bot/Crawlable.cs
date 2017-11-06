using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Bot
{
    [AttributeUsage(AttributeTargets.Class , Inherited = false, AllowMultiple = true)]
    sealed class Crawlable : Attribute
    {
        public Crawlable() {
        }
    }
}
