// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Habuto Koudura
// Created          : 10-28-2017
//
// Last Modified By : Habuto Koudura
// Last Modified On : 10-29-2017
// ***********************************************************************
// <copyright file="Vector.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************


using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Fornax.Net.Util.Collections;

/// <summary>
/// The Search namespace.
/// </summary>
namespace Fornax.Net.Search
{
    /// <summary>
    /// Vector of points.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IComparer{Fornax.Net.Search.Vector}" />
    [Serializable]
    public struct Vector : IComparable<Vector> , java.io.Serializable.__Interface
    {
        /// <summary>
        /// The values
        /// </summary>
        private static IList<double> values = new List<double>();
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public IList<double> Value => values;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> struct.
        /// </summary>
        /// <param name="fieldValues">The field values.</param>
        public Vector(ICollection<double> fieldValues) {
            values = fieldValues.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> struct.
        /// </summary>
        /// <param name="fieldValues">The field values.</param>
        public Vector(params double[] fieldValues) {
            values = new List<double>(fieldValues);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> struct.
        /// </summary>
        /// <param name="fieldValues">The field values.</param>
        public Vector(ISet<double> fieldValues) {
            values = fieldValues.ToList();
        }


        /// <summary>
        /// Abses this instance.
        /// </summary>
        /// <returns>System.Double.</returns>
        public double Abs() { return Abs(this); }

        /// <summary>
        /// Similarities the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>System.Double.</returns>
        public double Similarity(Vector vector) { return Similarity(this, vector); }

        /// <summary>
        /// Coefficients the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>System.Double.</returns>
        public double Coefficient(Vector vector) { return Coefficient(this, vector); }

        /// <summary>
        /// Dots the product.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>System.Double.</returns>
        public double DotProduct(Vector vector) { return DotProduct(this, vector); }

        /// <summary>
        /// Gets the absolute value of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>System.Decimal.</returns>
        public static double Abs(Vector vector) {
            double acc = 0;
            foreach (double item in vector.Value) {
                acc += item * item;
            }
            return Math.Sqrt(acc);
        }

        /// <summary>
        /// Dots the product.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>System.Decimal.</returns>
        public static double DotProduct(Vector v1, Vector v2) {
            double acc = 0;
            for (int i = 0; i < v1.Value.Count(); i++) {
                acc += v1.Value[i] * v2.Value[i];
            }
            return acc;
        }

        /// <summary>
        /// Similarities the specified v1.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>System.Decimal.</returns>
        public static double Similarity(Vector v1, Vector v2) {
            return DotProduct(v1, v2) / (Abs(v1) * Abs(v2));
        }

        /// <summary>
        /// Coefficients the specified v1.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>System.Single.</returns>
        public static float Coefficient(Vector v1, Vector v2) {
            var setv1 = new HashSet<double>(v1.Value);
            var setv2 = new HashSet<double>(v2.Value);

            float un = setv1.Union(setv2).Count();
            float @int = setv1.Intersect(setv2).Count();
            return @int / un;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is Vector) {
                return ((Vector)(obj)) == this;
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() {
            return Collections.GetHashCode(Value);
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero
        /// <paramref name="x" /> is less than <paramref name="y" />.Zero
        /// <paramref name="x" /> equals <paramref name="y" />.Greater than zero
        /// <paramref name="x" /> is greater than <paramref name="y" />.</returns>
        public int Compare(Vector x, Vector y) {
            Contract.Requires(x != null && y != null);
            if (x == y) return 0;
            return (Abs(x) > Abs(y)) ? 1 : -1;         
        }

        public int CompareTo(Vector other)
        {
            return Compare(this, other);
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator == (Vector left, Vector right) {
            return Collections.Equals(left.Value, right.Value);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator != (Vector left, Vector right) {
            return !(left == right);
        }
    }

}
