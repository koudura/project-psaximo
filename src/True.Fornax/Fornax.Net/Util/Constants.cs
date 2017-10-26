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

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Permissions;

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
        public const string WS_BROKERS = " \t\n\r\f";
        internal const string Num_Brokers = ".0123456789";
        internal const string GenOp_Brokers = " `^+=\\{};<,/[]@#\t\n\r\f";
        internal const string QueryOP_Broker = " &|\":*?%!>[]$~()";
        internal const string DocOP_Broker = "-.'";

        public static char[] Brokers => new[] { ' ', '\n', '\t', '\r', '\f' };
        #endregion

        #region Boolean Constants 

        /// <summary>
        /// Represents the Boolean Rule of Conjuction (AND).
        /// </summary>
        public const string AND = "AND";
        /// <summary>
        /// Represents the Boolean Rule of Conjuction (&&).
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

        internal static string FornaxDir => string.Format("{0}{1}", LoggingDirectory, @"\Fornax");
        internal static string LoggingDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static string TempPath => Path.GetTempPath();
        internal static string TempFile => Path.GetTempFileName();

        internal static DirectoryInfo BaseDirectory => GetBaseDir();

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

        internal const string CmpTxtfile = @".ztxt";
        internal const string CmpInvFile = @".zinx";
        internal const string CmpTrieFile = @".ztrx";
        internal const string CmpDictFile = @".zdix";
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
        internal const string ExtPartFile = @".4prt";
        /// <summary>
        /// fornax repository file for storing list of names raw files to be extracted, and their relative tags and attributes
        /// </summary>
        internal const string ExtRepoFile = @".4rep";
        /// <summary>
        ///  fornax cache file for storing short term data input e.g last query input(as char[]) and respective top 5-10 documents.
        ///  ,documents found with changes by the crawler. (attributes of query and results are also stored.)
        /// </summary>
        internal const string ExtCacheFile = @".4cache";
        /// <summary>
        /// fornax temporary storage file for disk-read/write opearations that take long processing time.
        /// e.g indexing a large corpus.
        /// </summary>
        internal const string ExtTempFile = @".4temp";
        /// <summary>
        /// fornax data file stores query history in a structured format, corpus history, suggestion history, 
        /// and long-term data from the cache file are moved here.
        /// </summary>
        internal const string ExtDataFile = @".4dat";
        /// <summary>
        /// fornax deletes file, stores the same data as repo-file <see cref="ExtRepoFile"/> only for file marked as to be deleted from 
        /// the index and fornax database by the crawler.
        /// </summary>
        internal const string ExtDelsFile = @".4del";

        #endregion

        #region query opeartors

        internal static string[] Query_Boolean => new[] { "&&", "AND", "&", "||", "OR", "NOT", "!" };
        internal static string[] Query_Boolean_OR => new[] { "||", "OR" };
        internal static string[] Query_Boolean_AND => new[] { "&", "&&", "AND" };
        internal static string[] Query_Boolean_NOT => new[] { "!", "NOT " };

        internal static char Query_Phrase => '"';
        internal static char Query_Zone => ':';
        internal static char Query_Wildcard => '*';
        internal static char Query_Truncated => '?';
        internal static char Query_Proximity_Dist => '%';
        internal static char Query_Proximity_Adj => '$';
        internal static char Query_Frequency => '>';

        internal static char OP_Leading => '(';
        internal static char OP_Trailing => ')';

        internal static string[] All_Query_Ops => new[] { "OR", "||", "NOT", "!", "AND", "&&", "&", "\"", ":", "*", "?", "%", "$", ">", "(", ")" };

        #endregion


        #region Methods        
        /// <summary>
        /// Determines whether the specified token is a conjuction operator <c>[AND : &&]</c>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <c>true</c> if the specified token is Conjuction operator.; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAND(string token) => (token.Equals(AND) || token.Equals(AND_0));

        /// <summary>
        /// Determines whether the specified token is a disjunction operator <c>[OR : ||]</c>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <c>true</c> if the specified token is disjunction operator.; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOR(string token) => (token.Equals(OR) || token.Equals(OR_0));

        /// <summary>
        /// Determines whether the specified token is a negation operator <c>[NOT : !]</c>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <c>true</c> if the specified token is negation operator.; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNOT(string token) => (token.Equals(NOT) || token.Equals(NOT_0));

        public static string[] GetQueryOperators() {
            return All_Query_Ops;
        }

        internal static string ErrorMessage<S, D>(Exception ex, S source, D destination) {
            return string.Format($"[{nameof(source)} failed to serialize {nameof(destination)} to file]....Exception thrown @...\n{ex.StackTrace}");
        }

        #endregion

    }
}
