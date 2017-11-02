using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fornax.Net.Util.IO;
using Fornax.Net.Util.IO.Readers;
using System.Threading.Tasks;

namespace Fornax.Net.Tests
{
    /// <summary>
    /// Summary description for FornaxReaderTests
    /// </summary>
    [TestClass]
    public class FornaxReaderTests
    {
        public FornaxReaderTests() {
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

        [TestMethod]
        public void TikaExtractHtml() {
            string htmlfile = @"C# Corner _ Looking for Something_.html";
            // FileWrapper wrapp = new FileWrapper(htmlfile);
            FornaxReader reader = new FornaxReader(htmlfile);
            var extracted = reader.TikaRead();
            Console.WriteLine(extracted.Text);
            Console.WriteLine(extracted.ContentType);
        }

        private static string vcf = @"test files\sample.vcf";
      //  private static string pdf = @"test files\Small09.pdf";
        private static string html = @"test files\Compression_Decompression string with C# - Stack Overflow.html";
        private static string slide = @"test files\lecture5-indexconstruction.ppt";
     //   private static string doc_text = @"test files\regularexpressionssupportingdoc.docx";
     //   private static string plain_txt = @"test files\stopwords.txt";
        private static string plain_cs = @"test files\Ranker.cs";
      //  private static string sheet = @"test files\grocery.xls";
        private static string email = @"test files\test-sample-message.eml";
        private static string xml = @"test files\Fornax.Net.xml";

        //[TestMethod]
        public void ToxySlide() {

            FornaxReader reader = new FornaxReader(slide);
            var @out = reader.ToxySlideRead();
            Console.WriteLine(@out.Note);
            Console.WriteLine(@out.Text);

        }

        [TestMethod]
        public void Toxydom() {
            FornaxReader reader = new FornaxReader(html);
            var res = reader.ToxyDomRead();
            Console.WriteLine(res.InnerText);
            Console.WriteLine(res.Name);
            Console.WriteLine(res.NodeString);
        }

        [TestMethod]
        public void Toxyvcf() {
            FornaxReader reader = new FornaxReader(vcf);
            var @out = reader.ToxyVCardRead();
            Console.WriteLine(@out.Attributes);
            Console.WriteLine(@out.Text);
        }

        [TestMethod]
        public void bufferRead() {
            FornaxReader reader = new FornaxReader(plain_cs);
            var @out = reader.BufferRead();
            Console.WriteLine(@out);
        }

        [TestMethod]
        public void ToxyEmail() {
            FornaxReader reader = new FornaxReader(email);
            var @out = reader.ToxyEmailRead();
            Console.WriteLine(@out.Attributes);
            Console.WriteLine(@out.Text);
        }

        [TestMethod]
        public void Xmled() {
            FornaxReader reader = new FornaxReader(xml);
            var @out = reader.XmlRead();
            Console.WriteLine(@out);

        }

       
        public async Task XmlAsync() {
            FornaxReader reader = new FornaxReader(xml);
            var stt = await reader.XmlReadAsync();
            Console.WriteLine(stt);
        }

        [TestMethod]
        public void Xmlasinc() {
            Task.WaitAll(XmlAsync());
        }
    }
}
