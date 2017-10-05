using System;
using System.IO;
using  org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;

namespace Fornax.Net.Util.IO.Readers
{
    public sealed class TextualReader : FornaxReader
    {
        public TextualReader(FileFormat singleformat, string filename) : this(singleformat, new FileInfo(filename)) {

        }

        public TextualReader(FileFormat singleformat, FileInfo file) : base(singleformat, file) {
        }

        public static string StripPDF(string filename) {
            if (string.IsNullOrEmpty(filename) || !new FileInfo(filename).Exists) {
                throw new ArgumentException("message", nameof(filename));
            }
            return null;

        }

    }
}
