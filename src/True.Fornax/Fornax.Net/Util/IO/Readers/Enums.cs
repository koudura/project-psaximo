using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
