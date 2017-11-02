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
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Fornax.Net.Util.Linq
{
    public static partial class Extensions
    {
        static readonly Random Random = new Random();

		/// <summary>
        /// Orders an enumerable collection <paramref name="source"/> by a conditional
        /// by a conditional sequence <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderBySequence<T, TProperty>(this IEnumerable<T> source, Func<T, TProperty> property, IEnumerable<TProperty> sequence) {
            var sequenceList = sequence.ToList();
            var sequenceDictionary = sequenceList.ToDictionary(s => s, sequenceList.IndexOf);
            return source.OrderBy(s => sequenceDictionary[property(s)]);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int page, int pageSize) {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Must be 1 or greater", page < 1 ? "page" : "pageSize");

            return source.Skip(--page * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Checks if a  <paramref name="source"/> contains <paramref name="value"/>. 
        /// Returns the value if value is present ,else the <see cref ="default(T)"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns><typeparamref name="default{T}"/> if false , else : value.</returns>
        public static T ContainsOrDefault<T>(this IEnumerable<T> source, T value) {
            return source.Contains(value) ? value : default(T);
        }

        /// <summary>
        /// Gets a Random element <typeparamref name="T"/> from <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T RandomElement<T>(this ICollection<T> collection) {
            return collection.ElementAt(Random.Next(collection.Count));
        }

        public static TResult FirstOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector) {
            var match = source.FirstOrDefault(predicate);
            return Equals(match, default(TSource)) ? default(TResult) : selector(match);
        }

        /// <summary>
        /// Asynchronously returns the first element of a sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the first element in <paramref name="source"/>.
        /// </returns>
        /// <param name="source">The <see cref="T:System.Linq.IQueryable`1"/> to return the first element of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
        public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.First(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the first element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <returns>
        /// A task that returns the first element in <paramref name="source"/> that passes the test in <paramref name="predicate"/>.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate"/>.-or-The source sequence is empty.</exception>
        public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.First(predicate), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <returns>
        /// A task that returns default(<typeparamref name="TSource"/>) if <paramref name="source"/> is empty; otherwise, the first element in <paramref name="source"/>.
        /// </returns>
        /// <param name="source">The <see cref="T:System.Linq.IQueryable`1"/> to return the first element of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.FirstOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the first element of a sequence that satisfies a specified condition or a default value if no such element is found.
        /// </summary>
        /// <returns>
        /// A task that returns default(<typeparamref name="TSource"/>) if <paramref name="source"/> is empty or if no element passes the test specified by <paramref name="predicate"/>; otherwise, the first element in <paramref name="source"/> that passes the test specified by <paramref name="predicate"/>.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.FirstOrDefault(predicate), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the last element in a sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the value at the last position in <paramref name="source"/>.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return the last element of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
        public static Task<TSource> LastAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Last(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the last element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <returns>
        /// A task that returns the last element in <paramref name="source"/> that passes the test specified by <paramref name="predicate"/>.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate"/>.-or-The source sequence is empty.</exception>
        public static Task<TSource> LastAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Last(predicate), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the last element in a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <returns>
        /// A task that returns default(<typeparamref name="TSource"/>) if <paramref name="source"/> is empty; otherwise, the last element in <paramref name="source"/>.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return the last element of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<TSource> LastOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.LastOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the last element of a sequence that satisfies a condition or a default value if no such element is found.
        /// </summary>
        /// <returns>
        /// A task that returns default(<typeparamref name="TSource"/>) if <paramref name="source"/> is empty or if no elements pass the test in the predicate function; otherwise, the last element of <paramref name="source"/> that passes the test in the predicate function.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static Task<TSource> LastOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.LastOrDefault(predicate), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the single element of the input sequence.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return the single element of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> has more than one element.</exception>
        public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Single(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.
        /// </summary>
        /// <returns>
        /// A task that returns the single element of the input sequence that satisfies the condition in <paramref name="predicate"/>.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate"/>.-or-More than one element satisfies the condition in <paramref name="predicate"/>.-or-The source sequence is empty.</exception>
        public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Single(predicate), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the single element of the input sequence, or default(<typeparamref name="TSource"/>) if the sequence contains no elements.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return the single element of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> has more than one element.</exception>
        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.SingleOrDefault(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists; this method throws an exception if more than one element satisfies the condition.
        /// </summary>
        /// <returns>
        /// A task that returns the single element of the input sequence that satisfies the condition in <paramref name="predicate"/>, or default(<typeparamref name="TSource"/>) if no such element is found.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">More than one element satisfies the condition in <paramref name="predicate"/>.</exception>
        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.SingleOrDefault(predicate), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the element at a specified index in a sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the element at the specified position in <paramref name="source"/>.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than zero.</exception>
        public static Task<TSource> ElementAtAsync<TSource>(this IQueryable<TSource> source, int index, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.ElementAt(index), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the element at a specified index in a sequence or a default value if the index is out of range.
        /// </summary>
        /// <returns>
        /// A task that returns default(<typeparamref name="TSource"/>) if <paramref name="index"/> is outside the bounds of <paramref name="source"/>; otherwise, the element at the specified position in <paramref name="source"/>.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<TSource> ElementAtOrDefaultAsync<TSource>(this IQueryable<TSource> source, int index, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.ElementAtOrDefault(index), cancellationToken);
        }

        /// <summary>
        /// Asynchronously determines whether a sequence contains a specified element by using the default equality comparer.
        /// </summary>
        /// <returns>
        /// A task that returns true if the input sequence contains an element that has the specified value; otherwise, false.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> in which to locate <paramref name="item"/>.</param>
        /// <param name="item">The object to locate in the sequence.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<bool> ContainsAsync<TSource>(this IQueryable<TSource> source, TSource item, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Contains(item), cancellationToken);
        }

        /// <summary>
        /// Asynchronously determines whether a sequence contains a specified element by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>.
        /// </summary>
        /// <returns>
        /// A task that returns true if the input sequence contains an element that has the specified value; otherwise, false.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> in which to locate <paramref name="item"/>.</param>
        /// <param name="item">The object to locate in the sequence.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare values.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<bool> ContainsAsync<TSource>(this IQueryable<TSource> source, TSource item, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Contains(item, comparer), cancellationToken);
        }

        /// <summary>
        /// Asynchronously determines whether two sequences are equal by using the default equality comparer to compare elements.
        /// </summary>
        /// <returns>
        /// A task that returns true if the two source sequences are of equal length and their corresponding elements compare equal; otherwise, false.
        /// </returns>
        /// <param name="source1">An <see cref="T:System.Linq.IQueryable`1"/> whose elements to compare to those of <paramref name="source2"/>.</param>
        /// <param name="source2">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose elements to compare to those of the first sequence.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is null.</exception>
        public static Task<bool> SequenceEqualAsync<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source1.SequenceEqual(source2), cancellationToken);
        }

        /// <summary>
        /// Asynchronously determines whether two sequences are equal by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to compare elements.
        /// </summary>
        /// <returns>
        /// A task that returns true if the two source sequences are of equal length and their corresponding elements compare equal; otherwise, false.
        /// </returns>
        /// <param name="source1">An <see cref="T:System.Linq.IQueryable`1"/> whose elements to compare to those of <paramref name="source2"/>.</param>
        /// <param name="source2">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose elements to compare to those of the first sequence.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> to use to compare elements.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source1"/> or <paramref name="source2"/> is null.</exception>
        public static Task<bool> SequenceEqualAsync<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source1.SequenceEqual(source2, comparer), cancellationToken);
        }

        /// <summary>
        /// Asynchronously determines whether a sequence contains any elements.
        /// </summary>
        /// <returns>
        /// A task that returns true if the source sequence contains any elements; otherwise, false.
        /// </returns>
        /// <param name="source">A sequence to check for being empty.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Any(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously determines whether any element of a sequence satisfies a condition.
        /// </summary>
        /// <returns>
        /// A task that returns true if any elements in the source sequence pass the test in the specified predicate; otherwise, false.
        /// </returns>
        /// <param name="source">A sequence whose elements to test for a condition.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Any(predicate), cancellationToken);
        }

        /// <summary>
        /// Asynchronously determines whether all the elements of a sequence satisfy a condition.
        /// </summary>
        /// <returns>
        /// A task that returns true if every element of the source sequence passes the test in the specified predicate, or if the sequence is empty; otherwise, false.
        /// </returns>
        /// <param name="source">A sequence whose elements to test for a condition.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static Task<bool> AllAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.All(predicate), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the number of elements in a sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the number of elements in the input sequence.
        /// </returns>
        /// <param name="source">The <see cref="T:System.Linq.IQueryable`1"/> that contains the elements to be counted.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source"/> is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
        public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Count(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the number of elements in the specified sequence that satisfies a condition.
        /// </summary>
        /// <returns>
        /// A task that returns the number of elements in the sequence that satisfies the condition in the predicate function.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> that contains the elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source"/> is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
        public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Count(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns an <see cref="T:System.Int64"/> that represents the total number of elements in a sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the number of elements in <paramref name="source"/>.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> that contains the elements to be counted.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The number of elements exceeds <see cref="F:System.Int64.MaxValue"/>.</exception>
        public static Task<long> LongCountAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.LongCount(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns an <see cref="T:System.Int64"/> that represents the number of elements in a sequence that satisfy a condition.
        /// </summary>
        /// <returns>
        /// A task that returns the number of elements in <paramref name="source"/> that satisfy the condition in the predicate function.
        /// </returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1"/> that contains the elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The number of matching elements exceeds <see cref="F:System.Int64.MaxValue"/>.</exception>
        public static Task<long> LongCountAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.LongCount(predicate), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the minimum value of a generic <see cref="T:System.Linq.IQueryable`1"/>.
        /// </summary>
        /// <returns>
        /// A task that returns the minimum value in the sequence.
        /// </returns>
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<TSource> MinAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Min(), cancellationToken);
        }

        /// <summary>
        /// Invokes a projection function on each element of a generic <see cref="T:System.Linq.IQueryable`1"/> and returns the minimum resulting value.
        /// </summary>
        /// <returns>
        /// A task that returns the minimum value in the sequence.
        /// </returns>
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the function represented by <paramref name="selector"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<TResult> MinAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Min(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously returns the maximum value in a generic <see cref="T:System.Linq.IQueryable`1"/>.
        /// </summary>
        /// <returns>
        /// A task that returns the maximum value in the sequence.
        /// </returns>
        /// <param name="source">A sequence of values to determine the maximum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<TSource> MaxAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Max(), cancellationToken);
        }

        /// <summary>
        /// Invokes a projection function on each element of a generic <see cref="T:System.Linq.IQueryable`1"/> and returns the maximum resulting value.
        /// </summary>
        /// <returns>
        /// A task that returns the maximum value in the sequence.
        /// </returns>
        /// <param name="source">A sequence of values to determine the maximum of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the function represented by <paramref name="selector"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<TResult> MaxAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Max(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of <see cref="T:System.Int32"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Int32"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
        public static Task<int> SumAsync(this IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of nullable <see cref="T:System.Int32"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int32"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
        public static Task<int?> SumAsync(this IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of <see cref="T:System.Int64"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Int64"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue"/>.</exception>
        public static Task<long> SumAsync(this IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of nullable <see cref="T:System.Int64"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int64"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue"/>.</exception>
        public static Task<long?> SumAsync(this IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of <see cref="T:System.Single"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Single"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<float> SumAsync(this IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of nullable <see cref="T:System.Single"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Single"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<float?> SumAsync(this IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of <see cref="T:System.Double"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Double"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<double> SumAsync(this IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of nullable <see cref="T:System.Double"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Double"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<double?> SumAsync(this IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of <see cref="T:System.Decimal"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Decimal"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue"/>.</exception>
        public static Task<decimal> SumAsync(this IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of a sequence of nullable <see cref="T:System.Decimal"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the values in the sequence.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Decimal"/> values to calculate the sum of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue"/>.</exception>
        public static Task<decimal?> SumAsync(this IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of <see cref="T:System.Int32"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
        public static Task<int> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of nullable <see cref="T:System.Int32"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
        public static Task<int?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of <see cref="T:System.Int64"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue"/>.</exception>
        public static Task<long> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of nullable <see cref="T:System.Int64"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue"/>.</exception>
        public static Task<long?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of <see cref="T:System.Single"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<float> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of nullable <see cref="T:System.Single"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<float?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of <see cref="T:System.Double"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<double> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of nullable <see cref="T:System.Double"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<double?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of <see cref="T:System.Decimal"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue"/>.</exception>
        public static Task<decimal> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the sum of the sequence of nullable <see cref="T:System.Decimal"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the sum of the projected values.
        /// </returns>
        /// <param name="source">A sequence of values of type <typeparamref name="TSource"/>.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue"/>.</exception>
        public static Task<decimal?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Sum(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Int32"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Int32"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<double> AverageAsync(this IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Int32"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the source sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int32"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<double?> AverageAsync(this IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Int64"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Int64"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<double> AverageAsync(this IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Int64"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the source sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Int64"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<double?> AverageAsync(this IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Single"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Single"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<float> AverageAsync(this IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Single"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the source sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Single"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<float?> AverageAsync(this IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Double"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Double"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<double> AverageAsync(this IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Double"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the source sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Double"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<double?> AverageAsync(this IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Decimal"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of <see cref="T:System.Decimal"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<decimal> AverageAsync(this IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Decimal"/> values.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the source sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of nullable <see cref="T:System.Decimal"/> values to calculate the average of.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> is null.</exception>
        public static Task<decimal?> AverageAsync(this IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Int32"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Int32"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Single"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<float> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Single"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<float?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Int64"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Int64"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Double"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Double"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of <see cref="T:System.Decimal"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values.
        /// </returns>
        /// <param name="source">A sequence of values that are used to calculate an average.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<decimal> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously computes the average of a sequence of nullable <see cref="T:System.Decimal"/> values that is obtained by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
        /// </returns>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Task<decimal?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Average(selector), cancellationToken);
        }

        /// <summary>
        /// Asynchronously applies an accumulator function over a sequence.
        /// </summary>
        /// <returns>
        /// A task that returns the final accumulator value.
        /// </returns>
        /// <param name="source">A sequence to aggregate over.</param>
        /// <param name="func">An accumulator function to apply to each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="func"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="source"/> contains no elements.</exception>
        public static Task<TSource> AggregateAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> func, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Aggregate(func), cancellationToken);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified seed value is used as the initial accumulator value.
        /// </summary>
        /// <returns>
        /// A task that returns the final accumulator value.
        /// </returns>
        /// <param name="source">A sequence to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to invoke on each element.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="func"/> is null.</exception>
        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Aggregate(seed, func), cancellationToken);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified seed value is used as the initial accumulator value, and the specified function is used to select the result value.
        /// </summary>
        /// <returns>
        /// A task that returns the transformed final accumulator value.
        /// </returns>
        /// <param name="source">A sequence to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="func">An accumulator function to invoke on each element.</param>
        /// <param name="selector">A function to transform the final accumulator value into the result value.</param>
        /// <param name="cancellationToken">The optional <see cref="T:System.Threading.CancellationToken"/> which can be used to cancel this task.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="func"/> or <paramref name="selector"/> is null.</exception>
        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => source.Aggregate(seed, func, selector), cancellationToken);
        }

    }
}
