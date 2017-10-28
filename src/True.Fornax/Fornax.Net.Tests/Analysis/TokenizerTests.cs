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
        private static string Text = @"The Project Gutenberg EBook of The Time Machine, by H.G. (Herbert George) Wells (#1 in our series by
H.G. (Herbert George) Wells ) Bookyards@gmail.com 9 oto-com
Copyright laws are changing all over the world. Be sure to check the copyright laws for your country before
downloading or redistributing this or any other Project Gutenberg eBook.
This header should be the first thing seen when viewing this Project Gutenberg file. Please do not remove it.
Do not change or edit the header without written permission.
Please read the ""legal small print,"" and other information about the eBook 4.5. and Project Gutenberg at the
bottom of this file. Included is important information about your specific rights and restrictions in how the file
may be used.You can also find out about how to make a donation to Project Gutenberg, and how to get
involved 563.0";


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

            var config = ConfigFactory.GetConfiguration(FetchAttribute.Weak, CachingMode.Static, FornaxLanguage.English, tokenizer, FileFormat.Txt);
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
    }
}
