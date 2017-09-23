/** MIT LICENSE
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
***/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Fornax.Net.Util.Collections.Generics
{
    /// <summary>
    /// A List that can be observed for changed and clear events.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ObservableList<T> : IList<T>
    {
        private readonly IList<T> monitoredList;

        public event EventHandler<ListChangedEventArgs> ListChanged = delegate { };
        public event EventHandler<ListChangedEventArgs> ListCleared = delegate { };

        
        public ObservableList() {
            monitoredList = new List<T>();
        }

        public ObservableList(IList<T> monitoredList) {
            this.monitoredList = monitoredList ?? throw new ArgumentNullException(nameof(monitoredList));
        }

        public ObservableList(IEnumerable<T> collection) {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            this.monitoredList = new List<T>(collection) ?? throw new ArgumentNullException(nameof(collection));
        }

        public int Count => this.monitoredList.Count;

        public bool IsReadOnly => this.monitoredList.IsReadOnly;


        public void Add(T item) {
            this.monitoredList.Add(item);
            OnListChanged(new ListChangedEventArgs(monitoredList.IndexOf(item), item, ListOperation.Add));
        }

        public void Clear() {
            this.monitoredList.Clear();
            OnListCleared(new ListChangedEventArgs(ListOperation.Clear));
        }

        public bool Contains(T item) {
            return this.monitoredList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            this.monitoredList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            return this.monitoredList.GetEnumerator();
        }

        public int IndexOf(T item) {
            return this.monitoredList.IndexOf(item);
        }

        public void Insert(int index, T item) {
            this.monitoredList.Insert(index, item);
            OnListChanged(new ListChangedEventArgs(index, item, ListOperation.Insert));
        }

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

        public void RemoveAt(int index) {

            var removed_item = this.monitoredList[index];
            this.monitoredList.RemoveAt(index);

            OnListChanged(new ListChangedEventArgs(index, removed_item, ListOperation.RemoveAt));
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.monitoredList.GetEnumerator();
        }

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


        protected virtual void OnListChanged(ListChangedEventArgs e) {
            ListChanged(this, e);
        }

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
            /// NOTE: <see cref="ObservableList{T}.ListCleared"/> returns <see cref="Item"/> = <seealso cref="default(T)"/>.
            /// </para>
            /// </summary>
            public virtual T Item => this.item;

            /// <summary>
            /// Type of operation carried out.
            /// </summary>
            public virtual ListOperation ChangeType => this.operation;
        }
    }
    /// <summary>
    /// Signified the type of change operation carried out the observable list,
    /// <seealso cref="ObservableList{T}"/>.
    /// </summary>
    public enum ListOperation
    {
        /// <summary>
        /// An item Was Added to the <see cref="ObservableList{T}"/>.
        /// </summary>
        Add,
        /// <summary>
        /// An item was removed at a specified index from <see cref="ObservableList{T}"/>.
        /// </summary>
        RemoveAt,
        /// <summary>
        /// The first occurence of an item was removed from <see cref="ObservableList{T}"/>.
        /// </summary>
        Remove,
        /// <summary>
        /// An item was Inserted into <see cref="ObservableList{T}"/>.
        /// </summary>
        Insert,
        /// <summary>
        /// The <see cref="ObservableList{T}"/> was cleared.
        /// This resetsa all index and Item to default values. 
        /// </summary>
        Clear,
        /// <summary>
        /// An Item was set in the <see cref="ObservableList{T}"/>. 
        /// </summary>
        Set
    }
}
