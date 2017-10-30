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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Fornax.Net.Analysis.Tools;
using Fornax.Net.Util.Collections;
using ProtoBuf;

namespace Fornax.Net.Index.Common
{
    /// <summary>
    /// Representation of the SoundexIndex for indexing soundex of words,
    /// Otherwise called Phonetic similarity index.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IDictionary{Fornax.Net.Analysis.Tools.Soundex, System.Collections.Generic.IList{System.String}}" />
    /// <seealso cref="System.IDisposable" />
    /// <remarks>This idea owes its
    /// origins to work in international police departments from the early 20th century, 
    /// seeking to match names for wanted criminals despite the names being
    /// spelled differentlyin differentcountries.It is mainly used to correct phonetic
    /// misspellings in proper nouns.</remarks>
    [Serializable, ProtoContract]
    [Progress("SoundexIndex", true, Documented = true, Tested = true)]
    public class SoundexIndex : IDictionary<Soundex, IList<string>>, IDisposable, IEnumerable<KeyValuePair<Soundex, IList<string>>>, java.io.Serializable.__Interface
    {
        [ProtoMember(1)]
        internal readonly IDictionary<Soundex, IList<string>> s_index;

        /// <summary>
        /// Gets an <see cref="ICollection{T}" /> containing the keys of the <see cref="SoundexIndex" />.
        /// </summary>
        public ICollection<Soundex> Keys => s_index.Keys;

        /// <summary>
        /// Gets an <see cref="ICollection{T}" /> containing the values in the <see cref="SoundexIndex" />.
        /// </summary>
        public ICollection<IList<string>> Values => s_index.Values;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="SoundexIndex" />.
        /// </summary>
        public int Count => s_index.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="SoundexIndex" /> is read-only.
        /// </summary>
        public bool IsReadOnly => s_index.IsReadOnly;

        /// <summary>
        /// Gets or sets the <see cref="IList{System.String}"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IList{System.String}"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IList<string> this[Soundex key] { get => s_index[key]; set => s_index[key] = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundexIndex"/> class.
        /// with initiator soundex and list of matching words.
        /// </summary>
        /// <param name="soundex">The soundex.</param>
        /// <param name="words">The words.</param>
        /// <exception cref="ArgumentNullException">soundex</exception>
        public SoundexIndex(Soundex soundex, List<string> words) {
            Contract.Requires(soundex != null && words != null);
            if (soundex == null || words == null) throw new ArgumentNullException($"{nameof(soundex)} or {nameof(words)} is null");

            s_index = new SortedList<Soundex, IList<string>> {
                { soundex, words }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundexIndex"/> class.
        /// with Soundex initiator.
        /// </summary>
        /// <param name="soundex">The soundex.</param>
        public SoundexIndex(Soundex soundex) : this(soundex, new List<string>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundexIndex"/> class.
        /// with/as an empty index.
        /// </summary>
        public SoundexIndex() {
            s_index = new SortedList<Soundex, IList<string>>();
        }

        /// <summary>
        /// Determines whether the <see cref="SoundexIndex" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="SoundexIndex" />.</param>
        /// <returns>
        ///   <see langword="true" /> if the <see cref="SoundexIndex" /> contains an element with the key; otherwise, <see langword="false" />.
        /// </returns>
        public bool ContainsKey(Soundex key) {
            return s_index.ContainsKey(key);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="SoundexIndex" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(Soundex key, IList<string> value) {
            s_index.Add(key, value);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="SoundexIndex" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        ///   <see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public bool Remove(Soundex key) {
            return s_index.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        ///   <see langword="true" /> if the object that implements <see cref="IDictionary{TKey, TValue}" /> contains an element with the specified key; otherwise, <see langword="false" />.
        /// </returns>
        public bool TryGetValue(Soundex key, out IList<string> value) {
            return s_index.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds an item to the <see cref="SoundexIndex" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="SoundexIndex" />.</param>
        public void Add(KeyValuePair<Soundex, IList<string>> item) {
            s_index.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="SoundexIndex" />.
        /// </summary>
        public void Clear() {
            s_index.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="SoundexIndex" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ICollection{T}" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> is found in the <see cref="ICollection{T}" />; otherwise, <see langword="false" />.
        /// </returns>
        public bool Contains(KeyValuePair<Soundex, IList<string>> item) {
            return s_index.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="ICollection{T}" />. The <see cref="Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<Soundex, IList<string>>[] array, int arrayIndex) => s_index.CopyTo(array, arrayIndex);

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="ICollection{T}" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(KeyValuePair<Soundex, IList<string>> item) {
            return s_index.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<Soundex, IList<string>>> GetEnumerator() {
            return s_index.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return s_index.GetEnumerator();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            ((IDisposable)s_index).Dispose();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return Collections.ToString(s_index);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this <see cref="SoundexIndex"/>
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this <see cref="SoundexIndex"/> instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj == this) return true;

            if (obj is SoundexIndex) {
                var snd = obj as SoundexIndex;
                return Collections.Equals(snd.s_index, s_index);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return Collections.GetHashCode(s_index);
        }
    }
}
