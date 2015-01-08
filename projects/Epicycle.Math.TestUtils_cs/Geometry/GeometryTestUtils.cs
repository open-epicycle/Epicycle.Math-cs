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

using Epicycle.Commons.Collections;
using Epicycle.Math.Geometry;
using Epicycle.Math.Geometry.Polytopes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.TestUtils.Geometry
{
    using System;

    public static class GeometryTestUtils
    {
        public static void ExpectClosedPolylinesAreEqual(IClosedPolyline3 line1, IClosedPolyline3 line2, double tolerance)
        {
            Assert.That(line1.Vertices.Count, Is.EqualTo(line2.Vertices.Count));

            foreach (var i in Enumerable.Range(0, line2.Vertices.Count))
            {
                if (Vector3.Distance(line1.Vertices[i], line2.Vertices[0]) < tolerance)
                {
                    foreach (int j in Enumerable.Range(0, line1.Vertices.Count))
                    {
                        Assert.That(Vector3.Distance(line1.Vertices[j], line2.Vertices.ElementAtCyclic(j - i)), Is.LessThan(tolerance));
                    }

                    return;
                }
            }

            Assert.Fail();
        }

        public struct Distance_of_Vector2_and_Box2_TestCase
        {
            public Vector2 Point { get; set; }
            public Box2 Box { get; set; }
            public double Distance { get; set; }
            public Vector2 ClosestPoint { get; set; }
        }

        internal static IEnumerable<Distance_of_Vector2_and_Box2_TestCase> Distance_of_Vector2_and_Box2_TestCases()
        {
            yield return new Distance_of_Vector2_and_Box2_TestCase
            {
                Box = new Box2(new Interval(1, 2), new Interval(3, 4)),
                Point = new Vector2(0, 1),
                Distance = Math.Sqrt(5),
                ClosestPoint = new Vector2(1, 3)
            };

            yield return new Distance_of_Vector2_and_Box2_TestCase
            {
                Box = new Box2(new Interval(1, 2), new Interval(3, 4)),
                Point = new Vector2(4, 0),
                Distance = Math.Sqrt(13),
                ClosestPoint = new Vector2(2, 3)
            };

            yield return new Distance_of_Vector2_and_Box2_TestCase
            {
                Box = new Box2(new Interval(1, 2), new Interval(3, 4)),
                Point = new Vector2(4, 6),
                Distance = Math.Sqrt(8),
                ClosestPoint = new Vector2(2, 4)
            };

            yield return new Distance_of_Vector2_and_Box2_TestCase
            {
                Box = new Box2(new Interval(1, 2), new Interval(3, 4)),
                Point = new Vector2(0, 5),
                Distance = Math.Sqrt(2),
                ClosestPoint = new Vector2(1, 4)
            };

            yield return new Distance_of_Vector2_and_Box2_TestCase
            {
                Box = new Box2(new Interval(1, 2), new Interval(3, 4)),
                Point = new Vector2(2, 5.5),
                Distance = 1.5,
                ClosestPoint = new Vector2(2, 4)
            };

            yield return new Distance_of_Vector2_and_Box2_TestCase
            {
                Box = new Box2(new Interval(1, 2), new Interval(3, 4)),
                Point = new Vector2(2, -0.5),
                Distance = 3.5,
                ClosestPoint = new Vector2(2, 3)
            };

            yield return new Distance_of_Vector2_and_Box2_TestCase
            {
                Box = new Box2(new Interval(1, 2), new Interval(3, 4)),
                Point = new Vector2(0.9, 3),
                Distance = 0.1,
                ClosestPoint = new Vector2(1, 3)
            };

            yield return new Distance_of_Vector2_and_Box2_TestCase
            {
                Box = new Box2(new Interval(1, 2), new Interval(2, 4)),
                Point = new Vector2(3.141, 3),
                Distance = 1.141,
                ClosestPoint = new Vector2(2, 3)
            };

            yield return new Distance_of_Vector2_and_Box2_TestCase
            {
                Box = new Box2(new Interval(1, 2), new Interval(3, 4)),
                Point = new Vector2(1.1, 3.9),
                Distance = 0,
                ClosestPoint = new Vector2(1.1, 3.9)
            };
        }
    }    
}
