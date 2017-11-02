using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fornax.Net.Util.IO.Compression;
using System.IO;
using Fornax.Net.Util.System;
using Fornax.Net.Util.Text;
using Fornax.Net.Tests.Tools;
using Fornax.Net.Util.IO.Writers;
using System.Threading.Tasks;

namespace Fornax.Net.Tests
{
    /// <summary>
    /// Summary description for LZ4Tests
    /// </summary>
    [TestClass]
    public class LZ4Tests
    {
        public LZ4Tests() {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        static FileInfo trie_file = new FileInfo(@"..\en_trie.tnx");
        static FileInfo trie_Zip = new FileInfo(@"..\en_trie.ztnx");
        BufferTrie trie = new BufferTrie();

       // [TestMethod]
        public void SerializeCompressTrie() {
            FornaxLanguage lang = FornaxLanguage.English;
            Vocabulary vocab = ConfigFactory.GetVocabulary(lang);

            foreach (var word in vocab.Dictionary) {
                trie.Insert(word);
            }
            Assert.IsNotNull(trie);

           // FornaxWriter.Write(trie, trie_file);
        //     LZ4Compressor.Compress(trie_file, trie_Zip);
            
        }

        [TestMethod]
        public void DecompressReadTrie() {
            Task.WaitAll(GetTrie());
            Assert.IsNotNull(trie);

            Console.WriteLine(trie.Search("abasements"));
            Console.WriteLine(trie.Search("shipplane"));
            Console.WriteLine(trie.Search("zwanziger"));
            Assert.AreEqual(true, trie.Search("hypochaeris"));
        }


        public async Task GetTrie() {
            trie =  await FornaxWriter.BufferReadAsync<BufferTrie>(trie_file);
        }
    }
}