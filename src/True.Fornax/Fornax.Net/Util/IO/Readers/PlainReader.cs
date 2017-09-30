using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toxy;
using TikaOnDotNet.TextExtraction;
using System.IO;
using java.io;


namespace Fornax.Net.Util.IO.Readers
{
    /// <summary>
    /// Plain Text reader for reading <see cref="FornaxFormat.Plain"/> type files.
    /// </summary>
    public sealed class PlainReader :  IDisposable
    {
        private readonly static bool isSafeLoad;
        private readonly Extractor ext;
        private readonly string name;

        /// <summary>
        /// Gets the type of the extractor.
        /// </summary>
        /// <value>
        /// The type of the extractor.
        /// </value>
        public Extractor ExtractorType => this.ext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainReader"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="extractor">The extractor.</param>
        public PlainReader(FileInfo file , Extractor extractor) : this(file.FullName,extractor) {
            if (file == null) {
                throw new ArgumentNullException(nameof(file));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainReader"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="extractor">The extractor.</param>
        public PlainReader(string filename, Extractor extractor) {
            this.name = filename ?? throw new ArgumentNullException(nameof(filename));
            this.ext = extractor;
        }
       
        public string GetContent() {
            switch (this.ext) {
                case Extractor.TOXY_PARSER:
                    return null;
                case Extractor.TIKA_PARSER:
                    return null;
                case Extractor.DOTNET_STREAM:
                    return null;
                case Extractor.JAVA_BUFFER:
                    return new BufferedReaderWrapper(this.name).GetContent();
                default:
                    return null;
            }
        }

        static PlainReader() {
            if(FornaxAssembly.TryResolveTika() && FornaxAssembly.TryResolveToxy()) {
                isSafeLoad = true;
            }
            isSafeLoad = false;
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}
