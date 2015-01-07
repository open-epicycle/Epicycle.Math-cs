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

using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Epicycle.Math.Geometry.Polytopes
{
    using System;

    [TestFixture]
    public sealed class Polygon3Test : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        [Test]
        public void CropBy_Plane_with_two_plane_orientations_yields_polygons_lying_on_respective_halfspaces_whose_area_sum_is_the_area_of_the_original_polygon()
        {
            var rectangles = new List<IPolygon>
            {
                new Box2(minX: 0,  minY: 0,  width: 2, height: 2),
                new Box2(minX: -1, minY: -1, width: 4, height: 2),
                new Box2(minX: -1, minY: -1, width: 2, height: 4)
            };

            var polygon2 = PolygonUtils.Union(rectangles);
            var cs = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var polygon3 = new Polygon3(cs, polygon2);

            var point1 = polygon3.PlaneToSpace(new Vector2(0.1, 0.2));
            var point2 = polygon3.PlaneToSpace(new Vector2(0.9, 0.7));
            var point3 = new Vector3(3.141, 2.718, 0.577);

            var plane = Plane.Through(point1, point2, point3);

            var half1 = polygon3.CropBy(plane);
            var half2 = polygon3.CropBy(plane.Reverse);

            Expect(half1.IsOnNegativeSide(Plane.ParallelTo(plane, _tolerance)));
            Expect(half2.IsOnPositiveSide(Plane.ParallelTo(plane, -_tolerance)));

            Expect(half1.InPlane.Area() + half2.InPlane.Area(), Is.EqualTo(polygon3.InPlane.Area()).Within(_tolerance));
        }

        [Test]
        public void CreateTriangle_returns_triangle_with_given_vertices()
        {
            var a = new Vector3(3.141, 2.718, 0.577);
            var b = new Vector3(1.414, 1.732, 2.236);
            var c = new Vector3(0.693, 1.202, 0.739);

            var triangle = Polygon3Utils.CreateTriangle(a, b, c);

            Expect(triangle.Contours().Count(), Is.EqualTo(1));

            var vertices = triangle.Vertices();

            Expect(vertices.Count(), Is.EqualTo(3));
           
            Expect(vertices.Select(v => Vector3.Distance(a, v)).Min(), Is.LessThan(_tolerance));
            Expect(vertices.Select(v => Vector3.Distance(b, v)).Min(), Is.LessThan(_tolerance));
            Expect(vertices.Select(v => Vector3.Distance(c, v)).Min(), Is.LessThan(_tolerance));
        }
    }
}
