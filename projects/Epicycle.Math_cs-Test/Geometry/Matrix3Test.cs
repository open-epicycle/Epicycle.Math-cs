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
