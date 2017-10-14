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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

using Fornax.Net.Util.Linq;
using Fornax.Net.Util.System;
using CultureContext = Fornax.Net.Util.System.CultureContext;

namespace Fornax.Net.Util.Collections
{
    /// <summary>
    /// A uitility class for handling collections.
    /// </summary>
    public static partial class Collections
    {
        /// <summary>
        /// Adds all <paramref name="elements"/> into  <paramref name="set"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set">The set.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>true if addition was successfull ; otherwise , false</returns>
        public static bool AddAll<T>(ISet<T> set, IEnumerable<T> elements) {
            bool result = false;
            foreach (T element in elements) {
                result |= set.Add(element);
            }
            return result;
        }

        /// <summary>
        /// Gets an <see cref="Enumerable"/> empty <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns> an empty <see cref="IList{T}"/> with default initial capacity.</returns>
        public static IList<T> EmptyList<T>() {
            return (IList<T>)Enumerable.Empty<T>();
        }

        /// <summary>
        /// Gets an Empty <see cref="Enumerable"/> <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns> an empty new <see cref="IDictionary{TKey, TValue}"/> with default initial capacity.</returns>
        public static IDictionary<TKey, TValue> EmptyMap<TKey, TValue>() {
            return new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Gets a new <see cref="SetFromMap{T}"/> from <paramref name="map"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="map">The map.</param>
        /// <returns>a new <seealso cref="SetFromMap{T}"/>.</returns>
        public static ISet<T> NewSetFromMap<T, S>(IDictionary<T, bool?> map) {
            return new SetFromMap<T>(map);
        }

        /// <summary>
        /// Reverses the items in the specific <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        public static void Reverse<T>(IList<T> list) {
            int size = list.Count;
            for (int i = 0, mid = size >> 1, j = size - 1; i < mid; i++, j--) {
                list.Swap(i, j);
            }
        }

        /// <summary>
        /// Reverses the order by default <see cref="Comparer{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IComparer<T> ReverseOrder<T>() {
            return ReverseComparer<T>.REVERSE_ORDER;
        }

        /// <summary>
        /// Reverses the order by <see cref="Comparer"/> <paramref name="cmp"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmp">The CMP.</param>
        /// <returns></returns>
        public static IComparer<T> ReverseOrder<T>(IComparer<T> cmp) {
            if (cmp == null)
                return ReverseOrder<T>();

            if (cmp is ReverseComparer2<T>)
                return ((ReverseComparer2<T>)cmp).cmp;

            return new ReverseComparer2<T>(cmp);
        }

        /// <summary>
        /// Shuffles the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        public static void Shuffle<T>(IList<T> list) {
            list.Shuffle(new Random());
        }

        /// <summary>
        /// Gets a singleton <see cref="HashSet{T}"/> drived from the <typeparamref name="T"/> <paramref name="o"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static ISet<T> Singleton<T>(T o) {
            return new HashSet<T>(new T[] { o });
        }

        /// <summary>
        /// Gets a Singleton Map as <see cref="Dictionary{TKey, TValue}"/> from 
        /// <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> SingletonMap<TKey, TValue>(TKey key, TValue value) {
            return new Dictionary<TKey, TValue> { { key, value } };
        }

        /// <summary>
        /// Gets an Unmodifiable (i.e.ReadOnly <see cref="IList{T}"/>) with initial capacity
        /// equals to <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The initail list.</param>
        /// <returns>a new <see cref="UnmodifiableList{T}(IList{T})"/>.</returns>
        public static IList<T> UnmodifiableList<T>(IList<T> list) {
            return new UnmodifiableListImpl<T>(list);
        }

        /// <summary>
        /// Gets an Unmodifiable (i.e.ReadOnly <see cref="IList{T}" />) with initial capacity
        /// equals to return value of <paramref name="method"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">The predicate.</param>
        /// <returns>
        /// a new <see cref="UnmodifiableList{T}(IList{T})" />.
        /// </returns>
        public static IList<T> UnmodifiableList<T>(Func<IList<T>> method) {
            return new UnmodifiableListImpl<T>(method.Invoke());
        }

        /// <summary>
        /// Gets an Unmodifiable (i.e.ReadOnly <see cref="IDictionary{TKey, TValue}"/>) with initial capacity
        /// equals to <paramref name="d"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="d">The initial dictionary.</param>
        /// <returns>a new <see cref="UnmodifiableMap{TKey, TValue}(IDictionary{TKey, TValue})"/>.</returns>
        public static IDictionary<TKey, TValue> UnmodifiableMap<TKey, TValue>(IDictionary<TKey, TValue> d) {
            return new UnmodifiableDictionary<TKey, TValue>(d);
        }

        /// <summary>
        /// Gets an Unmodifiable (i.e.ReadOnly <see cref="IDictionary{TKey, TValue}" />) with initial capacity
        /// equals to <paramref name="method" />.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="method">The initial dictionary method holder.</param>
        /// <returns>
        /// a new <see cref="UnmodifiableMap{TKey, TValue}(IDictionary{TKey, TValue})" />.
        /// </returns>
        public static IDictionary<TKey, TValue> UnmodifiableMap<TKey, TValue>(Func<IDictionary<TKey, TValue>> method) {
            return new UnmodifiableDictionary<TKey, TValue>(method.Invoke());
        }

        /// <summary>
        /// Gets an Unmodifiable (i.e.ReadOnly <see cref="ISet{T}"/>) with initial capacity
        /// equals to <paramref name="set"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set">The intitial set.</param>
        /// <returns>A new <see cref="UnmodifiableSet{T}(ISet{T})"/></returns>
        public static ISet<T> UnmodifiableSet<T>(ISet<T> set) {
            return new UnmodifiableSetImpl<T>(set);
        }

        /// <summary>
        /// Gets an Unmodifiable (i.e.ReadOnly <see cref="ISet{T}" />) with initial capacity
        /// equals to return of <paramref name="method"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">The method.</param>
        /// <returns>A new <see cref="UnmodifiableSet{T}(ISet{T})"/></returns>
        public static ISet<T> UnmodifiableSet<T>(Func<ISet<T>> method) {
            return new UnmodifiableSetImpl<T>(method.Invoke());
        }

        /// <summary>
        /// The same implementation of GetHashCode from Java's AbstractList
        /// (the default implementation for all lists).
        /// <para/>
        /// This algorithm depends on the order of the items in the list.
        /// It is recursive and will build the hash code based on the values of
        /// all nested collections.
        /// <para/>
        /// Note this operation currently only supports <see cref="IList{T}"/>, <see cref="ISet{T}"/>, 
        /// and <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static int GetHashCode<T>(IList<T> list) {
            int hashCode = 1;
            bool isValueType = typeof(T).GetTypeInfo().IsValueType;
            foreach (T e in list) {
                hashCode = 31 * hashCode +
                    (isValueType ? e.GetHashCode() : (e == null ? 0 : GetHashCode(e)));
            }

            return hashCode;
        }

        /// <summary>
        /// The same implementation of GetHashCode from Java's AbstractSet
        /// (the default implementation for all sets)
        /// <para/>
        /// This algorithm does not depend on the order of the items in the set.
        /// It is recursive and will build the hash code based on the values of
        /// all nested collections.
        /// <para/>
        /// Note this operation currently only supports <see cref="IList{T}"/>, <see cref="ISet{T}"/>, 
        /// and <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static int GetHashCode<T>(ISet<T> set) {
            int h = 0;
            bool isValueType = typeof(T).GetTypeInfo().IsValueType;
            using (var i = set.GetEnumerator()) {
                while (i.MoveNext()) {
                    T obj = i.Current;
                    if (isValueType) {
                        h += obj.GetHashCode();
                    } else if (obj != null) {
                        h += GetHashCode(obj);
                    }
                }
            }
            return h;
        }

        /// <summary>
        /// The same implementation of GetHashCode from Java's AbstractMap
        /// (the default implementation for all dictionaries)
        /// <para/>
        /// This algoritm does not depend on the order of the items in the dictionary.
        /// It is recursive and will build the hash code based on the values of
        /// all nested collections.
        /// <para/>
        /// Note this operation currently only supports <see cref="IList{T}"/>, <see cref="ISet{T}"/>, 
        /// and <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static int GetHashCode<TKey, TValue>(IDictionary<TKey, TValue> dictionary) {
            int h = 0;
            bool keyIsValueType = typeof(TKey).GetTypeInfo().IsValueType;
            bool valueIsValueType = typeof(TValue).GetTypeInfo().IsValueType;
            using (var i = dictionary.GetEnumerator()) {
                while (i.MoveNext()) {
                    TKey key = i.Current.Key;
                    TValue value = i.Current.Value;
                    int keyHash = (keyIsValueType ? key.GetHashCode() : (key == null ? 0 : GetHashCode(key)));
                    int valueHash = (valueIsValueType ? value.GetHashCode() : (value == null ? 0 : GetHashCode(value)));
                    h += keyHash ^ valueHash;
                }
            }
            return h;
        }

        /// <summary>
        /// This method generally assists with the recursive GetHashCode() that
        /// builds a hash code based on all of the values in a collection 
        /// including any nested collections (lists, sets, arrays, and dictionaries).
        /// <para/>
        /// Note this currently only supports <see cref="IList{T}"/>, <see cref="ISet{T}"/>, 
        /// and <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="obj">the object to build the hash code for</param>
        /// <returns>a value that represents the unique state of all of the values and 
        /// nested collection values in the object, provided the main object itself is 
        /// a collection, otherwise calls <see cref="object.GetHashCode()"/> on the 
        /// object that is passed.</returns>
        public static int GetHashCode(object obj) {
            if (obj == null) {
                return 0;
            }

            Type t = obj.GetType();
            if (t.GetTypeInfo().IsGenericType
                && (t.ImplementsGenericInterface(typeof(IList<>))
                || t.ImplementsGenericInterface(typeof(ISet<>))
                || t.ImplementsGenericInterface(typeof(IDictionary<,>)))) {
                var genericType = Convert.ChangeType(obj, t);
                return GetHashCode(genericType);
            }

            return obj.GetHashCode();
        }

        /// <summary>
        /// The same implementation of Equals from Java's AbstractList
        /// (the default implementation for all lists)
        /// <para/>
        /// This algorithm depends on the order of the items in the list. 
        /// It is recursive and will determine equality based on the values of
        /// all nested collections.
        /// <para/>
        /// Note this operation currently only supports <see cref="IList{T}"/>, <see cref="ISet{T}"/>, 
        /// and <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static bool Equals<T>(IList<T> listA, IList<T> listB) {
            if (ReferenceEquals(listA, listB)) {
                return true;
            }

            bool isValueType = typeof(T).GetTypeInfo().IsValueType;

            if (!isValueType && listA == null) {
                if (listB == null) {
                    return true;
                }
                return false;
            }


            using (IEnumerator<T> eA = listA.GetEnumerator()) {
                using (IEnumerator<T> eB = listB.GetEnumerator()) {
                    while (eA.MoveNext() && eB.MoveNext()) {
                        T o1 = eA.Current;
                        T o2 = eB.Current;

                        if (isValueType ?
                            !o1.Equals(o2) :
                            (!(o1 == null ? o2 == null : Equals(o1, o2)))) {
                            return false;
                        }
                    }

                    return (!(eA.MoveNext() || eB.MoveNext()));
                }
            }
        }

        /// <summary>
        /// The same implementation of Equals from Java's AbstractSet
        /// (the default implementation for all sets)
        /// <para/>
        /// This algoritm does not depend on the order of the items in the set.
        /// It is recursive and will determine equality based on the values of
        /// all nested collections.
        /// <para/>
        /// Note this operation currently only supports <see cref="IList{T}"/>, <see cref="ISet{T}"/>, 
        /// and <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static bool Equals<T>(ISet<T> setA, ISet<T> setB) {
            if (object.ReferenceEquals(setA, setB)) {
                return true;
            }

            if (setA == null) {
                if (setB == null) {
                    return true;
                }
                return false;
            }

            if (setA.Count != setB.Count) {
                return false;
            }

            bool isValueType = typeof(T).GetTypeInfo().IsValueType;

            foreach (T eB in setB) {
                bool contains = false;
                foreach (T eA in setA) {
                    if (isValueType ? eA.Equals(eB) : Equals(eA, eB)) {
                        contains = true;
                        break;
                    }
                }
                if (!contains) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// This is the same implemenation of Equals from Java's AbstractMap
        /// (the default implementation of all dictionaries)
        /// <para/>
        /// This algoritm does not depend on the order of the items in the dictionary.
        /// It is recursive and will determine equality based on the values of
        /// all nested collections.
        /// <para/>
        /// Note this operation currently only supports <see cref="IList{T}"/>, <see cref="ISet{T}"/>, 
        /// and <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static bool Equals<TKey, TValue>(IDictionary<TKey, TValue> dictionaryA, IDictionary<TKey, TValue> dictionaryB) {
            if (ReferenceEquals(dictionaryA, dictionaryB)) {
                return true;
            }

            if (dictionaryA == null) {
                if (dictionaryB == null) {
                    return true;
                }
                return false;
            }

            if (dictionaryA.Count != dictionaryB.Count) {
                return false;
            }

            bool valueIsValueType = typeof(TValue).GetTypeInfo().IsValueType;

            using (var i = dictionaryB.GetEnumerator()) {
                while (i.MoveNext()) {
                    KeyValuePair<TKey, TValue> e = i.Current;
                    TKey keyB = e.Key;
                    TValue valueB = e.Value;
                    if (valueB == null) {
                        if (!(dictionaryA.ContainsKey(keyB))) {
                            return false;
                        }
                    } else {
                        TValue valueA;
                        if (!dictionaryA.TryGetValue(keyB, out valueA) || (valueIsValueType ? !valueA.Equals(valueB) : !Equals(valueA, valueB))) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// A helper method to recursively determine equality based on
        /// the values of the collection and all nested collections.
        /// <para/>
        /// Note this operation currently only supports <see cref="IList{T}"/>, <see cref="ISet{T}"/>, 
        /// and <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        new public static bool Equals(object objA, object objB) {
            if (objA == null) {
                if (objB == null) {
                    return true;
                }
                return false;
            } else if (objB == null) {
                return false;
            }

            Type tA = objA.GetType();
            Type tB = objB.GetType();
            if (tA.GetTypeInfo().IsGenericType) {
                bool shouldReturn = false;

                if (tA.ImplementsGenericInterface(typeof(IList<>))) {
                    if (!(tB.GetTypeInfo().IsGenericType && tB.ImplementsGenericInterface(typeof(IList<>)))) {
                        return false; // type mismatch - must be a list
                    }
                    shouldReturn = true;
                } else if (tA.ImplementsGenericInterface(typeof(ISet<>))) {
                    if (!(tB.GetTypeInfo().IsGenericType && tB.ImplementsGenericInterface(typeof(ISet<>)))) {
                        return false; // type mismatch - must be a set
                    }
                    shouldReturn = true;
                } else if (tA.ImplementsGenericInterface(typeof(IDictionary<,>))) {
                    if (!(tB.GetTypeInfo().IsGenericType && tB.ImplementsGenericInterface(typeof(IDictionary<,>)))) {
                        return false; // type mismatch - must be a dictionary
                    }
                    shouldReturn = true;
                }

                if (shouldReturn) {
                    var genericTypeA = Convert.ChangeType(objA, tA);
                    var genericTypeB = Convert.ChangeType(objB, tB);
                    return Equals(genericTypeA, genericTypeB);
                }
            }

            return objA.Equals(objB);
        }

        /// <summary>
        /// This is the same implementation of ToString from Java's AbstractCollection
        /// (the default implementation for all sets and lists)
        /// </summary>
        public static string ToString<T>(ICollection<T> collection) {
            if (collection.Count == 0) {
                return "[]";
            }

            bool isValueType = typeof(T).GetTypeInfo().IsValueType;
            using (var it = collection.GetEnumerator()) {
                StringBuilder sb = new StringBuilder();
                sb.Append('[');
                it.MoveNext();
                while (true) {
                    T e = it.Current;
                    sb.Append(ReferenceEquals(e, collection) ? "(this Collection)" : (isValueType ? e.ToString() : ToString(e)));
                    if (!it.MoveNext()) {
                        return sb.Append(']').ToString();
                    }
                    sb.Append(',').Append(' ');
                }
            }
        }

        /// <summary>
        /// This is the same implementation of ToString from Java's AbstractCollection
        /// (the default implementation for all sets and lists), plus the ability
        /// to specify culture for formatting of nested numbers and dates. Note that
        /// this overload will change the culture of the current thread.
        /// </summary>
        public static string ToString<T>(ICollection<T> collection, CultureInfo culture) {
            using (var context = new CultureContext(culture)) {
                return ToString(collection);
            }
        }

        /// <summary>
        /// This is a helper method that assists with recursively building
        /// a string of the current collection and all nested collections.
        /// </summary>
        public static string ToString(object obj) {
            Type t = obj.GetType();
            if (t.GetTypeInfo().IsGenericType
                && (t.ImplementsGenericInterface(typeof(ICollection<>)))
                || t.ImplementsGenericInterface(typeof(IDictionary<,>))) {
                var genericType = Convert.ChangeType(obj, t);
                return ToString(genericType);
            }
           
            return obj.ToString();
        }

        /// <summary>
        /// This is a helper method that assists with recursively building
        /// a string of the current collection and all nested collections, plus the ability
        /// to specify culture for formatting of nested numbers and dates. Note that
        /// this overload will change the culture of the current thread.
        /// </summary>
        public static string ToString(object obj, CultureInfo culture) {
            using (var context = new CultureContext(culture)) {
                return ToString(obj);
            }
        }

        #region Nested Types

        #region ReverseComparer

        private class ReverseComparer<T> : IComparer<T>
        {
            internal static readonly ReverseComparer<T> REVERSE_ORDER = new ReverseComparer<T>();

            public int Compare(T x, T y) {
                return (new CaseInsensitiveComparer()).Compare(y, x);
            }
        }

        #endregion ReverseComparer

        #region ReverseComparer2

        private class ReverseComparer2<T> : IComparer<T>
        {
            /**
             * The comparer specified in the static factory.  This will never
             * be null, as the static factory returns a ReverseComparer
             * instance if its argument is null.
             *
             * @serial
             */
            internal readonly IComparer<T> cmp;

            public ReverseComparer2(IComparer<T> cmp) {
                Debug.Assert(cmp != null);
                this.cmp = cmp;
            }

            public int Compare(T t1, T t2) {
                return cmp.Compare(t2, t1);
            }

            public override bool Equals(object o) {
                return (o == this) ||
                    (o is ReverseComparer2<T> &&
                     cmp.Equals(((ReverseComparer2<T>)o).cmp));
            }

            public override int GetHashCode() {
                return cmp.GetHashCode() ^ int.MinValue;
            }

            public IComparer<T> Reversed() {
                return cmp;
            }
        }

        #endregion ReverseComparer2

        #region UnmodifiableListImpl

        private class UnmodifiableListImpl<T> : IList<T>
        {
            private readonly IList<T> list;

            public UnmodifiableListImpl(IList<T> list) {
                this.list = list ?? throw new ArgumentNullException("list");
            }

            public T this[int index] {
                get => list[index];
                set => throw new InvalidOperationException("Unable to modify this list.");
            }
            public bool IsReadOnly => true;

            public int Count => this.list.Count;

            public void Add(T item) {
                throw new InvalidOperationException("Unable to modify this list.");
            }

            public void Clear() {
                throw new InvalidOperationException("Unable to modify this list.");
            }

            public bool Contains(T item) {
                return list.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex) => this.list.CopyTo(array, arrayIndex);

            public IEnumerator<T> GetEnumerator() => this.list.GetEnumerator();

            public int IndexOf(T item) {
                return list.IndexOf(item);
            }

            public void Insert(int index, T item) {
                throw new InvalidOperationException("Unable to modify this list.");
            }

            public bool Remove(T item) {
                throw new InvalidOperationException("Unable to modify this list.");
            }

            public void RemoveAt(int index) {
                throw new InvalidOperationException("Unable to modify this list.");
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        #endregion UnmodifiableListImpl

        #region UnmodifiableDictionary

        private class UnmodifiableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        {
            private IDictionary<TKey, TValue> _dict;

            public UnmodifiableDictionary(IDictionary<TKey, TValue> dict) {
                _dict = dict;
            }

            public UnmodifiableDictionary() {
                _dict = new Dictionary<TKey, TValue>();
            }

            public void Add(TKey key, TValue value) {
                throw new InvalidOperationException("Unable to modify this dictionary.");
            }

            public bool ContainsKey(TKey key) {
                return _dict.ContainsKey(key);
            }

            public ICollection<TKey> Keys {
                get { return _dict.Keys; }
            }

            public bool Remove(TKey key) {
                throw new InvalidOperationException("Unable to modify this dictionary.");
            }

            public bool TryGetValue(TKey key, out TValue value) {
                return _dict.TryGetValue(key, out value);
            }

            public ICollection<TValue> Values {
                get { return _dict.Values; }
            }

            public TValue this[TKey key] {
                get {
                    TValue ret;
                    _dict.TryGetValue(key, out ret);
                    return ret;
                }
                set {
                    throw new InvalidOperationException("Unable to modify this dictionary.");
                }
            }

            public void Add(KeyValuePair<TKey, TValue> item) {
                throw new InvalidOperationException("Unable to modify this dictionary.");
            }

            public void Clear() {
                throw new InvalidOperationException("Unable to modify this dictionary.");
            }

            public bool Contains(KeyValuePair<TKey, TValue> item) {
                return _dict.Contains(item);
            }

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
                _dict.CopyTo(array, arrayIndex);
            }

            public int Count => this._dict.Count;

            public bool IsReadOnly => true;

            public bool Remove(KeyValuePair<TKey, TValue> item) {
                throw new InvalidOperationException("Unable to modify this dictionary.");
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
                return _dict.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return this._dict.GetEnumerator();
            }
        }

        #endregion UnmodifiableDictionary

        #region UnmodifiableSetImpl

        private class UnmodifiableSetImpl<T> : ISet<T>
        {
            private readonly ISet<T> set;
            public UnmodifiableSetImpl(ISet<T> set) {
                if (set == null)
                    throw new ArgumentNullException("set");
                this.set = set;
            }

            public int Count => this.set.Count;

            public bool IsReadOnly => true;

            public void Add(T item) {
                throw new InvalidOperationException("Unable to modify this set.");
            }

            public void Clear() {
                throw new InvalidOperationException("Unable to modify this set.");
            }

            public bool Contains(T item) => this.set.Contains(item);

            public void CopyTo(T[] array, int arrayIndex) {
                set.CopyTo(array, arrayIndex);
            }

            public void ExceptWith(IEnumerable<T> other) {
                throw new InvalidOperationException("Unable to modify this set.");
            }

            public IEnumerator<T> GetEnumerator() => this.set.GetEnumerator();

            public void IntersectWith(IEnumerable<T> other) {
                throw new InvalidOperationException("Unable to modify this set.");
            }

            public bool IsProperSubsetOf(IEnumerable<T> other) => this.set.IsProperSubsetOf(other);

            public bool IsProperSupersetOf(IEnumerable<T> other) => this.set.IsProperSupersetOf(other);

            public bool IsSubsetOf(IEnumerable<T> other) => this.set.IsSubsetOf(other);

            public bool IsSupersetOf(IEnumerable<T> other) => this.set.IsSupersetOf(other);

            public bool Overlaps(IEnumerable<T> other) => this.set.Overlaps(other);

            public bool Remove(T item) {
                throw new InvalidOperationException("Unable to modify this set.");
            }

            public bool SetEquals(IEnumerable<T> other) => this.set.SetEquals(other);

            public void SymmetricExceptWith(IEnumerable<T> other) {
                throw new InvalidOperationException("Unable to modify this set.");
            }

            public void UnionWith(IEnumerable<T> other) {
                throw new InvalidOperationException("Unable to modify this set.");
            }

            bool ISet<T>.Add(T item) {
                throw new InvalidOperationException("Unable to modify this set.");
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }
        #endregion

        #endregion Nested Types
    }
}