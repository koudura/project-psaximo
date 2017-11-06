using Fornax.Net.Index.Storage;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Index;
using Fornax.Net.Index.IO;
using Fornax.Net.Search;
using Fornax.Net.Util.IO.Readers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fornax.Net.Util.System;
using System.IO;
using System;

namespace Fornax.Net.Tests
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod]
        public void FreeText()
        {
            var lang = FornaxLanguage.English;
            var tokenizer = new PerFieldTokenizer();

            var config = ConfigFactory.GetConfiguration("impossiblee", FetchAttribute.Weak, CachingMode.Dynamic, lang, tokenizer, new FileFormat[] { FileFormat.Txt });




            FreeTextQuery ftq = new FreeTextQuery("kinda", config);

            string loc = @"..\TestRepo";
            var dir = new DirectoryInfo(loc);
            var repo = Repository.Create(dir, RepositoryType.Local, config, Extractor.Default);

            InvertedFile file = new InvertedFile();

            IndexFactory.Add(repo, file);
            IndexFactory.Save(config, file, repo);

            IndexSearcher searcher = new IndexSearcher(ftq, file, repo);
            var res = searcher.GetResults(SortBy.Relevance);

            foreach (var item in res)
            {
                Console.WriteLine(item.Document.Name);
            }

        }

        [TestMethod]
        public void fromHistory()
        {
            double st = Environment.TickCount;
            string sampleId = "QueryTest1";
            var config = ConfigFactory.OpenConfiguration(sampleId);
            FreeTextQuery ftq = new FreeTextQuery("retrieval", config);

            var load = IndexFactory.Load(sampleId);

           
            IndexSearcher searcher = new IndexSearcher(ftq, load.Index, load.Repository);
            var res = searcher.GetResults(SortBy.Relevance);
            double end = Environment.TickCount;
            double time = (end - st) / 1000; 

            Console.WriteLine($"operation went down in {time}secs");

            foreach (var item in res)
            {
                Console.WriteLine(item.Document.Name);
            }

        }
    }
}
