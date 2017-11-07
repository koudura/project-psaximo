// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-02-2017
// ***********************************************************************
// <copyright file="Postings.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
//// ***********************************************************************
//// Assembly         : Fornax.Net
//// Author           : Kodex Zone
//// Created          : 10-27-2017
////
//// Last Modified By : Kodex Zone
//// Last Modified On : 10-28-2017
//// ***********************************************************************
//// <copyright file="Postings.cs" company="True.inc">
//
//* Copyright (c) 2017 Koudura Ninci @True.Inc
//*
//* Permission is hereby granted, free of charge, to any person obtaining a copy
//* of this software and associated documentation files (the "Software"), to deal
//* in the Software without restriction, including without limitation the rights
//* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//* copies of the Software, and to permit persons to whom the Software is
//* furnished to do so, subject to the following conditions:
//* 
//* The above copyright notice and this permission notice shall be included in all
//* copies or substantial portions of the Software.
//* 
//* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//* SOFTWARE.
//*
//**/
//// </copyright>
//// <summary></summary>
//// ***********************************************************************


using System;
using System.Collections;
using System.Collections.Generic;

using Fornax.Net.Search;
using Fornax.Net.Util.Collections;

/// <summary>
/// The Index namespace.
/// </summary>
namespace Fornax.Net.Index
{

    /// <summary>
    /// Postings list representation for fornax.net
    /// </summary>
    /// <seealso cref="IEnumerable" />
    /// <seealso cref="ICloneable" />
    /// <seealso cref="Collections.Generic.IEnumerable{KeyValuePair{ulong,Vector}}" />
    /// <seealso cref="java.io.Serializable.__Interface" />
    /// <seealso cref="IComparable{Postings}" />
    /// <seealso cref="IDictionary{System.UInt64, Vector}" />
    [Serializable]
    public class Postings : IEnumerable, IComparable<Postings>, ICloneable, java.io.Serializable.__Interface
    {
        /// <summary>
        /// The postdictionary
        /// </summary>
        private readonly IDictionary<ulong, Vector> Postdictionary = new SortedList<ulong, Vector>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Postings" /> class.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="Docid">The docid.</param>
        /// <param name="init_position">The initialize position.</param>
        internal Postings(ulong Docid, long init_position)
        {
            Postdictionary = new SortedList<ulong, Vector> {
                { Docid, new Vector(init_position) }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Postings" /> class.
        /// </summary>
        /// <param name="Docid">The docid.</param>
        /// <param name="vector">The vector.</param>
        internal Postings(ulong Docid, Vector vector)
        {
            if (Postdictionary.TryGetValue(Docid, out Vector vect))
            {
                Postdictionary[Docid] = vector;
            }
            else
            {
                Postdictionary.Add(Docid, vector);
            }
        }

        private Postings(IDictionary<ulong, Vector> postdictionary)
        {
            Postdictionary = postdictionary;
        }

        /// <summary>
        /// Gets an <see cref="ICollection" /> containing the keys of the <see cref="IDictionary" />.
        /// </summary>
        /// <value>The keys.</value>
        internal ICollection<ulong> Keys => Postdictionary.Keys;

        /// <summary>
        /// Gets an <see cref=" ICollection`1" /> containing the values in the <see cref="IDictionary" />.
        /// </summary>
        /// <value>The values.</value>
        internal ICollection<Vector> Values => Postdictionary.Values;

        /// <summary>
        /// Gets the number of elements contained in the <see cref=" ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count => Postdictionary.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref=" ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        internal bool IsReadOnly => Postdictionary.IsReadOnly;

        /// <summary>
        /// Gets or sets the <see cref="Vector" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Vector.</returns>
        public Vector this[ulong key] { get => Postdictionary[key]; set => Postdictionary[key] = value; }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int CompareTo(Postings other)
        {
            if (other.Count > Count) return -1;
            if (Count > other.Count) return 1;
            else return 0;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<ulong, Vector>> GetEnumerator()
        {
            return Postdictionary.GetEnumerator();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            if (obj is Postings) return Collections.Equals(((Postings)(obj)).Postdictionary, Postdictionary);
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override int GetHashCode()
        {
            return Collections.GetHashCode(Postdictionary);
        }

        /// <summary>
        /// Determines whether the <see cref="IDictionary" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IDictionary" />.</param>
        /// <returns><see langword="true" /> if the <see cref="IDictionary" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
        internal bool ContainsKey(ulong key)
        {
            return Postdictionary.ContainsKey(key);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="IDictionary" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        internal void Add(ulong key, Vector value)
        {
            Postdictionary.Add(key, value);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="IDictionary" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="IDictionary" />.</returns>
        internal bool Remove(ulong key)
        {
            return Postdictionary.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if the object that implements <see cref="IDictionary" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
        internal bool TryGetValue(ulong key, out Vector value)
        {
            return Postdictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection" />.</param>
        internal void Add(KeyValuePair<ulong, Vector> item)
        {
            Postdictionary.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection" />.
        /// </summary>
        internal void Clear()
        {
            Postdictionary.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ICollection" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="ICollection" />; otherwise, <see langword="false" />.</returns>
        internal bool Contains(KeyValuePair<ulong, Vector> item)
        {
            return Postdictionary.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="ICollection" />. The <see cref="Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        internal void CopyTo(KeyValuePair<ulong, Vector>[] array, int arrayIndex)
        {
            Postdictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ICollection" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref=" ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref=" ICollection`1" />.</returns>
        internal bool Remove(KeyValuePair<ulong, Vector> item)
        {
            return Postdictionary.Remove(item);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance of postings list.
        /// </summary>
        /// <returns>A new object that is a copy of this instance of postings list.</returns>
        public object Clone()
        {
            return new Postings(Postdictionary);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Postdictionary).GetEnumerator();
        }
    }
}
