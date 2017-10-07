using System;
using System.Collections.Generic;
using System.IO;
using TikaOnDotNet;
using TikaOnDotNet.TextExtraction;
using TikaOnDotNet.TextExtraction.Stream;
using Toxy;

namespace Fornax.Net.Util.IO.Readers
{
    //TO DO: add ctor(FornaxFormat,directoryInfo|string|fileWrapper) //read fornaxformat from directory.
    //TO DO: add ctor(FileFormat, directoryInfo,string) // read all fileformat from directory.

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class FornaxReader : IDisposable
    {
        private FileInfo file;
        private FileFormat format;
        private FornaxFormat category;
        private static bool isSafeLoad;

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxReader" /> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <exception cref="ArgumentException">message - filename</exception>
        public FornaxReader(string filename)
            : this(new FileInfo(filename ?? throw new ArgumentNullException(nameof(filename)))) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxReader" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <exception cref="ArgumentNullException">file</exception>
        public FornaxReader(FileInfo file) {
            this.file = file ?? throw new ArgumentNullException(nameof(file));
            this.format = FormatExt.Parse(file.Extension);
            this.category = format.GetFornaxFormat();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxReader" /> class.
        /// </summary>
        /// <param name="fileWrapper">The file wrapper.</param>
        /// <exception cref="FornaxFileException"></exception>
        public FornaxReader(FileWrapper fileWrapper)
            : this(fileWrapper.Parse().AsFile ?? throw new FornaxFileException()) {
        }

        /// <summary>
        /// Uses Tika Extractor to Read Files.
        /// </summary>
        /// <exception cref="FornaxException">category</exception>
        public (string ContentType, string Text, IDictionary<string, string> Metadata) TikaRead() {
            var res = new TextExtractor().Extract(this.file.FullName);
            return (res.ContentType, res.Text, res.Metadata);
        }

        public void ToxyReadDom() {
            if (category != FornaxFormat.Dom)
                throw new FornaxFormatException(nameof(category));
            var context = new ParserContext(file.FullName);
            IDomParser parser = ParserFactory.CreateDom(context);

        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        static FornaxReader() {
            if (FornaxAssembly.TryResolveTika() && FornaxAssembly.TryResolveToxy()) {
                isSafeLoad = true;
            }
        }

    }
}