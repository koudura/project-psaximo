using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fornax.Net.Tests.Tools;
using Fornax.Net.Util.Text;
using Fornax.Net.Util.System;
using Fornax.Net.Util.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace Fornax.Net.Tests
{
    [TestClass]
    public class TrieTests
    {
        [TestMethod]
        public void TrieMethod() {
            //  string ama = "Game of thrones is awesome";


            //  var trie = new ClusterTrie();
            //  trie.Insert(ama);
            //  trie.Insert("my throne");
            //  trie.Insert("are you kidding");
            //  trie.Insert("its all fun and games");

            //  Console.WriteLine(trie.Root.EOS);
            //  Console.WriteLine(trie.ToString());


            //  var bftrie = new BufferTrie();
            ////  bftrie.Insert()

            //  Assert.AreEqual(true, trie.Search(ama));
            var trie_file = new FileInfo(@"..\test_Dict_trie.tnx");
            FornaxLanguage lang = FornaxLanguage.English;
            Vocabulary voc = ConfigFactory.GetVocabulary(lang);

            var trie = new BufferTrie();

            //var tclus = new ClusterTrie();
            var stops = new HashSet<string>(voc.StopWords);

            foreach (var item in voc.Dictionary) {
                Console.WriteLine(item);
                trie.Insert(item);
              //  tclus.Insert(item);
            }
            //Console.WriteLine(trie.ToString());
            Console.WriteLine("------------------------");
            //Console.WriteLine(tclus.ToString());
            Console.WriteLine(trie.Search("sublimable"));
            Console.WriteLine(trie.Search("unconsequential"));

            var eqlist = new EquatableList<string>() { "a", "b", "c", "d", "e" };
            var eqlist2 = new EquatableList<string>() { "e", "d", "c", "b", "a" };
            Console.WriteLine(eqlist == eqlist2);
            Serialize(trie);
        }



   
        public void Serialize<T>( T @object) where T : class{
            //    var lurch = new LurchTable<int, string>(10, LurchOrder.Access);
            string path = @"..\test_Dict_trie.tnx";
            try {
                IFormatter formatter = null;
                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None)) {
                    formatter = new BinaryFormatter();
                    formatter.Serialize(stream, @object);
                    stream.Close();
                }
            } catch (Exception ex) {
                Console.Error.WriteLine($"Failed to Serialize {nameof(@object)} to file.\n :cause: Exception {ex.Source} \n {ex.Message} \n {ex.StackTrace}");
            }

        }

        public T Desialize<T>(FileInfo file) where T : class {
            T @out = null; string path = null;
            try {
                if (file.Exists) path = file.FullName;
                IFormatter formatter = null;
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None)) {
                    formatter = new BinaryFormatter();
                    @out = (T)formatter.Deserialize(stream);
                    stream.Close();
                }
            } catch (Exception ex) {
                Console.Error.WriteLine($"Failed to DeSerialize file:{file.Name} to {nameof(@out)}.\n :cause: Exception {ex.Source} \n {ex.Message} \n {ex.StackTrace}");
            }
            return @out;

        }
    }
}
