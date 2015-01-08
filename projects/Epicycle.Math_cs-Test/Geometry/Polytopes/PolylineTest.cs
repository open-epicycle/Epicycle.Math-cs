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

using Epicycle.Math.TestUtils.Geometry;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    using System;

    [TestFixture]
    public sealed class PolylineTest : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        [Test]
        public void ClosedPolyline_SignedArea_is_correct_for_horizontal_triangle()
        {
            var x = -0.13;
            var y = 3.14;
            var a = 1.44;
            var h = 2.72;

            IEnumerable<Vector2> triangleVertices = new List<Vector2>
            {
                new Vector2(x, y),
                new Vector2(5.555, y + h),
                new Vector2(x + a, y)
            };

            var area = a * h / 2;

            var negativeTriangle = new ClosedPolyline(triangleVertices.ToList());
            Expect(negativeTriangle.SignedArea(), Is.EqualTo(-area).Within(_tolerance));

            var positiveTriangle = new ClosedPolyline(triangleVertices.Reverse().ToList());
            Expect(positiveTriangle.SignedArea(), Is.EqualTo(area).Within(_tolerance));
        }

        [Test]
        public void WindingNumber_of_Box2_around_point_inside_it_is_plus_one()
        {
            var box = new Box2(new Interval(3.141, 5.718), new Interval(0.577, 1.234));

            var point = new Vector2(3.6927, 1.1719);

            Expect(box.WindingNumber(point), Is.EqualTo(1));
        }

        [Test]
        public void WindingNumber_of_reverse_Box2_around_point_inside_it_is_minus_one()
        {
            var box = new Box2(new Interval(3.141, 5.718), new Interval(0.577, 1.234));

            var point = new Vector2(3.6927, 1.1719);

            Expect(box.AsReverse().WindingNumber(point), Is.EqualTo(-1));
        }

        [Test]
        public void WindingNumber_of_Box2_around_point_outside_it_is_zero()
        {
            var box = new Box2(new Interval(3.141, 5.718), new Interval(0.577, 1.234));

            var point = new Vector2(2.7183, 1.1719);

            Expect(box.AsReverse().WindingNumber(point), Is.EqualTo(0));
        }

        [Test]
        public void DistanceTo_of_Vector2_and_IClosedPolyline_is_correct_for_a_Box2
            ([ValueSource(typeof(GeometryTestUtils), "Distance_of_Vector2_and_Box2_TestCases")] GeometryTestUtils.Distance_of_Vector2_and_Box2_TestCase testCase)
        {
            var result = testCase.Point.DistanceTo((IClosedPolyline)testCase.Box);

            if (testCase.Distance > 0) // skipping interior test cases in which Box2 behavior is different (it is considered to be filled)
            {
                Expect(result, Is.EqualTo(testCase.Distance).Within(_tolerance));
            }
        }

        [Test]
        public void FindClosestPoint_is_correct_for_a_Box2
            ([ValueSource(typeof(GeometryTestUtils), "Distance_of_Vector2_and_Box2_TestCases")] GeometryTestUtils.Distance_of_Vector2_and_Box2_TestCase testCase)
        {
            Vector2 closetPoint;
            double distance2;
            testCase.Box.FindClosestPoint(testCase.Point, out closetPoint, out distance2);

            if (testCase.Distance > 0) // skipping interior test cases in which Box2 behavior is different (it is considered to be filled)
            {
                Expect(Math.Sqrt(distance2), Is.EqualTo(testCase.Distance).Within(_tolerance));
                Expect(Vector2.Distance(closetPoint, testCase.ClosestPoint), Is.LessThan(_tolerance));
            }
        }
    }
}
