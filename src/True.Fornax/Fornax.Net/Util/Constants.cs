/** MIT LICENSE
*   Copyright (c) 2017 Koudura Ninci @True.Inc
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
        public const string OR= "OR";
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

        internal const string WS_BROKERS = " \t\n\r\f";

        internal static readonly string LoggingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);


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

        public static string GetQueryOperators() {
            return null;
        }



        #endregion

    }
}
