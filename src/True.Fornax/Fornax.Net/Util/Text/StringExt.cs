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
using System.Text;

namespace Fornax.Net.Util.Text
{
    /// <summary>
    /// simple class for holding extension methods for string.
    /// </summary>
    public static partial class Extensions
    {

        /// <summary>
        /// Determines whether a string is Empty or null.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is empty or null] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        [Obsolete("Use System.String.IsNullOrEmpty(string) instead", false)]
        public static bool IsEmptyOrNull(this string str) {
            return (str.Equals(string.Empty) || str == null);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str, Encoding encoding) {
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str) {
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>
        /// Compares to ordinal.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int CompareToOrdinal(this string str, string value) {
            return string.CompareOrdinal(str, value);
        }

        /// <summary>
        /// Swaps the specified leadindex.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="leadindex">The leadindex.</param>
        /// <param name="leadlength">The leadlength.</param>
        /// <param name="trailindex">The trailindex.</param>
        /// <param name="traillength">The traillength.</param>
        /// <returns></returns>
        public static string Swap(this string str, int leadindex, int leadlength, int trailindex, int traillength) {
            StringBuilder bconst_str = new StringBuilder(str);
            bconst_str.Remove(leadindex, leadlength).Insert(leadindex, str.Substring(trailindex, traillength));
            bconst_str.Remove(trailindex, traillength).Insert(trailindex, str.Substring(leadindex, leadlength));
            return bconst_str.ToString();
        }

        /// <summary>
        ///  Determines whether an exact match of <paramref name="continuum"/> occurs 
        ///  after the <paramref name="index"/> in string <paramref name="str"/>.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="index">The offset value, ranges from [-1 to length of string <paramref name="str"/>]</param>
        /// <param name="continuum">The string to match.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool ContinuesWith(this string str, int index, string continuum) {
            Contract.Requires(!string.IsNullOrEmpty(continuum));
            if (!((index + 1 >= 0) && (index + 1 < str.Length)) || continuum.Length > str.Length) throw new ArgumentException();
            return ContinuesWith(str, index, continuum.ToCharArray());
        }

        /// <summary>
        /// Determines whether an exact match of <paramref name="continuum"/> occurs 
        /// after the <paramref name="index"/> in string <paramref name="str"/>.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="index">The offset value, ranges from [-1 to length of string <paramref name="str"/>] 
        ///  NOTE: (index = -1 is the same as string.StartsWSith()).
        /// </param>
        /// <param name="continuum">The set of characters to match.</param>
        /// <returns></returns>
        public static bool ContinuesWith(this string str, int index, char[] continuum) {
            int offset = index + 1, lim = str.Length - offset;
            if (!((offset >= 0) && (offset < str.Length))) throw new ArgumentException();
            if (continuum.Length > str.Length || str.Substring(offset, lim).Length < continuum.Length) return false;

            int i = 0; bool matches = true;
            do {
                matches = (str[offset + i] == continuum[i]) ? true : false;
                i++;
            } while (matches && i < continuum.Length);
            return matches;
        }

        /// <summary>
        /// Returns the character (Unicode code point) at the specified index.
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="index">The index to the values.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">if index is not in string</exception>
        public static int CodePointAt(this string str, int index) {
            if ((index < 0) || (index >= str.Length)) throw new IndexOutOfRangeException(nameof(index));
            return Character.CodePointAtImpl(str.ToCharArray(), index, str.Length);
        }

        /// <summary>
        /// Cleans the specified string. i.e. Removes all whitespaces in string
        /// and removes all special characters if and only if
        /// <paramref name="allowDelimiters" /> is set to false
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="allowDelimiters">if set to <c>true</c> [allow delimiters].</param>
        /// <returns></returns>
        public static string Clean(this string str, bool allowDelimiters) {
            Contract.Requires(str != null);
            if (str == null)  throw new ArgumentNullException(nameof(str));

            str.Replace(" ", "");
            char[] buf = str.Trim().ToCharArray();
            if (!allowDelimiters) {
                foreach (var item in buf) {
                    if (!Char.IsLetterOrDigit(item))
                        str.Replace(item.ToString(), "");
                }
            }
            return str;
        }

        /// <summary>
        /// Replaces a character at a specified <paramref name="index"/> in 
        /// a string with another <c>(char)</c> <paramref name="value"/>.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="index">The index of old character.</param>
        /// <param name="value">The new character to be inserted.</param>
        /// <returns>A string with character at <paramref name="index"/> replaced with <paramref name="value"/>.</returns>
        public static string ReplaceAt(this string str, int index, char value) {
            if (str == null) throw new ArgumentNullException(nameof(str));

            char[] stch = str.ToCharArray();
            stch[index] = value;
            return new string(stch);
        }

        /// <summary>
        /// Determines whether this instance of string is a valid word.
        /// Word is a string that does not contains only [a-zA-Z] regular expression.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is word; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWord(this string str) {
            foreach (var ch in str.ToCharArray()) {
                if (!(char.IsLetter(ch) || ch == '_')) return false;
            }
            return true;
        }

        /// <summary>
        /// Splits a string into substrings that are based on the characters in the separators.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="separators">The separators.</param>
        /// <returns>an array of substrings.</returns>
        public static string[] Split(this string str, string separators) {
            return str.Split(separators.ToCharArray());
        }
    }
}
