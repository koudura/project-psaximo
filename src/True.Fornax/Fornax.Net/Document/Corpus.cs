// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-29-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-03-2017
// ***********************************************************************
// <copyright file="Corpus.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Document namespace.
/// </summary>
namespace Fornax.Net.Document
{
    /// <summary>
    /// Class Corpus.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{KeyValuePair{ulong, string}}" />
    /// <seealso cref="System.Collections.Generic.IDictionary{System.UInt64, System.String}" />
    /// <seealso cref="java.io.Serializable.__Interface" />
    [Serializable]
    public class Corpus : IEnumerable<KeyValuePair<ulong, string>>, java.io.Serializable.__Interface
    {

        private readonly IDictionary<ulong, string> corpus = new SortedList<ulong, string>();

        /// <summary>
        /// Gets or sets the <see cref="string" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public string this[ulong key] { get => corpus[key]; set => corpus[key] = value; }


        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        internal ICollection<ulong> Keys => corpus.Keys;

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public ICollection<string> Values => corpus.Values;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => corpus.Count;

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        internal bool IsReadOnly => corpus.IsReadOnly;

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        internal void Add(ulong key, string value)
        {
            corpus.Add(key, value);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void Add(KeyValuePair<ulong, string> item)
        {
            corpus.Add(item);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        internal void Clear()
        {
            corpus.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        internal bool Contains(KeyValuePair<ulong, string> item)
        {
            return corpus.Contains(item);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        internal bool ContainsKey(ulong key)
        {
            return corpus.ContainsKey(key);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        internal void CopyTo(KeyValuePair<ulong, string>[] array, int arrayIndex)
        {
            corpus.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<ulong, string>> GetEnumerator()
        {
            return corpus.GetEnumerator();
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool Remove(ulong key)
        {
            return corpus.Remove(key);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool Remove(KeyValuePair<ulong, string> item)
        {
            return corpus.Remove(item);
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool TryGetValue(ulong key, out string value)
        {
            return corpus.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return corpus.GetEnumerator();
        }
    }
}
