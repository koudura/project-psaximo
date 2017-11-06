
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Fornax.Net.Util.Collections;

namespace Fornax.Net.Search
{
    [Serializable]
    public class HitList : ICloneable, IEnumerable, IComparer<KeyValuePair<ulong, double>>, IComparer<DocResult>
    {
        private IDictionary<ulong, double> hits;

        public HitList()
        {
            hits = new Dictionary<ulong, double>();
        }

        private HitList(IDictionary<ulong, double> _hits)
        {
            hits = _hits;
        }

        internal double this[ulong key] { get => hits[key]; set => hits[key] = value; }

        internal ICollection<ulong> Keys => hits.Keys;

        internal ICollection<double> Values => hits.Values;

        public int Count => hits.Count;

        internal bool IsReadOnly => hits.IsReadOnly;

        internal void Add(ulong key, double value)
        {
            hits.Add(key, value);
        }

        internal void Add(KeyValuePair<ulong, double> item)
        {
            hits.Add(item);
        }

        internal void Clear()
        {
            hits.Clear();
        }

        internal bool Contains(KeyValuePair<ulong, double> item)
        {
            return hits.Contains(item);
        }

        internal bool ContainsKey(ulong key)
        {
            return hits.ContainsKey(key);
        }

        internal void CopyTo(KeyValuePair<ulong, double>[] array, int arrayIndex)
        {
            hits.CopyTo(array, arrayIndex);
        }

        internal bool Remove(ulong key)
        {
            return hits.Remove(key);
        }

        internal bool Remove(KeyValuePair<ulong, double> item)
        {
            return hits.Remove(item);
        }

        internal bool TryGetValue(ulong key, out double value)
        {
            return hits.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return hits.GetEnumerator();
        }

        #region public non idict members
        public object Clone()
        {
            return new HitList(hits);
        }

        public IEnumerator<KeyValuePair<ulong, double>> GetEnumerator()
        {
            return hits.GetEnumerator();
        }

        public int Compare(KeyValuePair<ulong, double> x, KeyValuePair<ulong, double> y)
        {
            if (x.Value > y.Value) return -1;
            if (y.Value > x.Value) return 1;
            return 0;
        }

        public override string ToString()
        {
            return Collections.ToString(hits);
        }

        public override bool Equals(object obj)
        {
            if (obj is HitList)
            {
                return Collections.Equals(hits, ((HitList)(obj)).hits);
            }
            return obj == this;
        }

        public override int GetHashCode()
        {
            return Collections.GetHashCode(hits);
        }

        public int Compare(DocResult x, DocResult y)
        {
            if (x.Score > y.Score) return -1;
            if (y.Score > x.Score) return 1;
            return 0;
        }
        #endregion
    }
}
