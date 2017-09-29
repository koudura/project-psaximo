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
using Fornax.Net.Util.Collections.Generic;

namespace Fornax.Net.Util.Collections
{
    /// <summary>
    /// Utilities for easily wrapping .Net collections so they can be used with <see cref="object.Equals(object)"/>,
    /// <see cref="object.GetHashCode"/>, and <see cref="object.ToString"/> behaviour similar to that in java.
    /// </summary>
    /// <remarks>
    /// The equality checking of collections will recursively compare the values of all elements and any nested collections.
    /// The same goes for using <see cref="object.ToString()"/> - the string is based
    /// on the values in the collection and any nested collections.
    /// <para>
    /// Do note this has a side-effect that any custom <see cref="object.Equals(object)"/>
    /// <see cref="object.GetHashCode()"/>, and <see cref="object.ToString()"/> implementations
    /// for types that implement <see cref="IList{T}"/> (including arrays), <see cref="ISet{T}"/>,
    /// or <see cref="IDictionary{TKey, TValue}"/> will be ignored.
    /// </para>
    /// </remarks>
    public static class Equatable
    {
        /// <summary>
        /// Wraps any <see cref="IList{T}"/> (including <see cref="T:T[]"/>) with a 
        /// lightweight <see cref="EquatableList{T}"/> class that changes the behavior
        /// of <see cref="object.Equals(object)"/>, <see cref="object.GetHashCode()"/>, and <see cref="object.ToString()"/>
        /// so they consider all values in the <see cref="IList{T}"/> or any nested
        /// collections when comparing or making strings to represent them.
        /// </summary>
        /// <typeparam name="T">the type of the element</typeparam>
        /// <param name="list">Any <see cref="IList{T}"/> (including <see cref="T:T[]"/>)</param>
        /// <returns>An <see cref="EquatableList{T}"/> that wraps the provided <paramref name="list"/>, 
        /// or the value of <paramref name="list"/> unmodified if it already is an <see cref="EquatableList{T}"/></returns>
        public static IList<T> Wrap<T>(IList<T> list) => (list is EquatableList<T>) ? list : new EquatableList<T>(list, true);

        /// <summary>
        /// Wraps any <see cref="ISet{T}"/> with a 
        /// lightweight <see cref="EquatableSet{T}"/> class that changes the behavior
        /// of <see cref="object.Equals(object)"/>, <see cref="object.GetHashCode()"/>, and <see cref="object.ToString()"/>
        /// so they consider all values in the <see cref="ISet{T}"/> or any nested
        /// collections when comparing or making strings to represent them.
        /// </summary>
        /// <typeparam name="T">the type of element</typeparam>
        /// <param name="set">Any <see cref="IList{T}"/> (including <see cref="T:T[]"/>)</param>
        /// <returns>An <see cref="EquatableSet{T}"/> that wraps the provided <paramref name="set"/>, 
        /// or the value of <paramref name="set"/> unmodified if it already is an <see cref="EquatableSet{T}"/></returns>
        public static ISet<T> Wrap<T>(ISet<T> set) => (set is EquatableSet<T>) ? set : new EquatableSet<T>(set, true);

    }
}
