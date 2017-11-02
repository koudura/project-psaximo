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

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Fornax.Net.Util.Linq
{
    public static partial class Extensions
    {
        /// <summary>
        /// Removes all occurrence of each object <typeparamref name="T"/> in <paramref name="removeList"/>
        /// from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theSet">The set.</param>
        /// <param name="removeList">The collection of objects to remove from the <paramref name="theSet"/>.</param>
        [DebuggerStepThrough]
        public static void RemoveAll<T>(this ICollection<T> theSet, IEnumerable<T> removeList) {
            foreach (var item in removeList) {
                /**
                 *For safety reasons. 
                 */
                while (theSet.Contains<T>(item))
                    theSet.Remove(item);
            }
        }

        /// <summary>
        /// Adds all objects <typeparamref name="T"/> in <paramref name="itemsToAdd"/> to
        /// the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theSet">The set.</param>
        /// <param name="itemsToAdd">The enumerable collection of items to add.</param>
        [DebuggerStepThrough]
        public static void AddAll<T>(this ICollection<T> theSet, IEnumerable<T> itemsToAdd) {
            foreach (var item in itemsToAdd) {
                theSet.Add(item);
            }

        }
    }
}
