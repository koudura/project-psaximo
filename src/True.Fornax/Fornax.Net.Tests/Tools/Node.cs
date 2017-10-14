using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Collection = Fornax.Net.Util.Collections.Collections;

namespace Fornax.Net.Tests.Tools
{
    [Serializable]
    internal sealed class Node<T> : IDictionary<T,Node<T>>
    {

        internal IDictionary<T, Node<T>> children;
        private bool eos;

        internal bool EOS { get { return eos;} set { eos = value; } }

        public ICollection<T> Keys => this.children.Keys;

        public ICollection<Node<T>> Values => this.children.Values;

        public int Count => this.children.Count;

        public bool IsReadOnly => this.children.IsReadOnly;

        public Node<T> this[T key] { get => this.children[key]; set => this.children[key] = value; }

        internal Node() {
            this.children = new Dictionary<T, Node<T>>();
            this.eos = false;
        }

        public bool ContainsKey(T key) {
            return this.children.ContainsKey(key);
        }

        public void Add(T key, Node<T> value) {
            this.children.Add(key, value);
        }

        public bool Remove(T key) {
            return this.children.Remove(key);
        }

        public bool TryGetValue(T key, out Node<T> value) {
            return this.children.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<T, Node<T>> item) {
            this.children.Add(item);
        }

        public void Clear() {
            this.children.Clear();
        }

        public bool Contains(KeyValuePair<T, Node<T>> item) {
            return this.children.Contains(item);
        }

        public void CopyTo(KeyValuePair<T, Node<T>>[] array, int arrayIndex) {
            this.children.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<T, Node<T>> item) {
            return this.children.Remove(item);
        }

        public IEnumerator<KeyValuePair<T, Node<T>>> GetEnumerator() {
            return this.children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.children.GetEnumerator();
        }

        public override string ToString() {
            return Collection.ToString(this.children.ToList());
        }

        public override bool Equals(object obj) {
            if (obj is Node<T>) {
                var noden = obj as Node<T>;
                return Collection.Equals(children, noden.children);
            }
            return false;
        }

        public override int GetHashCode() {
            return Collection.GetHashCode(children);
        }
    }
}
