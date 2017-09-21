using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Lib
{
    internal class Dependency
    {
        private Dependency() { }

        public static Assembly Load(byte[] assembly) {
            return Assembly.Load(assembly);
        }
    }
}
