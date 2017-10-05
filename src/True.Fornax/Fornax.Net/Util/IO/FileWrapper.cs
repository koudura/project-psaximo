using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JFile = java.io.File;
using java.io;

namespace Fornax.Net.Util.IO
{
    public sealed class FileWrapper
    {
        JFile file;
        
        public FileWrapper(string wrapperName) {
            if (string.IsNullOrEmpty(wrapperName)) {
                throw new ArgumentException("message", nameof(wrapperName));
            }

            this.file = new JFile(wrapperName);
        }

        public string FileName => this.file.getName();

        public string FilePath => this.file.getPath();

        internal JFile File => this.file;
    }
}
