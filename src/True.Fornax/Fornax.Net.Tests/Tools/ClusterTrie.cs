using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Tests.Tools
{
    [Serializable]
    public class ClusterTrie : ITrie<string>
    {
        private Node<string> root;
        internal virtual Node<string> Root => this.root;

        public ClusterTrie() => this.root = new Node<string>();

        public bool Delete(string[] words) {
            throw new NotImplementedException();
        }

        public bool Delete(string phrase) {
            throw new NotImplementedException();
        }

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

        public void Insert(string phrase) {
            if (phrase == null) throw new ArgumentNullException(nameof(phrase));
            Insert(phrase.Split());
        }

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

        public bool Search(string phrase) {
            return Search(phrase.Split());
        }

        public override string ToString() {
            return Root.ToString();
        }

        public override bool Equals(object obj) {
            if (obj is ClusterTrie) {
                var trie = obj as ClusterTrie;
                return Root.Equals(trie.Root);
            }
            return false;
        }

        public override int GetHashCode() {
            return Root.GetHashCode();
        }
    }
}
