using System;
using System.IO;
using Fornax.Net.Analysis.Tools;
using Fornax.Net.Index.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fornax.Net.Tests.Analysis.Tools
{
    [TestClass]
    public class SynonymsTest
    {
        [TestMethod]
        public void GetSynonyms() {
            string word1 = "good";
            string word2 = "bad";
            
            
            string word5 = "good";
            printSynonyms(word5);

        }

        [TestMethod]
        public void NaiveSynset() {
            string word4 = "naive";
            printSynonyms(word4);
        }

        [TestMethod]
        public void NonwordSynset() {
            string word3 = "habuto";
            printSynonyms(word3);
        }

        
        public void  printSynonyms(string word) {
            var synonym1 = SynsetFactory.GetSynset(word,15);

            foreach (var item in synonym1.Synonyms) {
                Console.WriteLine(item);
            }
            Assert.IsInstanceOfType(synonym1, typeof(Synset));
        }

        [TestMethod]
        public void BuildSynsetIndex() {
            FileInfo file = new FileInfo(@"..\..\..\..\..\res\Wordnet\prolog\wn_s.pl");
            SynsetFactory factory = new SynsetFactory(file);
            SynsetIndex index = factory.Index;

            var synonyms = SynsetFactory.GetSynset("awesome", index,1);
            foreach (var item in synonyms.Synonyms) {
                Console.WriteLine(item);
            }
            Assert.IsInstanceOfType(synonyms, typeof(Synset));
        }


    }
}
