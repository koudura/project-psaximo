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
using ProtoBuf;

namespace Fornax.Net.Analysis.Tools
{
    /// <summary>
    /// A trie data structure holding a cluster of characters.
    /// i.e strings are inserted as phrases.
    /// Input includes phrase and sentence strings.
    /// </summary>
    /// <seealso cref="ITrie{T}" />
    [Serializable, ProtoContract]
    [Progress("ClusterTrie", true, Documented = true, Tested = true)]
    public class ClusterTrie : ITrie<string>
    {
        private Node<string> root;

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        [ProtoMember(1)]
        internal virtual Node<string> Root => this.root;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterTrie"/> class.
        /// </summary>
        public ClusterTrie() => this.root = new Node<string>();

        /// <summary>
        /// Deletes the specified words/phrase from this specified Cluster-trie.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns>
        /// true, if deletion was successful, otherwise, false.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete(string[] words) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the specified phrase from the Cluster-trie.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns>
        /// true, if deletion was successful, otherwise, false
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete(string phrase) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts the specified words/phrase into this Cluster-trie.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <exception cref="ArgumentNullException">words</exception>
        public void Insert(string[] words) {
            if (words == null) throw new ArgumentNullException(nameof(words));
            Node<string> current = this.root;
            foreach (var word in words) {
                var wordT = word.Trim().ToLower();
                if (!current.ContainsKey(wordT.Trim()))
                    current.Add(wordT, new Node<string>());
                current = current[wordT];
            }
            current.EOS = true;
        }

        /// <summary>
        /// Inserts the specified phrase into the Cluster-trie.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <exception cref="ArgumentNullException">phrase</exception>
        public void Insert(string phrase) {
            if (phrase == null) throw new ArgumentNullException(nameof(phrase));
            Insert(phrase.Split());
        }

        /// <summary>
        /// Searches for the specified words as phrase in the Cluster-trie.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns>true, if word/text is found ,otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">words</exception>
        public bool Search(string[] words) {
            if (words == null) throw new ArgumentNullException(nameof(words));
            Node<string> current = this.root;

            foreach (var word in words) {
                var wordT = word.Trim().ToLower();
                if (!current.ContainsKey(wordT)) {
                    return false;
                }
                current = current[wordT];
            }
            return current.EOS;
        }

        /// <summary>
        /// Searches for the specified phrase in the Cluster-trie.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns>true, if word/text is found ,otherwise, false.</returns>
        public bool Search(string phrase) {
            return Search(phrase.Split());
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance of cluster trie.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance of cluster trie.
        /// </returns>
        public override string ToString() {
            return Root.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this cluster-trie.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this cluster-trie; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj is ClusterTrie) {
                var trie = obj as ClusterTrie;
                return Root.Equals(trie.Root);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance of cluster-trie.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return Root.GetHashCode();
        }

    }
}
