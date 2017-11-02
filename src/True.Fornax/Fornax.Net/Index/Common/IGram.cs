// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Kodex Zone
// Created          : 10-24-2017
//
// Last Modified By : Kodex Zone
// Last Modified On : 10-29-2017
// ***********************************************************************
// <copyright file="IGrammable.cs" company="True.Inc">
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


using System.Collections.Generic;

namespace Fornax.Net.Index.Common
{
    /// <summary>
    /// Gram Interface for handling (K and N) gram of texts.
    /// </summary>
    public interface IGram
    {
        /// <summary>
        /// Gets the grams of a specific text.
        /// </summary>
        /// <value>The grams.</value>
        IEnumerable<string> Grams { get; }

        /// <summary>
        /// Gets  of the grams.
        /// </summary>
        /// <value>The size.</value>
        uint Size { get; }

        /// <summary>
        /// Gets the number of produced n-grams.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }

        /// <summary>
        /// Gets the un-tokenized text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
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
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        string ToString();
    }
}
