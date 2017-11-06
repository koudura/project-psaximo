// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Koudura Mazou
// Created          : 11-05-2017
//
// Last Modified By : Koudura Mazou
// Last Modified On : 11-05-2017
// ***********************************************************************
// <copyright file="DocResult.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using Fornax.Net.Document;

/// <summary>
/// The Search namespace.
/// </summary>
namespace Fornax.Net.Search
{
    /// <summary>
    /// Class DocResult.
    /// </summary>
    /// <seealso cref="System.IComparable{Fornax.Net.Search.DocResult}" />
    public class DocResult : IComparable<DocResult>
    {
        private readonly FileInfo _file;
        private readonly double _score;

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>The document.</value>
        public FileInfo Document => _file;

        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <value>The score.</value>
        internal double Score => _score;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocResult"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="score">The score.</param>
        public DocResult(string filename, double score)
            : this(new FileInfo(filename ?? throw new ArgumentNullException(nameof(filename))), score) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocResult"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="score">The score.</param>
        public DocResult(FileInfo file, double score)
        {
            _file = file;
            _score = score;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocResult"/> class.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="score">The score.</param>
        public DocResult(IDocument document, double score) : this(document.Link, score) { }


        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.</returns>
        public int CompareTo(DocResult other)
        {
         return (_score > other._score) ? -1 : (other._score > _score) ? 1 : 0;
        }
    }
}

