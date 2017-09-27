using System;
using System.Runtime.Serialization;

namespace Fornax.Net.Util.IO
{
    [Serializable]
    internal class FornaxAssemblyException : Exception
    {
        public FornaxAssemblyException() {
        }

        public FornaxAssemblyException(string message) : base(message) {
        }

        public FornaxAssemblyException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FornaxAssemblyException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}