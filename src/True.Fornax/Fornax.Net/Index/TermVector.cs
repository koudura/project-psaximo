////// ***********************************************************************
////// Assembly         : Fornax.Net
////// Author           : Habuto Koudura
////// Created          : 09-23-2017
//////
////// Last Modified By : Habuto Koudura
////// Last Modified On : 10-28-2017
////// ***********************************************************************
////// <copyright file="TermVector.cs" company="True.inc">
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
using Fornax.Net.Search;
using ProtoBuf;

/// <summary>
/// The Index namespace.
/// </summary>
namespace Fornax.Net.Index
{
    /// <summary>
    /// Represents a positional posting of a term and all its relative position of
    /// occurence.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{Fornax.Net.Index.Term, Fornax.Net.Search.Vector}}" />
    /// <seealso cref="System.ICloneable" />
    /// <seealso cref="System.Collections.Generic.IDictionary{Term, Vector}" />
    /// <seealso cref="System.Collections.Generic.IDictionary{Term, ISet{long}}" />
    /// <seealso cref="java.io.Serializable.__Interface" />
    /// <seealso cref="System.Collections.Generic.IDictionary{Term, ISet{ulong}}" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{Term}" />
    [Serializable, ProtoContract]
    public class TermVector : IEnumerable<KeyValuePair<Term,Vector>>, ICloneable ,java.io.Serializable.__Interface
    {
        /// <summary>
        /// The true vector
        /// </summary>
        private readonly IDictionary<Term, Vector> true_vector = new Dictionary<Term, Vector>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TermVector" /> class.
        /// </summary>
        public TermVector() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TermVector" /> class.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="position">The position.</param>
        internal TermVector(Term term, long position)
        {
            var set = new HashSet<double> {
                position
            };
            true_vector.Add(term, new Vector(set));
        }

        private TermVector(IDictionary<Term, Vector> true_vector)
        {
            this.true_vector = true_vector;
        }

        /// <summary>
        /// Gets or sets the <see cref="Vector" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Vector.</returns>
        public Vector this[Term key] { get => true_vector[key]; set => true_vector[key] = value; }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The keys.</value>
        internal ICollection<Term> Keys => true_vector.Keys;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The values.</value>
        internal ICollection<Vector> Values => true_vector.Values;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count => true_vector.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        internal bool IsReadOnly => true_vector.IsReadOnly;

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        internal void Add(Term key, Vector value)
        {
            true_vector.Add(key, value);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        internal void Add(KeyValuePair<Term, Vector> item)
        {
            true_vector.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        internal void Clear()
        {
            true_vector.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
        internal bool Contains(KeyValuePair<Term, Vector> item)
        {
            return true_vector.Contains(item);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns><see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
        internal bool ContainsKey(Term key)
        {
            return true_vector.ContainsKey(key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        internal void CopyTo(KeyValuePair<Term, Vector>[] array, int arrayIndex)
        {
            true_vector.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<Term, Vector>> GetEnumerator()
        {
            return true_vector.GetEnumerator();
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        internal bool Remove(Term key)
        {
            return true_vector.Remove(key);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        internal bool Remove(KeyValuePair<Term, Vector> item)
        {
            return true_vector.Remove(item);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
        internal bool TryGetValue(Term key, out Vector value)
        {
            return true_vector.TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<Term, Vector>> IEnumerable<KeyValuePair<Term, Vector>>.GetEnumerator()
        {
            return true_vector.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return true_vector.GetEnumerator();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return new TermVector(true_vector);
        }
    }
}
