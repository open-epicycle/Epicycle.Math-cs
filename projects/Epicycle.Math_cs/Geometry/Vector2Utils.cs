using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Vector2Utils
    {
        public static bool AreCcw(Vector2 u, Vector2 w)
        {
            return u.Cross(w) > 0;
        }

        public static bool AreCcw(Vector2 u, Vector2 v, Vector2 w)
        {
            var side1 = AreCcw(u, v);
            var side2 = AreCcw(v, w);

            if (side1 && side2)
            {
                return true;
            }

            if (!side1 && !side2)
            {
                return false;
            }

            return AreCcw(w, u);
        }

        public static double Angle(Vector2 a, Vector2 b, Vector2 c)
        {
            return Vector2.Angle(a - b, c - b);
        }

        public static double AngleWithX(this Vector2 @this)
        {
            return Math.Atan2(@this.Y, @this.X);
        }

        public static Vector2 UnitVector(double angle)
        {
            return new Vector2(Math.Cos(angle), Math.Sin(angle));
        }

        // finds least-squares solution to a system of orthogonality equations (i.e. result is fitted to be orthogonal to vectors)
        public static void FitOrthogonal(IEnumerable<Vector2> vectors, out Vector2 result, out double residual)
        {
            var mat = SymmetricMatrix2.TensorSquare(vectors.First());

            foreach (var vector in vectors.Skip(1))
            {
                mat += SymmetricMatrix2.TensorSquare(vector);
            }

            mat.ComputeLowEigendata(out residual, out result);
        }

        public static Vector2 FitOrthogonal(IEnumerable<Vector2> vectors)
        {
            double residual;
            Vector2 answer;

            FitOrthogonal(vectors, out answer, out residual);

            return answer;
        }

        public sealed class YamlSerialization
        {
            public double X { get; set; }
            public double Y { get; set; }

            public YamlSerialization() { }

            public YamlSerialization(Vector2 v)
            {
                X = v.X;
                Y = v.Y;
            }

            public Vector2 Deserialize()
            {
                return new Vector2(X, Y);
            }
        }
    }
}
