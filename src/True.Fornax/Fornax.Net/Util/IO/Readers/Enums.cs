﻿/***
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


using System.IO;

namespace Fornax.Net.Util.IO.Readers
{
    /// <summary>
    /// Specification of the type of extractor to use for extraction.
    /// </summary>
    public enum Extractor
    {
        /// <summary>
        /// Use <see cref="Toxy"/> Parsers to extract file.
        /// </summary>
        TOXY_PARSER,

        /// <summary>
        /// USe <see cref="TikaOnDotNet.TextExtraction"/> Parser to extract file.
        /// </summary>
        TIKA_PARSER,

        /// <summary>
        /// Use <see cref="StreamReader"/> to extract file.
        /// </summary>
        DOTNET_STREAM,

        /// <summary>
        /// Use <see cref="java.io.BufferedReader"/> and  <see cref="java.io.BufferedInputStream"/> to extract
        /// file.
        /// </summary>
        JAVA_BUFFER

    }

}