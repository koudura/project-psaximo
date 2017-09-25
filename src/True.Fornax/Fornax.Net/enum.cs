/** 
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
/// 
/// </summary>
public enum FetchAttribute
{
    Persistent,
    Weak
}

/// <summary>
///  Traversal Mode specifies the mode at which fornax network crawler.
///  crawls the web.
/// </summary>
public enum TraversalMode
{
    /// <summary>
    /// The <see cref="Minimal"/> approach rules :
    /// <list type="Rules">
    /// <item>No page retrieval scoring & ranking.</item>
    /// <item>Only gets a [n] number of pages</item><description>
    /// (where [n] is a specific threshold value that indicates the upper bound
    /// of the frontier. [Default = 10])
    /// </description>
    /// </list>
    /// </summary>
    Minimal,
    /// <summary>
    /// The <see cref="Detailed"/> retrieval approach rules :
    /// <list type="Rules">
    /// <item>Page retrieval scoring & ranking.</item>
    /// <item>Only Traverses the web graph and retrieves the top ranked documents relative to a link.</item>
    /// <description>
    /// (Where the top-ranked is determined by fornax.net, NOTE: the resulting pages may span a huge result.)
    /// </description>
    /// </list>
    /// </summary>
    Detailed,

    /// <summary>
    /// The <see cref="Normal"/> retrieval approach rules :
    /// <list type="Rules">
    /// <item>Page retrieval scoring & ranking.</item>
    /// <item>Only gets a [n] number of pages</item><description>
    /// (where [n] is a specific threshold value that indicates the upper bound
    /// of the frontier. [Default = 10])
    /// </description>
    /// </list>
    /// </summary>
    Normal,
    /// <summary>
    /// The <see cref="Absolute"/> retrieval approach rules :
    /// <list type="Rules">
    /// <item>page retrieval scoring & ranking.</item>
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

    Reduced,
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
    /// The default
    /// </summary>
    Default,
    /// <summary>
    /// All
    /// </summary>
    All,
    /// <summary>
    /// The image
    /// </summary>
    Image,
    /// <summary>
    /// The text
    /// </summary>
    Text,
    /// <summary>
    /// The slide
    /// </summary>     
    Slide,
    /// <summary>
    /// The spread sheet
    /// </summary>
    SpreadSheet,
    /// <summary>
    /// The email
    /// </summary>
    Email,
    /// <summary>
    /// The DOM
    /// </summary>
    DOM,
    /// <summary>
    /// The web
    /// </summary>
    Web,
    /// <summary>
    /// The media
    /// </summary>
    Media,
    /// <summary>
    /// The plain
    /// </summary>
    Plain,
    /// <summary>
    /// The zip
    /// </summary>
    Zip
}

/// <summary>
/// Enum to manage delegate handlers for loading and preloading Dependencies 
/// and external libraries. Manages eventHandlers Dynamic assembly runtime resolve.
/// </summary>
public enum FornaxDependency
{
    /// <summary>
    /// The load on demand procedure notifies its event handler to load dependency library on demand to embedded memory.
    /// This Event Listens constantly at runtime for changes made to the default assembly external directory.
    /// then creates another event handler for - task by user.
    /// </summary>
    LoadOnDemand,
    /// <summary>
    /// The pre load procedure initializes a function that loads all specified dependency librariies at initialization time.
    /// This Loads library to embedded resources via memory stream, and clears embedded resources on close or after a specified time by user.
    /// </summary>
    PreLoad,
    /// <summary>
    /// The register on demand procedure works similar to <see cref="LoadOnDemand"/>, but byte[] representation of dependency library gets 
    /// gets regisered to the embeded resource. Persistence is thereby ensured. This Event When called once, erradicates the need for another call
    /// during a separate runtime, because previously loaded library as being registered into [Fornax.Net.Common.SandBox].
    /// </summary>
    RegisterOnDemand,
    /// <summary>
    /// The register procedure, much like the <seealso cref="PreLoad"/>. But byte[] representation get registered into fornax.net
    /// see <seealso cref="RegisterOnDemand"/> for more registration details.
    /// </summary>
    Register

}
#endregion



#region  Misc Enumerations
/// <summary>
/// 
/// </summary>
public enum FileExtension { }

/// <summary>
/// Sorter enum for sorting a collection of documents.
/// </summary>
public enum SortBy {

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