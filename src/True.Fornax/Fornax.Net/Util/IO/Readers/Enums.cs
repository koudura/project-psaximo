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

namespace Fornax.Net.Util.IO.Readers
{
    /// <summary>
    /// Specification of the type of extractor to use for extraction.
    /// </summary>
    [Serializable]
    public enum Extractor
    {
        /// <summary>
        /// Fornax Managed Extraction protocol for file text-extraction.
        /// This Protocol observes the file format to decide whhich suitable extractor to apply
        /// to text-extraction.<para></para>
        /// This Protocol could be most expensive.
        /// </summary>
        Managed,

        /// <summary>
        /// Fornax Default Extraction Protocol for file text-extraction.
        /// Simple text-extraction protocol.
        ///<para>This Protocol is of medium expense.</para>
        /// </summary>
        Default,

        /// <summary>
        /// Fornax Minimal Extraction protocol for plain and dom files-text extension.
        /// If used on a DOM type file, tokenization would cause short-term expenses on the quality of extraction.
        /// <para>This Protocol should be used on Palin Text Files (<seealso cref="FornaxFormat.Plain"/>) for reduced expenses.</para>
        /// </summary>
        Minimal,

        /// <summary>
        /// Fornax Buffered Extraction Protocol for file text-extraction.
        /// This Protocol implements the JDK 7.0 Buffered reader to extract text content files.
        /// Use <see cref="java.io.BufferedReader"/> and  <see cref="java.io.BufferedInputStream"/> to extract
        /// file.
        /// <para>This Protocol Should be used if and only if knowledge of the buffered stream requirements are possessed.</para>
        /// </summary>
        Buffered

    }

}
