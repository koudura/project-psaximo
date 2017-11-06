using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Util.Text;
using ProtoBuf;

namespace Fornax.Net.Document
{
    /// <summary>
    /// Represents a part of Raw Text of a Document.This
    /// is displayed as result view.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Char}" />
    [Serializable, ProtoContract]
    public class Snippet : IEnumerable<char>, IComparable<Snippet>, java.io.Serializable.__Interface
    {
        private readonly string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="Snippet"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        /// <param name="text">The text.</param>
        public Snippet(int start, int length, string text)
        {
            this.text = text.Substring(start, length) + " ... ";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Snippet"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="tokenStream">The token stream.</param>
        public Snippet(int start, int end, TokenStream tokenStream)
        {
            text = GetRegion(start, end, tokenStream) + " ... ";
        }

        public Snippet(int end, string text)
        {
            this.text = GetRegion(end, text) + " ... ";
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is Snippet sno)
            {
                return (sno.text == text);
            }
            return false;
        }




        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<char> GetEnumerator()
        {
            return text.GetEnumerator();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return text.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return text;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return text.ToCharArray().GetEnumerator();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Snippet left, Snippet right)
        {
            return left.text.Equals(right.text);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Snippet left, Snippet right)
        {
            return !(left.text == right.text);
        }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        static string GetRegion(int start, int end, TokenStream stream)
        {
            int count = start;
            StringBuilder data = new StringBuilder();
            for (int i = start; i < end; i++)
            {
                data.Append(stream[i]).Append(" ");
            }
            return data.ToString().Trim();
        }

        private string GetRegion(int v, string text)
        {
            var tokenizer = new StringTokenizer(text);
            int I = 0;
            var @out = new StringBuilder();
            while (tokenizer.HasMoreTokens() && I < v)
            {
                var str = tokenizer.CurrentToken;
                @out.Append(str).Append(" ");
                I++;
            }
            return @out.ToString().TrimEnd();
        }

        static string ToText(string[] texts)
        {
            var b = new StringBuilder();
            foreach (var item in texts)
            {
                b.Append(item).Append(" ");
            }
            return b.ToString().Trim();
        }

        public int CompareTo(Snippet other)
        {
            return text.CompareTo(other.text);
        }
    }
}
