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

namespace Fornax.Net.Util.Collections.Generic
{
    /// <summary>
    /// Represents a strongly typed set of objects.
    /// Provides methods to manipulate the set. Also provides functionality
    /// to compare sets against each other through an implementations of
    /// <see cref="IEquatable{T}"/>, or to wrap an existing set to use
    /// the same comparison logic as this one while not affecting any of its
    /// other functionality.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    [Serializable]
    public class EquatableSet<T> : ISet<T>, IEquatable<ISet<T>>, ICloneable
    {
        private ISet<T> set;

        /// <summary>Initializes a new instance of the
        /// <see cref="EquatableSet{T}"/> class that is empty and has the
        /// default initial capacity.</summary>
        public EquatableSet() {
            this.set = new HashSet<T>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EquatableSet{T}"/>.
        /// <para/>
        /// If the <paramref name="wrap"/> parameter is <c>true</c>, the
        /// <paramref name="collection"/> is used as is without doing
        /// a copy operation. Otherwise, the collection is copied 
        /// </summary>
        /// <param name="collection">The collection that will either be wrapped or copied 
        /// depending on the value of <paramref name="wrap"/>.</param>
        /// <param name="wrap"><c>true</c> to wrap an existing <see cref="ISet{T}"/> without copying,
        /// or <c>false</c> to copy the collection into a new <see cref="HashSet{T}"/>.</param>
        /// <exception cref="ArgumentNullException">collection</exception>
        public EquatableSet(ISet<T> collection, bool wrap) {
            Contract.Requires(collection != null);
            if (collection == null) {
                throw new ArgumentNullException(nameof(collection));
            }
            if (wrap) this.set = collection;
            else this.set = new HashSet<T>(collection);
        }

        /// <summary>
        /// Initializes a new 
        /// instance of the <see cref="EquatableSet{T}"/>
        /// class that contains elements copied from the specified collection and has
        /// sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new set.</param>
        /// <exception cref="ArgumentNullException">collection</exception>
        public EquatableSet(ICollection<T> collection) {
            Contract.Requires(collection != null);
            if (collection == null) {
                throw new ArgumentNullException(nameof(collection));
            }
            this.set = new HashSet<T>(collection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquatableSet{T}"/> class that is 
        /// empty and uses the specified equality comparer for the set type.
        /// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation 
        /// to use when comparing values in the set, or null to use the default 
        /// <see cref="EqualityComparer{T}"/> implementation for the set type.</param>
        /// <exception cref="ArgumentNullException">collection</exception>
        public EquatableSet(IEqualityComparer<T> comparer) {
            Contract.Requires(comparer != null);
            if (comparer == null) {
                throw new ArgumentNullException(nameof(comparer));
            }

            this.set = new HashSet<T>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquatableSet{T}"/> class that uses the 
        /// specified equality comparer for the set type, contains elements 
        /// copied from the specified collection, and has sufficient capacity 
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new set.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use 
        /// when comparing values in the set, or <c>null</c> to use the default <see cref="EqualityComparer{T}"/>
        /// implementation for the set type.</param>
        /// <exception cref="ArgumentNullException">collection</exception>
        public EquatableSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) {
            Contract.Requires(collection != null && comparer != null);
            if (collection == null) {
                throw new ArgumentNullException(nameof(collection));
            }

            if (comparer == null) {
                throw new ArgumentNullException(nameof(comparer));
            }

            this.set = new HashSet<T>(collection, comparer);
        }

        #region ISet
        /// <summary>
        /// Gets the number of elements contained in the <see cref="EquatableSet{T}"/>.
        /// </summary>
        public virtual int Count => this.set.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="EquatableSet{T}"/> is read-only.
        /// </summary>
        public virtual bool IsReadOnly => this.set.IsReadOnly;

        /// <summary>
        /// Adds an element to the current set and returns a value to indicate if the element was successfully added.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns><c>true</c> if the element is added to the set; <c>false</c> if the element is already in the set.</returns>
        public virtual bool Add(T item) {
            return this.set.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="EquatableSet{T}"/>.
        /// </summary>
        public virtual void Clear() {
            this.set.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="EquatableSet{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="EquatableSet{T}"/>.</param>
        /// <returns><c>true</c> if item is found in the <see cref="EquatableSet{T}"/>; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(T item) {
            return this.set.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="EquatableSet{T}"/> to an <see cref="Array"/>, 
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements 
        /// copied from <see cref="EquatableSet{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public virtual void CopyTo(T[] array, int arrayIndex) {
            this.set.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes all elements in the specified collection from the current set.
        /// </summary>
        /// <param name="other">The collection of items to remove from the set.</param>
        public virtual void ExceptWith(IEnumerable<T> other) {
            this.set.ExceptWith(other);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() {
            return this.set.GetEnumerator();
        }

        /// <summary>
        /// Modifies the current set so that it contains only elements that are also in a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        public virtual void IntersectWith(IEnumerable<T> other) {
            this.set.IntersectWith(other);
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a proper subset of other; otherwise, <c>false</c>.</returns>
        public virtual bool IsProperSubsetOf(IEnumerable<T> other) {
            return this.set.IsProperSubsetOf(other);
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a proper superset of other; otherwise, <c>false</c>.</returns>
        public virtual bool IsProperSupersetOf(IEnumerable<T> other) {
            return this.set.IsProperSupersetOf(other);
        }

        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a subset of other; otherwise, <c>false</c>.</returns>
        public virtual bool IsSubsetOf(IEnumerable<T> other) {
            return this.set.IsSubsetOf(other);
        }

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a superset of other; otherwise, <c>false</c>.</returns>
        public virtual bool IsSupersetOf(IEnumerable<T> other) {
            return this.set.IsSupersetOf(other);
        }

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set and other share at least one common element; otherwise, <c>false</c>.</returns>
        public virtual bool Overlaps(IEnumerable<T> other) {
            return this.set.Overlaps(other);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="EquatableSet{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="EquatableSet{T}"/>.</param>
        /// <returns><c>true</c> if item was successfully removed from the <see cref="EquatableSet{T}"/>; otherwise, <c>false</c>. 
        /// This method also returns <c>false</c> if item is not found in the original <see cref="EquatableSet{T}"/>.</returns>
        public virtual bool Remove(T item) {
            return this.set.Remove(item);
        }

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is equal to other; otherwise, <c>false</c>.</returns>
        public virtual bool SetEquals(IEnumerable<T> other) {
            return this.set.SetEquals(other);
        }

        /// <summary>
        /// Modifies the current set so that it contains only elements that are present either in the 
        /// current set or in the specified collection, but not both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        public virtual void SymmetricExceptWith(IEnumerable<T> other) {
            this.set.SymmetricExceptWith(other);
        }

        /// <summary>
        /// Modifies the current set so that it contains all elements that are present in the 
        /// current set, in the specified collection, or in both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        public virtual void UnionWith(IEnumerable<T> other) {
            this.set.UnionWith(other);
        }

        void ICollection<T>.Add(T item) {
            ((ICollection<T>)this.set).Add(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="EquatableSet{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> for the <see cref="EquatableSet{T}"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)this.set).GetEnumerator();
        }

        #endregion


        #region Eqautables overrides  & Clone        

        /// <summary>
        /// Compares this sequence to <paramref name="other"/>, returning <c>true</c> if they 
        /// are equal, <c>false</c> otherwise.
        /// <para/>
        /// The comparison takes into consideration any values in this collection and values
        /// of any nested collections, but does not take into consideration the data type.
        /// Therefore, <see cref="EquatableSet{T}"/> can equal any <see cref="ISet{T}"/>
        /// with the exact same values (in any order).
        /// </summary>
        /// <param name="other">The other object
        /// to compare against.</param>
        /// <returns><c>true</c> if the sequence in <paramref name="other"/>
        /// <exception cref="ArgumentNullException">other</exception>
        public virtual  bool Equals(ISet<T> other) {
            if (other == null) {
                throw new ArgumentNullException(nameof(other));
            }

            return Collections.Equals(this, other);
        }

        /// <summary>Clones the <see cref="EquatableSet{T}"/>.</summary>
        /// <remarks>This is a shallow clone.</remarks>
        /// <returns>A new shallow clone of this
        /// <see cref="EquatableSet{T}"/>.</returns>
        public virtual object Clone() {
            return new EquatableSet<T>(this);
        }

        /// <summary>
        /// Returns a string representation of this collection (and any nested collections). 
        /// The string representation consists of a list of the collection's elements in 
        /// the order they are returned by its enumerator, enclosed in square brackets 
        /// ("[]"). Adjacent elements are separated by the characters ", " (comma and space).
        /// </summary>
        /// <returns>a string representation of this collection</returns>
        public override string ToString() {
            return Collections.ToString(this);
        }

        /// <summary>
        /// If the object passed implements <see cref="IList{T}"/>,
        /// compares this sequence to <paramref name="other"/>, returning <c>true</c> if they 
        /// are equal, <c>false</c> otherwise.
        /// <para/>
        /// The comparison takes into consideration any values in this collection and values
        /// of any nested collections, but does not take into consideration the data type.
        /// Therefore, <see cref="EquatableSet{T}"/> can equal any <see cref="ISet{T}"/>
        /// with the exact same values (in any order).
        /// </summary>
        /// <param name="other">The other object
        /// to compare against.</param>
        /// <returns><c>true</c> if the sequence in <paramref name="other"/>
        /// is the same as this one.</returns>
        /// <exception cref="ArgumentNullException">collection</exception>
        public override bool Equals(object obj) {
            if (obj == null) {
                throw new ArgumentNullException(nameof(obj));
            }

            if (!(obj is ISet<T>)) {
                return false;
            }
            return Equals(obj as ISet<T>);
        }

        /// <summary>
        /// Returns the hash code value for this list.
        /// <para/>
        /// The hash code determination takes into consideration any values in
        /// this collection and values of any nested collections, but does not
        /// take into consideration the data type. Therefore, the hash codes will
        /// be exactly the same for this <see cref="EquatableSet{T}"/> and another
        /// <see cref="ISet{T}"/> with the same values (in any order).
        /// </summary>
        /// <returns>the hash code value for this list</returns>
        public override int GetHashCode() {
            return Collections.GetHashCode(this);
        }
        #endregion


        #region operators
        /// <summary>Overload of the == operator, it compares a
        /// <see cref="EquatableSet{T}"/> to an <see cref="IEnumerable{T}"/>
        /// implementation.</summary>
        /// <param name="x">The <see cref="EquatableSet{T}"/> to compare
        /// against <paramref name="y"/>.</param>
        /// <param name="y">The <see cref="IEnumerable{T}"/> to compare
        /// against <paramref name="x"/>.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(EquatableSet<T> x, IEnumerable<T> y) {
            return Equals(x, y);
        }

        /// <summary>Overload of the != operator, it compares a
        /// <see cref="EquatableSet{T}"/> to an <see cref="IEnumerable{T}"/>
        /// implementation.</summary>
        /// <param name="x">The <see cref="EquatableSet{T}"/> to compare
        /// against <paramref name="y"/>.</param>
        /// <param name="y">The <see cref="IEnumerable{T}"/> to compare
        /// against <paramref name="x"/>.</param>
        /// <returns>True if the instances are not equal, false otherwise.</returns>
        public static bool operator != (EquatableSet<T> x, IEnumerable<T> y) {
            return !(x == y);
        }
        #endregion
    }
}