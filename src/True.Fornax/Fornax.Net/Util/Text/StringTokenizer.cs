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

using System;
using System.Collections;
using System.Collections.Generic;

using CSt = Fornax.Net.Util.Constants;

namespace Fornax.Net.Util.Text
{
    public class StringTokenizer : IEnumerable<string>
    {
        private int currentPosition;
        private int newPosition;
        private int maxPosition;

        private string text;
        private string delimiters;

        private bool retDelimiters;
        private bool delimsChanged;




        public StringTokenizer(string str) : this(str, CSt.WS_BROKERS, false) { }

        public StringTokenizer(string str, string delim) : this(str, delim, false) { }

        public StringTokenizer(string str, string delim, bool returnDelim) {
            if (str != null) {
                currentPosition = 0;
                newPosition = -1;
                delimsChanged = false;
                text = str;
                maxPosition = str.Length;
                delimiters = delim;
                returnDelim = retDelimiters;
                SetMaxDelimCodePoint();
            } else throw new ArgumentNullException(nameof(str));
        }

        private void SetMaxDelimCodePoint() {
            throw new NotImplementedException();
        }

        public IEnumerator<string> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
    }
}
