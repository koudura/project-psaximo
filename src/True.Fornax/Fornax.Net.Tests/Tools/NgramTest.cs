using System;
using Fornax.Net.Analysis.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fornax.Net.Tests.Tools
{
    [TestClass]
    public class NgramTest
    {
        [TestMethod]
        public void GetBoundedNgram()
        {
            string sentence = "one above all is here";
            Ngram ngram = new Ngram(sentence, 3, NgramModel.Word, true);
            foreach (var gram in ngram.Grams)
            {
                Console.WriteLine(gram);
            }
            Assert.AreEqual(3, (int)ngram.Size);
        }

        [TestMethod]
        public void GetOverSizedGram()
        {
            string sentence = "he is a boy";
            Ngram ngram = new Ngram(sentence, 6, NgramModel.Word);
            foreach (var gram in ngram.Grams)
            {
                Console.WriteLine(gram);
            }
            Assert.AreNotEqual(6, (int)ngram.Size);
        }

        [TestMethod]
        public void GetUnboundedKGram()
        {
            string sentence = "one above all is here";
            Ngram ngram = new Ngram(sentence, 2, NgramModel.Character);
            foreach (var gram in ngram.Grams)
            {
                Console.WriteLine("{0},{1}", gram, gram.Length);
            }
            Assert.AreEqual(2, (int)ngram.Size);
        }

        [TestMethod]
        public void GetBoundedKgram()
        {
            string sentence = "one above all is here";
            Ngram ngram = new Ngram(sentence, 4, NgramModel.Character, true);
            foreach (var gram in ngram.Grams)
            {
                Console.WriteLine(gram);
            }
            Assert.AreEqual(4,(int)ngram.Size);

        }
    }
}
