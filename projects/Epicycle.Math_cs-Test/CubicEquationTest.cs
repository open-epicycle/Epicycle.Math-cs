using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Epicycle.Math
{
    [TestFixture]
    public class CubicEquationTest : AssertionHelper
    {
        private const double _tolerance = 1e-9;

        [Test]
        public void The_roots_of_a_cubic_polynomial_which_decomposes_into_linear_factors_are_the_roots_of_the_factors()
        {
            var x1 = 0.577;
            var x2 = 2.718;
            var x3 = 3.141;

            var a = 1.618;
            var b = -a * (x1 + x2 + x3);
            var c = a * (x1 * x2 + x2 * x3 + x3 * x1);
            var d = -a * x1 * x2 * x3;

            var roots = CubicEquation.Solve(a, b, c, d).ToList();
            roots.Sort();

            Expect(roots[0], Is.EqualTo(x1).Within(_tolerance));
            Expect(roots[1], Is.EqualTo(x2).Within(_tolerance));
            Expect(roots[2], Is.EqualTo(x3).Within(_tolerance));
        }
    }
}
