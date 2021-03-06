﻿/***
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
