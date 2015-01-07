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

namespace Epicycle.Math.Geometry
{
    [TestFixture]
    public sealed class SymmetricMatrix3Test : AssertionHelper
    {
        private const double _tolerance = 1e-9;

        [Test, Sequential]
        public void Eigenvectors_and_eigenvalues_of_satisfy_defining_equations(
            [Values(1, 0.577, 0.577, 0.577, 0.577, 0.577, 0.577)] double xx,
            [Values(1, 2.718, 2.718, 0.577, 0.577, 0.577, 0.577)] double yy,
            [Values(1, 3.141, 3.141, 3.141, 0.577, 3.141, 0.577)] double zz,
            [Values(0, 0,     1.618, 1e-40, 1e-40, 1e-60, 1e-60)] double xy,
            [Values(0, 0,     2.665, 2e-40, 2e-40, 2e-60, 2e-60)] double yz,
            [Values(0, 0,     4.669, 3e-40, 3e-40, 3e-60, 3e-60)] double zx)
        {
            var mat = new SymmetricMatrix3(xx, yy, zz, xy, yz, zx);

            var loVal = mat.LowestEigenvalue();
            var loVec = mat.LowestEigenvector();

            Expect((mat - SymmetricMatrix3.Scalar(loVal)).Det(), Is.EqualTo(0).Within(_tolerance));
            Expect(Vector3.Distance(mat * loVec, loVal * loVec), Is.LessThan(_tolerance));
        }
    }
}
