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
    public sealed class Matrix3Test : AssertionHelper
    {
        const double _tolerance = 1e-9;

        [Test]
        public void FitProjective_returns_a_Matrix3_applying_which_to_box_vertices_yields_given_points()
        {
            var box = new Box2(1, 2, 3, 4);

            var points = new Vector2[]
            {
                new Vector2(3.141, 2.718),
                new Vector2(0.577, 3.256),
                new Vector2(1.123, 0.248),
                new Vector2(2.618, 1.722)
            };

            var mat = Matrix3Utils.FitProjective(box, points);

            var actualPoints = box.Vertices.Select(v => mat.ApplyProjective(v)).ToArray();

            for (var i = 0; i < 4; i++)
            {
                Expect(Vector2.Distance(actualPoints[i], points[i]) < _tolerance);
            }
        }
    }
}
