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
    public sealed class SymmetricMatrix2Test : AssertionHelper
    {
        private const double _tolerance = 1e-9;

        [Test, Sequential]
        public void Eigenvectors_and_eigenvalues_of_satisfy_defining_equations
            ([Values(0.577, 0.577, 0.577, 0.577)] double xx,
             [Values(2.718, 0.577, 0.577, 0.577)] double yy,
             [Values(3.141, 0,     1e-40, 1e-60)] double xy)
        {
            var mat = new SymmetricMatrix2(xx, yy, xy);

            var loVal = mat.LowEigenvalue();
            var loVec = mat.LowEigenvector();

            Expect((mat - SymmetricMatrix2.Scalar(loVal)).Det(), Is.EqualTo(0).Within(_tolerance));
            Expect(Vector2.Distance(mat * loVec, loVal * loVec), Is.LessThan(_tolerance));

            var hiVal = mat.HighEigenvalue();
            var hiVec = mat.HighEigenvector();

            Expect((mat - SymmetricMatrix2.Scalar(hiVal)).Det(), Is.EqualTo(0).Within(_tolerance));
            Expect(Vector2.Distance(mat * hiVec, hiVal * hiVec), Is.LessThan(_tolerance));

            Expect(loVal, Is.LessThan(hiVal + _tolerance));
            Expect(loVal + hiVal, Is.EqualTo(mat.Trace()).Within(_tolerance));
        }
    }
}
