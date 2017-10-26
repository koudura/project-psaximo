

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Fornax.Net.Util.Text;
using Cst = Fornax.Net.Util.Constants;

namespace Fornax.Net.Analysis.Tokenization
{
    public sealed class CharTokenizer : Tokenizer, ITokenizer
    {
        StringTokenizer tokenizer;
        private static string operators = (Cst.GenOp_Brokers + Cst.DocOP_Broker);

        public CharTokenizer(string text) : this(text,false) {
        }

        public CharTokenizer(string text, bool returnDelim1) : this(text,operators.ToCharArray(),returnDelim1) {
        }

        public CharTokenizer(string text, char[] delimiters, bool returnDelim) : base(text, returnDelim) {
            this.text = text;
            operators = new string(delimiters);
            tokenizer = new StringTokenizer(text, operators, returnDelim);
        }

        public override object CurrentElement => tokenizer.CurrentElement;

        public override string CurrentToken => tokenizer.CurrentToken;

        public override int CountTokens() {
            return tokenizer.CountTokens();
        }

        private IEnumerable<Token> Tokenize() {
            string regex = (returnDelim1) ? @"[\S]+" : $"[^{operators}]+";
            var tokens = Regex.Matches(text,regex, RegexOptions.Compiled);
            foreach (Match exact in tokens) {
                if (exact.Success) {
                    int start = exact.Index; int end = start + exact.Length - 1;
                    yield return new Token(start, end, text);
                }
            }
        }

        public override bool HasMoreElements() {
            return tokenizer.HasMoreElements();
        }

        public override bool HasMoreTokens() {
            return tokenizer.HasMoreTokens();
        }

        public override TokenStream GetTokens() {
            return new TokenStream(Tokenize());
        }
    }
}
