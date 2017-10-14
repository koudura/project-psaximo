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

namespace Fornax.Net.Util.Collections.Generic
{
    /// <summary>
    /// An Interface for wrapping the <see cref="SubList{T}"/> class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISubList<T>
    {
        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>
        /// The <c>T</c> data.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns> an item of <c>T</c> at <paramref name="index"/></returns>
        /// <exception cref="IndexOutOfRangeException">
        /// </exception>
        T this[int index] { get; set; }

        /// <summary>
        /// Gets the container, which is the Current state of the <see cref="IList{T}"/> which holds the sublist.
        /// </summary>
        /// <value>
        /// The container list.
        /// </value>
        IList<T> Container { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="SubList{T}" />.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="SubList{T}" /> is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Adds an item to the <see cref="SubList{T}" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="SubList{T}" />.</param>
        void Add(T item);

        /// <summary>
        /// Removes all items from the <see cref="SubList{T}" />.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the <see cref="SubList{T}" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="SubList{T}" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> is found in the <see cref="SubList{T}" />; otherwise, <see langword="false" />.
        /// </returns>
        bool Contains(T item);

        /// <summary>
        /// Copies the elements of the <see cref="SubList{T}" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="SubList{T}" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        void CopyTo(T[] array, int arrayIndex);

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        bool Equals(object obj);

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="SubList{T}"/> collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<T> GetEnumerator();

        /// <summary>
        /// Returns a hash code for this instance of sublist.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        int GetHashCode();

        /// <summary>
        /// Determines the index of a specific item in the <see cref="SubList{T}" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="SubList{T}" />.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        int IndexOf(T item);
        /// <summary>
        /// Inserts an item to the <see cref="SubList{T}" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref = "SubList{T}" />.</param>
        /// <exception cref="NotSupportedException"></exception>
        void Insert(int index, T item);

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="SubList{T}" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="SubList{T}" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        bool Remove(T item);

        /// <summary>
        /// Removes the <see cref="SubList{T}" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        void RemoveAt(int index);
    }
}