// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-29-2017
// ***********************************************************************
// <copyright file="BufferedReaderWrapper.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Text;

using FileReader = java.io.FileReader;
using BuffReader = java.io.BufferedReader;
using FileException = java.io.FileNotFoundException;
using IO_Exception = java.io.IOException;

/// <summary>
/// The Readers namespace.
/// </summary>
namespace Fornax.Net.Util.IO.Readers
{
    /// <summary>
    /// Class BufferedReaderWrapper.
    /// </summary>
    internal class BufferedReaderWrapper
    {
        private string file;
        private string content;
        private readonly static bool isSafeLoad;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedReaderWrapper"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <exception cref="ArgumentNullException">file</exception>
        public BufferedReaderWrapper(FileInfo file) : this(file.FullName) {
            if (file == null) {
                throw new ArgumentNullException(nameof(file));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedReaderWrapper"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <exception cref="ArgumentException">Should not be null or empty !!. - filename</exception>
        public BufferedReaderWrapper(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                throw new ArgumentException("Should not be null or empty !!.", nameof(filename));
            }
            
            this.file = filename;
            BuildContent();
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get => this.content; private set => this.content = value; }

        /// <summary>
        /// Builds the content.
        /// </summary>
        public void BuildContent() {
            StringBuilder builder = new StringBuilder();
            try {
                using (FileReader reader = new FileReader(this.file)) {
                    using (BuffReader bread = new BuffReader(reader)) {
                        string line;
                        while ((line = bread.readLine()) != null) {
                            builder.AppendLine(line);
                        }
                    }
                }
            } catch (Exception ex) when (ex is FileException || ex is IO_Exception || ex is FileNotFoundException || ex is IOException) {
                //do logging 
                this.content = null;
            }
            this.content = builder.ToString();
            return;
        }

        static BufferedReaderWrapper() {
            if (FornaxAssembly.TryResolveTika()) {
                isSafeLoad = true;
            }
            isSafeLoad = false;
        }
    }
}
