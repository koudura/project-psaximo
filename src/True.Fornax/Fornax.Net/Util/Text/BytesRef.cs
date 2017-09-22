 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Util.Text
{
    [Serializable]
    public sealed  class BytesRef : IComparable<BytesRef>
    {


        public static readonly byte[] EMPTY_BYTES = new byte[0];

      
        [WritableArray]
        public byte[] Bytes {

            get { return bytes; }
            set { Bytes = value; }
        }
        private byte[] bytes;

        public int CompareTo(BytesRef other) {
            throw new NotImplementedException();
        }
    }
}
