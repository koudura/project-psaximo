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

using Fornax.Net.Util.Collections.Generic;

namespace Fornax.Net.Util.Linq
{
    public static partial class Extensions
    {
        /// <summary>
        /// Shuffles the specified list with a <see cref="Random"/> rule object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="random">The random object.</param>
        public static void Shuffle<T>(this IList<T> list, Random random) {
            if (list == null || random == null) {
                throw new ArgumentNullException();
            }

            for (int i = list.Count; i > 1; i--) {
                int pos = random.Next(i);
                var x = list[i - 1];
                list[i - 1] = list[pos];
                list[pos] = x;
            }
        }

        /// <summary>
        /// Shuffles the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        public static void Shuffle<T>(this IList<T> list) {
            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }

            list.Shuffle(new Random());
        }

        /// <summary>
        /// A Sublist <see cref="ISubList{T}"/> from the specified <paramref name="list"/>. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="fromIndex">From index. <see cref="SubList.fromIndex"/></param>
        /// <param name="toIndex">To index. <see cref="SubList.toIndex"/></param>
        /// <returns> a <see cref="SubList{T}(IList{T}, int, int)"/> instance of <paramref name="list"/> that, when modified, modifies the original list.</returns>
        public static IList<T> SubList<T>(this IList<T> list, int fromIndex, int toIndex) {
            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }

            return new SubList<T>(list, fromIndex, toIndex);
        }

        /// <summary>
        /// Adds all <typeparamref name="T"/> <paramref name="values"/> in <see cref="IEnumerable{T}"/> into 
        /// specified <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="values">The values as <see cref="IEnumerable{T}"/></param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> values) {
            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }

            var lt = list as List<T>;

            if (lt != null)
                lt.AddRange(values);
            else {
                foreach (var item in values) {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// Swaps the elements at <paramref name="list"/> @<paramref name="indexA"/> 
        /// with @<paramref name="indexB"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="indexA">The index a.</param>
        /// <param name="indexB">The index b.</param>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }

            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

    }
}
