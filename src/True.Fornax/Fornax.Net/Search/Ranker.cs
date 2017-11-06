// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 11-04-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-05-2017
// ***********************************************************************
// <copyright file="Ranker.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The Search namespace.
/// </summary>
namespace Fornax.Net.Search
{
    /// <summary>
    /// Class Ranker.
    /// </summary>
    public static class Ranker 
    {
        /// <summary>
        /// Class ByModified.
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IComparer{Fornax.Net.Search.DocResult}" />
        internal class ByModified : IComparer<DocResult>
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero
            /// <paramref name="x" /> is less than <paramref name="y" />.Zero
            /// <paramref name="x" /> equals <paramref name="y" />.Greater than zero
            /// <paramref name="x" /> is greater than <paramref name="y" />.</returns>
            public int Compare(DocResult x, DocResult y)
            {
                return x.Document.LastWriteTime.CompareTo(y.Document.LastWriteTime);
            }
        }

        /// <summary>
        /// Class ByDate.
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IComparer{Fornax.Net.Search.DocResult}" />
        internal class ByDate : IComparer<DocResult>
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero
            /// <paramref name="x" /> is less than <paramref name="y" />.Zero
            /// <paramref name="x" /> equals <paramref name="y" />.Greater than zero
            /// <paramref name="x" /> is greater than <paramref name="y" />.</returns>
            public int Compare(DocResult x, DocResult y)
            {
                return x.Document.CreationTime.CompareTo(y.Document.CreationTime);
            }
        }

        /// <summary>
        /// Class ByName.
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IComparer{Fornax.Net.Search.DocResult}" />
        internal class ByName : IComparer<DocResult>
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero
            /// <paramref name="x" /> is less than <paramref name="y" />.Zero
            /// <paramref name="x" /> equals <paramref name="y" />.Greater than zero
            /// <paramref name="x" /> is greater than <paramref name="y" />.</returns>
            public int Compare(DocResult x, DocResult y)
            {
                return x.Document.Name.CompareTo(y.Document.Name);
            }
        }

        /// <summary>
        /// Class BySize.
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IComparer{Fornax.Net.Search.DocResult}" />
        internal class BySize : IComparer<DocResult>
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero
            /// <paramref name="x" /> is less than <paramref name="y" />.Zero
            /// <paramref name="x" /> equals <paramref name="y" />.Greater than zero
            /// <paramref name="x" /> is greater than <paramref name="y" />.</returns>
            public int Compare(DocResult x, DocResult y)
            {
                return x.Document.Length.CompareTo(y.Document.Length);
            }
        }

        /// <summary>
        /// Class ByRelevance.
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IComparer{Fornax.Net.Search.DocResult}" />
        internal class ByRelevance : IComparer<DocResult>
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero
            /// <paramref name="x" /> is less than <paramref name="y" />.Zero
            /// <paramref name="x" /> equals <paramref name="y" />.Greater than zero
            /// <paramref name="x" /> is greater than <paramref name="y" />.</returns>
            public int Compare(DocResult x, DocResult y)
            {
                return x.CompareTo(y);
            }
        }
    }
}
