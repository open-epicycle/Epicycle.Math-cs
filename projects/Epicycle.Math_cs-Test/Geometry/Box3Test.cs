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
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    [TestFixture]
    public sealed class Box3Test : AssertionHelper
    {
        private double _tolerance = 1e-10;

        [Test]
        public void Intersection_of_two_Box3_is_correct()
        {
            var a = new Box3(minCorner: new Vector3(2.72, 3.14, 1.23), dimensions: new Vector3(1.0, 2.0, 3.0));
            var b = new Box3(minCorner: new Vector3(3.5, 2.5, 3.21), dimensions: new Vector3(2.0, 1.5, 1.0));

            var c = Box3.Intersection(a, b);

            Expect(c.MinCorner.X, Is.EqualTo(3.5).Within(_tolerance));
            Expect(c.MinCorner.Y, Is.EqualTo(3.14).Within(_tolerance));
            Expect(c.MinCorner.Z, Is.EqualTo(3.21).Within(_tolerance));

            Expect(c.MaxCorner.X, Is.EqualTo(3.72).Within(_tolerance));
            Expect(c.MaxCorner.Y, Is.EqualTo(4.0).Within(_tolerance));
            Expect(c.MaxCorner.Z, Is.EqualTo(4.21).Within(_tolerance));
        }

        [Test]
        public void Intersection_of_Box3_with_empty_Box3_is_empty()
        {
            var a = new Box3(minCorner: new Vector3(2.72, 3.14, 1.23), dimensions: new Vector3(1.0, 2.0, 3.0));

            var c = Box3.Intersection(a, Box3.Empty);

            Expect(c.IsEmpty);
        }

        [Test]
        public void Hull_of_two_Box3_is_correct()
        {
            var a = new Box3(minCorner: new Vector3(2.72, 3.14, 1.23), dimensions: new Vector3(1.0, 2.0, 3.0));
            var b = new Box3(minCorner: new Vector3(3.5, 2.5, 3.21), dimensions: new Vector3(2.0, 1.5, 1.0));

            var c = Box3.Hull(a, b);

            Expect(c.MinCorner.X, Is.EqualTo(2.72).Within(_tolerance));
            Expect(c.MinCorner.Y, Is.EqualTo(2.5).Within(_tolerance));
            Expect(c.MinCorner.Z, Is.EqualTo(1.23).Within(_tolerance));

            Expect(c.MaxCorner.X, Is.EqualTo(5.5).Within(_tolerance));
            Expect(c.MaxCorner.Y, Is.EqualTo(5.14).Within(_tolerance));
            Expect(c.MaxCorner.Z, Is.EqualTo(4.23).Within(_tolerance));
        }

        [Test]
        public void Hull_of_Box3_with_empty_Box3_is_original_Box3()
        {
            var a = new Box3(minCorner: new Vector3(2.72, 3.14, 1.23), dimensions: new Vector3(1.0, 2.0, 3.0));

            var c = Box3.Hull(a, Box3.Empty);
            ExpectAlmostEqualBoxes(a, c, _tolerance);

            c = Box3.Hull(Box3.Empty, a);
            ExpectAlmostEqualBoxes(a, c, _tolerance);
        }

        [Test]
        public void Contains_returns_true_when_this_Box3_contains_the_other_Box3()
        {
            var a = new Box3(minCorner: new Vector3(2.72, 3.14, 1.23), dimensions: new Vector3(1.0, 2.0, 3.0));
            var b = new Box3(minCorner: new Vector3(3.6, 3.5, 3.21), dimensions: new Vector3(0.1, 1.5, 1.0));

            Expect(a.Contains(b));
        }

        [Test]
        public void Contains_returns_false_when_this_Box2_doesnt_contain_the_other_Box2()
        {
            var a = new Box3(minCorner: new Vector3(2.72, 3.14, 1.23), dimensions: new Vector3(1.0, 2.0, 3.0));
            var b = new Box3(minCorner: new Vector3(3.6, 3.5, 3.21), dimensions: new Vector3(0.1, 1.5, 1.03));

            Expect(!a.Contains(b));
        }

        [Test]
        public void Any_Box2_Contains_any_empty_Box2()
        {
            var a = new Box3(minCorner: new Vector3(2.72, 3.14, 1.23), dimensions: new Vector3(1.0, 2.0, 3.0));

            Expect(a.Contains(Box3.Empty));
        }

        [Test]
        public void Hull_of_a_sequence_of_Vector2_is_correct()
        {
            var points = new List<Vector3>()
            {
                new Vector3(2.72, 3.14, 1.0),
                new Vector3(3.72, 2.14, 0.1),
                new Vector3(4.72, 1.14, 2.0),
                new Vector3(4.02, 4.14, -1.0)
            };

            var c = Box3.Hull(points);

            Expect(c.MinCorner.X, Is.EqualTo(2.72).Within(_tolerance));
            Expect(c.MinCorner.Y, Is.EqualTo(1.14).Within(_tolerance));
            Expect(c.MinCorner.Z, Is.EqualTo(-1.0).Within(_tolerance));
            Expect(c.MaxCorner.X, Is.EqualTo(4.72).Within(_tolerance));
            Expect(c.MaxCorner.Y, Is.EqualTo(4.14).Within(_tolerance));
            Expect(c.MaxCorner.Z, Is.EqualTo(2.0).Within(_tolerance));
        }

        private void ExpectAlmostEqualBoxes(Box3 a, Box3 b, double _tolerance)
        {
            Expect(a.MinCorner.X, Is.EqualTo(b.MinCorner.X).Within(_tolerance));
            Expect(a.MinCorner.Y, Is.EqualTo(b.MinCorner.Y).Within(_tolerance));
            Expect(a.MinCorner.Z, Is.EqualTo(b.MinCorner.Z).Within(_tolerance));

            Expect(a.MaxCorner.X, Is.EqualTo(b.MaxCorner.X).Within(_tolerance));
            Expect(a.MaxCorner.Y, Is.EqualTo(b.MaxCorner.Y).Within(_tolerance));
            Expect(a.MaxCorner.Z, Is.EqualTo(b.MaxCorner.Z).Within(_tolerance));
        }
    }
}
