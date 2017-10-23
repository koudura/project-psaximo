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
    /// A trie data structure holding a bufffer of characters.
    /// i.e strings are inserted by character precedence per text-input.
    /// Inputs include [character] array and text as [string].
    /// </summary>
    /// <seealso cref="ITrie{T}" />
    [Serializable, ProtoContract]
    [Progress("BufferTrie", true, Documented = true, Tested = true)]
    public class BufferTrie : ITrie<char>
    {
        private Node<char> root;

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        [ProtoMember(1)]
        internal virtual Node<char> Root => root;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferTrie"/> class.
        /// </summary>
        public BufferTrie() => root = new Node<char>();

        /// <summary>
        /// Inserts the specified buffer to the Trie.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="ArgumentNullException">buffer</exception>
        public void Insert(char[] buffer) {
            if (buffer == null) {
                throw new ArgumentNullException(nameof(buffer));
            }

            Node<char> current = root;
            foreach (var @char in buffer) {
                if (!current.ContainsKey(@char))
                    current.Add(@char, new Node<char>());
                current = current[@char];
            }
            current.EOS = true;
        }

        /// <summary>
        /// Inserts the specified word/text into the buffer Trie.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <exception cref="ArgumentNullException">word</exception>
        public void Insert(string word) {
            if (word == null) {
                throw new ArgumentNullException(nameof(word));
            }

            Insert(word.Trim().ToCharArray());
        }

        /// <summary>
        /// Searches for the specified buffer of characters in this
        /// Buffer trie.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>true, if buffer is found ,otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">buffer</exception>
        public bool Search(char[] buffer) {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            Node<char> current = root;
            foreach (var @char in buffer) {
                if (!current.ContainsKey(@char)) {
                    return false;
                }
                current = current[@char];
            }
            return current.EOS;
        }

        /// <summary>
        /// Searches the specified word/text in this Buffer trie.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>true, if word/text is found ,otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">word</exception>
        public bool Search(string word) {
            if (word == null) {
                throw new ArgumentNullException(nameof(word));
            }
            return Search(word.Trim().ToCharArray());
        }

        /// <summary>
        /// Deletes the specified buffer of characters from this Buffer trie.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>true, if deletion was successful, otherwise, false</returns>
        /// <exception cref="ArgumentNullException">buffer</exception>
        public bool Delete(char[] buffer) {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            return Delete(root, buffer, 0);
        }

        /// <summary>
        /// Deletes the specified word/ text from this specified Buffer-trie.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>true, if deletion was successful, otherwise, false.</returns>
        public bool Delete(string word) {
            return Delete(word.ToCharArray());
        }

        private bool Delete(Node<char> current, char[] word, int offset) {
            if (offset == word.Length) {
                if (!current.EOS) return false;

                current.EOS = false;
                return current.Count == 0;
            }
            if (!current.ContainsKey(word[offset])) return false;

            bool isDel = Delete(current[word[offset]], word, offset + 1);
            if (isDel) {
                current.Remove(word[offset]);
                return current.Count == 0;
            }
            return false;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance of buffer trie.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance of buffer trie.
        /// </returns>
        public override string ToString() {
            return Root.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this buffertrie.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this buffertrie; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj is BufferTrie) {
                var trie = obj as BufferTrie;
                return Root.Equals(trie.Root);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance of buffer-trie.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return Root.GetHashCode();
        }
    }
}
