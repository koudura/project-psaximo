/***
* Copyright (c) 2017 Koudura Ninci @True.Inc
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*
**/

using System;
using System.IO;
using ProtoBuf;
using JFile = java.io.File;


namespace Fornax.Net.Util.IO
{
    /// <summary>
    /// Represents a wrapper class for the java.io.File from
    /// the jdk realm.
    /// </summary>
    [Serializable, ProtoContract]
    public sealed class FileWrapper
    {
        [ProtoMember(1)]
        JFile file;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWrapper"/> class.
        /// </summary>
        /// <param name="wrapperName">Name of the file or directory.</param>
        /// <exception cref="ArgumentException">message - wrapperName</exception>
        public FileWrapper(string wrapperName) {
            if (string.IsNullOrEmpty(wrapperName)) {
                throw new ArgumentException("message", nameof(wrapperName));
            }

            this.file = new JFile(wrapperName);
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName => this.file.getName();
       
        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath => this.file.getPath();

        internal JFile File => this.file;

        internal (FileInfo AsFile, DirectoryInfo AsDirectory) Parse() {
            if (this.file.isFile())
                return (new FileInfo(FileName), null);
            else if (this.file.isDirectory())
                return (null, new DirectoryInfo(FileName));
            else throw new FileNotFoundException($"{FileName} not found");
        }
    }
}
