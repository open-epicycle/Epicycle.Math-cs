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

namespace Epicycle.Math.Geometry
{
    [TestFixture]
    public sealed class Box2Test : AssertionHelper
    {
        private double _tolerance = 1e-10;

        [Test]
        public void Intersection_of_two_Box2_is_correct()
        {
            var a = new Box2(2.72, 3.14, 1.0, 2.0);
            var b = new Box2(3.5, 2.5, 2.0, 1.5);

            var c = Box2.Intersection(a, b);

            Expect(c.MinCorner.X, Is.EqualTo(3.5).Within(_tolerance));
            Expect(c.MinCorner.Y, Is.EqualTo(3.14).Within(_tolerance));
            Expect(c.MaxCorner.X, Is.EqualTo(3.72).Within(_tolerance));
            Expect(c.MaxCorner.Y, Is.EqualTo(4.0).Within(_tolerance));
        }

        [Test]
        public void Intersection_of_Box2_with_empty_Box2_is_empty()
        {
            var a = new Box2(-2.72, -3.14, 2.0, 4.0);

            var c = Box2.Intersection(a, Box2.Empty);

            Expect(c.IsEmpty);
        }

        [Test]
        public void Hull_of_two_Box2_is_correct()
        {
            var a = new Box2(2.72, 3.14, 1.0, 2.0);
            var b = new Box2(3.5, 2.5, 2.0, 1.5);

            var c = Box2.Hull(a, b);

            Expect(c.MinCorner.X, Is.EqualTo(2.72).Within(_tolerance));
            Expect(c.MinCorner.Y, Is.EqualTo(2.5).Within(_tolerance));
            Expect(c.MaxCorner.X, Is.EqualTo(5.5).Within(_tolerance));
            Expect(c.MaxCorner.Y, Is.EqualTo(5.14).Within(_tolerance));
        }

        [Test]
        public void Hull_of_Box2_with_empty_Box2_is_original_Box2()
        {
            var a = new Box2(2.72, 3.14, 1.0, 2.0);

            var c = Box2.Hull(a, Box2.Empty);
            ExpectAlmostEqualBoxes(a, c, _tolerance);

            c = Box2.Hull(Box2.Empty, a);
            ExpectAlmostEqualBoxes(a, c, _tolerance);
        }

        [Test]
        public void Contains_returns_true_when_this_Box2_contains_the_other_Box2()
        {
            var a = new Box2(2.72, 3.14, 1.0, 2.0);
            var b = new Box2(3.0, 3.5, 0.5, 1.5);

            Expect(a.Contains(b));
        }

        [Test]
        public void Contains_returns_false_when_this_Box2_doesnt_contain_the_other_Box2()
        {
            var a = new Box2(2.72, 3.14, 1.0, 2.0);
            var b = new Box2(3.0, 3.5, 0.8, 1.5);

            Expect(!a.Contains(b));
        }

        [Test]
        public void Any_Box2_Contains_any_empty_Box2()
        {
            var a = new Box2(2.72, 3.14, 1.0, 2.0);

            Expect(a.Contains(Box2.Empty));
        }

        [Test]
        public void Hull_of_a_sequence_of_Vector2_is_correct()
        {
            var points = new List<Vector2>()
            {
                new Vector2(2.72, 3.14),
                new Vector2(3.72, 2.14),
                new Vector2(4.72, 1.14),
                new Vector2(4.02, 4.14)
            };

            var c = Box2.Hull(points);

            Expect(c.MinCorner.X, Is.EqualTo(2.72).Within(_tolerance));
            Expect(c.MinCorner.Y, Is.EqualTo(1.14).Within(_tolerance));
            Expect(c.MaxCorner.X, Is.EqualTo(4.72).Within(_tolerance));
            Expect(c.MaxCorner.Y, Is.EqualTo(4.14).Within(_tolerance));
        }

        private void ExpectAlmostEqualBoxes(Box2 a, Box2 b, double _tolerance)
        {
            Expect(a.MinCorner.X, Is.EqualTo(b.MinCorner.X).Within(_tolerance));
            Expect(a.MinCorner.Y, Is.EqualTo(b.MinCorner.Y).Within(_tolerance));
            Expect(a.MaxCorner.X, Is.EqualTo(b.MaxCorner.X).Within(_tolerance));
            Expect(a.MaxCorner.Y, Is.EqualTo(b.MaxCorner.Y).Within(_tolerance));
        }

        [Test]
        public void DistanceTo_of_Vector2_and_Box2_is_correct
            ([ValueSource(typeof(GeometryTestUtils), "Distance_of_Vector2_and_Box2_TestCases")] GeometryTestUtils.Distance_of_Vector2_and_Box2_TestCase testCase)
        {
            var result = testCase.Point.DistanceTo(testCase.Box);

            Expect(result, Is.EqualTo(testCase.Distance).Within(_tolerance));
        }
    }
}
