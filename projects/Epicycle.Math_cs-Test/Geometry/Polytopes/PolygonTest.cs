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

using Epicycle.Commons;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    using System;

    [TestFixture]
    public sealed class PolygonTest : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        private ClosedPolyline Square(Vector2 corner, double side, bool positive)
        {
            var vertices = new List<Vector2>
            {
                corner,
                new Vector2(corner.X + side, corner.Y),
                new Vector2(corner.X + side, corner.Y + side),
                new Vector2(corner.X,        corner.Y + side)
            };

            if (!positive)
            {
                vertices.Reverse();
            }

            return new ClosedPolyline(vertices);
        }

        [Test]
        public void Area_is_correct_for_a_Polygon_with_axis_aligned_square_contours()
        {
            var corner1 = new Vector2(0.1, 0.2);
            var side1 = 4.5;

            var corner2 = new Vector2(0.9, 0.8);
            var side2 = 2.3;

            var corner3 = new Vector2(-5.1, -8.2);
            var side3 = 3.14;

            var builder = new PolygonBuilder();
            builder.AddContour(Square(corner1, side1, positive: true));
            builder.AddContour(Square(corner2, side2, positive: false));
            builder.AddContour(Square(corner3, side3, positive: true));

            var area = BasicMath.Sqr(side1) - BasicMath.Sqr(side2) + BasicMath.Sqr(side3);
            Expect(builder.ExtractPolygon().Area(), Is.EqualTo(area).Within(_tolerance));
        }

        [Test]
        public void Difference_between_square_p_and_square_q_contained_in_p_results_in_p_with_q_added_as_a_hole_contour()
        {
            var corner1 = new Vector2(0.1, 0.2);
            var side1 = 4.5;
            var bigPolygon = (Polygon)Square(corner1, side1, positive: true);

            var corner2 = new Vector2(0.9, 0.8);
            var side2 = 2.3;
            var smallPolygon = (Polygon)Square(corner2, side2, positive: true);

            var result = PolygonUtils.Difference(bigPolygon, smallPolygon);

            Expect(result.Contours.Count, Is.EqualTo(2));

            var contour0 = result.Contours.ElementAt(0);
            var contour1 = result.Contours.ElementAt(1);

            Expect(Math.Sign(contour0.SignedArea()) != Math.Sign(contour1.SignedArea()));

            var outContour = contour0.SignedArea() > 0 ? contour0 : contour1;
            var inContour = contour0.SignedArea() > 0 ? contour1 : contour0;

            Expect(outContour.Vertices.Count, Is.EqualTo(bigPolygon.CountVertices()));
            Expect(outContour.SignedArea(), Is.EqualTo(bigPolygon.Area()).Within(_tolerance));

            Expect(inContour.Vertices.Count, Is.EqualTo(smallPolygon.CountVertices()));
            Expect(inContour.SignedArea(), Is.EqualTo(-smallPolygon.Area()).Within(_tolerance));
        }

        [Test]
        public void Difference_of_two_empty_polygons_is_empty()
        {
            var p = Polygon.Empty;
            var q = Polygon.Empty;

            var result = PolygonUtils.Difference(p, q);

            Expect(result.IsEmpty);
        }

        [Test]
        public void Positive_Offset_of_a_square_is_correct()
        {
            var square = (Polygon)Square(corner: new Vector2(0, 0), side: 1.0, positive: true);

            var result = square.Offset(1.0);

            Expect(result.IsSimple);
            Expect(result.CountVertices(), Is.EqualTo(4));
            Expect(result.Area(), Is.EqualTo(9).Within(_tolerance));
        }

        [Test]
        public void Negative_Offset_of_a_square_is_correct()
        {
            var square = (Polygon)Square(corner: new Vector2(0, 0), side: 1.0, positive: true);

            var result = square.Offset(-0.25);

            Expect(result.IsSimple);
            Expect(result.CountVertices(), Is.EqualTo(4));
            Expect(result.Area(), Is.EqualTo(0.25).Within(_tolerance));
        }

        [Test]
        public void Offset_of_an_empty_polygon_is_empty()
        {
            var p = Polygon.Empty;

            var result = p.Offset(7);

            Expect(result.IsEmpty);
        }

        [Test]
        public void Intersection_of_3_rectangles_yields_the_correct_square()
        {
            var rectangles = new List<IPolygon>
            {
                new Box2(minX: 0,  minY: 0,  width: 2, height: 2),
                new Box2(minX: -1, minY: -1, width: 4, height: 2),
                new Box2(minX: -1, minY: -1, width: 2, height: 4)
            };

            var result = PolygonUtils.Intersection(rectangles);

            Expect(result.IsSimple);
            Expect(result.CountVertices(), Is.EqualTo(4));
            Expect(result.Area(), Is.EqualTo(1).Within(_tolerance));

            var resultBox = result.BoundingBox();
            Expect(PolygonUtils.Difference(resultBox, result).Area(), Is.EqualTo(0).Within(_tolerance));
            Expect(resultBox.Width, Is.EqualTo(1).Within(_tolerance));
            Expect(resultBox.Height, Is.EqualTo(1).Within(_tolerance));
            Expect(resultBox.MinCorner.X, Is.EqualTo(0).Within(_tolerance));
            Expect(resultBox.MinCorner.Y, Is.EqualTo(0).Within(_tolerance));
        }

        [Test]
        public void Union_of_3_rectangles_yields_the_correct_octagon()
        {
            var rectangles = new List<IPolygon>
            {
                new Box2(minX: 0,  minY: 0,  width: 2, height: 2),
                new Box2(minX: -1, minY: -1, width: 4, height: 2),
                new Box2(minX: -1, minY: -1, width: 2, height: 4)
            };

            var result = PolygonUtils.Union(rectangles);

            Expect(result.IsSimple);
            Expect(result.CountVertices(), Is.EqualTo(8));
            Expect(result.Area(), Is.EqualTo(13).Within(_tolerance));

            var resultBox = result.BoundingBox();
            Expect(resultBox.Width, Is.EqualTo(4).Within(_tolerance));
            Expect(resultBox.Height, Is.EqualTo(4).Within(_tolerance));
            Expect(resultBox.MinCorner.X, Is.EqualTo(-1).Within(_tolerance));
            Expect(resultBox.MinCorner.Y, Is.EqualTo(-1).Within(_tolerance));
        }

        private void ExpectPolygonsAreEqualWithinTolerance(IPolygon p, IPolygon q, double tolerance)
        {
            var pMinusQ = PolygonUtils.Difference(p, q);
            var qMinusP = PolygonUtils.Difference(q, p);

            Expect(qMinusP.Area(), Is.LessThan(tolerance));
            Expect(pMinusQ.Area(), Is.LessThan(tolerance));
        }

        private IPolygon MakeHoleIn(IPolygon p)
        {
            var hole = new Box2(minX: 1, minY: 1, width: 1, height: 1);

            return PolygonUtils.Difference(p, hole);
        }

        [Test]
        public void Closing_with_offset_above_required_removes_hole()
        {
            var square = new Box2(minX: 0, minY: 0, width: 3, height: 3);

            var polygon = MakeHoleIn(square);

            var result = polygon.Closing(0.51);

            ExpectPolygonsAreEqualWithinTolerance(result, square, _tolerance);
        }

        [Test]
        public void Closing_with_offset_below_required_doesnt_remove_hole()
        {
            var square = new Box2(minX: 0, minY: 0, width: 3, height: 3);

            var polygon = MakeHoleIn(square);

            var result = polygon.Closing(0.49);

            ExpectPolygonsAreEqualWithinTolerance(result, polygon, _tolerance);
        }

        [Test]
        public void RelativeClosing_with_offset_above_required_removes_hole()
        {
            var square = new Box2(minX: 0, minY: 0, width: 2.5, height: 3);

            var polygon = MakeHoleIn(square);

            var result = polygon.RelativeClosing(0.51 / Math.Max(square.Width, square.Height));

            ExpectPolygonsAreEqualWithinTolerance(result, square, _tolerance);
        }

        [Test]
        public void RelativeClosing_with_offset_below_required_doesnt_remove_hole()
        {
            var square = new Box2(minX: 0, minY: 0, width: 2.5, height: 3);

            var polygon = MakeHoleIn(square);

            var result = polygon.RelativeClosing(0.49 / Math.Max(square.Width, square.Height));

            ExpectPolygonsAreEqualWithinTolerance(result, polygon, _tolerance);
        }

        [Test]
        public void RelativeClosing_of_empty_polygon_is_empty()
        {
            var result = Polygon.Empty.RelativeClosing(0.1);

            Expect(result.IsEmpty);
        }

        private IPolygon AddComponentTo(IPolygon p)
        {
            var component = new Box2(minX: -2, minY: -2, width: 1, height: 1);

            return PolygonUtils.Union(p, component);
        }

        [Test]
        public void Opening_with_offset_above_required_removes_extra_component()
        {
            var square = new Box2(minX: 0, minY: 0, width: 3, height: 3);

            var polygon = AddComponentTo(square);

            var result = polygon.Opening(0.6);

            ExpectPolygonsAreEqualWithinTolerance(result, square, _tolerance);
        }

        [Test]
        public void Opening_with_offset_below_required_doesnt_remove_extra_component()
        {
            var square = new Box2(minX: 0, minY: 0, width: 3, height: 3);

            var polygon = AddComponentTo(square);

            var result = polygon.Opening(0.4);

            ExpectPolygonsAreEqualWithinTolerance(result, polygon, _tolerance);
        }

        [Test]
        public void CropBy_Line2_with_two_line_orientations_yields_polygons_lying_on_respective_halfplanes_whose_union_is_the_original_polygon()
        {
            var rectangles = new List<IPolygon>
            {
                new Box2(minX: 0,  minY: 0,  width: 2, height: 2),
                new Box2(minX: -1, minY: -1, width: 4, height: 2),
                new Box2(minX: -1, minY: -1, width: 2, height: 4)
            };

            var polygon = PolygonUtils.Union(rectangles);

            var point1 = new Vector2(0.1, 0.2);
            var point2 = new Vector2(0.9, 0.7);
            var line = Line2.Through(point1, point2);

            var half1 = polygon.CropBy(line);
            var half2 = polygon.CropBy(line.Reverse);

            Expect(half1.IsOnNegativeSide(Line2.ParallelTo(line, _tolerance)));
            Expect(half2.IsOnPositiveSide(Line2.ParallelTo(line, -_tolerance)));

            ExpectPolygonsAreEqualWithinTolerance(PolygonUtils.Union(half1, half2), polygon, _tolerance);
        }

        [Test]
        public void MinkowskiSum_of_two_boxes_amounts_to_summing_minima_and_maxima()
        {
            var box1 = new Box2(new Interval(0.123, 0.456), new Interval(2.718, 3.141));
            var box2 = new Box2(new Interval(0.777, 0.888), new Interval(-1, +1));

            var expectedAnswer = Box2.Hull(box1.MinCorner + box2.MinCorner, box1.MaxCorner + box2.MaxCorner);
            var actualAnswer = PolygonUtils.MinkowskiSum(box1, box2);

            ExpectPolygonsAreEqualWithinTolerance(actualAnswer, expectedAnswer, _tolerance);
        }

        [Test]
        public void MinkowskiDifference_of_two_boxes_amounts_to_subtracting_minima_and_maxima()
        {
            var box1 = new Box2(new Interval(0.123, 0.456), new Interval(2.718, 3.141));
            var box2 = new Box2(new Interval(0.0777, 0.0888), new Interval(-0.1, +0.1));

            var expectedAnswer = Box2.Hull(box1.MinCorner - box2.MaxCorner, box1.MaxCorner - box2.MinCorner);
            var actualAnswer = PolygonUtils.MinkowskiDifference(box1, box2);

            ExpectPolygonsAreEqualWithinTolerance(actualAnswer, expectedAnswer, _tolerance);
        }

        [Test]
        public void MinkowskiSum_is_distributive_with_Union()
        {
            var box1 = new Box2(new Interval(0.123, 0.456), new Interval(2.718, 3.141));
            var box2 = new Box2(new Interval(0.777, 0.888), new Interval(-1, +1));
            var box3 = new Box2(new Interval(1.519, 1.784), new Interval(-0.256, +0.324));

            var sum1 = PolygonUtils.MinkowskiSum(box1, PolygonUtils.Union(box2, box3));
            var sum2 = PolygonUtils.Union(PolygonUtils.MinkowskiSum(box1, box2), PolygonUtils.MinkowskiSum(box1, box3));

            ExpectPolygonsAreEqualWithinTolerance(sum1, sum2, _tolerance);
        }

        [Test]
        public void MinkowskiDifference_is_distributive_with_Union()
        {
            var box1 = new Box2(new Interval(0.123, 0.456), new Interval(2.718, 3.141));
            var box2 = new Box2(new Interval(0.777, 0.888), new Interval(-1, +1));
            var box3 = new Box2(new Interval(1.519, 1.784), new Interval(-0.256, +0.324));

            var diff1 = PolygonUtils.MinkowskiDifference(box1, PolygonUtils.Union(box2, box3));
            var diff2 = PolygonUtils.Union(PolygonUtils.MinkowskiDifference(box1, box2), PolygonUtils.MinkowskiDifference(box1, box3));

            ExpectPolygonsAreEqualWithinTolerance(diff1, diff2, _tolerance);
        }
    }
}
