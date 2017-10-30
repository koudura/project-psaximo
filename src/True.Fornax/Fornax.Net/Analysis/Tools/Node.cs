// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Habuto Koudura
// Created          : 10-21-2017
//
// Last Modified By : Habuto Koudura
// Last Modified On : 10-21-2017
// ***********************************************************************
// <copyright file="Node.cs" company="Microsoft">
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
using System.Linq;

using ProtoBuf;
using Collection = Fornax.Net.Util.Collections.Collections;

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Fornax.Net.Analysis.Tools
{
    /// <summary>
    /// Trie Node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IDictionary{T, Fornax.Net.Analysis.Tools.Node{T}}" />
    /// <seealso cref="IDictionary{TKey, Node}" />
    [Serializable, ProtoContract]
    [Progress("Node<T>", true, Documented = true, Tested = true)]
    internal class Node<T> : IDictionary<T, Node<T>>
    {
        /// <summary>
        /// The children
        /// </summary>
        [ProtoMember(1)]
        internal IDictionary<T, Node<T>> children;
        /// <summary>
        /// The eos
        /// </summary>
        private bool eos;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Node{T}"/> is eos.
        /// </summary>
        /// <value><c>true</c> if eos; otherwise, <c>false</c>.</value>
        [ProtoMember(2)]
        internal bool EOS { get { return eos; } set { eos = value; } }

        /// <summary>
        /// Gets an <see cref="Node{T}" /> containing the keys of the <see cref="Node{T}" />.
        /// </summary>
        /// <value>The keys.</value>
        [ProtoMember(3)]
        public ICollection<T> Keys => children.Keys;

        /// <summary>
        /// Gets an <see cref="ICollection" /> containing the values in the <see cref="Node{T}" />.
        /// </summary>
        /// <value>The values.</value>
        [ProtoMember(4)]
        public ICollection<Node<T>> Values => children.Values;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection" />.
        /// </summary>
        /// <value>The count.</value>
        [ProtoMember(5)]
        public int Count => children.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        [ProtoMember(6)]
        public bool IsReadOnly => children.IsReadOnly;

        /// <summary>
        /// Gets or sets the <see cref="Node{T}" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Node&lt;T&gt;.</returns>
        public Node<T> this[T key] { get => children[key]; set => children[key] = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node{T}" /> class.
        /// </summary>
        internal Node() {
            children = new Dictionary<T, Node<T>>();
            eos = false;
        }

        /// <summary>
        /// Determines whether the <see cref="Node{T}" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="Node{T}" />.</param>
        /// <returns><see langword="true" /> if the <see cref="Node{T}" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
        public bool ContainsKey(T key) {
            return children.ContainsKey(key);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(T key, Node<T> value) {
            children.Add(key, value);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public bool Remove(T key) {
            return children.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
        public bool TryGetValue(T key, out Node<T> value) {
            return children.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<T, Node<T>> item) {
            children.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear() {
            children.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
        public bool Contains(KeyValuePair<T, Node<T>> item) {
            return children.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<T, Node<T>>[] array, int arrayIndex) {
            children.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public bool Remove(KeyValuePair<T, Node<T>> item) {
            return children.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<T, Node<T>>> GetEnumerator() {
            return children.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return children.GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() {
            return Collection.ToString(children.ToList());
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) {
            if (obj is Node<T>) {
                var noden = obj as Node<T>;
                return Collection.Equals(children, noden.children);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() {
            return Collection.GetHashCode(children);
        }

    }
}