using System;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Util.System;
using Fornax.Net.Util.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fornax.Net.Tests.Analysis
{
    [TestClass]
    public class TokenizerTests
    {
        private static string Text = @" http://news.google.com The Project Gutenberg EBook of The Time Machine -End of whiteSpace tokenization
i am the one you want to know how to , i am the 45.6 H.G. wells of rackaracka@youtube.emu, oh lotd vs GOT fags 6, $reef in tug
$89 \users\koudura. 
";


        [TestMethod]
        public void WhiteSpaceTokenizerTest() {

            var ws_tokenizer = new WhitespaceTokenizer(Text);
            while (ws_tokenizer.HasMoreTokens()) {
                var token = ws_tokenizer.CurrentToken;
                Console.WriteLine(token);
            }

            Console.WriteLine("\n----------End of whiteSpace tokenization.------------\n");

            Assert.IsNotNull(ws_tokenizer, "ws_tokenizer as turned out to be bugged");
            var tokenStream = ws_tokenizer.GetTokens();
            while (tokenStream.MoveNext()) {
                var curr = tokenStream.Current;
                Console.WriteLine($"Value_[{curr.ToString()}] , Type_[{curr.Type}], Index_[{curr.Start}]");
            }
            Assert.IsInstanceOfType(tokenStream, typeof(TokenStream));
        }


        [TestMethod]
        public void CharTokenizerTest() {
            var ch_tokenizer = new CharTokenizer(Text);
            while (ch_tokenizer.HasMoreTokens()) {
                var token = ch_tokenizer.CurrentToken;
                Console.WriteLine(token);
            }

            Console.WriteLine("\n----------End of char tokenization.------------\n");

            Assert.IsNotNull(ch_tokenizer, "ws_tokenizer as turned out to be bugged");
            var tokenStream = ch_tokenizer.GetTokens();
            while (tokenStream.MoveNext()) {
                var curr = tokenStream.Current;
                Console.WriteLine($"Value_[{curr.ToString()}] , Type_[{curr.Type}], Index_[{curr.Start}]");
            }
            Assert.IsInstanceOfType(tokenStream, typeof(TokenStream));
        }

        [TestMethod]
        public void NumberTokenizerTest() {
            var num_tokenizer = new NumericTokenizer(Text);
            while (num_tokenizer.HasMoreTokens()) {
                var token = num_tokenizer.CurrentToken;
                Console.WriteLine(token);
            }

            Console.WriteLine("\n----------End of num tokenization.------------\n");

            Assert.IsNotNull(num_tokenizer, "ws_tokenizer as turned out to be bugged");
            var tokenStream = num_tokenizer.GetTokens();
            while (tokenStream.MoveNext()) {
                var curr = tokenStream.Current;
                Console.WriteLine($"Value_[{curr.ToString()}] , Type_[{curr.Type}], Index_[{curr.Start}]");
            }
            Assert.IsInstanceOfType(tokenStream, typeof(TokenStream));
        }

        [TestMethod]
        public void BiasTokenizerTest() {
            Tokenizer tokenizer = null;

            var config = ConfigFactory.GetConfiguration("bias0",FetchAttribute.Weak, CachingMode.Static, FornaxLanguage.English, tokenizer, FileFormat.Txt);
            config.Tokenizer = new BiasTokenizer(Text);
            config.Save();

            var tk = config.Tokenizer;
            while (tk.HasMoreTokens()) {
                var token = tk.CurrentToken;
                Console.WriteLine(token);
            }
            Console.WriteLine("\n----------End of context-sensitive tokenization.------------\n");
            Assert.IsNotNull(tk, "ws_tokenizer as turned out to be bugged");
            var tokenStream = tk.GetTokens();
            while (tokenStream.MoveNext()) {
                var curr = tokenStream.Current;
                Console.WriteLine($"Value_[{curr.ToString()}] , Type_[{curr.Type}], Index_[{curr.Start}]");
            }
            Assert.IsInstanceOfType(tokenStream, typeof(TokenStream));
        }

        [TestMethod]
        public void FieldTokenizerTest()
        {
            Tokenizer tokenizer = null;

            var config = ConfigFactory.GetConfiguration("fieldTokenizerTest", FetchAttribute.Weak, CachingMode.Static, FornaxLanguage.English, tokenizer, FileFormat.Txt);
            config.Tokenizer = new PerFieldTokenizer(Text);
            config.Save();

            var tk = config.Tokenizer;
            while (tk.HasMoreTokens())
            {
                var token = tk.CurrentToken;
                Console.WriteLine(token);
            }
            Console.WriteLine("\n----------End of context-sensitive tokenization.------------\n");
            Assert.IsNotNull(tk, "ws_tokenizer as turned out to be bugged");
            var tokenStream = tk.GetTokens();
            while (tokenStream.MoveNext())
            {
                var curr = tokenStream.Current;
                Console.WriteLine($"Value_[{curr.ToString()}] , Type_[{curr.Type}], Index_[{curr.Start}]");
            }
            Assert.IsInstanceOfType(tokenStream, typeof(TokenStream));
        }
    }
}
