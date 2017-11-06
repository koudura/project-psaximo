using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fornax.Net.Analysis.Tools;
using Fornax.Net.Index.Common;

namespace corvuslib
{
    public partial class Querier
    {
        public class CorrectWord
        {
            private static GramIndex correcter_index;
            private static string _text;

            public CorrectWord(string text)
            {
                _text = Task.Factory.StartNew(() => GetCorrection(text.ToLower().Trim())).Result;
            }

            private string GetCorrection(string v)
            {
                var correct = GenerationSuggetion(0.6f).ToArray();
                return (correct.Length > 0) ? correct[0] : v;
            }

            public static IEnumerable<string> GenerationSuggetion(float threshold)
            {
                var ngram = new Ngram(_text, 2, NgramModel.Character, true);

                var sub = GramFactory.SubGramIndex(ngram, correcter_index);
                var common = GramFactory.IntersectOf(sub);
                var close = EditFactory.RetrieveCommon(_text, common, threshold);

                foreach (var found in close)
                {
                    yield return found.Key;
                }
            }

            public override string ToString()
            {
                return _text;
            }

            static CorrectWord()
            {
                correcter_index = GramFactory.Default_BiGram;
            }


        }

    }
}
