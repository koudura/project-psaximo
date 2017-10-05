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

using CST = Fornax.Net.Util.Constants;

namespace Fornax.Net.Util.Text
{
    /// <summary> The <see cref="StringTokenizer"/> class alows an application to break a string 
    /// into tokens by performing code point comparison. <para></para>The <see cref="StringTokenizer"/> ethods do not distinguish 
    /// among identifiers, numbers, and quated strings, nor do they recognize and skip comments.
    /// </summary>
    public sealed class StringTokenizer : ITokenizer
    {
        private int currentPosition;
        private int newPosition;
        private int maxPosition;

        private string text;
        private string delimiters;

        private bool retDelimiters;
        private bool delimsChanged;

        private int maxDelimCodePoint;
        private bool hasSurrogatees = false;
        private int[] delimiterCodepoints;

        /// <summary>
        /// Sets the maximum delimiter code point to the highest <see cref="char"/> in the delimiter set.
        /// </summary>
        private void SetMaxDelimCodePoint() {
            if (delimiters == null) {
                maxDelimCodePoint = 0;
                return;
            }
            int c = 0, m = 0, count = 0;
            for (int i = 0; i < delimiters.Length; i += Character.CharCount(c)) {
                c = delimiters[i];
                if (c >= Character.MinHighSurrogate && c <= Character.MaxLowSurrogate) {
                    c = delimiters.CodePointAt(i);
                    hasSurrogatees = true;
                }
                if (m < c) m = c;
                count++;
            }
            maxDelimCodePoint = m;
            if (hasSurrogatees) {
                delimiterCodepoints = new int[count];
                for (int i = 0, j = 0; i < count; i++, j += Character.CharCount(c)) {
                    c = delimiters.CodePointAt(j);
                    delimiterCodepoints[i] = c;
                }
            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// Using Default delimiter set, which is <code>(" ";\t;\n;\r;\f")</code>.
        /// delimiters will not be treated as tokens.
        /// </summary>
        /// <param name="str">The string.</param>
        public StringTokenizer(string str) : this(str, CST.WS_BROKERS, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// Using specified characters in <paramref name="delim"/> as delimiters.
        /// delimiters will not be treated as tokens.
        /// </summary>
        /// <param name="str">The string to tokenize.</param>
        /// <param name="delim">The delimiter as string.</param>
        public StringTokenizer(string str, string delim) : this(str, delim, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// Using specified characters in <paramref name="delim"/> as delimiters.
        /// <paramref name="returnDelim"/> determines if delimiters are treated as tokens.
        /// </summary>
        /// <param name="str">The string to tokenize.</param>
        /// <param name="delim">The delimiters as string.</param>
        /// <param name="returnDelim">if set to <c>true</c> [return delimiter].</param>
        /// <exception cref="ArgumentNullException">str</exception>
        public StringTokenizer(string str, string delim, bool returnDelim) {
            if (str != null) {
                currentPosition = 0;
                newPosition = -1;
                delimsChanged = false;
                text = str;
                maxPosition = str.Length;
                delimiters = delim;
                retDelimiters = returnDelim;
                SetMaxDelimCodePoint();
            } else throw new ArgumentNullException(nameof(str));
        }

        /// <summary>
        /// Skips the delimiters.
        /// </summary>
        /// <param name="startPos">The start position.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private int SkipDelimiters(int startPos) {
            if (delimiters == null) throw new NullReferenceException();

            int position = startPos;
            while (!retDelimiters && position < maxPosition) {
                if (!hasSurrogatees) {
                    char c = text[position];
                    if ((c > maxDelimCodePoint) || (delimiters.IndexOf(c) < 0)) break;
                    position++;
                } else {
                    int c = text.CodePointAt(position);
                    if ((c > maxDelimCodePoint) || !IsDelimiter(c)) break;
                    position += Character.CharCount(c);
                }
            }
            return position;
        }

        /// <summary>
        /// Determines whether the specified code point is delimiter.
        /// </summary>
        /// <param name="codePoint">The code point.</param>
        /// <returns>
        ///   <c>true</c> if the specified code point is delimiter; otherwise, <c>false</c>.
        /// </returns>
        private bool IsDelimiter(int codePoint) {
            for (int i = 0; i < delimiterCodepoints.Length; i++) {
                if (delimiterCodepoints[i] == codePoint) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Skips ahead from <paramref name="strtPos"/> and returns the index of the next delimiter
        /// character encountered, or <see cref="maxPosition"/> if no such delimiter is found.
        /// </summary>
        /// <param name="strtPos">The START position.</param>
        /// <returns></returns>
        private int ScanToken(int strtPos) {
            int position = strtPos;
            while (position < maxPosition) {
                if (!hasSurrogatees) {
                    char c = text[position];
                    if ((c <= maxDelimCodePoint) && (delimiters.IndexOf(c) >= 0)) break;
                    position++;
                } else {
                    int c = text.CodePointAt(position);
                    if ((c <= maxDelimCodePoint) && IsDelimiter(c)) break;
                    position += Character.CharCount(c);
                }
            }

            if (retDelimiters && (strtPos == position)) {
                if (!hasSurrogatees) {
                    char c = text[position];
                    if (c <= maxDelimCodePoint && (delimiters.IndexOf(c) >= 0))
                        position++;
                } else {
                    int c = text.CodePointAt(position);
                    if ((c <= maxDelimCodePoint) && IsDelimiter(c))
                        position += Character.CharCount(c);
                }
            }
            return position;

        }


        /// <summary>
        /// Returns the current token from this string tokenizer.
        /// </summary>
        /// <returns>the current token from this string tokenizer.</returns>
        public string CurrentToken {
            get {

                currentPosition = (newPosition >= 0 && !delimsChanged) ? newPosition : SkipDelimiters(currentPosition);

                delimsChanged = false;
                newPosition = -1;

                if (currentPosition >= maxPosition) throw new IndexOutOfRangeException(nameof(currentPosition));
                int start = currentPosition;
                currentPosition = ScanToken(currentPosition);
                return text.Substring(start, (currentPosition - start));
            }
        }

        /// <summary>
        /// Returns the next tokens afer changing the delimiters.
        /// </summary>
        /// <param name="delim">The new delimiters.</param>
        /// <returns></returns>
        public string NextToken(string delim) {
            delimiters = delim;

            delimsChanged = true;

            SetMaxDelimCodePoint();
            return CurrentToken;
        }

        /// <summary>
        /// Tests if there are more tokens available from this tokenizer's string.
        /// If this method returns <c>true</c>, then a subsequent call to <see cref="NextToken"/> 
        /// wil successfully return a token.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if and only if there is at least one token in the string after the current position
        ///   ; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMoreTokens() {
            /**
             * Temporarily store this position and use it in the  following nexToken method,
             * only if the delimiters have changed int the NextToken() invocation.
             * then use the computed value.
             */
            newPosition = SkipDelimiters(currentPosition);
            return (newPosition < maxPosition);
        }

        /// <summary>
        /// Returns the same value as the <see cref="HasMoreTokens()"/> method.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if there are more tokens; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMoreElements() {
            return HasMoreTokens();
        }

        /// <summary>
        /// Returns the value as the <code>NextToken</code> method, except that its declared value is
        /// <see cref="object"/> rather than <see cref="string"/>.
        /// </summary>
        /// <returns>the next token in the string. <seealso cref="ITokenizer"/> , <seealso cref="CurrentToken"/></returns>
        public object CurrentElement => CurrentToken;

        /// <summary>
        /// Calculates the number of times that this tokenizer's <code>NextToken</code> method can be called before
        /// it generates an exception. The current position is not advanced.
        /// </summary>
        /// <returns>the number of tokens remaining in the string using the current delimiter set. <seealso cref="CurrentToken"/></returns>
        public int CountTokens() {
            int count = 0;
            int currentPos = currentPosition;
            while (currentPos < maxPosition) {
                currentPos = SkipDelimiters(currentPos); ;
                if (currentPos >= maxPosition) break;
                currentPos = ScanToken(currentPos);
                count++;
            }
            return count;
        }

    }
}