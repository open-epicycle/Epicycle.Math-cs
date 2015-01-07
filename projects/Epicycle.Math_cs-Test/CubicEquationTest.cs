// [[[[INFO>
// Copyright 2015 Epicycle (http://epicycle.org, https://github.com/open-epicycle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/open-epicycle/Epicycle.Math-cs
// ]]]]

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
