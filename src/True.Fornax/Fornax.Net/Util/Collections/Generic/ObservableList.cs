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

namespace Fornax.Net.Util.Collections.Generic
{
    /// <summary>
    /// A List that can be observed for changed and clear events.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [Progress("ObservableList",true,Documented = true,Tested = true)]
    public class ObservableList<T> : IList<T> , java.io.Serializable.__Interface
    {
        private readonly IList<T> monitoredList;

        /// <summary>
        /// Event Occurs on when <see cref="ObservableList{T}"/> is changed or an item in changed.
        /// </summary>
        public event EventHandler<ListChangedEventArgs> ListChanged = delegate { };
        /// <summary>
        /// Event Occurs on when <see cref="ObservableList{T}"/> is cleared.
        /// </summary>
        public event EventHandler<ListChangedEventArgs> ListCleared = delegate { };

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        public ObservableList() {
            this.monitoredList = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        /// <param name="monitoredList">The monitored list.</param>
        /// <exception cref="ArgumentNullException">monitoredList</exception>
        public ObservableList(IList<T> monitoredList) {
            this.monitoredList = monitoredList ?? throw new ArgumentNullException(nameof(monitoredList));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="ArgumentNullException">
        /// collection
        /// or
        /// collection
        /// </exception>
        public ObservableList(IEnumerable<T> collection) {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            this.monitoredList = new List<T>(collection) ?? throw new ArgumentNullException(nameof(collection));
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ObservableList{T}"/>
        /// </summary>
        public int Count => this.monitoredList.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="ObservableList{T}"/> is read-only.
        /// </summary>
        public bool IsReadOnly => this.monitoredList.IsReadOnly;

        /// <summary>
        /// Adds an item to the <see cref="ObservableList{T}"/>
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ObservableList{T}"/>.</param>
        public void Add(T item) {
            this.monitoredList.Add(item);
            OnListChanged(new ListChangedEventArgs(monitoredList.IndexOf(item), item, ListOperation.Add));
        }

        /// <summary>
        /// Removes all items from the <see cref="ObservableList{T}"/>
        /// </summary>
        public void Clear() {
            this.monitoredList.Clear();
            OnListCleared(new ListChangedEventArgs(ListOperation.Clear));
        }

        /// <summary>
        /// Determines whether the <see cref="ObservableList{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ObservableList{T}"/>.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> is found in the <see cref="ObservableList{T}"/>; otherwise, <see langword="false" />.
        /// </returns>
        public bool Contains(T item) {
            return this.monitoredList.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ObservableList{T}"/> to an <see cref="T:Array" />, starting at a particular <see cref="T:Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="ObservableList{T}"/>.
        /// The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex) {
            this.monitoredList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator() {
            return this.monitoredList.GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="ObservableList{T}"/>
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ObservableList{T}"/>.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(T item) {
            return this.monitoredList.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="ObservableList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="ObservableList{T}"/>.</param>
        public void Insert(int index, T item) {
            this.monitoredList.Insert(index, item);
            OnListChanged(new ListChangedEventArgs(index, item, ListOperation.Insert));
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ObservableList{T}"/>
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ObservableList{T}"/>.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="ObservableList{T}"/>; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(T item) {
            lock (this) {
                var index = monitoredList.IndexOf(item);
                if (monitoredList.Remove(item)) {
                    OnListChanged(new ListChangedEventArgs(index, item, ListOperation.Remove));
                    return true;
                }
            }
            return false;
        }
     
        /// <summary>
        /// Removes the <see cref="ObservableList{T}"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index) {

            var removed_item = this.monitoredList[index];
            this.monitoredList.RemoveAt(index);

            OnListChanged(new ListChangedEventArgs(index, removed_item, ListOperation.RemoveAt));
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return this.monitoredList.GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">
        /// index
        /// or
        /// index
        /// </exception>
        public T this[int index] {
            get {
                if (!(index >= 0 && index < monitoredList.Count)) throw new IndexOutOfRangeException(nameof(index));
                return monitoredList[index];
            }
            set {
                if (!(index >= 0 && index < monitoredList.Count)) throw new IndexOutOfRangeException(nameof(index));
                if (monitoredList[index].Equals(value)) return;

                monitoredList[index] = value;
                OnListChanged(new ListChangedEventArgs(index, value, ListOperation.Set));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ListChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ListChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnListChanged(ListChangedEventArgs e) {
            ListChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ListCleared" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ListChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnListCleared(ListChangedEventArgs e) {
            ListCleared(this, e);
        }

        /// <summary>
        /// List Changed Event handler class. <seealso cref="EventArgs"/>.
        /// used by <see cref="ListChanged"/>.
        /// </summary>
        public class ListChangedEventArgs : EventArgs
        {
            private readonly int index;
            private T item;
            private ListOperation operation;

            internal ListChangedEventArgs(int index, T item, ListOperation listOperation) {
                this.index = index;
                this.item = item;
                this.operation = listOperation;
            }

            internal ListChangedEventArgs(ListOperation listOperation) {
                this.index = default(int);
                this.item = default(T);
                this.operation = listOperation;
            }

            /// <summary>
            /// The Index at Which an Operation was carried out.
            /// <para>NOTE: <see cref="ObservableList{T}.ListCleared"/> returns <see cref="Index"/> = 0;</para> 
            /// </summary>
            public virtual int Index => this.index;

            /// <summary>
            /// The Item that was either  Removed, Added , Inserted or Set into the <see cref="ObservableList{T}"/>.
            /// <para>
            /// NOTE: <see cref="ObservableList{T}.ListCleared"/> returns <see cref="Item"/> = default(T).
            /// </para>
            /// </summary>
            public virtual T Item => this.item;

            /// <summary>
            /// Type of operation carried out.
            /// </summary>
            public virtual ListOperation ChangeType => this.operation;
        }
    }
}
