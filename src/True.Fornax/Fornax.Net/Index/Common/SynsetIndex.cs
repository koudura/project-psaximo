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
using Fornax.Net.Util.Collections;
using ProtoBuf;

namespace Fornax.Net.Index.Common
{
    /// <summary>
    /// Synoynym(s) Index for fornax.net.
    /// </summary>
    /// <seealso cref="ISet{T}" />
    [Serializable,ProtoContract]
    [Progress("SynsetIndex", true, Documented = true, Tested = true)]
    public class SynsetIndex : java.io.Serializable.__Interface , IEnumerable<string>
    {
        [ProtoMember(1)]
        private readonly ISet<string> index = new HashSet<string>();

        /// <summary>
        /// Gets the number of elements contained in the <see cref="SynsetIndex" />.
        /// </summary>
        public int Count => index.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="SynsetIndex" /> is read-only.
        /// </summary>
        internal bool IsReadOnly => index.IsReadOnly;

        /// <summary>
        /// Adds an element to the current set and returns a value to indicate if the element was successfully added.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns>
        ///   <see langword="true" /> if the element is added to the set; <see langword="false" /> if the element is already in the set.
        /// </returns>
        internal bool Add(string item) {
            return index.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="SynsetIndex" />.
        /// </summary>
        internal void Clear() {
            index.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="SynsetIndex" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="SynsetIndex" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> is found in the <see cref="SynsetIndex" />; otherwise, <see langword="false" />.
        /// </returns>
        internal bool Contains(string item) {
            return index.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="SynsetIndex" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="SynsetIndex" />. The <see cref="Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        internal void CopyTo(string[] array, int arrayIndex) {
            index.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj is SynsetIndex) {
                var tm = obj as SynsetIndex;
                return Collections.Equals(index, tm.index);
            }
            return false;
        }

        /// <summary>
        /// Removes all elements in the specified collection from the current set.
        /// </summary>
        /// <param name="other">The collection of items to remove from the set.</param>
        internal void ExceptWith(IEnumerable<string> other) {
            index.ExceptWith(other);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<string> GetEnumerator() {
            return index.GetEnumerator();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return Collections.GetHashCode(index);
        }

        /// <summary>
        /// Modifies the current set so that it contains only elements that are also in a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        internal void IntersectWith(IEnumerable<string> other) {
            index.IntersectWith(other);
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///   <see langword="true" /> if the current set is a proper subset of <paramref name="other" />; otherwise, <see langword="false" />.
        /// </returns>
        internal bool IsProperSubsetOf(IEnumerable<string> other) {
            return index.IsProperSubsetOf(other);
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///   <see langword="true" /> if the current set is a proper superset of <paramref name="other" />; otherwise, <see langword="false" />.
        /// </returns>
        internal bool IsProperSupersetOf(IEnumerable<string> other) {
            return index.IsProperSupersetOf(other);
        }

        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///   <see langword="true" /> if the current set is a subset of <paramref name="other" />; otherwise, <see langword="false" />.
        /// </returns>
        internal bool IsSubsetOf(IEnumerable<string> other) {
            return index.IsSubsetOf(other);
        }

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///   <see langword="true" /> if the current set is a superset of <paramref name="other" />; otherwise, <see langword="false" />.
        /// </returns>
        internal bool IsSupersetOf(IEnumerable<string> other) {
            return index.IsSupersetOf(other);
        }

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///   <see langword="true" /> if the current set and <paramref name="other" /> share at least one common element; otherwise, <see langword="false" />.
        /// </returns>
        internal bool Overlaps(IEnumerable<string> other) {
            return index.Overlaps(other);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="SynsetIndex" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="SynsetIndex" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="SynsetIndex" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        internal bool Remove(string item) {
            return index.Remove(item);
        }

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///   <see langword="true" /> if the current set is equal to <paramref name="other" />; otherwise, false.
        /// </returns>
        internal bool SetEquals(IEnumerable<string> other) {
            return index.SetEquals(other);
        }

        /// <summary>
        /// Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        internal void SymmetricExceptWith(IEnumerable<string> other) {
            index.SymmetricExceptWith(other);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return Collections.ToString(index);
        }

        /// <summary>
        /// Modifies the current set so that it contains all elements that are present in the current set, in the specified collection, or in both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        internal void UnionWith(IEnumerable<string> other) {
            index.UnionWith(other);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return index.GetEnumerator();
        }
    }


}
