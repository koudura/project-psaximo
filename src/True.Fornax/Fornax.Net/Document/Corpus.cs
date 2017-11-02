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
    public class Corpus : IEnumerable<KeyValuePair<ulong, string>>, java.io.Serializable.__Interface
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
        internal string this[ulong key] { get => corpus[key]; set => corpus[key] = value; }


        internal ICollection<ulong> Keys => corpus.Keys;

        internal ICollection<string> Values => corpus.Values;

        public int Count => corpus.Count;

        internal bool IsReadOnly => corpus.IsReadOnly;

        internal void Add(ulong key, string value)
        {
            corpus.Add(key, value);
        }

        internal void Add(KeyValuePair<ulong, string> item)
        {
            corpus.Add(item);
        }

        internal void Clear()
        {
            corpus.Clear();
        }

        internal bool Contains(KeyValuePair<ulong, string> item)
        {
            return corpus.Contains(item);
        }

        internal bool ContainsKey(ulong key)
        {
            return corpus.ContainsKey(key);
        }

        internal void CopyTo(KeyValuePair<ulong, string>[] array, int arrayIndex)
        {
            corpus.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<ulong, string>> GetEnumerator()
        {
            return corpus.GetEnumerator();
        }

        internal bool Remove(ulong key)
        {
            return corpus.Remove(key);
        }

        internal bool Remove(KeyValuePair<ulong, string> item)
        {
            return corpus.Remove(item);
        }

        internal bool TryGetValue(ulong key, out string value)
        {
            return corpus.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return corpus.GetEnumerator();
        }
    }
}
