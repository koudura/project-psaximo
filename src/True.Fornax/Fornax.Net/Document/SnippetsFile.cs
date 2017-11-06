// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 11-05-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-05-2017
// ***********************************************************************
// <copyright file="SnippetsFile.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Util.Collections;

/// <summary>
/// The Document namespace.
/// </summary>
namespace Fornax.Net.Document
{
    /// <summary>
    /// Class SnippetsFile.
    /// </summary>
    /// <seealso cref="System.ICloneable" />
    /// <seealso cref="System.Collections.IEnumerable" />
    /// 
    [Serializable]
    public class SnippetsFile : ICloneable , IEnumerable , java.io.Serializable.__Interface
    {
        IDictionary<ulong, Snippet> snippet_index;

        public SnippetsFile()
        {
            snippet_index = new SortedList<ulong, Snippet>();
        }

        private SnippetsFile(IDictionary<ulong, Snippet> snippet_index)
        {
            this.snippet_index = snippet_index;
        }

        /// <summary>
        /// Gets or sets the <see cref="Snippet" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Snippet.</returns>
        public Snippet this[ulong key] { get => snippet_index[key]; set => snippet_index[key] = value; }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        internal ICollection<ulong> Keys => snippet_index.Keys;

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        internal ICollection<Snippet> Values => snippet_index.Values;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => snippet_index.Count;

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        internal bool IsReadOnly => snippet_index.IsReadOnly;

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        internal void Add(ulong key, Snippet value)
        {
            snippet_index.Add(key, value);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void Add(KeyValuePair<ulong, Snippet> item)
        {
            snippet_index.Add(item);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        internal void Clear()
        {
            snippet_index.Clear();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return new SnippetsFile(snippet_index);
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        internal bool Contains(KeyValuePair<ulong, Snippet> item)
        {
            return snippet_index.Contains(item);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        internal bool ContainsKey(ulong key)
        {
            return snippet_index.ContainsKey(key);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        internal void CopyTo(KeyValuePair<ulong, Snippet>[] array, int arrayIndex)
        {
            snippet_index.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>IEnumerator&lt;KeyValuePair&lt;System.UInt64, Snippet&gt;&gt;.</returns>
        public IEnumerator<KeyValuePair<ulong, Snippet>> GetEnumerator()
        {
            return snippet_index.GetEnumerator();
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool Remove(ulong key)
        {
            return snippet_index.Remove(key);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool Remove(KeyValuePair<ulong, Snippet> item)
        {
            return snippet_index.Remove(item);
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool TryGetValue(ulong key, out Snippet value)
        {
            return snippet_index.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return snippet_index.GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Collections.ToString(snippet_index);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (Collections.Equals(snippet_index, ((SnippetsFile)(obj)).snippet_index)) return true;
            return obj == this;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Collections.GetHashCode(snippet_index);
        }
    }
}
