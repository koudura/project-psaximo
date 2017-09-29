using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Util.IO
{
    class FornaxFileException : FornaxException
    {
        public FornaxFileException() {
        }

        public FornaxFileException(string message) : base(message) {
        }

        public FornaxFileException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FornaxFileException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
        }

        public override string ToString() {
            return base.ToString();
        }
    }
}
