using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    [TestFixture]
    public sealed class Ray3Test : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        [Test]
        public void IntersectionWithPlaneXY_returns_point_which_lies_on_ray()
        {
            var ray = new Ray3(origin: new Vector3(3.141, 2.718, 0.577), direction: new Vector3(1.23, 4.56, -3.21));

            var result = ray.IntersectWithPlaneXY();

            Expect(result, Is.Not.Null);
            Expect(((Vector3)result - ray.Origin).Cross(ray.Direction).Norm, Is.LessThan(_tolerance));
        }

        [Test]
        public void IntersectionWithPlaneXY_returns_null_when_ray_doesnt_intersect_plane_xy()
        {
            var ray = new Ray3(origin: new Vector3(3.141, 2.718, -0.577), direction: new Vector3(1.23, 4.56, -3.21));

            var result = ray.IntersectWithPlaneXY();

            Expect(result, Is.Null);
        }

        [Test]
        public void Intersect_with_Plane_passing_through_given_point_on_ray_returns_the_same_point()
        {
            var ray = new Ray3(origin: new Vector3(3.141, 2.718, 0.577), direction: new Vector3(1.23, 4.56, -3.21));

            var point = ray.PointAt(8.555);

            var plane = new Plane(point, normal: new Vector3(0.77, -0.84, 1.35));

            var result = ray.Intersect(plane);

            Expect(result, Is.Not.Null);
            Expect(Vector3.Distance(result, point), Is.LessThan(_tolerance));
        }

        [Test]
        public void Intersect_with_Plane_passing_through_a_point_on_ray_continuation_is_null()
        {
            var ray = new Ray3(origin: new Vector3(3.141, 2.718, 0.577), direction: new Vector3(1.23, 4.56, -3.21));

            var point = ray.PointAt(-8.555);

            var plane = new Plane(point, normal: new Vector3(0.77, -0.84, 1.35));

            var result = ray.Intersect(plane);

            Expect(result, Is.Null);
        }
    }
}
