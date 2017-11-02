// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Habuto Koudura
// Created          : 09-19-2017
//
// Last Modified By : Habuto Koudura
// Last Modified On : 10-29-2017
// ***********************************************************************
// <copyright file="Constants.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Permissions;

/// <summary>
/// The Util namespace.
/// </summary>
namespace Fornax.Net.Util
{
    /// <summary>
    /// Holds All static representation of constants needed by fornax.
    /// and methods for manipulation and preprocessing of constants.
    /// </summary>
    internal static class Constants
    {
        #region Char Constants  

        /// <summary>
        /// Represents a Whitespace character. <c> char = ' ' </c> .
        /// </summary>
        public const char _WS_ = ' ';

        /// <summary>
        /// The whitespace string brokers
        /// </summary>
        public const string WS_BROKERS = " \t\n\r\f\v";
        /// <summary>
        /// The number brokers
        /// </summary>
        internal const string Num_Brokers = " 0123456789";
        /// <summary>
        /// The gen op brokers
        /// </summary>
        internal const string GenOp_Brokers = " `^+=\\{};<,/[]#\t\n\r\f";
        /// <summary>
        /// The query op broker
        /// </summary>
        internal const string QueryOP_Broker = " &|\":*?%!>[]$~()";
        /// <summary>
        /// The document op broker
        /// </summary>
        internal const string DocOP_Broker = "-.@'_";

        /// <summary>
        /// Gets the brokers.
        /// </summary>
        /// <value>The brokers.</value>
        public static char[] Brokers => new[] { ' ', '\n', '\t', '\r', '\f' };
        #endregion

        #region Boolean Constants 

        /// <summary>
        /// Represents the Boolean Rule of Conjuction (AND).
        /// </summary>
        public const string AND = "AND";
        /// <summary>
        /// The and 0
        /// </summary>
        public const string AND_0 = "&&";

        /// <summary>
        /// Represents the Boolean Rule of Disjunction (OR).
        /// </summary>
        public const string OR = "OR";
        /// <summary>
        /// Represents the Boolean rule of Disjunction (||).
        /// </summary>
        public const string OR_0 = "||";

        /// <summary>
        /// Represents the Boolean Rule of Negation (NOT).
        /// </summary>
        public const string NOT = "NOT";
        /// <summary>
        /// Represents the Boolean Rule of Negation (!).
        /// </summary>
        public const string NOT_0 = "!";

        #endregion

        /// <summary>
        /// Gets the fornax dir.
        /// </summary>
        /// <value>The fornax dir.</value>
        internal static string FornaxDir => string.Format("{0}{1}", LoggingDirectory, "\\Fornax");
        /// <summary>
        /// Gets the logging directory.
        /// </summary>
        /// <value>The logging directory.</value>
        internal static string LoggingDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        /// <summary>
        /// Gets the temporary path.
        /// </summary>
        /// <value>The temporary path.</value>
        internal static string TempPath => Path.GetTempPath();
        /// <summary>
        /// Gets the temporary file.
        /// </summary>
        /// <value>The temporary file.</value>
        internal static string TempFile => Path.GetTempFileName();

        #region fornax files

        /// <summary>
        /// The fornax gram file
        /// </summary>
        internal static FileInfo FornaxGramFile = GetCurrentFile(BaseDirectory, "__.gmx");
        /// <summary>
        /// The fornax edit file
        /// </summary>
        internal static FileInfo FornaxEditFile = GetCurrentFile(BaseDirectory, "__.edx");
        /// <summary>
        /// The fornax logs file
        /// </summary>
        internal static FileInfo FornaxLogsFile = GetCurrentFile(BaseDirectory, "__.lgx");
        /// <summary>
        /// The fornax compressed gram file
        /// </summary>
        internal static FileInfo FornaxCompressedGramFile = GetCurrentFile(BaseDirectory, "__.zgmx");
        /// <summary>
        /// The fornax trie file
        /// </summary>
        internal static FileInfo FornaxTrieFile = GetCurrentFile(BaseDirectory, "__.trx");
        /// <summary>
        /// The fornax mod file
        /// </summary>
        internal static FileInfo FornaxModFile = GetCurrentFile(BaseDirectory, "__.mdx");
        /// <summary>
        /// The fornax vocab file
        /// </summary>
        internal static FileInfo FornaxVocabFile = GetCurrentFile(BaseDirectory, "__.dix");

        #endregion 

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DirectoryInfo.</returns>
        internal static DirectoryInfo GetCurrentDirectory(string name) {
            DirectoryInfo currdir = null;
            try {
                string dirPath = Path.Combine(BaseDirectory.FullName, name);
                currdir = new DirectoryInfo(dirPath);
                if (!currdir.Exists) currdir.Create();
                return currdir;
            } catch (Exception) { }
            Contract.Ensures(currdir != null);
            return currdir;
        }

        /// <summary>
        /// Gets the current file.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="name">The name.</param>
        /// <returns>FileInfo.</returns>
        internal static FileInfo GetCurrentFile(DirectoryInfo dir, string name) {
            FileInfo currfile = null;
            try {
                string dirPath = Path.Combine(dir.FullName, name);
                currfile = new FileInfo(dirPath);
                if (!currfile.Exists) currfile.Create();
                return currfile;
            } catch (Exception) { }
            Contract.Ensures(currfile != null);
            return currfile;
        }

        /// <summary>
        /// Gets the base directory.
        /// </summary>
        /// <value>The base directory.</value>
        internal static DirectoryInfo BaseDirectory => GetBaseDir();

        /// <summary>
        /// Gets the base dir.
        /// </summary>
        /// <returns>DirectoryInfo.</returns>
        /// <exception cref="NotImplementedException"></exception>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static DirectoryInfo GetBaseDir() {

            DirectoryInfo basedir = null;
            try {
                basedir = new DirectoryInfo(FornaxDir);
                if (!basedir.Exists) basedir.Create();
                return basedir;
            } catch (Exception) { }
            Contract.Ensures(basedir != null);
            throw new NotImplementedException();
        }

        #region Fornax FileTypeExtensions

        /// <summary>
        /// The CMP txtfile
        /// </summary>
        internal const string CmpTxtfile = @".ztxt";
        /// <summary>
        /// The CMP inv file
        /// </summary>
        internal const string CmpInvFile = @".zinx";
        /// <summary>
        /// The CMP trie file
        /// </summary>
        internal const string CmpTrieFile = @".ztrx";
        /// <summary>
        /// The CMP dictionary file
        /// </summary>
        internal const string CmpDictFile = @".zdix";
        /// <summary>
        /// The CMP data file
        /// </summary>
        internal const string CmpDataFile = @".z4nax";

        /***All this fornax file type would be stored in a compressed folder.
         * each file has name&ext as => __.4nax, _.4cache...
         * 
         * **/

        /// <summary>
        /// fornax inverted index file, this file would have variants depending on the type of repository to be indexed.
        /// </summary>
        internal const string ExtInvFile = @".4nax";
        /// <summary>
        /// fornax index segment/part file, for storing segments of index during indexing before being moved to the inverted file.
        /// it also stores a position-offset flag during indexing, in case any crash occurs. the flag can be used to resume index from
        /// crash point.
        /// </summary>
        internal const string ExtPartFile = @".prtx";
        /// <summary>
        /// fornax repository file for storing list of names raw files to be extracted, and their relative tags and attributes
        /// </summary>
        internal const string ExtRepoFile = @".repx";
        /// <summary>
        /// fornax cache file for storing short term data input e.g last query input(as char[]) and respective top 5-10 documents.
        /// ,documents found with changes by the crawler. (attributes of query and results are also stored.)
        /// </summary>
        internal const string ExtCacheFile = @".4cache";
        /// <summary>
        /// fornax temporary storage file for disk-read/write opearations that take long processing time.
        /// e.g indexing a large corpus.
        /// </summary>
        internal const string ExtTempFile = @".tempx";
        /// <summary>
        /// fornax data file stores query history in a structured format, corpus history, suggestion history,
        /// and long-term data from the cache file are moved here.
        /// </summary>
        internal const string ExtDataFile = @".datx";
        /// <summary>
        /// fornax deletes file, stores the same data as repo-file <see cref="ExtRepoFile" /> only for file marked as to be deleted from
        /// the index and fornax database by the crawler.
        /// </summary>
        internal const string ExtDelsFile = @".delx";

        #endregion

        #region query opeartors

        /// <summary>
        /// Gets the query boolean.
        /// </summary>
        /// <value>The query boolean.</value>
        internal static string[] Query_Boolean => new[] { "&&", "AND", "&", "||", "OR", "NOT", "!" };
        /// <summary>
        /// Gets the query boolean or.
        /// </summary>
        /// <value>The query boolean or.</value>
        internal static string[] Query_Boolean_OR => new[] { "||", "OR" };
        /// <summary>
        /// Gets the query boolean and.
        /// </summary>
        /// <value>The query boolean and.</value>
        internal static string[] Query_Boolean_AND => new[] { "&", "&&", "AND" };
        /// <summary>
        /// Gets the query boolean not.
        /// </summary>
        /// <value>The query boolean not.</value>
        internal static string[] Query_Boolean_NOT => new[] { "!", "NOT " };

        /// <summary>
        /// Gets the query phrase.
        /// </summary>
        /// <value>The query phrase.</value>
        internal static char Query_Phrase => '"';
        /// <summary>
        /// Gets the query zone.
        /// </summary>
        /// <value>The query zone.</value>
        internal static char Query_Zone => ':';
        /// <summary>
        /// Gets the query wildcard.
        /// </summary>
        /// <value>The query wildcard.</value>
        internal static char Query_Wildcard => '*';
        /// <summary>
        /// Gets the query truncated.
        /// </summary>
        /// <value>The query truncated.</value>
        internal static char Query_Truncated => '?';
        /// <summary>
        /// Gets the query proximity dist.
        /// </summary>
        /// <value>The query proximity dist.</value>
        internal static char Query_Proximity_Dist => '%';
        /// <summary>
        /// Gets the query proximity adj.
        /// </summary>
        /// <value>The query proximity adj.</value>
        internal static char Query_Proximity_Adj => '$';
        /// <summary>
        /// Gets the query frequency.
        /// </summary>
        /// <value>The query frequency.</value>
        internal static char Query_Frequency => '>';

        /// <summary>
        /// Gets the op leading.
        /// </summary>
        /// <value>The op leading.</value>
        internal static char OP_Leading => '(';
        /// <summary>
        /// Gets the op trailing.
        /// </summary>
        /// <value>The op trailing.</value>
        internal static char OP_Trailing => ')';

        /// <summary>
        /// Gets all query ops.
        /// </summary>
        /// <value>All query ops.</value>
        internal static string[] All_Query_Ops => new[] { "OR", "||", "NOT", "!", "AND", "&&", "&", "\"", ":", "*", "?", "%", "$", ">", "(", ")" };

        #endregion


        #region Methods        
        /// <summary>
        /// Determines whether the specified token is and.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns><c>true</c> if the specified token is and; otherwise, <c>false</c>.</returns>
        public static bool IsAND(string token) => (token.Equals(AND) || token.Equals(AND_0));

        /// <summary>
        /// Determines whether the specified token is a disjunction operator <c>[OR : ||]</c>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns><c>true</c> if the specified token is disjunction operator.; otherwise, <c>false</c>.</returns>
        public static bool IsOR(string token) => (token.Equals(OR) || token.Equals(OR_0));

        /// <summary>
        /// Determines whether the specified token is a negation operator <c>[NOT : !]</c>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns><c>true</c> if the specified token is negation operator.; otherwise, <c>false</c>.</returns>
        public static bool IsNOT(string token) => (token.Equals(NOT) || token.Equals(NOT_0));

        /// <summary>
        /// Gets the query operators.
        /// </summary>
        /// <returns>System.String[].</returns>
        public static string[] GetQueryOperators() {
            return All_Query_Ops;
        }

        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <param name="ex">The ex.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>System.String.</returns>
        internal static string ErrorMessage<S, D>(Exception ex, S source, D destination) {
            return string.Format($"[{nameof(source)} failed to serialize {nameof(destination)} to file]....Exception thrown @...\n{ex.StackTrace}");
        }

        #endregion

    }
}
