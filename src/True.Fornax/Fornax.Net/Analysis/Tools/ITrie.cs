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

using ProtoBuf;

namespace Fornax.Net.Analysis.Tools
{
    /// <summary>
    /// Representation of the Trie data structure and Trie-Operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ProtoContract]
    public interface ITrie<T>
    {
        /// <summary>
        /// Deletes the specified word from trie.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>true, if deletion was successful, otherwise, false.</returns>
        bool Delete(T[] word);

        /// <summary>
        /// Deletes the specified word from trie.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>true, if deletion was successful, otherwise, false.</returns>
        bool Delete(string word);

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance of Trie.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        bool Equals(object obj);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        int GetHashCode();

        /// <summary>
        /// Inserts the specified word into Trie.
        /// </summary>
        /// <param name="word">The word.</param>
        void Insert(T[] word);

        /// <summary>
        /// Inserts the specified word into Trie.
        /// </summary>
        /// <param name="word">The word.</param>
        void Insert(string word);

        /// <summary>
        /// Searches for the specified word in Trie.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>true, if word/text is found ,otherwise, false.</returns>
        bool Search(T[] word);

        /// <summary>
        /// Searches for the specified word in Trie.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>true, if word/text is found ,otherwise, false.</returns>
        bool Search(string word);

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        string ToString();

    }
}
