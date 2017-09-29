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
using System.Linq;
using System.Runtime.Serialization;

namespace Fornax.Net.Util.Collections
{
    public static partial class Collections {

        /// <summary>
        /// A Readonly Representation of a <see cref="ISet{T}"/> from a <see cref="IDictionary.Values"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <seealso cref="System.Collections.Generic.ICollection{T}" />
        /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
        /// <seealso cref="System.Collections.IEnumerable" />
        /// <seealso cref="System.Collections.Generic.ISet{T}" />
        /// <seealso cref="System.Collections.Generic.IReadOnlyCollection{T}" />
        /// <seealso cref="System.Runtime.Serialization.ISerializable" />
        /// <seealso cref="System.Runtime.Serialization.IDeserializationCallback" />
        internal class SetFromMap<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ISet<T>, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback {
            private readonly IDictionary<T, bool?> m;

            [NonSerialized]
            private ICollection<T> s;

            internal SetFromMap(IDictionary<T, bool?> map) {
                if (map.Any())
                    throw new ArgumentException("Map is not empty");
                m = map;
                s = map.Keys;
            }

            public void Clear() {
                m.Clear();
            }

            public int Count => m.Count;

            public bool IsReadOnly => this.s.IsReadOnly;

            public bool Contains(T item) => m.ContainsKey(item);

            public bool Remove(T item) => m.Remove(item);

            public bool Add(T item) {
                this.m.Add(item, true);
                return this.m.ContainsKey(item);
            }

            void ICollection<T>.Add(T item) => m.Add(item, true);

            public IEnumerator<T> GetEnumerator() => s.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => s.GetEnumerator();

            public override string ToString() => s.ToString();

            public override int GetHashCode() => s.GetHashCode();

            public override bool Equals(object obj) {
                return obj == this || s.Equals(obj);
            }

            public virtual bool ContainsAll(IEnumerable<T> other) {
                return this.OrderBy(x => x).SequenceEqual(other.OrderBy(x => x));
            }

            public void CopyTo(T[] array, int arrayIndex) {
                m.Keys.CopyTo(array, arrayIndex);
            }

            public bool GetIsReadOnly() => false;

            public bool SetEquals(IEnumerable<T> other) {
                if (other == null) {
                    throw new ArgumentNullException("other");
                }
                SetFromMap<T> set = other as SetFromMap<T>;
                if (set != null) {
                    if (this.m.Count != set.Count) {
                        return false;
                    }
                    return ContainsAll(set);
                }
                ICollection<T> is2 = other as ICollection<T>;
                if (((is2 != null) && (this.m.Count == 0)) && (is2.Count > 0)) {
                    return false;
                }
                foreach (var item in this) {
                    if (!is2.Contains(item)) {
                        return false;
                    }
                }
                return true;
            }

            #region Not Implemented Members
            public void ExceptWith(IEnumerable<T> other) {
                throw new NotImplementedException();
            }

            public void IntersectWith(IEnumerable<T> other) {
                throw new NotImplementedException();
            }

            public bool IsProperSubsetOf(IEnumerable<T> other) {
                throw new NotImplementedException();
            }

            public bool IsProperSupersetOf(IEnumerable<T> other) {
                throw new NotImplementedException();
            }

            public bool IsSubsetOf(IEnumerable<T> other) {
                throw new NotImplementedException();
            }

            public bool IsSupersetOf(IEnumerable<T> other) {
                throw new NotImplementedException();
            }

            public bool Overlaps(IEnumerable<T> other) {
                throw new NotImplementedException();
            }

            public void SymmetricExceptWith(IEnumerable<T> other) {
                throw new NotImplementedException();
            }

            public void UnionWith(IEnumerable<T> other) {
                throw new NotImplementedException();
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context) {
                throw new NotImplementedException();
            }

            public void OnDeserialization(object sender) {
                throw new NotImplementedException();
            }
            #endregion
        }

    }
}
