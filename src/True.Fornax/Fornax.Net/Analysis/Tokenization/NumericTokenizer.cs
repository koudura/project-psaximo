using System.Collections.Generic;
using System.Text.RegularExpressions;

using Fornax.Net.Util;
using Fornax.Net.Util.Text;
using Cst = Fornax.Net.Util.Constants;


namespace Fornax.Net.Analysis.Tokenization
{
    public sealed class NumericTokenizer : Tokenizer, ITokenizer
    {
        StringTokenizer stringTokenizer;
        readonly string operators = Cst.Num_Brokers;

        public NumericTokenizer(string text) : this(text, false) {
        }

        public NumericTokenizer(string text, bool returnDelim) : base(text, returnDelim) {
            stringTokenizer = new StringTokenizer(text, operators, returnDelim);
        }

        public override object CurrentElement => stringTokenizer.CurrentElement;

        public override string CurrentToken => stringTokenizer.CurrentToken;

        public override int CountTokens() {
            return stringTokenizer.CountTokens();
        }

        public override TokenStream GetTokens() {
            return new TokenStream(Tokenize());
        }

        public override bool HasMoreElements() {
            return stringTokenizer.HasMoreElements();
        }

        public override bool HasMoreTokens() {
            return stringTokenizer.HasMoreTokens();
        }

        IEnumerable<Token> Tokenize() {
            string regex = (returnDelim1) ? @"[\S]+" : $"[^{operators}]+";
            var tokens = Regex.Matches(text, regex, RegexOptions.Compiled);
            foreach (Match exact in tokens) {
                if (exact.Success) {
                    int start = exact.Index; int end = start + exact.Length - 1;
                    yield return new Token(start, end, text);
                }
            }
        }
    }
}
