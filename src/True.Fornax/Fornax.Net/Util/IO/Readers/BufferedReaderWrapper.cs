using System;
using System.IO;
using System.Text;

using FileReader = java.io.FileReader;
using BuffReader = java.io.BufferedReader;
using FileException = java.io.FileNotFoundException;
using IO_Exception = java.io.IOException;

namespace Fornax.Net.Util.IO.Readers
{
   internal class BufferedReaderWrapper
    {
        private string file;
        private string content;
        private readonly static bool isSafeLoad;

        public BufferedReaderWrapper(FileInfo file) : this(file.FullName) {
            if (file == null) {
                throw new ArgumentNullException(nameof(file));
            }
        }

        public BufferedReaderWrapper(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                throw new ArgumentException("message", nameof(filename));
            }
            
            this.file = filename;
        }

        public string CurrentContent { get => this.content; set => this.content = value; }

        public string GetContent() {
            StringBuilder builder = new StringBuilder();

            try {
                using (FileReader reader = new FileReader(this.file)) {
                    using (BuffReader bread = new BuffReader(reader)) {
                        string line;
                        while ((line = bread.readLine()) != null) {
                            builder.AppendFormat($"{line}{Environment.NewLine}");
                        }
                    }
                }
            } catch (Exception ex) when (ex is FileException || ex is IO_Exception || ex is FileNotFoundException || ex is IOException) {
                //do logging 
                this.content = null;
            }
            this.content = builder.ToString();
            return this.content;
        }

        static BufferedReaderWrapper() {
            if (FornaxAssembly.TryResolveTika()) {
                isSafeLoad = true;
            }
            isSafeLoad = false;
        }
    }
}
