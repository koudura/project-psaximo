using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Index
{
    /// <summary>
    /// A <see cref="Term"/> represents a word from text. This is the unit of search.  It is
    /// composed of two elements, the text of the word, as a string, and the name of
    /// the field that the text occurred in.
    /// <para/>
    /// Note: A <see cref="Term"/> may represent more than words from text fields, but also
    /// things like dates, email addresses, urls, etc.
    /// </summary>
    public sealed class Term
    {
        public Term() {
        }
    }
}
