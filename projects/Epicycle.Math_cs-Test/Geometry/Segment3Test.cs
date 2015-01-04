using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    using System;

    [TestFixture]
    public sealed class Segment3Test : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        [Test]
        public void Cropping_Segment3_by_plane_produces_correct_segment_when_start_point_is_on_positive_side_and_end_point_is_on_negative_side()
        {
            var segment = new Segment3(new Vector3(3.14, 2.88, -1.77), new Vector3(0.52, -5.08, 1.23));

            var point = segment.PointAt(0.256);

            var plane = new Plane(point, normal: new Vector3(-0.66, -1.44, 0.87));

            var crop = segment.CropBy(plane);

            Expect(crop != null);
            Expect(Vector3.Distance(crop.Value.Start, segment.Start), Is.LessThan(_tolerance));
            Expect(Vector3.Distance(crop.Value.End, point), Is.LessThan(_tolerance));
        }

        [Test]
        public void Cropping_Segment3_by_plane_produces_correct_segment_when_start_point_is_on_negative_side_and_end_point_is_on_positive_side()
        {
            var segment = new Segment3(new Vector3(3.14, 2.88, -1.77), new Vector3(0.52, -5.08, 1.23));

            var point = segment.PointAt(0.256);

            var plane = new Plane(point, normal: new Vector3(0.66, 1.44, -0.87));

            var crop = segment.CropBy(plane);

            Expect(crop != null);
            Expect(Vector3.Distance(crop.Value.Start, point), Is.LessThan(_tolerance));
            Expect(Vector3.Distance(crop.Value.End, segment.End), Is.LessThan(_tolerance));
        }

        [Test]
        public void Cropping_Segment3_by_plane_produces_original_segment_when_entire_segment_is_on_positive_side()
        {
            var segment = new Segment3(new Vector3(3.14, 2.88, -1.77), new Vector3(0.52, -5.08, 1.23));

            var point = segment.PointAt(-0.256);

            var plane = new Plane(point, normal: new Vector3(0.66, 1.44, -0.87));

            var crop = segment.CropBy(plane);

            Expect(crop != null);
            Expect(Vector3.Distance(crop.Value.Start, segment.Start), Is.LessThan(_tolerance));
            Expect(Vector3.Distance(crop.Value.End, segment.End), Is.LessThan(_tolerance));
        }

        [Test]
        public void Cropping_Segment3_by_plane_produces_null_when_entire_segment_is_on_negative_side()
        {
            var segment = new Segment3(new Vector3(3.14, 2.88, -1.77), new Vector3(0.52, -5.08, 1.23));

            var point = segment.PointAt(1.256);

            var plane = new Plane(point, normal: new Vector3(0.66, 1.44, -0.87));

            var crop = segment.CropBy(plane);

            Expect(crop == null);
        }

        [Test]
        public void Distance_of_point_to_vertical_segment_intersecting_points_height_level_equals_distance_of_point_to_intersection_point()
        {
            var p = new Vector3(3.141, 2.718, 0.577);

            var displacement = new Vector2(0.123, 0.256);

            var q0 = p + new Vector3(displacement, 0.482);
            var q1 = p + new Vector3(displacement, -1.234);

            var segment = new Segment3(q0, q1);

            Expect(p.DistanceTo(segment), Is.EqualTo(displacement.Norm).Within(_tolerance));
        }

        [Test]
        public void Distance_of_point_to_vertical_segment_above_points_height_level_equals_distance_of_point_to_lower_segment_end()
        {
            var p = new Vector3(3.141, 2.718, 0.577);

            var displacement = new Vector2(0.123, 0.256);

            var q0 = p + new Vector3(displacement, 0.482);
            var q1 = p + new Vector3(displacement, 1.234);

            var segment = new Segment3(q0, q1);

            Expect(p.DistanceTo(segment), Is.EqualTo(Vector3.Distance(p, q0)).Within(_tolerance));
        }

        [Test]
        public void Distance_of_point_to_vertical_segment_below_points_height_level_equals_distance_of_point_to_higher_segment_end()
        {
            var p = new Vector3(3.141, 2.718, 0.577);

            var displacement = new Vector2(0.123, 0.256);

            var q0 = p + new Vector3(displacement, -0.482);
            var q1 = p + new Vector3(displacement, -1.234);

            var segment = new Segment3(q0, q1);

            Expect(p.DistanceTo(segment), Is.EqualTo(Vector3.Distance(p, q0)).Within(_tolerance));
        }

        [Test]
        public void IntersectionWithPlaneXY_of_Segment3_with_endpoints_collinear_with_a_point_on_plane_XY_on_different_sides_of_the_plane_equals_to_the_point()
        {
            var pointXY = new Vector3(3.141, 2.718, 0);

            var delta = new Vector3(0.256, 0.777, 0.577);

            var point1 = pointXY + 0.124 * delta;
            var point2 = pointXY - 0.369 * delta;

            var intersection = new Segment3(point1, point2).IntersectionWithPlaneXY();

            Expect(intersection, Is.Not.Null);
            Expect(Vector2.Distance(intersection.Value, pointXY.XY), Is.LessThan(_tolerance));
        }

        [Test]
        public void IntersectionWithPlaneXY_of_Segment3_with_endpoints_on_the_same_side_of_plane_XY_is_null()
        {
            var point1 = new Vector3(-3.141, 2.718, 0.577);
            var point2 = new Vector3(0.256, -1.234, 0.676);

            var intersection = new Segment3(point1, point2).IntersectionWithPlaneXY();

            Expect(intersection, Is.Null);
        }
    }
}
