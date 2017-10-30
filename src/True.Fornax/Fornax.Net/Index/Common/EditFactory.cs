// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Habuto Koudura
// Created          : 10-29-2017
//
// Last Modified By : Habuto Koudura
// Last Modified On : 10-29-2017
// ***********************************************************************
// <copyright file="EditFactory.cs" company="Microsoft">
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
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Fornax.Net.Common.Similarity;
using Fornax.Net.Util;
using Fornax.Net.Util.IO.Writers;
using Fornax.Net.Util.System;

/// <summary>
/// The Common namespace.
/// </summary>
namespace Fornax.Net.Index.Common
{
    /// <summary>
    /// Class EditFactory. This class cannot be inherited.
    /// </summary>
    public sealed class EditFactory
    {
        /// <summary>
        /// The edits file
        /// </summary>
        private static FileInfo edits_file;
        /// <summary>
        /// The fornax edits
        /// </summary>
        private static FileInfo FornaxEdits = Constants.FornaxEditFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditFactory"/> class.
        /// </summary>
        /// <param name="edits_file">The edits file.</param>
        public EditFactory(FileInfo edits_file) {
            Contract.Requires(edits_file != null);
        }

        /// <summary>
        /// Gets the default fornax edits index.
        /// </summary>
        /// <value>The default.</value>
        public static EditIndex Default => ReadAsync().Result;

        /// <summary>
        /// Validates the edits.
        /// </summary>
        /// <param name="edit_file">The edit file.</param>
        /// <exception cref="FileLoadException">edit_file</exception>
        private void ValidateEdits(FileInfo edit_file) {
            edits_file = (edit_file.Extension == ".4dat") ? edit_file : throw new FileLoadException(nameof(edit_file));
        }

        /// <summary>
        /// Saves the edits index to file.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Task.</returns>
        public async Task SaveEdits(EditIndex index) {
            await FornaxWriter.WriteAsync(index, edits_file);
        }

        public async Task<EditIndex> GetEdits() {
            return await FornaxWriter.ReadAsync<EditIndex>(edits_file);
        }

        /// <summary>
        /// Gets the index of the edit.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns>EditIndex.</returns>
        /// <exception cref="ArgumentNullException">words</exception>
        public static EditIndex GetEditIndex(IEnumerable<string> words) {
            Contract.Requires(words != null);
            if (words == null) throw new ArgumentNullException(nameof(words));
            return GetEditIndex(words.ToArray());
        }


        /// <summary>
        /// Gets the index of the edit.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns>EditIndex.</returns>
        /// <exception cref="ArgumentNullException">words</exception>
        public static EditIndex GetEditIndex(string[] words) {
            Contract.Requires(words != null);
            if (words == null) throw new ArgumentNullException(nameof(words));

            var lev = new LevenshteinEdit(); var editIndex = new EditIndex();
            Array.Sort(words);

            for (int i = 0; i < words.Length; i++) {
                var source = words[i];
                for (int j = i; j < words.Length; j++) {
                    var target = words[j];
                    float score = lev.GetDistance(target, source);
                    if (!editIndex.TryGetValue(source, out SortedList<float, List<int>> table)) {
                        editIndex.Add(source, new SortedList<float, List<int>>() { { score, new List<int>() { { j } } } });
                    } else {
                        if (table.TryGetValue(score, out List<int> pointers)) {
                            pointers.Add(j);
                        } else {
                            table.Add(score, new List<int>() { { j } });
                        }
                    }
                }
            }
            return editIndex;
        }


        /// <summary>
        /// Gets the similar words to the given word, from an edits-index.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="index">The index.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns>ISet&lt;KeyValuePair&lt;System.String, System.Single&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">word
        /// or
        /// index</exception>
        public static ISet<KeyValuePair<string, float>> GetSimilars(string word, EditIndex index, float threshold = 0.5f) {
            Contract.Requires(word != null && index != null);
            if (word == null) {
                throw new ArgumentNullException(nameof(word));
            }
            if (index == null) {
                throw new ArgumentNullException(nameof(index));
            }

            var setFound = new SortedSet<KeyValuePair<string, float>>(index);
            if (index.TryGetValue(word, out SortedList<float, List<int>> table)) {
                foreach (var score in table.Keys) {
                    if (score >= threshold) {
                        Acquire(ref setFound, score, table[score], index.Keys.ToList());
                    }
                }
            }
            return setFound;
        }


        /// <summary>
        /// get edit index as an asynchronous operation.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns>Task&lt;EditIndex&gt;.</returns>
        public static EditIndex GetEditIndexAsync(IEnumerable<string> words) {
            return GetEditIndex(words);
        }

        /// <summary>
        /// Acquires the specified set found.
        /// </summary>
        /// <param name="setFound">The set found.</param>
        /// <param name="score">The score.</param>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        static void Acquire(ref SortedSet<KeyValuePair<string, float>> setFound, float score, List<int> list, List<string> item) {
            foreach (var i in list) {
                setFound.Add(new KeyValuePair<string, float>(item[i], score));
            }
        }

        /// <summary>
        /// read as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;EditIndex&gt;.</returns>
        static async Task<EditIndex> ReadAsync() {
            return await FornaxWriter.ReadAsync<EditIndex>(FornaxEdits);
        }

        /// <summary>
        /// Builds the fornax edits.
        /// </summary>
        /// <returns>Task.</returns>
        static async Task BuildFornaxEdits() {
            var Defindex = GetEditIndexAsync(ConfigFactory.GetVocabulary(FornaxLanguage.English).Dictionary);
            await FornaxWriter.WriteAsync(Defindex, FornaxEdits);
        }

        /// <summary>
        /// Initializes static members of the <see cref="EditFactory"/> class.
        /// </summary>
        static EditFactory() {
         
                Task.WaitAll(BuildFornaxEdits());
            
        }
    }

}
