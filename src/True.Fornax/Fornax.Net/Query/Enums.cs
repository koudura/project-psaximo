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

using Fornax.Net.Index;
using Fornax.Net.Index.Storage;
using Fornax.Net.Util;

namespace Fornax.Net.Query
{
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
        /// for further analysis by fornax, afterwards fornax inturn works a retrieval of documents  with ("comic": in all <see cref="FieldScope"/>s and see<see cref="Zone"/>)
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
        /// {exclusive advanced queries include : Wildcard,frequency,proximity, }
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
        /// The Free Text or multi-term Query.<br></br>
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
        WildCard = 23,
        /// <summary>
        /// The truncated query type.
        /// i.e "term?" where '?' is the truncation symbol.
        /// </summary>
        Truncated  = 24//would be treated as wildcard with only 1 eqd range.


    }

    /// <summary>
    /// Indicator For Query Expansion. 
    /// </summary>
    public enum Expand
    {
        /// <summary>
        /// Use explicit context-free phonetic expansion of query terms.
        /// </summary>
        Phonetic,

        /// <summary>
        /// No expansion of query terms.
        /// </summary>
        Default,

        /// <summary>
        /// Use explicit conntext-free word inflexion (such as synonyms,homonyms...) to expand query terms.
        /// </summary>
        Inflexive
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

    internal static class QExt
    {
        internal static bool IsExpandable(this Expand expand) {
            switch (expand) {
                case Expand.Phonetic:
                    return true;
                case Expand.Default:
                    return false;
                case Expand.Inflexive:
                    return true;
                default:
                    return false;
            }
        }
    }

}