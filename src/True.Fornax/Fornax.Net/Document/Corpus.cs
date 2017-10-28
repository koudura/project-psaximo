using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Document
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IDictionary{System.UInt64, System.String}" />
    /// <seealso cref="java.io.Serializable.__Interface" />
    [Serializable]
    public class Corpus : IDictionary<ulong, string>, java.io.Serializable.__Interface 
    {

        private readonly IDictionary<ulong, string> corpus = new SortedList<ulong, string>();

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string this[ulong key] { get => corpus[key]; set => corpus[key] = value; }


        public ICollection<ulong> Keys => corpus.Keys;

        public ICollection<string> Values => corpus.Values;

        public int Count => corpus.Count;

        public bool IsReadOnly => corpus.IsReadOnly;

        public void Add(ulong key, string value) {
            corpus.Add(key, value);
        }

        public void Add(KeyValuePair<ulong, string> item) {
            corpus.Add(item);
        }

        public void Clear() {
            corpus.Clear();
        }

        public bool Contains(KeyValuePair<ulong, string> item) {
            return corpus.Contains(item);
        }

        public bool ContainsKey(ulong key) {
            return corpus.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<ulong, string>[] array, int arrayIndex) {
            corpus.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<ulong, string>> GetEnumerator() {
            return corpus.GetEnumerator();
        }

        public bool Remove(ulong key) {
            return corpus.Remove(key);
        }

        public bool Remove(KeyValuePair<ulong, string> item) {
            return corpus.Remove(item);
        }

        public bool TryGetValue(ulong key, out string value) {
            return corpus.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return corpus.GetEnumerator();
        }
    }
}
