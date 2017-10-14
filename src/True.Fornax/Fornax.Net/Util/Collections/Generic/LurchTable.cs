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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

//// <summary>
//// Generic Data structures 
//// </summary>
namespace Fornax.Net.Util.Collections.Generic
{

    /// <summary>
    /// LurchTable stands for "Least Used Recently Concurrent Hash Table" and has definite
    /// similarities to both the <seealso cref="ConcurrentDictionary{TKey, TValue}" /> as well as Java's LinkedHashMap.
    /// This gives you a thread-safe dictionary/hashtable that stores element ordering by
    /// insertion, updates, or access.  In addition it can be configured to use a 'hard-limit'
    /// count of items that will automatically 'pop' the oldest item in the collection.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <seealso cref="IDictionary{TKey, TValue}" />
    /// <seealso cref="IDisposable" />
    [Serializable]
    [Progress("LurchTable",false,Documented = true,Tested = false)]
    public partial class LurchTable<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable
    {

        /// <summary>
        /// Method signature for the ItemUpdated event
        /// </summary>
        /// <param name="previous">The previous KVP.</param>
        /// <param name="next">The next KVP.</param>
        public delegate void ItemUpdatedMethod(KeyValuePair<TKey, TValue> previous, KeyValuePair<TKey, TValue> next);

        /// <summary>
        /// Event raised after an item is removed from the collection
        /// </summary>
        public event Action<KeyValuePair<TKey, TValue>> ItemRemoved;

        /// <summary>
        /// Event raised after an item is updated in the collection
        /// </summary>
        public event ItemUpdatedMethod ItemUpdated;

        /// <summary>
        /// Event raised after an item is added to the collection
        /// </summary>
        public event Action<KeyValuePair<TKey, TValue>> ItemAdded;

        private const int OverAlloc = 128;
        private const int FreeSlots = 32;

        private readonly IEqualityComparer<TKey> _comparer;
        private readonly int _hsize, _lsize;
        private int _limit;
        private readonly int _allocSize, _shift, _shiftMask;
        private readonly LurchOrder _ordering;
        private readonly object[] _locks;
        private readonly int[] _buckets;
        private readonly FreeList[] _free;

        private Entry[][] _entries;
        private int _used, _count;
        private int _allocNext, _freeVersion;

        /// <summary>
        /// Creates a LurchTable that can store up to (capacity) items efficiently.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public LurchTable(int capacity)
            : this(LurchOrder.None, int.MaxValue, capacity >> 1, capacity >> 4, capacity >> 8, EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Creates a LurchTable that can store up to (capacity) items efficiently.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="ordering">The ordering.</param>
        public LurchTable(int capacity, LurchOrder ordering)
            : this(ordering, int.MaxValue, capacity >> 1, capacity >> 4, capacity >> 8, EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Creates a LurchTable that can store up to (capacity) items efficiently.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="ordering">The ordering.</param>
        /// <param name="comparer">The comparer.</param>
        public LurchTable(int capacity, LurchOrder ordering, IEqualityComparer<TKey> comparer)
            : this(ordering, int.MaxValue, capacity >> 1, capacity >> 4, capacity >> 8, comparer) { }

        /// <summary>
        /// Creates a LurchTable that orders items by (ordering) and removes items once the specified (limit) is reached.
        /// </summary>
        /// <param name="ordering">The ordering.</param>
        /// <param name="limit">The limit.</param>
        public LurchTable(LurchOrder ordering, int limit)
            : this(ordering, limit, limit >> 1, limit >> 4, limit >> 8, EqualityComparer<TKey>.Default) { }

        /// <summary>
        /// Creates a LurchTable that orders items by (ordering) and removes items once the specified (limit) is reached.
        /// </summary>
        /// <param name="ordering">The ordering.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="comparer">The comparer.</param>
        public LurchTable(LurchOrder ordering, int limit, IEqualityComparer<TKey> comparer)
            : this(ordering, limit, limit >> 1, limit >> 4, limit >> 8, comparer) { }

        /// <summary>
        /// Creates a LurchTable that orders items by (ordering) and removes items once the specified (limit) is reached.
        /// </summary>
        /// <param name="ordering">The type of linking for the items</param>
        /// <param name="limit">The maximum allowable number of items, or int.MaxValue for unlimited</param>
        /// <param name="hashSize">The number of hash buckets to use for the collection, usually 1/2 estimated capacity</param>
        /// <param name="allocSize">The number of entries to allocate at a time, usually 1/16 estimated capacity</param>
        /// <param name="lockSize">The number of concurrency locks to preallocate, usually 1/256 estimated capacity</param>
        /// <param name="comparer">The element hash generator for keys</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// limit
        /// or
        /// ordering
        /// </exception>
        public LurchTable(LurchOrder ordering, int limit, int hashSize, int allocSize, int lockSize, IEqualityComparer<TKey> comparer) {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException("limit");
            if (ordering == LurchOrder.None && limit < int.MaxValue)
                throw new ArgumentOutOfRangeException("ordering");

            _limit = limit <= 0 ? int.MaxValue : limit;
            _comparer = comparer;
            _ordering = ordering;

            allocSize = (int)Math.Min((long)allocSize + OverAlloc, 0x3fffffff);
            //last power of 2 that is less than allocSize
            for (_shift = 7; _shift < 24 && (1 << (_shift + 1)) < allocSize; _shift++) { }
            _allocSize = 1 << _shift;
            _shiftMask = _allocSize - 1;

            _hsize = HashUtilities.SelectPrimeNumber(Math.Max(127, hashSize));
            _buckets = new int[_hsize];

            _lsize = HashUtilities.SelectPrimeNumber(lockSize);
            _locks = new object[_lsize];
            for (int i = 0; i < _lsize; i++)
                _locks[i] = new object();

            _free = new FreeList[FreeSlots];
            Initialize();
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection" />.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Retrieves the LurchOrder Ordering enumeration this instance was created with.
        /// </summary>
        /// <value>
        /// The ordering.
        /// </value>
        public LurchOrder Ordering => _ordering;

        /// <summary>
        /// Retrives the key comparer being used by this instance.
        /// </summary>
        /// <value>
        /// The comparer.
        /// </value>
        public IEqualityComparer<TKey> Comparer => _comparer;

        /// <summary>
        /// Gets or Sets the record limit allowed in this instance.
        /// </summary>
        /// <value>
        /// The limit.
        /// </value>
        public int Limit {
            get { return _limit; }
            set { _limit = value; }
        }

        /// <summary>
        /// WARNING: not thread-safe, reinitializes all internal structures.  Use Clear() for a thread-safe
        /// delete all.  If you have externally provided exclusive access this method may be used to more
        /// efficiently clear the collection.
        /// </summary>
        /// <exception cref="LurchCorruptionException">
        /// Raised if an overflow/mismatch/corruption of internal data of lurch table occurs.
        /// </exception>
        public void Initialize() {
            lock (this) {
                _freeVersion = _allocNext = 0;
                _count = 0;
                _used = 1;

                Array.Clear(_buckets, 0, _hsize);
                _entries = new[] { new Entry[_allocSize] };
                for (int slot = 0; slot < FreeSlots; slot++) {
                    var index = Interlocked.CompareExchange(ref _used, _used + 1, _used);
                    if (index != slot + 1)
                        throw new LurchCorruptionException();

                    _free[slot].Tail = index;
                    _free[slot].Head = index;
                }

                if (_count != 0 || _used != FreeSlots + 1)
                    throw new LurchCorruptionException();
            }
        }

        private KeyCollection _keyCollection;
        private ValueCollection _valueCollection;

        /// <summary>
        /// Gets an <see cref="ICollection" /> containing the keys of the <see cref="IDictionary" />.
        /// </summary>
        public KeyCollection Keys => _keyCollection ?? (_keyCollection = new KeyCollection(this));

        /// <summary>
        /// Gets an <see cref="ICollection" /> containing the values in the <see cref="IDictionary" />.
        /// </summary>
        public ValueCollection Values => _valueCollection ?? (_valueCollection = new ValueCollection(this));

        [Obsolete]
        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        [Obsolete]
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        #region IDisposable Members

        /// <summary>
        /// Clears references to all objects and invalidates the collection
        /// </summary>
        public void Dispose() {
            _entries = null;
            _used = _count = 0;
        }

        #endregion

        #region IDictionary Members

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        public void Clear() {
            if (_entries == null) throw new ObjectDisposedException(GetType().Name);
            foreach (var item in this)
                Remove(item.Key);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
        /// </summary>
        public bool ContainsKey(TKey key) {
            if (_entries == null) throw new ObjectDisposedException(GetType().Name);
            return TryGetValue(key, out TValue value);
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        public TValue this[TKey key] {
            set {
                var info = new AddInfo { Value = value, CanUpdate = true };
                Insert(key, ref info);
            }
            get {
                if (!TryGetValue(key, out TValue value))
                    throw new ArgumentOutOfRangeException();
                return value;
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements <see cref="IDictionary"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value) {
            int hash = _comparer.GetHashCode(key) & int.MaxValue;
            return InternalGetValue(hash, key, out value);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        public void Add(TKey key, TValue value) {
            var info = new AddInfo { Value = value };
            if (InsertResult.Inserted != Insert(key, ref info))
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="IDictionary"/>.
        /// </summary>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <param name="key">The key of the element to remove.</param>
        public bool Remove(TKey key) {
            var del = new DelInfo();
            return Delete(key, ref del);
        }

        #endregion

        #region IDictionaryEx Members

        /// <summary>
        /// Adds a key/value pair to the  <see cref="IDictionary"/> if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value to be added, if the key does not already exist.</param>
        public TValue GetOrAdd(TKey key, TValue value) {
            var info = new AddInfo { Value = value, CanUpdate = false };
            if (InsertResult.Exists == Insert(key, ref info))
                return info.Value;
            return value;
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public bool TryAdd(TKey key, TValue value) {
            var info = new AddInfo { Value = value, CanUpdate = false };
            return InsertResult.Inserted == Insert(key, ref info);
        }

        /// <summary>
        /// Updates an element with the provided key to the value if it exists.
        /// </summary>
        /// <returns>Returns true if the key provided was found and updated to the value.</returns>
        /// <param name="key">The object to use as the key of the element to update.</param>
        /// <param name="value">The new value for the key if found.</param>
        public bool TryUpdate(TKey key, TValue value) {
            var info = new UpdateInfo { Value = value };
            return InsertResult.Updated == Insert(key, ref info);
        }

        /// <summary>
        /// Updates an element with the provided key to the value if it exists.
        /// </summary>
        /// <returns>Returns true if the key provided was found and updated to the value.</returns>
        /// <param name="key">The object to use as the key of the element to update.</param>
        /// <param name="value">The new value for the key if found.</param>
        /// <param name="comparisonValue">The value that is compared to the value of the element with key.</param>
        public bool TryUpdate(TKey key, TValue value, TValue comparisonValue) {
            var info = new UpdateInfo(comparisonValue) { Value = value };
            return InsertResult.Updated == Insert(key, ref info);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="IDictionary"/>.
        /// </summary>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="value">The value that was removed.</param>
        public bool TryRemove(TKey key, out TValue value) {
            var info = new DelInfo();
            if (Delete(key, ref info)) {
                value = info.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        #endregion

        #region IConcurrentDictionary Members

        /// <summary>
        /// Adds a key/value pair to the  <see cref="IDictionary"/> if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="fnCreate">Constructs a new value for the key.</param>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> fnCreate) {
            var info = new Add2Info { Create = fnCreate };
            Insert(key, ref info);
            return info.Value;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="IDictionary"/> if the key does not already exist, 
        /// or updates a key/value pair if the key already exists.
        /// </summary>
        public TValue AddOrUpdate(TKey key, TValue addValue, KeyValueUpdate<TKey, TValue> fnUpdate) {
            var info = new Add2Info(addValue) { Update = fnUpdate };
            Insert(key, ref info);
            return info.Value;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="IDictionary"/> if the key does not already exist, 
        /// or updates a key/value pair if the key already exists.
        /// </summary>
        /// <remarks>
        /// Adds or modifies an element with the provided key and value.  If the key does not exist in the collection,
        /// the factory method fnCreate will be called to produce the new value, if the key exists, the converter method
        /// fnUpdate will be called to create an updated value.
        /// </remarks>
        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> fnCreate, KeyValueUpdate<TKey, TValue> fnUpdate) {
            var info = new Add2Info { Create = fnCreate, Update = fnUpdate };
            Insert(key, ref info);
            return info.Value;
        }

        /// <summary>
        /// Add, update, or fetche a key/value pair from the dictionary via an implementation of the
        /// <see cref="T:CSharpTest.Net.Collections.ICreateOrUpdateValue"/> interface.
        /// </summary>
        public bool AddOrUpdate<T>(TKey key, ref T createOrUpdateValue) where T : ICreateOrUpdateValue<TKey, TValue> {
            var result = Insert(key, ref createOrUpdateValue);
            return result == InsertResult.Inserted || result == InsertResult.Updated;
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="IDictionary"/>
        /// by calling the provided factory method to construct the value if the key is not already present in the collection.
        /// </summary>
        public bool TryAdd(TKey key, Func<TKey, TValue> fnCreate) {
            var info = new Add2Info { Create = fnCreate };
            return InsertResult.Inserted == Insert(key, ref info);
        }

        /// <summary>
        /// Modify the value associated with the result of the provided update method
        /// as an atomic operation, Allows for reading/writing a single record within
        /// the syncronization lock.
        /// </summary>
        public bool TryUpdate(TKey key, KeyValueUpdate<TKey, TValue> fnUpdate) {
            var info = new Add2Info { Update = fnUpdate };
            return InsertResult.Updated == Insert(key, ref info);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="IDictionary"/>
        /// if the fnCondition predicate is null or returns true.
        /// </summary>
        public bool TryRemove(TKey key, KeyValuePredicate<TKey, TValue> fnCondition) {
            var info = new DelInfo { Condition = fnCondition };
            return Delete(key, ref info);
        }

        /// <summary>
        /// Conditionally removes a key/value pair from the dictionary via an implementation of the
        /// <see cref="T:CSharpTest.Net.Collections.IRemoveValue"/> interface.
        /// </summary>
        public bool TryRemove<T>(TKey key, ref T removeValue) where T : IRemoveValue<TKey, TValue> {
            return Delete(key, ref removeValue);
        }

        #endregion

        #region ICollection<KeyValuePair Members

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) {
            if (TryGetValue(item.Key, out TValue test))
                return EqualityComparer<TValue>.Default.Equals(item.Value, test);
            return false;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) {
            var del = new DelInfo(item.Value);
            return Delete(item.Key, ref del);
        }

        #endregion

        #region IEnumerator Members

        private bool MoveNext(ref EnumState state) {
            if (_entries == null) throw new ObjectDisposedException(GetType().Name);

            if (state.Current > 0)
                state.Current = state.Next;

            if (state.Current > 0) {
                state.Next = _entries[state.Current >> _shift][state.Current & _shiftMask].Link;
                return true;
            }

            state.Unlock();
            while (++state.Bucket < _hsize) {
                if (_buckets[state.Bucket] == 0)
                    continue;

                state.Lock(_locks[state.Bucket % _lsize]);

                state.Current = _buckets[state.Bucket];
                if (state.Current > 0) {
                    state.Next = _entries[state.Current >> _shift][state.Current & _shiftMask].Link;
                    return true;
                }

                state.Unlock();
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns><seealso cref="Enumerator"/></returns>
        public Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() { return GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        #endregion

        #region Peek/Dequeue

        /// <summary>
        /// Retrieves the oldest entry in the collection based on the ordering supplied to the constructor.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True if the out parameter value was set.
        /// </returns>
        /// <exception cref="InvalidOperationException">Raised if the table is unordered</exception>
        /// <exception cref="ObjectDisposedException">Raised on a table post-Disposed call.</exception>
        public bool Peek(out KeyValuePair<TKey, TValue> value) {
            if (_ordering == LurchOrder.None)
                throw new InvalidOperationException();
            if (_entries == null)
                throw new ObjectDisposedException(GetType().Name);

            while (true) {
                int index = Interlocked.CompareExchange(ref _entries[0][0].Prev, 0, 0);
                if (index == 0) {
                    value = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                int hash = _entries[index >> _shift][index & _shiftMask].Hash;
                if (hash >= 0) {
                    int bucket = hash % _hsize;
                    lock (_locks[bucket % _lsize]) {
                        if (index == _entries[0][0].Prev &&
                            hash == _entries[index >> _shift][index & _shiftMask].Hash) {
                            value = new KeyValuePair<TKey, TValue>(
                                _entries[index >> _shift][index & _shiftMask].Key,
                                _entries[index >> _shift][index & _shiftMask].Value
                            );
                            return true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes the oldest entry in the collection based on the ordering supplied to the constructor.
        /// If an item is not available a busy-wait loop is used to wait for for an item.
        /// </summary>
        /// <returns>
        /// The Key/Value pair removed.
        /// </returns>
        /// <exception cref="InvalidOperationException">Raised if the table is unordered</exception>
        /// <exception cref="ObjectDisposedException">Raised on a table post-Disposed call.</exception>
        public KeyValuePair<TKey, TValue> Dequeue() {
            if (_ordering == LurchOrder.None)
                throw new InvalidOperationException();
            if (_entries == null)
                throw new ObjectDisposedException(GetType().Name);

            KeyValuePair<TKey, TValue> value;
            while (!TryDequeue(out value)) {
                while (0 == Interlocked.CompareExchange(ref _entries[0][0].Prev, 0, 0))
                    Thread.Sleep(0);
            }
            return value;
        }

        /// <summary>
        /// Removes the oldest entry in the collection based on the ordering supplied to the constructor.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// False if no item was available
        /// </returns>
        /// <exception cref="InvalidOperationException">Raised if the table is unordered</exception>
        public bool TryDequeue(out KeyValuePair<TKey, TValue> value) {
            return TryDequeue(null, out value);
        }

        /// <summary>
        /// Removes the oldest entry in the collection based on the ordering supplied to the constructor.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// False if no item was available
        /// </returns>
        /// <exception cref="InvalidOperationException">Raised if the table is unordered</exception>
        /// <exception cref="ObjectDisposedException">Raised on a table post-Disposed call.</exception>
        /// <exception cref="LurchCorruptionException">Raised if there is an internal mismatch or corruption of data in lurch table.</exception>
        public bool TryDequeue(Predicate<KeyValuePair<TKey, TValue>> predicate, out KeyValuePair<TKey, TValue> value) {
            if (predicate == null) {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (_ordering == LurchOrder.None)
                throw new InvalidOperationException();
            if (_entries == null)
                throw new ObjectDisposedException(GetType().Name);

            while (true) {
                int index = Interlocked.CompareExchange(ref _entries[0][0].Prev, 0, 0);
                if (index == 0) {
                    value = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                int hash = _entries[index >> _shift][index & _shiftMask].Hash;
                if (hash >= 0) {
                    int bucket = hash % _hsize;
                    lock (_locks[bucket % _lsize]) {
                        if (index == _entries[0][0].Prev &&
                            hash == _entries[index >> _shift][index & _shiftMask].Hash) {
                            if (predicate != null) {
                                var item = new KeyValuePair<TKey, TValue>(
                                    _entries[index >> _shift][index & _shiftMask].Key,
                                    _entries[index >> _shift][index & _shiftMask].Value
                                );
                                if (!predicate(item)) {
                                    value = item;
                                    return false;
                                }
                            }

                            int next = _entries[index >> _shift][index & _shiftMask].Link;
                            bool removed = false;

                            if (_buckets[bucket] == index) {
                                _buckets[bucket] = next;
                                removed = true;
                            } else {
                                int test = _buckets[bucket];
                                while (test != 0) {
                                    int cmp = _entries[test >> _shift][test & _shiftMask].Link;
                                    if (cmp == index) {
                                        _entries[test >> _shift][test & _shiftMask].Link = next;
                                        removed = true;
                                        break;
                                    }
                                    test = cmp;
                                }
                            }
                            if (!removed)
                                throw new LurchCorruptionException();

                            value = new KeyValuePair<TKey, TValue>(
                                _entries[index >> _shift][index & _shiftMask].Key,
                                _entries[index >> _shift][index & _shiftMask].Value
                            );
                            Interlocked.Decrement(ref _count);
                            if (_ordering != LurchOrder.None)
                                InternalUnlink(index);
                            FreeSlot(ref index, Interlocked.Increment(ref _freeVersion));

                            ItemRemoved?.Invoke(value);

                            return true;
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Implementation

        enum InsertResult { Inserted = 1, Updated = 2, Exists = 3, NotFound = 4 }

        bool InternalGetValue(int hash, TKey key, out TValue value) {
            if (_entries == null)
                throw new ObjectDisposedException(GetType().Name);

            int bucket = hash % _hsize;
            lock (_locks[bucket % _lsize]) {
                int index = _buckets[bucket];
                while (index != 0) {
                    if (hash == _entries[index >> _shift][index & _shiftMask].Hash &&
                        _comparer.Equals(key, _entries[index >> _shift][index & _shiftMask].Key)) {
                        value = _entries[index >> _shift][index & _shiftMask].Value;
                        if (hash == _entries[index >> _shift][index & _shiftMask].Hash) {
                            if (_ordering == LurchOrder.Access) {
                                InternalUnlink(index);
                                InternalLink(index);
                            }
                            return true;
                        }
                    }
                    index = _entries[index >> _shift][index & _shiftMask].Link;
                }

                value = default(TValue);
                return false;
            }
        }

        InsertResult Insert<T>(TKey key, ref T value) where T : ICreateOrUpdateValue<TKey, TValue> {
            if (_entries == null)
                throw new ObjectDisposedException(GetType().Name);

            int hash = _comparer.GetHashCode(key) & int.MaxValue;

            InsertResult result = InternalInsert(hash, key, out var added, ref value);

            if (added > _limit && _ordering != LurchOrder.None) {
                TryDequeue(out KeyValuePair<TKey, TValue> ignore);
            }
            return result;
        }

        InsertResult InternalInsert<T>(int hash, TKey key, out int added, ref T value) where T : ICreateOrUpdateValue<TKey, TValue> {
            int bucket = hash % _hsize;
            lock (_locks[bucket % _lsize]) {
                TValue temp;
                int index = _buckets[bucket];
                while (index != 0) {
                    if (hash == _entries[index >> _shift][index & _shiftMask].Hash &&
                        _comparer.Equals(key, _entries[index >> _shift][index & _shiftMask].Key)) {
                        temp = _entries[index >> _shift][index & _shiftMask].Value;
                        var original = temp;
                        if (value.UpdateValue(key, ref temp)) {
                            _entries[index >> _shift][index & _shiftMask].Value = temp;

                            if (_ordering == LurchOrder.Modified || _ordering == LurchOrder.Access) {
                                InternalUnlink(index);
                                InternalLink(index);
                            }

                            ItemUpdated?.Invoke(new KeyValuePair<TKey, TValue>(key, original), new KeyValuePair<TKey, TValue>(key, temp));

                            added = -1;
                            return InsertResult.Updated;
                        }

                        added = -1;
                        return InsertResult.Exists;
                    }
                    index = _entries[index >> _shift][index & _shiftMask].Link;
                }
                if (value.CreateValue(key, out temp)) {
#pragma warning disable 612,618
                    index = AllocSlot();
#pragma warning restore 612,618
                    _entries[index >> _shift][index & _shiftMask].Hash = hash;
                    _entries[index >> _shift][index & _shiftMask].Key = key;
                    _entries[index >> _shift][index & _shiftMask].Value = temp;
                    _entries[index >> _shift][index & _shiftMask].Link = _buckets[bucket];
                    _buckets[bucket] = index;

                    added = Interlocked.Increment(ref _count);
                    if (_ordering != LurchOrder.None)
                        InternalLink(index);

                    ItemAdded?.Invoke(new KeyValuePair<TKey, TValue>(key, temp));

                    return InsertResult.Inserted;
                }
            }

            added = -1;
            return InsertResult.NotFound;
        }

        bool Delete<T>(TKey key, ref T value) where T : IRemoveValue<TKey, TValue> {
            if (_entries == null)
                throw new ObjectDisposedException(GetType().Name);

            int hash = _comparer.GetHashCode(key) & int.MaxValue;
            int bucket = hash % _hsize;
            lock (_locks[bucket % _lsize]) {
                int prev = 0;
                int index = _buckets[bucket];
                while (index != 0) {
                    if (hash == _entries[index >> _shift][index & _shiftMask].Hash &&
                        _comparer.Equals(key, _entries[index >> _shift][index & _shiftMask].Key)) {
                        TValue temp = _entries[index >> _shift][index & _shiftMask].Value;

                        if (value.RemoveValue(key, temp)) {
                            int next = _entries[index >> _shift][index & _shiftMask].Link;
                            if (prev == 0)
                                _buckets[bucket] = next;
                            else
                                _entries[prev >> _shift][prev & _shiftMask].Link = next;

                            Interlocked.Decrement(ref _count);
                            if (_ordering != LurchOrder.None)
                                InternalUnlink(index);
                            FreeSlot(ref index, Interlocked.Increment(ref _freeVersion));

                            ItemRemoved?.Invoke(new KeyValuePair<TKey, TValue>(key, temp));

                            return true;
                        }
                        return false;
                    }

                    prev = index;
                    index = _entries[index >> _shift][index & _shiftMask].Link;
                }
            }
            return false;
        }

        void InternalLink(int index) {
            Interlocked.Exchange(ref _entries[index >> _shift][index & _shiftMask].Prev, 0);
            Interlocked.Exchange(ref _entries[index >> _shift][index & _shiftMask].Next, ~0);
            int next = Interlocked.Exchange(ref _entries[0][0].Next, index);
            if (next < 0)
                throw new LurchCorruptionException();

            while (0 != Interlocked.CompareExchange(ref _entries[next >> _shift][next & _shiftMask].Prev, index, 0)) { }

            Interlocked.Exchange(ref _entries[index >> _shift][index & _shiftMask].Next, next);
        }

        void InternalUnlink(int index) {
            while (true) {
                int cmp;
                int prev = _entries[index >> _shift][index & _shiftMask].Prev;
                while (prev >= 0 && prev != (cmp = Interlocked.CompareExchange(
                            ref _entries[index >> _shift][index & _shiftMask].Prev, ~prev, prev)))
                    prev = cmp;
                if (prev < 0)
                    throw new LurchCorruptionException();

                int next = _entries[index >> _shift][index & _shiftMask].Next;
                while (next >= 0 && next != (cmp = Interlocked.CompareExchange(
                            ref _entries[index >> _shift][index & _shiftMask].Next, ~next, next)))
                    next = cmp;
                if (next < 0)
                    throw new LurchCorruptionException();

                if ((Interlocked.CompareExchange(
                        ref _entries[prev >> _shift][prev & _shiftMask].Next, next, index) == index)) {
                    while (Interlocked.CompareExchange(
                               ref _entries[next >> _shift][next & _shiftMask].Prev, prev, index) != index) { }
                    return;
                }

                if (~next != Interlocked.CompareExchange(
                        ref _entries[index >> _shift][index & _shiftMask].Next, next, ~next))
                    throw new LurchCorruptionException();
                if (~prev != Interlocked.CompareExchange(
                        ref _entries[index >> _shift][index & _shiftMask].Prev, prev, ~prev))
                    throw new LurchCorruptionException();
            }
        }

        [Obsolete("ignore for testing.")]
        int AllocSlot() {
            while (true) {
                int allocated = _entries.Length * _allocSize;
                var previous = _entries;

                while (_count + OverAlloc < allocated || _used < allocated) {
                    int next;
                    if (_count + FreeSlots < _used) {
                        int freeSlotIndex = Interlocked.Increment(ref _allocNext);
                        int slot = (freeSlotIndex & int.MaxValue) % FreeSlots;
                        next = Interlocked.Exchange(ref _free[slot].Head, 0);
                        if (next != 0) {
                            int nextFree = _entries[next >> _shift][next & _shiftMask].Link;
                            if (nextFree == 0) {
                                Interlocked.Exchange(ref _free[slot].Head, next);
                            } else {
                                Interlocked.Exchange(ref _free[slot].Head, nextFree);
                                return next;
                            }
                        }
                    }

                    next = _used;
                    if (next < allocated) {
                        int alloc = Interlocked.CompareExchange(ref _used, next + 1, next);
                        if (alloc == next) {
                            return next;
                        }
                    }
                }

                lock (this) {
                    //time to grow...
                    if (ReferenceEquals(_entries, previous)) {
                        Entry[][] arentries = new Entry[_entries.Length + 1][];
                        _entries.CopyTo(arentries, 0);
                        arentries[arentries.Length - 1] = new Entry[_allocSize];

                        Interlocked.CompareExchange(ref _entries, arentries, previous);
                    }
                }
            }
        }

        void FreeSlot(ref int index, int ver) {
            _entries[index >> _shift][index & _shiftMask].Key = default(TKey);
            _entries[index >> _shift][index & _shiftMask].Value = default(TValue);
            Interlocked.Exchange(ref _entries[index >> _shift][index & _shiftMask].Link, 0);

            int slot = (ver & int.MaxValue) % FreeSlots;
            int prev = Interlocked.Exchange(ref _free[slot].Tail, index);

            if (prev <= 0 || 0 != Interlocked.CompareExchange(ref _entries[prev >> _shift][prev & _shiftMask].Link, index, 0)) {
                throw new LurchCorruptionException();
            }
        }

        #endregion

        #region Internal Structures

        struct FreeList
        {
            public int Head;
            public int Tail;
        }

        struct Entry
        {
            public int Prev, Next; // insertion/access sequence ordering
            public int Link;
            public int Hash; // hash value of entry's Key
            public TKey Key; // key of entry
            public TValue Value; // value of entry
        }

        struct EnumState
        {
            private object _locked;
            public int Bucket, Current, Next;
            public void Init() {
                Bucket = -1;
                Current = 0;
                Next = 0;
                _locked = null;
            }

            public void Unlock() {
                if (_locked != null) {
                    Monitor.Exit(_locked);
                    _locked = null;
                }
            }

            public void Lock(object lck) {
                if (_locked != null)
                    Monitor.Exit(_locked);
                Monitor.Enter(_locked = lck);
            }
        }

        struct DelInfo : IRemoveValue<TKey, TValue>
        {
            public TValue Value;
            readonly bool _hasTestValue;
            readonly TValue _testValue;
            public KeyValuePredicate<TKey, TValue> Condition;

            public DelInfo(TValue expected) {
                Value = default(TValue);
                _testValue = expected;
                _hasTestValue = true;
                Condition = null;
            }

            public bool RemoveValue(TKey key, TValue value) {
                Value = value;

                if (_hasTestValue && !EqualityComparer<TValue>.Default.Equals(_testValue, value))
                    return false;
                if (Condition != null && !Condition(key, value))
                    return false;

                return true;
            }
        }

        struct AddInfo : ICreateOrUpdateValue<TKey, TValue>
        {
            public bool CanUpdate;
            public TValue Value;
            public bool CreateValue(TKey key, out TValue value) {
                value = Value;
                return true;
            }

            public bool UpdateValue(TKey key, ref TValue value) {
                if (!CanUpdate) {
                    Value = value;
                    return false;
                }

                value = Value;
                return true;
            }
        }

        struct Add2Info : ICreateOrUpdateValue<TKey, TValue>
        {
            readonly bool _hasAddValue;
            readonly TValue _addValue;
            public TValue Value;
            public Func<TKey, TValue> Create;
            public KeyValueUpdate<TKey, TValue> Update;

            public Add2Info(TValue addValue) : this() {
                _hasAddValue = true;
                _addValue = addValue;
            }

            public bool CreateValue(TKey key, out TValue value) {
                if (_hasAddValue) {
                    value = Value = _addValue;
                    return true;
                }
                if (Create != null) {
                    value = Value = Create(key);
                    return true;
                }
                value = Value = default(TValue);
                return false;
            }

            public bool UpdateValue(TKey key, ref TValue value) {
                if (Update == null) {
                    Value = value;
                    return false;
                }

                value = Value = Update(key, value);
                return true;
            }
        }

        struct UpdateInfo : ICreateOrUpdateValue<TKey, TValue>
        {
            public TValue Value;
            readonly bool _hasTestValue;
            readonly TValue _testValue;

            public UpdateInfo(TValue expected) {
                Value = default(TValue);
                _testValue = expected;
                _hasTestValue = true;
            }

            bool ICreateValue<TKey, TValue>.CreateValue(TKey key, out TValue value) {
                value = default(TValue);
                return false;
            }
            public bool UpdateValue(TKey key, ref TValue value) {
                if (_hasTestValue && !EqualityComparer<TValue>.Default.Equals(_testValue, value))
                    return false;

                value = Value;
                return true;
            }
        }
        #endregion
    }

    #region Delegates

    /// <summary> Provides a delegate that performs an atomic update of a key/value pair </summary>
    public delegate TValue KeyValueUpdate<TKey, TValue>(TKey key, TValue original);

    /// <summary> Provides a delegate that performs a test on key/value pair </summary>
    public delegate bool KeyValuePredicate<TKey, TValue>(TKey key, TValue original);

    #endregion 
}