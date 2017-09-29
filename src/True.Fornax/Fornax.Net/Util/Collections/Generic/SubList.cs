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
    /// A Sub-List that is contained in a <see cref="IList{T}"/> .
    /// The Container of the Sublist is used to create an Instance of a Sublist.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IList{T}" />
    [Serializable]
    [Progress("SubList<T>",true ,Documented = true, Tested = true)]

    public sealed class SubList<T> : IList<T>, ISubList<T>
    {
        private readonly IList<T> list;
        private readonly int fromIndex;
        private int toIndex;

        /// <summary>
        /// Creates a ranged view of the given <paramref name="list"/>.
        /// </summary>
        /// <param name="list">The original list to view.</param>
        /// <param name="fromIndex">The inclusive starting index.</param>
        /// <param name="toIndex">The exclusive ending index.</param>
        public SubList(IList<T> list, int fromIndex, int toIndex) {
            Contract.Requires(list != null);
            if (list == null)
                throw new ArgumentNullException(nameof(list) + " is null");

            if (fromIndex < 0)
                throw new ArgumentOutOfRangeException("fromIndex");

            if (toIndex > list.Count)
                throw new ArgumentOutOfRangeException("toIndex");

            if (toIndex < fromIndex)
                throw new ArgumentOutOfRangeException("toIndex");

            if (list == null || list.Count == 0)
                throw new ArgumentNullException("list");

            this.list = list;
            this.fromIndex = fromIndex;
            this.toIndex = toIndex;
        }
        /// <summary>
        /// Determines the index of a specific item in the <see cref="SubList{T}" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="SubList{T}" />.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(T item) {
            for (int i = this.fromIndex, fakeIndex = 0; i < this.toIndex; i++, fakeIndex++) {
                var current = this.list[i];

                if (current == null && item == null)
                    return fakeIndex;

                if (current.Equals(item)) {
                    return fakeIndex;
                }
            }

            return -1;
        }

        /// <summary>
        /// Inserts an item to the <see cref="SubList{T}`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref = "SubList{T}`1" />.</param>
        /// <exception cref="NotSupportedException"></exception>
        public void Insert(int index, T item) {
            this.list.Insert(this.fromIndex + index, item);
        }

        /// <summary>
        /// Removes the <see cref="SubList{T}`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index) {
            this.list.RemoveAt(this.fromIndex + index);
            this.toIndex--;
        }

        /// <summary>
        /// Gets or sets the item <see cref="T"/>  at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns> an item of <see cref="T"/> at <paramref name="index"/></returns>
        /// <exception cref="IndexOutOfRangeException">
        /// </exception>
        public T this[int index] {
            get {
                if ((index >= 0) && (index < this.list.Count))
                    return this.list[this.fromIndex + index];
                else throw new IndexOutOfRangeException();
            }
            set {
                if ((index >= 0) && (index < this.list.Count))
                    this.list[this.fromIndex + index] = value;
                else throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="SubList{T}`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="SubList{T}`1" />.</param>
        public void Add(T item) {
            this.list.Insert(this.toIndex - 1, item);
        }

        /// <summary>
        /// Removes all items from the <see cref="SubList{T}`1" />.
        /// </summary>
        public void Clear() {
            for (int i = this.toIndex - 1; i >= this.fromIndex; i--) {
                this.list.RemoveAt(i);
            }
            this.toIndex = this.fromIndex;
        }

        /// <summary>
        /// Determines whether the <see cref="SubList{T}`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="SubList{T}`1" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> is found in the <see cref="SubList{T}`1" />; otherwise, <see langword="false" />.
        /// </returns>
        public bool Contains(T item) {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Copies the elements of the <see cref="SubList{T}`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="SubList{T}`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex) {
            int count = array.Length - arrayIndex;

            for (int i = this.fromIndex, arrayi = arrayIndex; i <= Math.Min(this.toIndex - 1, this.fromIndex + count - 1); i++, arrayi++) {
                array[arrayi] = this.list[i];
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="SubList{T}" />.
        /// </summary>
        public int Count => Math.Max(this.toIndex - this.fromIndex, 0);

        /// <summary>
        /// Gets a value indicating whether the <see cref="SubList{T}" /> is read-only.
        /// </summary>
        public bool IsReadOnly => this.list.IsReadOnly;

        /// <summary>
        /// Gets the container, which is the Current state of the <see cref="IList{T}"/> which holds the sublist.
        /// </summary>
        /// <value>
        /// The container list.
        /// </value>
        public IList<T> Container => this.list;

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="SubList{T}`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="SubList{T}`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(T item) {
            var index = IndexOf(item);

            if (index < 0)
                return false;

            this.list.RemoveAt(this.fromIndex + index);
            this.toIndex--;

            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="SubList{T}"/> collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator() {
            return YieldItems().GetEnumerator();
        }


        private IEnumerable<T> YieldItems() {
            for (int i = this.fromIndex; i <= Math.Min(this.toIndex - 1, this.list.Count - 1); i++) {
                yield return this.list[i];
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            var list = obj as SubList<T>;
            return list != null &&
                   EqualityComparer<IList<T>>.Default.Equals(this.list, list.list) &&
                   Count == list.Count &&
                   EqualityComparer<IList<T>>.Default.Equals(Container, list.Container);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="list1">The list1.</param>
        /// <param name="list2">The list2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(SubList<T> list1, SubList<T> list2) {
            return EqualityComparer<SubList<T>>.Default.Equals(list1, list2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="list1">The list1.</param>
        /// <param name="list2">The list2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(SubList<T> list1, SubList<T> list2) {
            return !(list1 == list2);
        }

        /// <summary>
        /// Returns a hash code for this instance of sublist.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return this.list.GetHashCode();
        }


        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="SubList{T}"/> collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return this.list.GetEnumerator();
        }
    }
}
