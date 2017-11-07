using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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

            var config = ConfigFactory.GetConfiguration("JpgRepo",FetchAttribute.Weak, CachingMode.Dynamic, lang, tokenizer, new FileFormat[] { FileFormat.Txt, FileFormat.Pdf , FileFormat.Jpg});
            var repo = Repository.Create(dir, RepositoryType.Local, config, Extractor.Default);

            InvertedFile file = new InvertedFile();
            
            IndexFactory.Add(repo, file);
            IndexFactory.Save(config, file, repo);
        }

        [TestMethod]
        public void FSRepositoryLoadTest1()
        {
            string sampleId = "JpgRepo";
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
            var snips = load.Repository.Snippets;
            var corp = load.Repository.Corpus;

            foreach (var doc in corp)
            {
                var id = doc.Key;
                System.Console.WriteLine($"{id} :  [{Path.GetExtension(doc.Value)}]_{doc.Value}\n");
            }
        }

        [TestMethod]
        public void ConfigRepoTest() {
            var configIds = Directory.EnumerateDirectories(@"C:\Users\Koudura Mazou\AppData\Roaming\Fornax","User[*].config", SearchOption.TopDirectoryOnly);
            foreach (var item in configIds)
            {
                var id = item.Substring(item.IndexOf("[", item.Length - item.IndexOf("]")));
                System.Console.WriteLine(id);
            }
            System.Console.WriteLine("\n\n");
            IList<string> ids = new List<string>();
            foreach (var item in configIds)
            {
                if (!item.Contains("(clone)"))
                {
                    var start = item.IndexOf("[");
                    var end = item.LastIndexOf("]");
                    var id = new StringBuilder();
                    for (int i = start + 1; i < end; i++)
                    {
                        id.Append(item[i]);
                    }
                    System.Console.WriteLine(id.ToString());
                    ids.Add(id.ToString().Trim());
                }

            }


            System.Console.WriteLine(Path.GetFileName(@"C:\Users\Koudura Mazou\AppData\Roaming\Fornax"));

        }
    }
}
