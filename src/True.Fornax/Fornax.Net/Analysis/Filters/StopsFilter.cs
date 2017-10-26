﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Util.Resources;
using Fornax.Net.Util.System;
using Fornax.Net.Util.Text;

namespace Fornax.Net.Analysis.Filters
{
    public sealed class StopsFilter : Filter
    {
        private static FornaxLanguage language;

        public StopsFilter(string text) : base(text) {
        }

        public StopsFilter(FornaxLanguage lang, string text) : base(text) {
            Contract.Requires(language != null);
           language = lang; 
                                                                                                                                                     
        }

        private static Vocabulary Vocabs => ConfigFactory.GetVocabulary(language);

        public override IEnumerable<string> Accepts(IEnumerable<string> collection) {
            foreach (var item in collection) {
                if (!IsStop(item)) {
                    yield return item;
                }
            }
        }

        public override IEnumerable<string> Accepts(string text, char[] delimiters) {
            var tokenizer = new StringTokenizer(text, new string(delimiters));
            while (tokenizer.HasMoreTokens()) {
                var token = tokenizer.CurrentToken;
                if (!IsStop(token)) {
                    yield return token;
                }
            }
        }

        private static bool IsStop(string word) {
            return Vocabs.StopWords.Contains(word);
        }

        public override string Accepts(char[] delimiters) {
            StringBuilder output = new StringBuilder();
            var tokenizer = new StringTokenizer(text, new string(delimiters));
            while (tokenizer.HasMoreTokens()) {
                var token = tokenizer.CurrentToken;
                if (!IsStop(token)) {
                    output.Append(token + " ");
                }
            }
            return output.ToString().Trim();
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

        public override IEnumerable<string> Accepts(IEnumerable<string> collection, FornaxLanguage language) {
            var stopWords = ConfigFactory.GetVocabulary(language).StopWords;
            foreach (var item in collection) {
                if (!stopWords.Contains(item)) {
                    yield return item;
                }
            }
        }
    }
}
