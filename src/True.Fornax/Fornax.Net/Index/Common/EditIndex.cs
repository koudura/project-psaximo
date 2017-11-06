////// ***********************************************************************
////// Assembly         : Fornax.Net
////// Author           : Habuto Koudura
////// Created          : 10-23-2017
//////
////// Last Modified By : Habuto Koudura
////// Last Modified On : 10-29-2017
////// ***********************************************************************
////// <copyright file="EditIndex.cs" company="Microsoft">
/////***
////* Copyright (c) 2017 Koudura Ninci @True.Inc
////*
////* Permission is hereby granted, free of charge, to any person obtaining a copy
////* of this software and associated documentation files (the "Software"), to deal
////* in the Software without restriction, including without limitation the rights
////* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
////* copies of the Software, and to permit persons to whom the Software is
////* furnished to do so, subject to the following conditions:
////* 
////* The above copyright notice and this permission notice shall be included in all
////* copies or substantial portions of the Software.
////* 
////* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
////* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
////* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
////* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
////* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
////* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
////* SOFTWARE.
////*
////**/
////// </copyright>
////// <summary></summary>
////// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using Fornax.Net.Util.Collections;

/// <summary>
/// The Common namespace.
/// </summary>
namespace Fornax.Net.Index.Common
{
    /// <summary>
    /// Class EditIndex. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IDictionary{System.String, SortedList{float, List{int}}}" />
    /// <seealso cref="System.Collections.Generic.IComparer{KeyValuePair{string, float}}" />
    /// <seealso cref="System.Collections.Generic.IDictionary{List{string}, SortedList{float, List{int}}}" />
    [Serializable]
    public sealed class EditIndex :IEnumerable, IComparer<KeyValuePair<string,float>>, IDisposable
    {
        /// <summary>
        /// The edit index
        /// </summary>
        IDictionary<string, SortedList<float, List<int>>> edit_index = new Dictionary<string, SortedList<float, List<int>>>();

        /// <summary>
        /// Gets or sets the <see cref="SortedList{System.Single, List{System.Int32}}"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>SortedList&lt;System.Single, List&lt;System.Int32&gt;&gt;.</returns>
        internal SortedList<float, List<int>> this[string key] { get => edit_index[key]; set => edit_index[key] = value; }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The keys.</value>
        internal ICollection<string> Keys => edit_index.Keys;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The values.</value>
        internal ICollection<SortedList<float, List<int>>> Values => edit_index.Values;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count => edit_index.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        internal bool IsReadOnly => edit_index.IsReadOnly;

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        internal void Add(string key, SortedList<float, List<int>> value) {
            edit_index.Add(key, value);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        internal void Add(KeyValuePair<string, SortedList<float, List<int>>> item) {
            edit_index.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        internal void Clear() {
            edit_index.Clear();
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero
        /// <paramref name="x" /> is less than <paramref name="y" />.Zero
        /// <paramref name="x" /> equals <paramref name="y" />.Greater than zero
        /// <paramref name="x" /> is greater than <paramref name="y" />.</returns>
        public int Compare(KeyValuePair<string, float> x, KeyValuePair<string, float> y) {
            if (x.Value > y.Value) return -1;
            if (y.Value > x.Value) return 1;
            return 0;
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
        internal bool Contains(KeyValuePair<string, SortedList<float, List<int>>> item) {
            return edit_index.Contains(item);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns><see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
        internal bool ContainsKey(string key) {
            return edit_index.ContainsKey(key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        internal void CopyTo(KeyValuePair<string, SortedList<float, List<int>>>[] array, int arrayIndex) {
            edit_index.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is EditIndex) {
                return Collections.Equals(((EditIndex)(obj)).edit_index, edit_index);
            }
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, SortedList<float, List<int>>>> GetEnumerator() {
            return edit_index.GetEnumerator();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() {
            return Collections.GetHashCode(edit_index);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        internal bool Remove(string key) {
            return edit_index.Remove(key);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        internal bool Remove(KeyValuePair<string, SortedList<float, List<int>>> item) {
            return edit_index.Remove(item);
        }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() {
            return Collections.ToString(edit_index);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
        internal bool TryGetValue(string key, out SortedList<float, List<int>> value) {
            return edit_index.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return edit_index.GetEnumerator();
        }

        public void Dispose()
        {
            ((IDisposable)(edit_index)).Dispose();
        }
    }
}
