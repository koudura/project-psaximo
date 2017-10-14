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
    /// Represents a strongly typed list of objects that can be accessed by index.
    /// Provides methods to manipulate lists. Also provides functionality
    /// to compare lists against each other through an implementations of
    /// <see cref="IEquatable{T}"/>, or to wrap an existing list to use
    /// the same comparison logic as this one while not affecting any of its
    /// other functionality.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <seealso cref="IList{T}" />
    /// <seealso cref="IEquatable{T}" />
    [Serializable]
    [Progress("EquatableList<T>", true, Documented = true, Tested = true)]
    public class EquatableList<T> : IList<T>, IEquatable<IList<T>>, ICloneable
    {
        private readonly IList<T> list;

        #region ctor        
        /// <summary>Initializes a new instance of the
        /// <see cref="EquatableList{T}"/> class that is empty and has the
        /// default initial capacity.</summary>
        public EquatableList() {
            this.list = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EquatableList{T}"/>.
        /// <para/>
        /// If the <paramref name="wrap"/> parameter is <c>true</c>, the
        /// <paramref name="collection"/> is used as is without doing
        /// a copy operation. Otherwise, the collection is copied 
        /// (which is the same operation as the 
        /// <see cref="EquatableList{T}.EquatableList(IEnumerable{T})"/> overload). 
        /// <para/>
        /// The internal <paramref name="collection"/> is used for
        /// all operations except for <see cref="Equals(object)"/>, <see cref="GetHashCode()"/>,
        /// and <see cref="ToString()"/>, which are all based on deep analysis
        /// of this collection and any nested collections.
        /// </summary>
        /// <param name="collection">The collection that will either be wrapped or copied 
        /// depending on the value of <paramref name="wrap"/>.</param>
        /// <param name="wrap"><c>true</c> to wrap an existing <see cref="IList{T}"/> without copying,
        /// or <c>false</c> to copy the collection into a new <see cref="List{T}"/>.</param>
        public EquatableList(IList<T> collection, bool wrap) {
            Contract.Requires(collection != null);
            if (collection == null)
                throw new ArgumentNullException(nameof(collection) + " is null");

            if (wrap) this.list = collection;
            else this.list = new List<T>(collection);
        }

        /// <summary>
        /// Initializes a new 
        /// instance of the <see cref="EquatableList{T}"/>
        /// class that contains elements copied from the specified collection and has
        /// sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public EquatableList(IEnumerable<T> collection) {
            Contract.Requires(collection != null);
            if (collection == null)
                throw new ArgumentNullException(nameof(collection) + " is null");

            this.list = new List<T>(collection);
        }

        /// <summary>Initializes a new instance of the <see cref="EquatableList{T}"/>
        /// class that is empty and has the specified initial capacity.</summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public EquatableList(int capacity) {
            list = new List<T>(capacity);
        }
        #endregion


        #region IList
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        public virtual T this[int index] { get => this.list[index]; set => this.list[index] = value; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="EquatableList{T}"/>.
        /// </summary>
        public virtual int Count => this.list.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="EquatableList{T}"/> is read-only.
        /// </summary>
        public virtual bool IsReadOnly => this.list.IsReadOnly;

        /// <summary>
        /// Adds an item to the <see cref="EquatableList{T}" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="EquatableList{T} />.</param>
        public virtual void Add(T item) {
            this.list.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="EquatableList{T}"/>.
        /// </summary>
        public virtual void Clear() {
            this.list.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="EquatableList{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="EquatableList{T}"/>.</param>
        /// <returns><c>true</c> if the Object is found in the <see cref="EquatableList{T}"/>; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(T item) {
            return this.list.Contains(item);
        }

        /// <summary>
        /// Copies the entire <see cref="EquatableList{T}"/> to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the 
        /// destination of the elements copied from <see cref="EquatableList{T}"/>. 
        /// The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public virtual void CopyTo(T[] array, int arrayIndex) {
            this.list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="EquatableList{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="EquatableList{T}"/>.</returns>
        public virtual IEnumerator<T> GetEnumerator() => this.list.GetEnumerator();

        /// <summary>
        /// Determines the index of a specific <paramref name="item"/> in the <see cref="EquatableList{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="EquatableList{T}"/>.</param>
        /// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        public virtual int IndexOf(T item) => this.list.IndexOf(item);

        /// <summary>
        /// Inserts an item to the <see cref="EquatableList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="EquatableList{T}"/>.</param>
        public virtual void Insert(int index, T item) => this.list.Insert(index, item);

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="EquatableList{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="EquatableList{T}"/>.</param>
        /// <returns><c>true</c> if <paramref name="item"/> was successfully removed from the 
        /// <see cref="EquatableList{T}"/>; otherwise, <c>false</c>. This method also returns 
        /// <c>false</c> if item is not found in the original <see cref="EquatableList{T}"/>.</returns>
        public virtual bool Remove(T item) => this.list.Remove(item);

        /// <summary>
        /// Removes the <see cref="EquatableList{T}"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public virtual void RemoveAt(int index) => this.list.RemoveAt(index);

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="EquatableList{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> for the <see cref="EquatableList{T}"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator() => this.list.GetEnumerator();

        #endregion

        #region Eqautables overrides  & Clone

        /// <summary>
        /// Compares this sequence to another <see cref="IList{T}"/>
        /// implementation, returning <c>true</c> if they are equal, <c>false</c> otherwise.
        /// <para/>
        /// The comparison takes into consideration any values in this collection and values
        /// of any nested collections, but does not take into consideration the data type.
        /// Therefore, <see cref="EquatableList{T}"/> can equal any <see cref="IList{T}"/>
        /// with the exact same values in the same order.
        /// </summary>
        /// <param name="other">The other <see cref="IList{T}"/> implementation
        /// to compare against.</param>
        /// <returns><c>true</c> if the sequence in <paramref name="other"/>
        /// is the same as this one.</returns>
        public virtual bool Equals(IList<T> other) => Collections.Equals(this, other);

        public virtual object Clone() {
            return new EquatableList<T>(this);
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
        /// Therefore, <see cref="EquatableList{T}"/> can equal any <see cref="IList{T}"/>
        /// with the exact same values in the same order.
        /// </summary>
        /// <param name="other">The other object
        /// to compare against.</param>
        /// <returns><c>true</c> if the sequence in <paramref name="other"/>
        /// is the same as this one.</returns>
        public override bool Equals(object obj) {
            if (!(obj is IList<T>)) {
                return false;
            }
            return Equals(obj as IList<T>);
        }

        /// <summary>
        /// Returns the hash code value for this list.
        /// <para/>
        /// The hash code determination takes into consideration any values in
        /// this collection and values of any nested collections, but does not
        /// take into consideration the data type. Therefore, the hash codes will
        /// be exactly the same for this <see cref="EquatableList{T}"/> and another
        /// <see cref="IList{T}"/> (including arrays) with the same values in the
        /// same order.
        /// </summary>
        /// <returns>the hash code value for this list</returns>
        public override int GetHashCode() {
            return Collections.GetHashCode(this);
        }
        #endregion

        #region operators
        /// <summary>Overload of the == operator, it compares a
        /// <see cref="EquatableList{T}"/> to an <see cref="IEnumerable{T}"/>
        /// implementation.</summary>
        /// <param name="x">The <see cref="EquatableList{T}"/> to compare
        /// against <paramref name="y"/>.</param>
        /// <param name="y">The <see cref="IEnumerable{T}"/> to compare
        /// against <paramref name="x"/>.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator == (EquatableList<T> x, IEnumerable<T> y) => Equals(x, y);

        /// <summary>Overload of the != operator, it compares a
        /// <see cref="EquatableList{T}"/> to an <see cref="IEnumerable{T}"/>
        /// implementation.</summary>
        /// <param name="y">The <see cref="EquatableList{T}"/> to compare
        /// against <paramref name="x"/>.</param>
        /// <param name="x">The <see cref="IEnumerable{T}"/> to compare
        /// against <paramref name="y"/>.</param>
        /// <returns>True if the instances are not equal, false otherwise.</returns>
        public static bool operator != (EquatableList<T> x, IEnumerable<T> y) =>   !(x == y);
        #endregion
    }
}