using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Epicycle.Commons;
using Epicycle.Commons.Collections;
using Epicycle.Math.Geometry.Polytopes;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class TestUtils
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
