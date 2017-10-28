

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Fornax.Net.Util.Collections;

namespace Fornax.Net.Search
{
    /// <summary>
    ///  Vector of points.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IComparer{Fornax.Net.Search.Vector}" />
    public struct Vector : IComparer<Vector>
    {
        private static List<decimal> values = new List<decimal>();
        public List<decimal> Value => values;

        public Vector(List<decimal> fieldValues) {
            values = fieldValues;
        }

        public Vector(params decimal[] fieldValues) {
            values = new List<decimal>(fieldValues);
        }

        public static decimal Abs(Vector vector) {
            decimal acc = 0;
            foreach (decimal item in vector.Value) {
                acc += item * item;
            }
            return (decimal)Math.Sqrt(Decimal.ToDouble(acc));
        }

        public static decimal DotProduct(Vector v1, Vector v2) {
            decimal acc = 0;
            for (int i = 0; i < v1.Value.Count(); i++) {
                acc += v1.Value[i] * v2.Value[i];
            }
            return acc;
        }

        public static decimal Similarity(Vector v1, Vector v2) {
            return DotProduct(v1, v2) / (Abs(v1) * Abs(v2));
        }

        public static float Coefficient(Vector v1, Vector v2) {
            var setv1 = new HashSet<decimal>(v1.Value);
            var setv2 = new HashSet<decimal>(v2.Value);

            float un = setv1.Union(setv2).Count();
            float @int = setv1.Intersect(setv2).Count();
            return @int / un;
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if( obj is Vector) {
                return ((Vector)(obj)) == this;
            }
            return false;
        }

        public override int GetHashCode() {
            return Collections.GetHashCode(Value);
        }

        public int Compare(Vector x, Vector y) {
            Contract.Requires(x != null && y != null);

            if (Abs(x) > Abs(y)) return 1;
            else if (Abs(x) == Abs(y)) return 0;
            else return -1;
        }

        public static bool operator == (Vector left, Vector right) {
            return Abs(left) == Abs(right);
        }

        public static bool operator != (Vector left, Vector right) {
            return !(left == right);
        }
    }
}
