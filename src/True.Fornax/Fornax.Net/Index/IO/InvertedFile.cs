// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Habuto Koudura
// Created          : 09-23-2017
//
// Last Modified By : Habuto Koudura
// Last Modified On : 10-29-2017
// ***********************************************************************
// <copyright file="InvertedFile.cs" company="Microsoft">
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


using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

/// <summary>
/// The IO namespace.
/// </summary>
namespace Fornax.Net.Index.IO
{
    /// <summary>
    /// Class InvertedFile.
    /// </summary>
    /// <seealso cref="System.ICloneable" />
    /// <seealso cref="System.Collections.Generic.IDictionary{Fornax.Net.Index.Term, Fornax.Net.Index.Postings}" />
    [Serializable, ProtoContract]
    public class InvertedFile : ICloneable , IEnumerable
    {
        /// <summary>
        /// The index
        /// </summary>
        [ProtoMember(1)]
        private readonly IDictionary<Term, Postings> index = new SortedList<Term, Postings>();

        /// <summary>
        /// Gets or sets the <see cref="Postings" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Postings.</returns>
        internal Postings this[Term key] { get => index[key]; set => index[key] = value; }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The keys.</value>
        internal ICollection<Term> Keys => index.Keys;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The values.</value>
        internal ICollection<Postings> Values => index.Values;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count => index.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        internal bool IsReadOnly => index.IsReadOnly;

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        internal void Add(Term key, Postings value) {
            index.Add(key, value);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        internal void Add(KeyValuePair<Term, Postings> item) {
            index.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        internal void Clear() {
            index.Clear();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            var clone = ((ICloneable)(index)).Clone();
            return (InvertedFile)clone;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
        internal bool Contains(KeyValuePair<Term, Postings> item) {
            return index.Contains(item);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns><see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
        internal bool ContainsKey(Term key) {
            return index.ContainsKey(key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        internal void CopyTo(KeyValuePair<Term, Postings>[] array, int arrayIndex) {
            index.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<Term, Postings>> GetEnumerator() {
            return index.GetEnumerator();
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        internal bool Remove(Term key) {
            return index.Remove(key);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        internal bool Remove(KeyValuePair<Term, Postings> item) {
            return index.Remove(item);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
        internal bool TryGetValue(Term key, out Postings value) {
            return index.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return index.GetEnumerator();
        }
    }
}
