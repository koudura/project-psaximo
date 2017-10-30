using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Util.IO.Readers
{
    public sealed class ZipReader 
    {
        FileInfo _zipFile;

        public ZipReader(FileInfo zipfile) {
            _zipFile = zipfile;
        }

  
    }
}
