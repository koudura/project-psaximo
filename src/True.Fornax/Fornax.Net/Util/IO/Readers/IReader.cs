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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fornax.Net.Util.IO.Readers
{
    /// <summary>
    /// Reader methods founctions for  Extraction of Text-Content from files.
    /// </summary>
    public interface IReader
    {
        /// <summary>
        /// Implementation of the BufferedReader .net, may be used to extract types of
        /// <see cref="FornaxFormat.Dom"/> , <see cref="FornaxFormat.Plain"/>, <see cref="FornaxFormat.Web"/>.
        /// NOTE: Only the Raw-Text content is returned.
        /// </summary>
        /// <returns>String Text content of the specified file.</returns>
        string BufferRead();

        /// <summary>
        /// Asynchronized implementation of the BufferedReader .net, may be used to extract types of
        /// <see cref="FornaxFormat.Dom"/> , <see cref="FornaxFormat.Plain"/>, <see cref="FornaxFormat.Web"/>.
        /// NOTE: Only the Raw-Text content is returned.
        /// </summary>
        /// <returns>String Text content of the specified file.</returns>
        Task<string> BufferReadAsync();

        /// <summary>
        /// Use Tika Extractor to Read file.
        /// </summary>
        /// <exception cref="FornaxException">category</exception>
        (string ContentType, string Text, IDictionary<string, string> Metadata) TikaRead();

        /// <summary>
        /// Use Tika Extractor to Read file asynchronously.
        /// </summary>
        Task<(string ContentType, string Text, IDictionary<string, string> Metadata)> TikaReadAsync();

        /// <summary>
        /// Uses Toxy Dom Reader to extract content from DOM file.
        /// i.e. @ <see cref="FornaxFormat.Dom" />.
        /// </summary>
        /// <returns>
        /// content details of the Dom-object file.
        /// </returns>
        /// <exception cref="FornaxFormatException">category</exception>
        (string Name, string Text, string InnerText, string NodeString, Dictionary<string, string> Attributes) ToxyDomRead();

        /// <summary>
        /// Uses Toxy Dom Reader to extract content from DOM file asynchronously.
        /// i.e. @ <see cref="FornaxFormat.Dom" />.
        /// </summary>
        /// <returns>
        /// content details of the Dom-object file.
        /// </returns>
        /// <exception cref="FornaxFormatException">category</exception>
        Task<(string Name, string Text, string InnerText, string NodeString, Dictionary<string, string> Attributes)> ToxyDomReadAsync();

        /// <summary>
        /// Uses Toxy Email Reader to extract content from email files.
        /// i.e. <see cref="FornaxFormat.Email"/>.
        /// </summary>
        /// <returns>content details of the email file</returns>
        /// <exception cref="FornaxFormatException">file</exception>
        (string Attributes, string Text) ToxyEmailRead();

        /// <summary>
        /// Uses Toxy Email Reader to extract content from email files asynchronously.
        /// i.e. <see cref="FornaxFormat.Email"/>.
        /// </summary>
        /// <returns>content details of the email file</returns>
        /// <exception cref="FornaxFormatException">file</exception>
        Task<(string Attributes, string Text)> ToxyEmailReadAsync();

        /// <summary>
        /// Uses Toxy Slide Reader to extract content from Slide file.
        /// i.e. @ <see cref="FornaxFormat.Slide" />
        /// </summary>
        /// <returns>
        /// content details of the slide-show file.
        /// </returns>
        /// <exception cref="FornaxFormatException">file</exception>
        (string Note, string Text) ToxySlideRead();

        /// <summary>
        /// Uses Toxy Slide Reader to extract content from Slide file asynchronously.
        /// i.e. @ <see cref="FornaxFormat.Slide" />
        /// </summary>
        /// <returns>
        /// content details of the slide-show file.
        /// </returns>
        /// <exception cref="FornaxFormatException">file</exception>
        Task<(string Note, string Text)> ToxySlideReadAsync();

        /// <summary>
        /// Uses Toxy Text Reader to extract content from text and plain files.
        /// i.e. @ <see cref="FornaxFormat.Plain" /> and <see cref="FornaxFormat.Text" />
        /// </summary>
        /// <returns>
        /// content details of the plain-text file.
        /// </returns>
        /// <exception cref="FornaxFormatException">file</exception>
        string ToxyTextRead();

        /// <summary>
        /// Uses Toxy Text Reader to extract content from text and plain files asynchronously.
        /// i.e. @ <see cref="FornaxFormat.Plain" /> and <see cref="FornaxFormat.Text" />
        /// </summary>
        /// <returns>
        /// content details of the plain-text file.
        /// </returns>
        /// <exception cref="FornaxFormatException">file</exception>
        Task<string> ToxyTextReadAsync();

        /// <summary>
        /// Uses Toxy V-card reader to extract content from vcf files. 
        /// </summary>
        /// <returns>content details of the vcf file.</returns>
        /// <exception cref="FornaxFormatException">file</exception>
        (string Attributes, string Text) ToxyVCardRead();

        /// <summary>
        /// Uses Toxy V-card reader to extract content from vcf files asynchronously.
        /// </summary>
        /// <returns>content details of the vcf file.</returns>
        /// <exception cref="FornaxFormatException">file</exception>
        Task<(string Attributes, string Text)> ToxyVCardReadAsync();

        /// <summary>
        /// Reads an Xml file.
        /// </summary>
        /// <returns>Text content of the specified xml</returns>
        string XmlRead();

        /// <summary>
        /// Reads an Xml file asynchronously.
        /// </summary>
        /// <returns>Text content of the specified xml</returns>
        Task<string> XmlReadAsync();
    }
}