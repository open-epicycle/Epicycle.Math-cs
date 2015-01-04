using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Epicycle.Commons;

namespace Epicycle.Math
{
    using System;

    public static class CubicEquation
    {
        private readonly static Complex u = new Complex(-1d / 2, Math.Sqrt(3) / 2);
        private readonly static Complex v = new Complex(-1d / 2, -Math.Sqrt(3) / 2);

        // assumes all roots are real
        public static double[] Solve(double a, double b, double c, double d)
        {
            var b2 = b * b;
            var b3 = b * b2;
            var ac = a * c;
            var abc = ac * b;
            var a2d = a * a * d;

            var delta0 = b2 - 3 * ac;
            var delta1 = 2 * b3 - 9 * abc + 27 * a2d;

            var z = new Complex(delta1 / 2, BasicMath.Sqrt(4 * delta0 * delta0 * delta0 - delta1 * delta1) / 2);
            var w = 2 * Complex.Pow(z, 1d / 3);

            var threeA = 3 * a;

            return new double[]
            {
                -(b + w.Real) / threeA,
                -(b + (u * w).Real) / threeA,
                -(b + (v * w).Real) / threeA
            };
        }
    }
}
