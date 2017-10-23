/***
* Copyright (c) 2017 Koudura Ninci @True.Inc
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*
**/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;

using Fornax.Net.Analysis.Tools;
using Fornax.Net.Util.Resources;
using Fornax.Net.Util.Text;

namespace Fornax.Net.Index.Common
{
    /// <summary>
    /// Synonyms Factory [@owned by fornax.net] for generating and handling of synonyms and inflexion expansions
    /// of a word.
    /// </summary>
    [Progress("SynsetFactory", true, Documented = true, Tested = true)]
    public sealed class SynsetFactory : IDisposable
    {
        static TextWriter error = Console.Error;
        private FileInfo synsFile;
        private SynsetIndex index;
        private static SynsetIndex @default;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynsetFactory"/> class.
        /// That reads the format from a synonym file. 
        /// </summary>
        /// <param name="file">The file.</param>
        /// <exception cref="ArgumentNullException">file</exception>
        public SynsetFactory(FileInfo file) {
            Contract.Requires(file != null && file.Exists);
            synsFile = file ?? throw new ArgumentNullException(nameof(file));
            index = new SynsetIndex();
            Build();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SynsetFactory"/> class.
        /// That reads the format from a synonym file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public SynsetFactory(string filename) : this(new FileInfo(filename)) {
        }

        /// <summary>
        /// Gets the index of the synonyms derived from input synonyms format file. </summary>
        /// <value>
        /// The index of the synonyms.
        /// </value>
        public SynsetIndex Index { get { return index; } private set { index = value; } }

        /// <summary>
        /// Gets the default Fornax.Net Index of synonyms.
        /// </summary>
        /// <value>
        /// The default index.
        /// </value>
        public static SynsetIndex Default { get { return @default; } private set { @default = value; } }

        /// <summary>
        /// Gets the synset of a word from existing index.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="depthOfExpansion">The depth of expansion.</param>
        /// <returns>
        /// A synset for the <paramref name="word" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">word</exception>
        public static Synset GetSynset(string word, uint? depthOfExpansion = null) {
            if (word == null) {
                throw new ArgumentNullException(nameof(word));
            }
            return new Synset(word, depthOfExpansion);
        }

        /// <summary>
        /// Gets the set of synonyms for a word, from a specified index.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="indice">The indice.</param>
        /// <param name="depthOfExpansion">The depth of expansion.</param>
        /// <returns>
        /// A synset for the <paramref name="word" /> through <paramref name="indice" />.
        /// </returns>
        public static Synset GetSynset(string word, SynsetIndex indice, uint? depthOfExpansion = null) {
            return new Synset(word, indice, depthOfExpansion);
        }

        /// <summary>
        /// A New Instance of Synset Factory derived from file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>A new instance of the synsetFactory.</returns>
        public static SynsetFactory New(FileInfo file) { return new SynsetFactory(file); }

        /// <summary>
        /// Merges two occurrences of synonym index into one, using the union equality 
        /// comparer.
        /// </summary>
        /// <param name="index1">The index1.</param>
        /// <param name="index2">The index2.</param>
        /// <returns>A union of two synonyms index, index1 + index2</returns>
        public static SynsetIndex Merge(SynsetIndex index1, SynsetIndex index2) {
            return (SynsetIndex)(index1.Union(index2));
        }

        /// <summary>
        /// Intersects the specified index1 with index2.
        /// </summary>
        /// <param name="index1">The index1.</param>
        /// <param name="index2">The index2.</param>
        /// <returns>A synset index of at least one element in common to both <paramref name="index1"/> and <paramref name="index2"/>.</returns>
        public static SynsetIndex Intersect(SynsetIndex index1, SynsetIndex index2) {
            return (SynsetIndex)(index1.Intersect(index2));
        }

        /// <summary>
        /// Rebuilds the specified synset index using factory by acquring synonyms from <paramref name="newFile"/>.
        /// </summary>
        /// <param name="newFile">The new synonyms file.</param>
        public void Rebuild(FileInfo newFile) {
            Dispose();
            synsFile = newFile;
            index = new SynsetIndex();
            Build();
        }

        private static SynsetIndex Init() {
            SynsetIndex ind = new SynsetIndex();
            var lines = from ln in dictionary.en_syns.Split(":\n")
                        let d = ln.Insert(0, "(").Insert(ln.Length + 1, " )")
                        where d.Length > 4
                        select d;
            foreach (var line in lines) {
                ind.Add(line);
            }
            return ind;
        }

        /// <summary>
        /// Builds Synonym Index from a file using the prolog file format from Wordnet.
        /// </summary>
        /// <exception cref="InvalidDataException">Corrupt Prolog File Detected !!</exception>
        private void Build() {
            FileInfo PrologFile = synsFile;
            string line;
            IDictionary<string, IList<string>> word2nums = new Dictionary<string, IList<string>>();
            IDictionary<string, IList<string>> num2words = new Dictionary<string, IList<string>>();

            int NotDecent = 0, mod = 1, row = 1;

            using (StreamReader reader = new StreamReader(PrologFile.FullName)) {
                int lineNum = 0;
                while ((line = reader.ReadLine()) != null) {

                    if ((++row) % mod == 0) {
                        mod *= 2;
                    }

                    //Predicate Syntax Check.
                    if (!line.StartsWith("s")) {
                        error.WriteLine($"Illegal Form Detected @ line : {lineNum} , value : {line}");
                        throw new InvalidDataException("Corrupt Prolog File Detected !!");
                    }

                    //Call to parser Tuple which returns word and number.
                    var data = ParseLine(ref line);
                    string word = data.Word;
                    string num = data.Number;

                    if (!word.IsWord()) {
                        NotDecent++;
                        continue;
                    }

                    //Filling up word2nums table
                    if (!word2nums.TryGetValue(word, out IList<string> num_lis)) {
                        num_lis = new List<string> { num };
                        word2nums.Add(word, num_lis);
                    } else { num_lis.Add(num); }

                    //filling up num2words table
                    if (!num2words.TryGetValue(num, out IList<string> word_lis)) {
                        word_lis = new List<string> { word };
                        num2words.Add(num, word_lis);
                    } else { word_lis.Add(word); }

                    lineNum++;
                }
                reader.Close();
            }
            Indexx(word2nums, num2words);
        }

        /// <summary>
        /// Indexes the specified word2nums.
        /// </summary>
        /// <param name="word2nums">The word2nums.</param>
        /// <param name="numtowords">The numtowords.</param>
        private void Indexx(IDictionary<string, IList<string>> word2nums, IDictionary<string, IList<string>> numtowords) {
            int row = 0, mod = 1;
            try {
                var it1 = word2nums.Keys.GetEnumerator();
                while (it1.MoveNext()) {
                    StringBuilder builder = new StringBuilder("(");
                    builder.Append(it1.Current);

                    int n = Indexx(word2nums, numtowords, it1.Current, builder);
                    if (n > 0) {
                        if ((++row % mod) == 0) {
                            mod *= 2;
                        }
                        builder.Append(")" + Environment.NewLine);
                        index.Add(builder.ToString());
                    }
                }
            } catch (Exception xxp) when (xxp is FileNotFoundException || xxp is FileLoadException) {
                error.WriteLine($"Error has occured in writing index to file.. {xxp.Message} @ {xxp.Source}");
            }
        }

        /// <summary>
        /// Indexes the specified word2nums.
        /// </summary>
        /// <param name="word2nums">The word2nums.</param>
        /// <param name="num2words">The num2words.</param>
        /// <param name="g_str">The g string.</param>
        /// <param name="buitlder">The buitlder.</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException">Index has been found to be corrupt</exception>
        private static int Indexx(IDictionary<string, IList<string>> word2nums, IDictionary<string, IList<string>> num2words, string g_str, StringBuilder buitlder) {
            if (word2nums.TryGetValue(g_str, out IList<string> num_keys)) {
                var it2 = num_keys.GetEnumerator();

                ISet<string> already = new SortedSet<string>();
                while (it2.MoveNext()) {
                    if (num2words.TryGetValue(it2.Current, out IList<string> words_list)) {
                        already.UnionWith(words_list);
                    } else { throw new InvalidDataException("Index has been found to be corrupt"); }
                }
                int num = 0;
                already.Remove(g_str);
                var it = already.GetEnumerator();
                while (it.MoveNext()) {
                    if (!it.Current.IsWord()) {
                        continue;
                    }
                    num++;
                    buitlder.Append(it.Current).Append(", ");
                }
                return num;
            }
            return 0;
        }

        private static (string Word, string Number) ParseLine(ref string lineValue) {
            lineValue = lineValue.TrimStart('s', '(');
            string num = lineValue.Substring(0, lineValue.IndexOf(','));
            lineValue = lineValue.Substring(lineValue.IndexOf('\'') + 1);
            string word = lineValue.Substring(0, lineValue.IndexOf('\''));

            return (word, num);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose() {
            ((IDisposable)error).Dispose();
            ((IDisposable)index).Dispose();
            index = new SynsetIndex();
        }

        /// <summary>
        /// Initializes the <see cref="SynsetFactory"/> class.
        /// </summary>
        static SynsetFactory() {
            @default = Init();
        }
    }
}
