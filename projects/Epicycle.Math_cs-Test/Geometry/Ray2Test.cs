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
    public sealed class Ray2Test : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        private void ExpectMeetingPointFor4Rotations(Ray2 ray, Box2 box, Vector2 expectedPoint)
        {
            var point = ray.MeetingPoint(box);

            Expect(point.HasValue);
            Expect(Vector2.Distance(point.Value, expectedPoint), Is.LessThan(_tolerance));
            
            for (int i = 0; i < 3; i++)
            {
                ray = ray.Rotate90();
                box = box.Rotate90();

                point = ray.MeetingPoint(box);
                expectedPoint = expectedPoint.Rotate90();

                Expect(point.HasValue);
                Expect(Vector2.Distance(point.Value, expectedPoint), Is.LessThan(_tolerance));
            }
        }

        [Test]
        public void Ray2_MeetingPoint_with_Box2_is_correct_for_Ray2_Origin_inside_Box2()
        {
            var ray = new Ray2(origin: new Vector2(1, 2), direction: new Vector2(1, -1));
            var box = new Box2(minCorner: new Vector2(0, 0), dimensions: new Vector2(2, 3));

            ExpectMeetingPointFor4Rotations(ray, box, expectedPoint: new Vector2(2, 1));
        }

        [Test]
        public void Ray2_MeetingPoint_with_Box2_is_correct_for_Ray2_Origin_outside_Box2_and_nonempty_intersection()
        {
            var ray = new Ray2(origin: new Vector2(4, 2), direction: new Vector2(-2, -1));
            var box = new Box2(minCorner: new Vector2(0, 0), dimensions: new Vector2(2, 3));

            ExpectMeetingPointFor4Rotations(ray, box, expectedPoint: new Vector2(2, 1));
        }

        [Test]
        public void Ray2_MeetingPoint_with_Box2_is_null_when_there_is_no_intersection()
        {
            var ray = new Ray2(origin: new Vector2(4, 2), direction: new Vector2(-1, 1));
            var box = new Box2(minCorner: new Vector2(0, 0), dimensions: new Vector2(2, 3));

            var point = ray.MeetingPoint(box);

            Expect(point, Is.Null);

            for (int i = 0; i < 3; i++)
            {
                ray = ray.Rotate90();
                box = box.Rotate90();

                point = ray.MeetingPoint(box);

                Expect(point, Is.Null);
            }
        }

        [Test]
        public void Ray2_MeetingPoint_with_Box2_is_correct_when_ray_is_parallel_to_axis()
        {
            var box = new Box2(minCorner: new Vector2(0, 0), dimensions: new Vector2(2, 2));

            var ray1 = new Ray2(origin: new Vector2(-1, 1), direction: new Vector2(1, 0));
            var point1 = ray1.MeetingPoint(box);

            Expect(point1.HasValue);
            Expect(Vector2.Distance(point1.Value, new Vector2(0, 1)), Is.LessThan(_tolerance));

            var ray2 = new Ray2(origin: new Vector2(1, -1), direction: new Vector2(0, 1));
            var point2 = ray2.MeetingPoint(box);

            Expect(point2.HasValue);
            Expect(Vector2.Distance(point2.Value, new Vector2(1, 0)), Is.LessThan(_tolerance));

            var ray3 = new Ray2(origin: new Vector2(-1, 2.1), direction: new Vector2(1, 0));
            var point3 = ray3.MeetingPoint(box);

            Expect(point3, Is.Null);

            var ray4 = new Ray2(origin: new Vector2(-0.1, -0.1), direction: new Vector2(0, 1));
            var point4 = ray4.MeetingPoint(box);

            Expect(point4, Is.Null);
        }

        [Test]
        public void Ray2_MeetingPoint_with_Box2_is_null_when_Box2_is_empty()
        {
            var ray1 = new Ray2(origin: new Vector2(-1, 2.1), direction: new Vector2(3.14, -0.88));
            Expect(ray1.MeetingPoint(Box2.Empty), Is.Null);

            var ray2 = new Ray2(origin: new Vector2(-1, 2.1), direction: new Vector2(3.14, 0));
            Expect(ray2.MeetingPoint(Box2.Empty), Is.Null);

            var ray3 = new Ray2(origin: new Vector2(-1, 2.1), direction: new Vector2(0, -0.88));
            Expect(ray3.MeetingPoint(Box2.Empty), Is.Null);
        }
    }
}
