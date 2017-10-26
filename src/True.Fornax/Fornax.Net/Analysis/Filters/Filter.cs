

using System.Collections.Generic;
using Fornax.Net.Util.System;

namespace Fornax.Net.Analysis.Filters
{
    public abstract class Filter
    {
        protected string text;

        protected Filter(string text) {
            this.text = text;
        }

        public abstract IEnumerable<string> Accepts(IEnumerable<string> collection);

        public abstract IEnumerable<string> Accepts(string text, char[] delimiters);

        public abstract IEnumerable<string> Accepts(IEnumerable<string> collection, FornaxLanguage language);

        public abstract string Accepts(char[] delimiters);

        public static IEnumerable<string> Accepts(IEnumerable<string> inputCollection, ISet<string> filterSet) {
            foreach (var item in inputCollection) {
                if (!filterSet.Contains(item)) {
                    yield return item;
                }
            }
        }


        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return base.ToString();
        }
    }
}
