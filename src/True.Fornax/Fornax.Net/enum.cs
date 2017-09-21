/** MIT LICENSE
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

using Fornax.Net.Util;
using Fornax.Net.Index;

#region Query Enumerations 

/// <summary>
///  SearchMode determines how fornax would interprete an input query string.
/// </summary>
public enum SearchMode
{
    /// <summary>
    /// The Free mode simply transposes all <see cref="Constants._WS_"/>(s) between each <see cref="Term"/> of a query to a
    /// boolean representation (<seealso cref="Constants.OR"/>). 
    /// <para></para><br>
    /// e.g <code> string query = "fornax is awesome" </code>. 
    /// </br>
    /// <see cref="Free"/> interpretes the query in [fornax OR is OR awesome] which inturn works a retrieval of documents with
    /// either ("fornax","is","awesome")
    /// <br>  NOTE: Query Analysis is required but not shown in this example.</br>
    /// </summary>
    Free = 0,

    /// <summary>
    /// The Native mode simply transposes all <see cref="Constants._WS_"/>(s) between each <see cref="Term"/> of a query to a
    /// boolean representation (<seealso cref="Constants.AND"/>). 
    /// <para></para><br>
    /// e.g <code> string query = "fornax is awesome" </code>. 
    /// </br>
    /// <see cref="Native"/> interpretes the query in [fornax AND is AND awesome] which inturn works a retrieval of only documents with
    ///  all ("fornax","is","awesome").
    /// <br>  NOTE: Query Analysis is required but not shown in this example.</br>
    /// </summary>
    Native = 1,

    /// <summary>
    /// This Represents a natural Language query mode. <see cref="Natural"/> tells fornax to process a query as a natural language query.
    /// <para></para>
    /// e.g <code> string naturalQuery = "give me all my comics"" </code>. 
    /// <see cref="Natural"/> interpretes the query into ["give"[verb] , "all"[quantity-all] , "comics"(noun-query)]...This format is then returned
    /// for further analysis by fornax, afterwards fornax inturn works a retrieval of documents  with ("comic": in all <see cref="FieldScope"/>s and see<see cref="Zones"/>)
    /// as well as doucuments with <see cref="FornaxFormat"/> as comicbook.
    /// <br>  NOTE: Only Use this option if and only if a natural language exclusive search is needed 
    /// i.e. <see cref="Natural"/> never process a query as <see cref="Free"/> or <see cref="Native"/> do.</br>
    /// </summary>
    Natural = -1,

    /// <summary>
    /// The Normal mode simply taste's a query before tagging for interpretation.
    /// <para></para>
    /// e.g <code> string query = "fornax is awesome" </code>. 
    /// <see cref="Normal"/> tastes the query and there by interpretes the query in [fornax OR is OR awesome] which inturn works a retrieval of documents with
    /// either ("fornax","is","awesome")
    /// <br>  NOTE: Query Analysis is required but not shown in this example.</br>
    /// </summary>
    Normal = 2
}

/// <summary>
///  Represents the <see cref="QueryType"/> category of a fornax query.
///  i.e. Simple, Standard and Advanced Queries ,with increasing functionalities and type support. 
/// </summary>
public enum QueryMode
{
    /// <summary>
    /// The advanced querymode represents a query that can hold all <see cref="QueryType"/>s in fornax.net
    /// <seealso cref="QueryType"/> for all query types.
    /// </summary>
    Advanced,

    /// <summary>
    /// The standard querymode indicates that query can support <see cref="QueryType"/>s
    /// {Free,Keyword,Boolean,Phrase and Zone queries}
    /// </summary>
    Standard,

    /// <summary>
    /// The simple query indicates to fornax the default state analysis for queries, which only support 
    /// {Free and Keyword queries}. if the string contains any fornax query operator, it will be treated as a non-query operator 
    /// thereby filtered out.
    /// </summary>
    Simple
}

/// <summary>
/// Represents the type(s) of query supported by fornax.net
/// </summary>
public enum QueryType
{

    /// <summary>
    /// The Free Text or multi-term Query</summary.<br></br>
    /// e.g "Fornax is awesome";
    /// </summary>
    Free = 00,
    /// <summary>
    /// The keyword or single-term query  
    /// e.g "Awesome" 
    /// <para>NOTE: should not be mistaken for Phrase query.</para>
    /// </summary>
    KeyWord = 01,

    /// <summary>
    /// The boolean query type. 
    /// e.g "mit AND apache".
    /// </summary>
    Boolean = 10,
    /// <summary>
    /// The phrase query. 
    /// e.g "\"America\""
    /// </summary>
    Phrase = 11,
    /// <summary>
    /// The zone query type.  
    /// e.g "title:query".
    /// </summary>
    Zone = 12,

    /// <summary>
    /// The frequency query type. 
    /// e.g "america >30"
    /// </summary>
    Frequency = 20,
    /// <summary>
    /// The proximity query type. 
    /// e.g "check $1 mate"[adjacency] , "check %2 mate"[fuzzy]
    /// </summary>
    Proximity = 21,
    /// <summary>
    /// The regex query type. 
    /// e.g "%d+[querystring]*"
    /// </summary>
    Regex = 22,
    /// <summary>
    /// The wild card query.
    /// e.g "mo*e*" => {money,morsel,...}
    /// </summary>
    WildCard = 23


}

/// <summary>
/// Indicator For Query Expansion. 
/// </summary>
public enum Expand
{
    /// <summary>
    /// Expand Query. 
    /// </summary>
    YES,
    /// <summary>
    /// Do not expand query.
    /// </summary>
    NO,
    /// <summary>
    /// let fornax decide if query should be expanded.
    /// this method is based of many heuristics such {e.g number of results is lowe than fornax preset.}
    /// </summary>
    AUTO
}

/// <summary>
/// Suggestion Module Linker. Indicates to fornax.net suggestion module the decision to enable
/// or disable query suggestion , document suggestion and more like this queries.
/// </summary>
public enum Suggestion
{
    /// <summary>
    /// Turn on Suggestion module.
    /// </summary>
    ON,
    /// <summary>
    /// Turn off suggestion module.
    /// </summary>
    OFF = ~ON
}

#endregion

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

#region Index Enumerations

/// <summary>
/// 
/// </summary>
public enum IndexMode
{

    /// <summary>
    /// The per cache indexing mode.
    /// </summary>
    PerCache,

    /// <summary>
    /// The per file indexing mode.
    /// </summary>
    PerFile,

    /// <summary>
    /// The per memory indexing mode.
    /// </summary>
    PerMemory,

    /// <summary>
    /// The per segment indexing mode.
    /// </summary>
    PerSegment

}

/// <summary>
///  <see cref="FieldScope"/> defines a fieldbased query. 
///  Scope to field implementation used here.
///  Field Scope is an attribute or property of a file that can be extracted or retreived by fornax.net.
/// </summary>
public enum FieldScope : sbyte
{
    /// <summary>
    /// The content of document.
    /// </summary>
    Content = 0x0000,

    /// <summary>
    /// The date created of document.
    /// </summary>
    Date = 0x0001,

    /// <summary>
    /// The date last modified.
    /// </summary>
    Modified = 0x0002,

    /// <summary>
    /// The name or/and title of the document.
    /// </summary>
    Title = 0x0003,

    /// <summary>
    /// The path or link to doument in directory.
    /// </summary>
    Path = 0x0004,

    /// <summary>
    /// The file format of document.
    /// </summary>
    Type = 0x0005,

    /// <summary>
    /// The meta data
    /// </summary>
    MetaData = 0x0006

}

/// <summary>
/// Zone is a Searchable <see cref="FieldScope"/>
/// </summary>
public enum Zone : byte
{
    /// <summary>
    /// The content. see <seealso cref="FieldScope.Content"/>
    /// </summary>
    Content = 0b1111,
    /// <summary>
    /// The metadata see <seealso cref="FieldScope.MetaData"/>
    /// </summary>
    Metadata = 0b1110,
    /// <summary>
    /// The title or name of the document. see <seealso cref="FieldScope.Title"/>
    /// </summary>
    Title = 0b1100,
    /// <summary>
    /// The facet, which is a segment of any searchable fieldscope.
    /// </summary>
    Facet = 0b1000,
    /// <summary>
    /// The custom Zone which is to be defined by user.
    /// </summary>
    Custom =0b0001

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