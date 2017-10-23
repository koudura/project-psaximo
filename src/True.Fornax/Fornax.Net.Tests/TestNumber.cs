using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fornax.Net.Util.Numerics;
using Fornax.Net.Analysis.Tools;
using Fornax.Net.Common.Snowball.en;
using Fornax.Net.Common.Snowball.fr;
using Fornax.Net.Util.IO.Readers;
using System.IO;

namespace Fornax.Net.Tests
{
    /// <summary>
    /// Summary description for TestNumber
    /// </summary>
    [TestClass]
    public class TestNumber
    {
        public TestNumber() {



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

        [TestMethod]
        public void TestMethod1() {

            int i = 0b001101011010000;
            int j = i.NumberOfTrailingZeroes();
            Console.WriteLine("testing");
            Assert.AreEqual(4, j);
            
        }

        [TestMethod]
        public void StemTest() {
            string word = "finlands";
            string word1 = "abaisserai";

            FornaxStemmer stemmer = new FornaxStemmer();
            Console.WriteLine(stemmer.StemWord(word));
           FrenchStemmer porter = new FrenchStemmer();
            Console.WriteLine(porter.Stem(word1));

          //  Assert.AreEqual("", stemmer.StemWord(word));
            Assert.AreEqual("abaiss", porter.Stem(word1));
        }

      //  [TestMethod]
        public void BufferTest() {
            string file = @"C:\Users\Kodex Zone\Source\Repos\project-psaximo\res\Box\en\en_stem.txt";
            string relative = @"..\..\..\..\..\res";
            string lib = @"..\..\..\..\..\res\Wordnet\prolog\";
            Directory.Exists(relative);

         //   BufferedReaderWrapper reader = new BufferedReaderWrapper(file);
            Console.WriteLine("Complete");
            // Assert.IsNotNull(reader.GetCon
            //tent());
            Console.WriteLine(Path.GetTempFileName());
       //     var h = new DirectoryInfo(file).
            Assert.AreEqual(true, Directory.Exists(lib));
        }
    }
}
