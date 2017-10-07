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


namespace Fornax.Net {

    #region IO Enumerations 
    /// <summary>
    /// Caching Mode determines what caching technique should be used. 
    /// </summary>
    public enum CachingMode
    {
        /// <summary>
        /// The dynamic caching technique. 
        /// This technique dynamically stores transactions in cache. 
        /// </summary>
        Dynamic,
        /// <summary>
        /// The static caching technique. 
        /// This technique statically s tores transactions in cache.
        /// </summary>
        Static,
        /// <summary> 
        /// The default caching technique. A fornax.net specific technique.
        /// </summary>
        Default
    }

    /// <summary>
    /// Defines What fornax does when an exception is thrown on retrieval of a file.
    /// </summary>
    public enum FetchAttribute
    {
        /// <summary>
        /// Continues to retry retrieval until a specified time or file is accessible again.
        /// possible Reentrancy.
        /// </summary>
        Persistent,
        /// <summary>
        /// Ignores or skips file on retrieval exception. NoReentrancy
        /// </summary>
        Weak
    }

    /// <summary>
    ///  Traversal Mode specifies the mode at which fornax network crawler.
    ///  crawls the web.
    /// </summary>
    public enum TraversalMode
    {
        /// <summary>
        /// The <see cref="Minimal"/> approach rules : <para>
        /// <list type="Rules">
        /// <item>No page retrieval scoring and ranking.</item>
        /// <item>Only gets a [n] number of pages</item><description>
        /// (where [n] is a specific threshold value that indicates the upper bound
        /// of the frontier. [Default = 10]).
        /// </description><para></para>
        /// <item>Only gets pages relative to the absolute link/page as inputed by user.</item>
        /// <description>this option doesn't initiate branching from the hostname.</description>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// All there is to Minimal includes:
        /// 1. no ranking on retrieval.
        /// 2. a specified maximum of [byte.MaxValue] number of pages can only be traversed and fetched.
        /// 3. Traversion begins from the specified page instead of from hostname as in <see cref="Absolute"/>
        /// and <see cref="Detailed"/>.
        /// </remarks>
        Minimal,

        /// <summary>
        /// The <see cref="Normal"/> retrieval approach rules : <para>
        /// <list type="Rules">
        /// <item>Page retrieval scoring and ranking.</item>
        /// <item>Only gets a [n] number of pages</item><description>
        /// (where [n] is a specific threshold value that indicates the upper bound
        /// of the frontier. [Default = 15]).
        /// </description><para></para>
        /// <item>Only gets pages relative to the absolute link/page as inputed by user.</item>
        /// <description>this option doesn't initiate branching from the hostname.</description>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// All there is to Normal includes:
        /// 1. Ranking on retrieval.
        /// 2. a specified maximum of [byte.MaxValue] number of pages can only be traversed and fetched.
        /// 3. Traversion begins from the specified page instead of from hostname as in <see cref="Absolute"/>
        /// and <see cref="Detailed"/>.
        /// </remarks>
        Normal,

        /// <summary>
        /// The <see cref="Detailed"/> retrieval approach rules : <para>
        /// <list type="Rules">
        /// <item>Page retrieval scoring and ranking.</item>
        /// <item>Only Traverses the web graph and retrieves the top ranked documents relative to a link.</item>
        /// <description>
        /// (Where the top-ranked is determined by fornax.net, NOTE: the resulting pages may span a huge result.)
        /// </description>
        /// <item>
        ///  Traversion of pages also include Host-name as root. i.e the hostname serves as root for this traversion.
        ///  NOTE: This occurs on a separate thread.
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        Detailed,

        /// <summary>
        /// The <see cref="Absolute"/> retrieval approach rules :
        /// <list type="Rules">
        /// <item>page retrieval scoring and ranking.</item>
        /// <item>Traverses the web graph of the given link to the lowest depth.</item><description>
        /// </description>
        /// </list>
        /// </summary>
        Absolute

    }

    /// <summary>
    /// 
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// The reduced type is as static-compressed cache.
        /// </summary>
        Reduced,
        /// <summary>
        /// The verbatim type is as dynamic-buffer cache.
        /// </summary>
        Verbatim,

    }
    #endregion

    #region Fornax Enumerations 

    /// <summary>
    /// File Format Categories supported by Fornax.Net
    /// </summary>
    public enum FornaxFormat
    {

        /// <summary>
        /// Default supported types on settings
        /// (.pdf, .doc, .docx, .ppt, .pptx, .xls, .xlsx, .txt, .html, .xml)
        /// </summary>
        Default,
        /// <summary>
        /// All supported types.
        /// </summary>
        All,
        /// <summary>
        /// Supported Images files (.jpg, .png, .jpeg)
        /// </summary>
        Image,
        /// <summary>
        ///  Supported Raw-text files. (.pdf, .docx)
        /// </summary>
        Text,
        /// <summary>
        /// Microsoft  Power point slides (.ppt, .pptx)
        /// </summary>     
        Slide,
        /// <summary>
        /// Microsoft Spread sheet files (.xls, .xlsx)
        /// </summary>
        Sheet,
        /// <summary>
        ///  Email/Contact files (.msg, .eml)
        /// </summary>
        Email,
        /// <summary>
        /// DOM (document object model) files (.html, .xml)
        /// </summary>
        Dom,
        /// <summary>
        /// Supported Web server files (.asp , .aspx)
        /// </summary>
        Web,
        /// <summary>
        /// Supported Media files. (.mp3, .mp4)
        /// </summary>
        Media,
        /// <summary>
        ///  Plain-text files. (.ans, .ascii, .txt)
        /// </summary>
        Plain,
        /// <summary>
        ///  Zipped/Compressed files. (.zip, .rar)
        /// </summary>
        Zip
    }

    #endregion

    #region  Misc Enumerations

    /// <summary>
    /// Sorter enum for sorting a collection of documents.
    /// </summary>
    public enum SortBy
    {

        /// <summary>
        /// Sort by The relevance to query. 
        /// This is the default Sort mode.
        /// </summary>
        Relevance,
        /// <summary>
        /// Sort by the date last Modified.
        /// </summary>
        Modified,
        /// <summary>
        /// Sort by the date of creation.
        /// </summary>
        Date,
        /// <summary>
        /// Sort lexographically by the name or title.
        /// </summary>
        Name,
        /// <summary>
        /// Sort by the length or size.
        /// </summary>
        Size
    }
    #endregion
}