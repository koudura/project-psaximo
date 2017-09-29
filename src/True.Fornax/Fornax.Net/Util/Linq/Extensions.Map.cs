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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

using Fornax.Net.Util.System;
using Fornax.Net.Util.Text;
using CC = Fornax.Net.Util.Collections.Collections;

namespace Fornax.Net.Util.Linq
{
    public static partial class Extensions
    {

        /// <summary>
        /// This is the same implementation of ToString from Java's AbstractMap
        /// (the default implementation for all dictionaries)
        /// </summary>
        public static string ToString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) {
            if (dictionary == null) {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (dictionary.Count == 0) {
                return "{}";
            }

            bool keyIsValueType = typeof(TKey).GetTypeInfo().IsValueType;
            bool valueIsValueType = typeof(TValue).GetTypeInfo().IsValueType;
            using (var i = dictionary.GetEnumerator()) {
                StringBuilder sb = new StringBuilder();
                sb.Append('{');
                i.MoveNext();
                while (true) {
                    KeyValuePair<TKey, TValue> e = i.Current;
                    TKey key = e.Key;
                    TValue value = e.Value;
                    sb.Append(object.ReferenceEquals(key, dictionary) ? "(this Dictionary)" : (keyIsValueType ? key.ToString() : CC.ToString(key)));
                    sb.Append('=');
                    sb.Append(object.ReferenceEquals(value, dictionary) ? "(this Dictionary)" : (valueIsValueType ? value.ToString() : CC.ToString(value)));
                    if (!i.MoveNext()) {
                        return sb.Append('}').ToString();
                    }
                    sb.Append(',').Append(' ');
                }
            }
        }

        /// <summary>
        /// This is the same implementation of ToString from Java's AbstractMap
        /// (the default implementation for all dictionaries), plus the ability
        /// to specify culture for formatting of nested numbers and dates. Note that
        /// this overload will change the culture of the current thread.
        /// </summary>
        public static string ToString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, CultureInfo culture) {
            if (dictionary == null) {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (culture == null) {
                throw new ArgumentNullException(nameof(culture));
            }

            using (var context = new CultureContext(culture)) {
                return ToString(dictionary);
            }
        }

        /// <summary>
        /// Puts all each <see cref="KeyValuePair{TKey, TValue}"/> into a <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="kvps">The KVPS.</param>
        public static void PutAll<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> kvps) {
            if (kvps == null)
                throw new ArgumentNullException();
            foreach (var kvp in kvps) {
                dict[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// Puts the specified key into <see cref="IDictionary{TKey, TValue}"/> like <see cref="IDictionary{TKey, TValue}.Add(TKey, TValue)"/>
        /// but returns the value that was added, but unlike a <see cref="IDictionary{TKey, TValue}"/> it replaces the old value at
        /// <typeparamref name="TKey"/> with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TValue Put<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value) {
            if (dict == null)
                return default(TValue);

            var oldValue = dict.ContainsKey(key) ? dict[key] : default(TValue);
            dict[key] = value;
            return oldValue;
        }

        private static readonly int NONE = 0, SLASH = 1, UNICODE = 2, CONTINUE = 3, KEY_DONE = 4, IGNORE = 5;
        private static string lineSeparator = Environment.NewLine;

        /// <summary>
        /// Loads properties from the specified <see cref="Stream"/>. The encoding is
        /// ISO8859-1. 
        /// </summary>
        /// <remarks>
        /// <![CDATA[Sourced From Apache Harmony via Lucene]]>
        /// The Properties file is interpreted according to the
        /// following rules:
        /// <list type="bullet">
        ///     <item><description>
        ///         Empty lines are ignored.
        ///     </description></item>
        ///     <item><description>
        ///         Lines starting with either a "#" or a "!" are comment lines and are
        ///         ignored.
        ///     </description></item>
        ///     <item><description>
        ///         A backslash at the end of the line escapes the following newline
        ///         character ("\r", "\n", "\r\n"). If there's a whitespace after the
        ///         backslash it will just escape that whitespace instead of concatenating
        ///         the lines. This does not apply to comment lines.
        ///     </description></item>
        ///     <item><description>
        ///         A property line consists of the key, the space between the key and
        ///         the value, and the value. The key goes up to the first whitespace, "=" or
        ///         ":" that is not escaped. The space between the key and the value contains
        ///         either one whitespace, one "=" or one ":" and any number of additional
        ///         whitespaces before and after that character. The value starts with the
        ///         first character after the space between the key and the value.
        ///     </description></item>
        ///     <item><description>
        ///         Following escape sequences are recognized: "\ ", "\\", "\r", "\n",
        ///         "\!", "\#", "\t", "\b", "\f", and "&#92;uXXXX" (unicode character).
        ///     </description></item>
        /// </list>
        /// <para/>
        /// This method is to mimic and interoperate with the Properties class in Java, which
        /// is essentially a string dictionary that natively supports importing and exporting to this format.
        /// </remarks>
        /// <param name="dict">This dictionary.</param>
        /// <param name="input">The <see cref="Stream"/>.</param>
        /// <exception cref="IOException">If error occurs during reading from the <see cref="Stream"/>.</exception>
        public static void Load(this IDictionary<string, string> dict, Stream input) {
            if (input == null) {
                throw new ArgumentNullException("input");
            }
            lock (dict) {
                int mode = NONE, unicode = 0, count = 0;
                char nextChar;
                char[] buf = new char[40];
                int offset = 0, keyLength = -1, intVal;
                bool firstChar = true;
                Stream bis = input;

                while (true) {
                    intVal = bis.ReadByte();
                    if (intVal == -1) {
                        if (mode == UNICODE && count < 4) {
                            throw new ArgumentException("Invalid Unicode sequence: expected format \\uxxxx"); //$NON-NLS-1$
                        }
                        // if mode is SLASH and no data is read, should append '\u0000' to buffer
                        if (mode == SLASH) {
                            buf[offset++] = '\u0000';
                        }
                        break;
                    }
                    nextChar = (char)(intVal & 0xff);

                    if (offset == buf.Length) {
                        char[] newBuf = new char[buf.Length * 2];
                        Array.Copy(buf, 0, newBuf, 0, offset);
                        buf = newBuf;
                    }
                    if (mode == UNICODE) {
                        int digit = Character.Digit(nextChar, 16);
                        if (digit >= 0) {
                            unicode = (unicode << 4) + digit;
                            if (++count < 4) {
                                continue;
                            }
                        } else if (count <= 4) {
                            // luni.09=Invalid Unicode sequence: illegal character
                            throw new ArgumentException("Invalid Unicode sequence: illegal character"); //$NON-NLS-1$
                        }
                        mode = NONE;
                        buf[offset++] = (char)unicode;
                        if (nextChar != '\n') {
                            continue;
                        }
                    }
                    if (mode == SLASH) {
                        mode = NONE;
                        switch (nextChar) {
                            case '\r':
                                mode = CONTINUE; // Look for a following \n
                                continue;
                            case '\n':
                                mode = IGNORE; // Ignore whitespace on the next line
                                continue;
                            case 'b':
                                nextChar = '\b';
                                break;
                            case 'f':
                                nextChar = '\f';
                                break;
                            case 'n':
                                nextChar = '\n';
                                break;
                            case 'r':
                                nextChar = '\r';
                                break;
                            case 't':
                                nextChar = '\t';
                                break;
                            case 'u':
                                mode = UNICODE;
                                unicode = count = 0;
                                continue;
                        }
                    } else {
                        switch (nextChar) {
                            case '#':
                            case '!':
                                if (firstChar) {
                                    while (true) {
                                        intVal = bis.ReadByte();
                                        if (intVal == -1) {
                                            break;
                                        }
                                        // & 0xff not required
                                        nextChar = (char)intVal;
                                        if (nextChar == '\r' || nextChar == '\n') {
                                            break;
                                        }
                                    }
                                    continue;
                                }
                                break;
                            case '\n':
                                if (mode == CONTINUE) { // Part of a \r\n sequence
                                    mode = IGNORE; // Ignore whitespace on the next line
                                    continue;
                                }
                                // fall into the next case
                                mode = NONE;
                                firstChar = true;
                                if (offset > 0 || (offset == 0 && keyLength == 0)) {
                                    if (keyLength == -1) {
                                        keyLength = offset;
                                    }
                                    string temp = new string(buf, 0, offset);
                                    dict.Put(temp.Substring(0, keyLength), temp
                                            .Substring(keyLength));
                                }
                                keyLength = -1;
                                offset = 0;
                                continue;
                            case '\r':
                                mode = NONE;
                                firstChar = true;
                                if (offset > 0 || (offset == 0 && keyLength == 0)) {
                                    if (keyLength == -1) {
                                        keyLength = offset;
                                    }
                                    string temp = new string(buf, 0, offset);
                                    dict.Put(temp.Substring(0, keyLength), temp
                                            .Substring(keyLength));
                                }
                                keyLength = -1;
                                offset = 0;
                                continue;
                            case '\\':
                                if (mode == KEY_DONE) {
                                    keyLength = offset;
                                }
                                mode = SLASH;
                                continue;
                            case ':':
                            case '=':
                                if (keyLength == -1) { // if parsing the key
                                    mode = NONE;
                                    keyLength = offset;
                                    continue;
                                }
                                break;
                        }
                        if (nextChar < 256 && char.IsWhiteSpace(nextChar)) {
                            if (mode == CONTINUE) {
                                mode = IGNORE;
                            } 
                            if (offset == 0 || offset == keyLength || mode == IGNORE) {
                                continue;
                            }
                            if (keyLength == -1) {
                                mode = KEY_DONE;
                                continue;
                            }
                        }
                        if (mode == IGNORE || mode == CONTINUE) {
                            mode = NONE;
                        }
                    }
                    firstChar = false;
                    if (mode == KEY_DONE) {
                        keyLength = offset;
                        mode = NONE;
                    }
                    buf[offset++] = nextChar;
                }
                if (keyLength == -1 && offset > 0) {
                    keyLength = offset;
                }
                if (keyLength >= 0) {
                    string temp = new string(buf, 0, offset);
                    dict.Put(temp.Substring(0, keyLength), temp.Substring(keyLength));
                }
            }
        }

        /// <summary>
        /// Stores the mappings in this Properties to the specified
        /// <see cref="Stream"/>, putting the specified comment at the beginning. The
        /// output from this method is suitable for being read by the
        /// <see cref="Load(IDictionary{string, string}, Stream)"/> method.
        /// </summary>
        /// <param name="dict">This dictionary.</param>
        /// <param name="output">The output <see cref="Stream"/> to write to.</param>
        /// <param name="comments">The comments to put at the beginning.</param>
        /// <exception cref="IOException">If an error occurs during the write to the <see cref="Stream"/>.</exception>
        /// <exception cref="InvalidCastException">If the key or value of a mapping is not a <see cref="string"/>.</exception>
        public static void Store(this IDictionary<string, string> dict, Stream output, string comments) {
            lock (dict) {
                StreamWriter writer = new StreamWriter(output, Encoding.GetEncoding("iso-8859-1")); //$NON-NLS-1$
                if (comments != null) {
                    WriteComments(writer, comments);
                }
                writer.Write('#');
                writer.Write(new DateTime().ToString("yyyy-MM-dd"));
                writer.Write(lineSeparator);

                StringBuilder buffer = new StringBuilder(200);
                foreach (var entry in dict) {
                    string key = entry.Key;
                    DumpString(buffer, key, true);
                    buffer.Append('=');
                    DumpString(buffer, entry.Value, false);
                    buffer.Append(lineSeparator);
                    writer.Write(buffer.ToString());
                    buffer.Length = 0;
                }
                writer.Flush();
            }
        }

        private static void WriteComments(TextWriter writer, string comments) {
            writer.Write('#');
            char[] chars = comments.ToCharArray();
            for (int index = 0; index < chars.Length; index++) {
                if (chars[index] == '\r' || chars[index] == '\n') {
                    int indexPlusOne = index + 1;
                    if (chars[index] == '\r' && indexPlusOne < chars.Length
                            && chars[indexPlusOne] == '\n') {
                        // "\r\n"
                        continue;
                    }
                    writer.Write(lineSeparator);
                    if (indexPlusOne < chars.Length
                            && (chars[indexPlusOne] == '#' || chars[indexPlusOne] == '!')) {
                        // return char with either '#' or '!' afterward
                        continue;
                    }
                    writer.Write('#');
                } else {
                    writer.Write(chars[index]);
                }
            }
            writer.Write(lineSeparator);
        }

        private static void DumpString(StringBuilder buffer, string str, bool isKey) {
            int index = 0, length = str.Length;
            if (!isKey && index < length && str[index] == ' ') {
                buffer.Append("\\ "); //$NON-NLS-1$
                index++;
            }

            for (; index < length; index++) {
                char ch = str[index];
                switch (ch) {
                    case '\t':
                        buffer.Append("\\t"); //$NON-NLS-1$
                        break;
                    case '\n':
                        buffer.Append("\\n"); //$NON-NLS-1$
                        break;
                    case '\f':
                        buffer.Append("\\f"); //$NON-NLS-1$
                        break;
                    case '\r':
                        buffer.Append("\\r"); //$NON-NLS-1$
                        break;
                    default:
                        if ("\\#!=:".IndexOf(ch) >= 0 || (isKey && ch == ' ')) {
                            buffer.Append('\\');
                        }
                        if (ch >= ' ' && ch <= '~') {
                            buffer.Append(ch);
                        } else {
                            buffer.Append(ToHexaDecimal(ch));
                        }
                        break;
                }
            }
        }

        private static char[] ToHexaDecimal(int ch) {
            char[] hexChars = { '\\', 'u', '0', '0', '0', '0' };
            int hexChar, index = hexChars.Length, copyOfCh = ch;
            do {
                hexChar = copyOfCh & 15;
                if (hexChar > 9) {
                    hexChar = hexChar - 10 + 'A';
                } else {
                    hexChar += '0';
                }
                hexChars[--index] = (char)hexChar;
            } while ((copyOfCh = (int)((uint)copyOfCh >> 4)) != 0);
            return hexChars;
        }
    }
}
