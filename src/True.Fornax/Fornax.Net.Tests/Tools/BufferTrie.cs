using System;

namespace Fornax.Net.Tests.Tools
{
    [Serializable]
    public class BufferTrie : ITrie<char>
    {
        private Node<char> root;

        internal virtual Node<char> Root => this.root;

        public BufferTrie() => this.root = new Node<char>();

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

        public void Insert(string word) {
            if (word == null) {
                throw new ArgumentNullException(nameof(word));
            }

            Insert(word.ToCharArray());
        }

        public bool Search(char[] buffer) {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        
            Node<char> current = this.root;
            foreach (var @char in buffer) {
                if (!current.ContainsKey(@char)) {
                    return false;
                }
                current = current[@char];
            }
            return current.EOS;
        }

        public bool Search(string word) {
            if (word == null) {
                throw new ArgumentNullException(nameof(word));
            }
            return Search(word.ToCharArray());
        }

        public bool Delete(char[] buffer) {
            if (buffer == null)  throw new ArgumentNullException(nameof(buffer));
            return Delete(this.root, buffer, 0);
        }

        public bool Delete(string word) {
           return Delete(word.ToCharArray());
        }

        private bool Delete(Node<char> current, char[] word, int offset) {
            if( offset == word.Length) {
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

        public override string ToString() {
            return Root.ToString();
        }

        public override bool Equals(object obj) {
            if (obj is BufferTrie) {
                var trie = obj as BufferTrie;
                return Root.Equals(trie.Root);
            }
            return false;
        }

        public override int GetHashCode() {
            return Root.GetHashCode();
        }
    }
}