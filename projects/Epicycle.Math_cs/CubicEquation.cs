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

using Epicycle.Commons;
using System.Numerics;

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
