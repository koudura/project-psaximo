// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 10-31-2017
// ***********************************************************************
// <copyright file="IDocument.cs" company="True.Inc">
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

// </copyright>
// <summary></summary>
// ***********************************************************************

using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Index;

namespace Fornax.Net.Document
{
    /// <summary>
    /// Interface IDocument
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// Gets the capture.
        /// </summary>
        /// <value>The capture.</value>
        Snippet Capture { get; }
        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>The format.</value>
        FileFormat Format { get; }
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        ulong ID { get; }
        /// <summary>
        /// Gets the link.
        /// </summary>
        /// <value>The link.</value>
        string Link { get; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        /// <summary>
        /// Gets the terms.
        /// </summary>
        /// <value>The terms.</value>
        TermVector Terms { get; }
        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        bool Equals(object obj);
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        int GetHashCode();
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString();
    }
}