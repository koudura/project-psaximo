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
using System.Threading.Tasks;

using Fornax.Net.Analysis.Tools;
using Fornax.Net.Common.Similarity;
using Fornax.Net.Properties;
using Fornax.Net.Util.Text;
using Token = Fornax.Net.Analysis.Tokenization.Token;

namespace Fornax.Net.Index.Common
{
    /// <summary>
    /// Soundex Factory clas, for handling phonetic and soundex related functionalities and operations.
    /// </summary>
    [Serializable]
    [Progress("SoundexFactory", false, Documented = true, Tested = false)]
    public sealed class SoundexFactory
    {
        FileInfo words_file;
        SoundexIndex words_index;
        ICollection<string> words;
        bool isCollection;
        private static SoundexIndex @default;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundexFactory"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <exception cref="ArgumentNullException">file</exception>
        public SoundexFactory(FileInfo file) {
            Contract.Requires(file != null && file.Exists);
            words_file = file ?? throw new ArgumentNullException(nameof(file));
            words_index = GetIndex();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundexFactory"/> class,
        /// with a collection of english words at which to retrieve soundex index.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <exception cref="ArgumentNullException">words</exception>
        public SoundexFactory(ICollection<string> words) {
            Contract.Requires(words != null);
            this.words = words ?? throw new ArgumentNullException(nameof(words));
            isCollection = true;
        }

        /// <summary>
        /// Creates a new instance of the Soundex factory class.
        /// </summary>
        /// <param name="file">The file containing engllish words per line.</param>
        /// <returns>A new instance of soundexfacctory.</returns>
        public static SoundexFactory New(FileInfo file) {
            return new SoundexFactory(file);
        }

        /// <summary>
        /// Creates a new insatnce of the Soundex factory class.
        /// </summary>
        /// <param name="words">The collection of words to be indexed.</param>
        /// <returns>A new instance of the soundex factory.</returns>
        public static SoundexFactory New(IEnumerable<string> words) {
            return new SoundexFactory(words.ToList());
        }

        /// <summary>
        /// Gets the default soundex index for fornax.net english dictionary.
        /// </summary>
        /// <value>
        /// The default index.
        /// </value>
        public static SoundexIndex Default => @default = InitAsync().Result;

        private static SoundexIndex Init() {
            @default = new SoundexIndex();
            StringTokenizer tokenizer = new StringTokenizer(dictionary.en_voc);
            while (tokenizer.HasMoreTokens()) {
                string token = tokenizer.CurrentToken;
                var sound = new Soundex(token);
                Add(token, sound, ref @default);
            }
            return @default;
        }

        private static async Task<SoundexIndex> InitAsync() {
            return await Task.Factory.StartNew(Init);
        }

        /// <summary>
        /// Gets the soundex index of the collection of words.
        /// </summary>
        /// <returns>A SoundexIndex of all words in <see cref="words_file"/> or <see cref="words"/>.</returns>
        public SoundexIndex GetIndex() {
            if (!isCollection) {
                SoundexIndex index = new SoundexIndex();
                using (var reader = new StreamReader(words_file.FullName)) {
                    string line;

                    while ((line = reader.ReadLine()) != null) {
                        if (line.IsWord()) {
                            var sound = new Soundex(line);
                            Add(line, sound, ref index);
                        }
                    }
                }
                return index;
            }
            return GetIndex(words);
        }

        /// <summary>
        /// Gets the soundex index of the collection of words asynchronously.
        /// </summary>
        /// <returns>A Soundex Index of all words in <see cref="words_file"/> or <see cref="words"/>.</returns>
        public async Task<SoundexIndex> GetIndexAsync() {
            if (!isCollection) {
                SoundexIndex index = new SoundexIndex();
                using (var reader = new StreamReader(words_file.FullName)) {
                    string line;

                    while ((line = await reader.ReadLineAsync()) != null) {
                        if (line.IsWord()) {
                            var sound = new Soundex(line);
                            Add(line, sound, ref index);
                        }
                    }
                }
                return index;
            }
            return await GetIndexAsync(words);
        }

        /// <summary>
        /// Gets the immediate soundex index of a specified collection of words asynchronuosly.
        /// </summary>
        /// <param name="w">The w.</param>
        /// <returns>
        /// The Soundex index of the <paramref name="words" />.
        /// </returns>
        public static async Task<SoundexIndex> GetIndexAsync(IEnumerable<string> w) {
            return await Task.Factory.StartNew(() => GetIndex(w));
        }

        private static void Add(string word, Soundex sound, ref SoundexIndex index) {
            if (index.ContainsKey(sound)) {
                if (!index[sound].Contains(word))
                    index[sound].Add(word);
            } else {
                index.Add(sound, new List<string> { word });
            }
        }

        /// <summary>
        /// Gets the soundex of a specified english word.
        /// NOTE: This must be a valid english word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>The soundex code of the word.</returns>
        public static Soundex GetSoundex(string word) {
            return new Soundex(word);
        }

        /// <summary>
        /// Gets the soundex of a specified token of <see cref="TokenAttribute"/>
        /// NOTE. this must be a valid english word.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static Soundex GetSoundex(Token token) {
            return new Soundex(token);
        }

        /// <summary>
        /// Gets the immediate soundex index of a specified collection of words.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns>The Soundex index of the <paramref name="words"/>.</returns>
        public static SoundexIndex GetIndex(IEnumerable<string> words) {
            SoundexIndex index = new SoundexIndex();
            foreach (var word in words) {
                var sound = new Soundex(word);
                Add(word, sound, ref index);
            }
            return index;
        }

        /// <summary>
        /// Gets All words that have a common soundex code.
        /// i.e all words that have a phonetic mismatch possibility.
        /// </summary>
        /// <param name="soundex">The soundex.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">soundex</exception>
        public static IList<string> GetWords(Soundex soundex, SoundexIndex index) {
            Contract.Requires(soundex != null && index != null);
            if (soundex == null || index == null) throw new ArgumentNullException($"{nameof(soundex)} or {nameof(index)} is invalid.");

            if (index.ContainsKey(soundex)) {
                return index[soundex];
            } else { return new List<string>(); }
        }

        /// <summary>
        /// Returns the score for which two words sound alike.
        /// the score varies from 0 to 1, words with exact pronounciation returns 1.0,
        /// but score closer to 0, indicates absolute phonetic unsimilarity.
        /// </summary>
        /// <param name="word1">The word1.</param>
        /// <param name="word2">The word2.</param>
        /// <returns>The similarity score between the sound of <paramref name="word1"/> and <paramref name="word2"/>.</returns>
        public static float Similarity(string word1, string word2) {
            Contract.Requires(word1 != null && word2 != null);

            var sound1 = new Soundex(word1).Value; var sound2 = new Soundex(word2).Value;
            return new LevenshteinEdit().GetDistance(sound1, sound2);
        }

        /// <summary>
        /// Merges the specified soundex index with another soundex index,
        /// as a union of each other by using the default equality comparer.
        /// </summary>
        /// <param name="index1">The index1.</param>
        /// <param name="index2">The index2.</param>
        /// <returns>a union of both soundex index oone and two inclusive.</returns>
        public static SoundexIndex Merge(SoundexIndex index1, SoundexIndex index2) {
            Contract.Requires(index1 != null && index2 != null);

            var new_Index = index1.Union(index2);
            return (SoundexIndex)new_Index;
        }

        /// <summary>
        /// Merges the specified soundex index with another soundex index asynchronuously,
        /// as a union of each other by using the default equality comparer.
        /// </summary>
        /// <param name="index1">The index1.</param>
        /// <param name="index2">The index2.</param>
        /// <returns>a union of both soundex index oone and two inclusive.</returns>
        public static async Task<SoundexIndex> MergeAsync(SoundexIndex index1, SoundexIndex index2) {
            return await Task.Factory.StartNew(() => Merge(index1, index2));
        }

        /// <summary>
        /// Produces the intersection set of two Soundex Index by using the default equality comparer to compare values.
        /// </summary>
        /// <param name="firstIndex">The first index.</param>
        /// <param name="secondIndex">Index of the second.</param>
        /// <returns>the intersection set of <paramref name="firstIndex"/> and <paramref name="secondIndex"/>.</returns>
        public static SoundexIndex Intersect(SoundexIndex firstIndex, SoundexIndex secondIndex) {
            Contract.Requires(firstIndex != null && secondIndex != null);

            var new_index = firstIndex.Intersect(secondIndex);
            return (SoundexIndex)new_index;
        }

        /// <summary>
        /// Asynchronously produces the intersection set of two Soundex Index by using the default equality comparer to compare values.
        /// </summary>
        /// <param name="firstIndex">The first index.</param>
        /// <param name="secondIndex">Index of the second.</param>
        /// <returns>the intersection set of <paramref name="firstIndex"/> and <paramref name="secondIndex"/>.</returns>
        public static async Task<SoundexIndex> IntersectAsync(SoundexIndex firstIndex, SoundexIndex secondIndex) {
            return await Task.Factory.StartNew(() => Intersect(firstIndex, secondIndex));
        }

        /// <summary>
        /// Adds the specified word to an existing Soundex index.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="existingIndex">Index of the existing.</param>
        /// <returns>true; if addition was succeful; otherwise, false.</returns>
        public static bool Add(string word, SoundexIndex existingIndex) {
            if (word.IsWord()) {
                Add(word, new Soundex(word), ref existingIndex);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compares two soundex codes for equality.
        /// This differs from similarity measure.
        /// </summary>
        /// <param name="sound1">The sound1.</param>
        /// <param name="sound2">The sound2.</param>
        /// <returns>true; if sound1 equals sound2; otherwise; false.</returns>
        public static bool Equals(Soundex sound1, Soundex sound2) {
            Contract.Requires(sound1 != null && sound2 != null);
            return sound1.Equals(sound2);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return base.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is SoundexFactory) {
                var d = obj as SoundexFactory;
                return d.GetIndex().Equals(GetIndex());
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return 163 * base.GetHashCode();
        }
    }
}
