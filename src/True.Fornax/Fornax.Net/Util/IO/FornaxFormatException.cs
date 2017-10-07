using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Util.IO
{
    public class FornaxFormatException : FormatException
    {
        public FornaxFormatException() {
        }

        public FornaxFormatException(string message) : base(message) {
        }

        public FornaxFormatException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FornaxFormatException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
