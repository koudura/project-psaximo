using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Search
{
    internal static class Weight
    {
        internal static double Idf(long n, long df)
        {
            return Math.Log10(n / df);
        }

        internal static double Tf(long tf)
        {
            return 1 + Math.Log10(tf);
        }

        internal static double TfxIdf (long tf, long n, long df)
        {
            return Tf(tf) * Idf(n, df);
        }

    }
}
