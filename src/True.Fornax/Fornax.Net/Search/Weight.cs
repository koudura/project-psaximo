// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 10-30-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-05-2017
// ***********************************************************************
// <copyright file="Weight.cs" company="Microsoft">
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
    /// Class Weight.
    /// </summary>
    internal static class Weight
    {
        /// <summary>
        /// Returns the Inverse-document frequency score for the given
        /// size-n of doc collection and doc-frequency.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="df">The df.</param>
        /// <returns>System.Double.</returns>
        internal static double Idf(ulong n, ulong df)
        {
            return Math.Log10(n / df);
        }

        /// <summary>
        /// Gets the score for the term frequency.
        /// </summary>
        /// <param name="tf">The term frequency count.</param>
        /// <returns>System.Double.</returns>
        internal static double Tf(ulong tf)
        {
            return 1 + Math.Log10(tf);
        }

        /// <summary>
        /// Evaluates the Term-frequency_Inverse document frequncy for 
        /// a given set of term count and doc collection size.
        /// </summary>
        /// <param name="tf">The term frequency count.</param>
        /// <param name="n">The n-size of doc-collection.</param>
        /// <param name="df">The document frequncy count.</param>
        /// <returns>System.Double.</returns>
        internal static double TfxIdf(ulong tf, ulong n, ulong df)
        {
            return Tf(tf) * Idf(n, df);
        }

    }
}
