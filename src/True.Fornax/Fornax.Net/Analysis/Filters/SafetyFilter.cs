using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Properties;
using Fornax.Net.Util.Resources;
using Fornax.Net.Util.System;
using Fornax.Net.Util.Text;

namespace Fornax.Net.Analysis.Filters
{
    public class SafetyFilter : Filter
    {
        static FornaxLanguage fornaxLanguage;

        public SafetyFilter(FornaxLanguage language, string text) : base(text) {
            Contract.Requires(language != null);
            fornaxLanguage = language;
        }

        public SafetyFilter(string text) : this(FornaxLanguage.English, text) {

        }

        private static Vocabulary Vocabs => ConfigFactory.GetVocabulary(fornaxLanguage);

        public override IEnumerable<string> Accepts(IEnumerable<string> collection) {
            foreach (var word in collection) {
                if (!IsUnsafeWord(word)) {
                    yield return word;
                }
            }
        }

        public override IEnumerable<string> Accepts(string text, char[] delimiters) {
            var tokenizer = new StringTokenizer(text, new string(delimiters));
            while (tokenizer.HasMoreTokens()) {
                var token = tokenizer.CurrentToken;
                if (!IsUnsafeWord(token)) {
                    yield return token;
                }
            }
        }

        public bool IsUnsafeWord(string word) {
            return Vocabs.BadWords.Contains(word.ToLower());
        }

        public override string Accepts(char[] delimiters) {
            StringBuilder output = new StringBuilder();
            var tokenizer = new StringTokenizer(text, new string(delimiters));
            while (tokenizer.HasMoreTokens()) {
                var token = tokenizer.CurrentToken;
                if (!IsUnsafeWord(token)) {
                    output.Append(token + " ");
                }
            }
            return output.ToString().Trim();
        }

        public override IEnumerable<string> Accepts(IEnumerable<string> collection, FornaxLanguage language) {
            var badWords = ConfigFactory.GetVocabulary(language).BadWords;
            foreach (var item in collection) {
                if (!badWords.Contains(item)) {
                    yield return item;
                }
            }
        }
    }
}
