using System;
using System.IO;

namespace Fornax.Net.Util.IO.Readers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class FornaxReader : IDisposable
    {
        private FileInfo file;
        private FileFormat format;
        private FornaxFormat category;

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxReader"/> class.
        /// </summary>
        /// <param name="singleformat">The singleformat.</param>
        /// <param name="filename">The filename.</param>
        /// <exception cref="ArgumentException">message - filename</exception>
        public FornaxReader(FileFormat singleformat, string filename) : this(singleformat, new FileInfo(filename)) {
            if (string.IsNullOrEmpty(filename)) {
                throw new ArgumentException("message", nameof(filename));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxReader"/> class.
        /// </summary>
        /// <param name="singleformat">The singleformat.</param>
        /// <param name="file">The file.</param>
        public FornaxReader(FileFormat singleformat, FileInfo file) {
            this.file = file ?? throw new ArgumentNullException(nameof(file));
            this.format = singleformat;
            this.category = singleformat.GetFornaxFormat();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxReader"/> class.
        /// </summary>
        /// <param name="singleformat">The singleformat.</param>
        /// <param name="fileWrapper">The file wrapper.</param>
        public FornaxReader(FileFormat singleformat, FileWrapper fileWrapper) {
            this.file = getFileInfo(fileWrapper ?? throw new ArgumentNullException());
            this.format = singleformat;
            this.category = format.GetFornaxFormat();
        }

        public void Read() {


        }


        private FileInfo getFileInfo(FileWrapper wrapper) {
            if (wrapper.File.isFile())
                return new FileInfo(wrapper.FileName);
            else
                throw new FornaxFileException();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

    }
}