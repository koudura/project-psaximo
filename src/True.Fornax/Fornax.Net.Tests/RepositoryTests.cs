using System.IO;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Index;
using Fornax.Net.Index.IO;
using Fornax.Net.Index.Storage;
using Fornax.Net.Util.IO.Readers;
using Fornax.Net.Util.System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fornax.Net.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void FSRepositoryCreate()
        {
            string loc = @"..\TestRepo";
            var dir = new DirectoryInfo(loc);


            var lang = FornaxLanguage.English;
            var tokenizer = new PerFieldTokenizer();

            var config = ConfigFactory.GetConfiguration("Copora@Inc",FetchAttribute.Weak, CachingMode.Dynamic, lang, tokenizer, new FileFormat[] { FileFormat.Txt, FileFormat.Pdf });
            var repo = Repository.Create(dir, RepositoryType.Local, config, Extractor.Default);

            InvertedFile file = new InvertedFile();
            
            IndexFactory.Add(repo, file);
            IndexFactory.Save(config, file);
        }

        [TestMethod]
        public void FSRepositoryLoadTest1()
        {
            string sampleId = "Copora@Inc";
            var load = IndexFactory.Load(sampleId);
            foreach (var item in load.Index)
            {
                System.Console.Write($"{item.Key.ToString()} Type[{item.Key.Token.Type}] :[ ");
                foreach (var post in item.Value)
                {
                    System.Console.Write($"{post.Key}, ");
                }
                System.Console.Write("]\n");
            }
            var corp = load.Repository.Corpora;
            foreach (var item in corp)
            {
                System.Console.WriteLine($"{item.Capture} \n: {item.ID} \n {item.Link} \n {item.Terms.Count}");
            }
        }
    }
}
