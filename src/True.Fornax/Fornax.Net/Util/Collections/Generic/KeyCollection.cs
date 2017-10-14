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
    public partial class LurchTable<TKey, TValue> {

        /// <summary>
        /// Provides the collection of Keys for the LurchTable
        /// </summary>
        /// <seealso cref="IDictionary{TKey, TValue}" />
        /// <seealso cref="IDisposable" />
        public class KeyCollection : ICollection<TKey> {

            private readonly LurchTable<TKey, TValue> _owner;

            internal KeyCollection(LurchTable<TKey, TValue> owner) {
                _owner = owner;
            }

            /// <summary>
            /// Determines whether the <see cref="ICollection" /> contains a specific value.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="ICollection" />.</param>
            /// <returns>
            ///   <see langword="true" /> if <paramref name="item" /> is found in the <see cref="ICollection" />; otherwise, <see langword="false" />.
            /// </returns>
            public bool Contains(TKey item) {
                return _owner.ContainsKey(item);
            }

            /// <summary>
            /// Copies the elements of the <see cref="ICollection" /> to an <see cref="Array" />, starting at a particular <see cref="Array" /> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
            public void CopyTo(TKey[] array, int arrayIndex) {
                foreach (var item in _owner)
                    array[arrayIndex++] = item.Key;
            }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="ICollection" />.
            /// </summary>
            public int Count => _owner.Count;

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns> An <seealso cref="Enumerator"/> Instance for <see cref="LurchTable{TKey, TValue}"/></returns>
            public Enumerator GetEnumerator() {
                return new Enumerator(_owner);
            }

            /// <summary>
            /// Provides an enumerator that iterates through the collection.
            /// </summary>
            /// <seealso cref="IDictionary{TKey, TValue}" />
            /// <seealso cref="IDisposable" />
            public struct Enumerator : IEnumerator<TKey> {
                private readonly LurchTable<TKey, TValue> _owner;
                private EnumState _state;

                internal Enumerator(LurchTable<TKey, TValue> owner) {
                    _owner = owner;
                    _state = new EnumState();
                    _state.Init();
                }

                /// <summary>
                /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
                /// </summary>
                public void Dispose() {
                    _state.Unlock();
                }

                object IEnumerator.Current => Current;

                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                /// <value>
                /// The current.
                /// </value>
                /// <exception cref="InvalidOperationException">Raised on invalid lurch operation.</exception>
                /// <exception cref="ObjectDisposedException">Raised on a table post-Disposed call.</exception>
                public TKey Current {
                    get {
                        int index = _state.Current;
                        if (index <= 0)
                            throw new InvalidOperationException();
                        if (_owner._entries == null)
                            throw new ObjectDisposedException(GetType().Name);
                        return _owner._entries[index >> _owner._shift][index & _owner._shiftMask].Key;
                    }
                }


                /// <summary>
                /// Advances the enumerator to the next element of the collection.
                /// </summary>
                /// <returns>
                ///   <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.
                /// </returns>
                public bool MoveNext() {
                    return _owner.MoveNext(ref _state);
                }

                /// <summary>
                /// Sets the enumerator to its initial position, which is before the first element in the collection.
                /// </summary>
                public void Reset() {
                    _state.Unlock();
                    _state.Init();
                }
            }

            [Obsolete]
            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() {
                return new Enumerator(_owner);
            }
            [Obsolete]
            IEnumerator IEnumerable.GetEnumerator() {
                return new Enumerator(_owner);
            }
            [Obsolete]
            bool ICollection<TKey>.IsReadOnly => true;
            [Obsolete]
            void ICollection<TKey>.Add(TKey item) {
                throw new NotSupportedException();
            }
            [Obsolete]
            void ICollection<TKey>.Clear() {
                throw new NotSupportedException();
            }
            [Obsolete]
            bool ICollection<TKey>.Remove(TKey item) {
                throw new NotSupportedException();
            }
        }
       
    }
}