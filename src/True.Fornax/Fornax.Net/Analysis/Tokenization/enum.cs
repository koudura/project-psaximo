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

namespace Fornax.Net.Analysis.Tokenization
{
    public enum TokenAttribute : int
    {
        /// <summary>
        /// Represents the number token types. 
        /// </summary>
        Number = 0, //regex match [//d]+

        /// <summary>
        /// Represents any email in a field of text.
        /// </summary>
        Email = 1, //regex match ([\\w]+@[\\w]+\.[\\w]+]

        /// <summary>
        /// Represents any date in a field of text.
        /// </summary>
        Date = 2, //if and only if convertible to datatime format.

        /// <summary>
        /// Represents any word in a field of text.
        /// words must contain all letters.
        /// </summary>
        Word = 3, //.isWord() or match [\\w]+

        /// <summary>
        /// Represents any acronym or abbreviations in a field of text.
        /// </summary>
        Acronym = 4, //must match [[\\w]+.]

        /// <summary>
        /// Represents any isolated character in a field of text.
        /// </summary>
        Character = 5, //must mactch [\\s\\S]

        /// <summary>
        /// Represents any [link|uri|path] string in a field of text.
        /// </summary>
        Link = 6, //must be uri.wellformeduristring

        /// <summary>
        /// Represents any set or singlenton operator(s) (i.e non-word symbols) in a field of text.
        /// </summary>
        Operator = 7, //must match [\\W]+

        /// <summary>
        /// Represents any string that (cannot or do not fall into any other category of attributes),
        /// in a field of text.
        /// </summary>
        Unknown = 8 //no match found
    }
}
