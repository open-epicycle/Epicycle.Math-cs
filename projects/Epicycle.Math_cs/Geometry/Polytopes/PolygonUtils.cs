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

using ClipperLib;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    using System;

    using ClipperContour = List<ClipperLib.IntPoint>;
    using ClipperPolygon = List<List<ClipperLib.IntPoint>>;

    public static class PolygonUtils
    {
        public static IEnumerable<Vector2> Vertices(this IPolygon @this)
        {
            return @this.Contours.SelectMany(c => c.Vertices);
        }

        public static IPolygon ToBoxCoords(this IPolygon @this, Box2 box)
        {
            return new Polygon(@this.Contours.Select(c => c.ToBoxCoords(box)));            
        }

        public static IPolygon FromBoxCoords(this IPolygon @this, Box2 box)
        {
            return new Polygon(@this.Contours.Select(c => c.FromBoxCoords(box)));
        }

        public static bool IsOnPositiveSide(this IPolygon @this, Line2 line)
        {
            foreach (var vertex in @this.Vertices())
            {
                if (!vertex.IsOnPositiveSide(line))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsOnNegativeSide(this IPolygon @this, Line2 line)
        {
            foreach (var vertex in @this.Vertices())
            {
                if (vertex.IsOnPositiveSide(line))
                {
                    return false;
                }
            }

            return true;
        }

        public static double DistanceTo(this Vector2 @this, IPolygon polygon)
        {
            return Math.Sqrt(polygon.Distance2(@this));
        }

        public static void FindClosestPoint(this IPolygon @this, Vector2 point, out Vector2? closestPoint, out double distance2)
        {
            if (@this.Contains(point))
            {
                closestPoint = point;
                distance2 = 0;
            }
            else
            {
                closestPoint = null;
                distance2 = double.PositiveInfinity;

                foreach (var contour in @this.Contours)
                {
                    double contourDistance2;
                    Vector2 contourClosestPoint;
                    contour.FindClosestPoint(point, out contourClosestPoint, out contourDistance2);

                    if (contourDistance2 < distance2)
                    {
                        closestPoint = contourClosestPoint;
                        distance2 = contourDistance2;                        
                    }
                }
            }
        }

        public static Vector2? FindClosestPoint(this IPolygon @this, Vector2 point)
        {
            Vector2? answer;
            double distance2;
            @this.FindClosestPoint(point, out answer, out distance2);

            return answer;
        }

        public static IPolygon TranslateBy(this IPolygon @this, Vector2 translation)
        {
            return new Polygon(@this.Contours.Select(c => c.TranslateBy(translation)));
        }

        // intersection of polygon with half plane lying on negative (left) side of line: this is the natural convention since we use external normals
        public static IPolygon CropBy(this IPolygon p, Line2 line)
        {
            var box = p.BoundingBox();            

            var point = line.Project(box.Center);
            var radius = box.Diameter / 2;

            var collinearDisplacement = radius * line.Direction;
            var transverseDisplacement = 2 * collinearDisplacement.Rotate90();

            var vertex1 = point + collinearDisplacement;
            var vertex2 = point - collinearDisplacement;

            var vertices = new List<Vector2>
            {
                vertex1,
                vertex1 + transverseDisplacement,
                vertex2 + transverseDisplacement,
                vertex2
            };

            var square = new Polygon(vertices);

            return Intersection(p, square);
        }

        public static IPolygon Intersection(IPolygon p, IPolygon q)
        {
            return BinaryOperation(p, q, ClipType.ctIntersection);
        }

        public static IPolygon Union(IPolygon p, IPolygon q)
        {
            return BinaryOperation(p, q, ClipType.ctUnion);
        }

        public static IPolygon Intersection(IEnumerable<IPolygon> polygons)
        {
            return MultinaryOperation(polygons, ClipType.ctIntersection);
        }

        public static IPolygon Union(IEnumerable<IPolygon> polygons)
        {
            return MultinaryOperation(polygons, ClipType.ctUnion);
        }

        public static IPolygon Difference(IPolygon p, IPolygon q)
        {
            return BinaryOperation(p, q, ClipType.ctDifference);
        }

        public static IPolygon SymmetricDifference(IPolygon p, IPolygon q)
        {
            return BinaryOperation(p, q, ClipType.ctXor);
        }

        public static IPolygon MinkowskiSum(IPolygon p, IPolygon q)
        {
            if (p.IsEmpty || q.IsEmpty)
            {
                return Polygon.Empty;
            }

            var boundingBox = Box2.Hull(p.BoundingBox(), q.BoundingBox());
            boundingBox = Box2.Hull(boundingBox, new Box2(-boundingBox.MaxCorner, boundingBox.Dimensions));

            var fixedAnswer = new ClipperPolygon();
            var fixedP = ConvertToFixedPoint(p, boundingBox);            

            foreach (var qContour in q.Contours)
            {
                var fixedQContour = ConvertToFixedPoint(qContour, boundingBox);

                try
                {
                    var clipper = new Clipper();
                    clipper.AddPaths(fixedAnswer, PolyType.ptSubject, closed: true);
                    clipper.AddPaths(Clipper.MinkowskiSum(fixedQContour, fixedP, pathIsClosed: true), PolyType.ptClip, closed: true);

                    var tempAnswer = new ClipperPolygon();

                    clipper.Execute(ClipType.ctUnion, tempAnswer);

                    fixedAnswer = tempAnswer;
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCEPTION: {0}", e);
                }
            }

            return ConvertToFloatingPoint(fixedAnswer, boundingBox);
        }

        public static IPolygon MinkowskiDifference(IPolygon p, IPolygon q)
        {
            if (p.IsEmpty || q.IsEmpty)
            {
                return Polygon.Empty;
            }

            var boundingBox = Box2.Hull(p.BoundingBox(), q.BoundingBox());
            boundingBox = Box2.Hull(boundingBox, new Box2(-boundingBox.MaxCorner, boundingBox.Dimensions));

            var fixedAnswer = new ClipperPolygon();
            var fixedP = ConvertToFixedPoint(p, boundingBox);

            foreach (var qContour in q.Contours)
            {
                var fixedQContour = MinusConvertToFixedPoint(qContour, boundingBox);

                try
                {
                    var clipper = new Clipper();
                    clipper.AddPaths(fixedAnswer, PolyType.ptSubject, closed: true);
                    clipper.AddPaths(Clipper.MinkowskiSum(fixedQContour, fixedP, pathIsClosed: true), PolyType.ptClip, closed: true);

                    var tempAnswer = new ClipperPolygon();

                    clipper.Execute(ClipType.ctUnion, tempAnswer);

                    fixedAnswer = tempAnswer;
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCEPTION: {0}", e);
                }
            }

            return ConvertToFloatingPoint(fixedAnswer, boundingBox);
        }

        private static IPolygon BinaryOperation(IPolygon p, IPolygon q, ClipType operationType)
        {
            if(p.IsEmpty && q.IsEmpty)
            {
                return Polygon.Empty;
            }

            var boundingBox = Box2.Hull(p.BoundingBox(), q.BoundingBox());

            var fixedP = ConvertToFixedPoint(p, boundingBox);
            var fixedQ = ConvertToFixedPoint(q, boundingBox);            

            try
            {
                var clipper = new Clipper();
                clipper.AddPaths(fixedP, PolyType.ptSubject, closed: true);
                clipper.AddPaths(fixedQ, PolyType.ptClip, closed: true);

                var fixedAnswer = new ClipperPolygon();

                clipper.Execute(operationType, fixedAnswer);

                return ConvertToFloatingPoint(fixedAnswer, boundingBox);
            }
            catch(Exception e)
            {
                Console.WriteLine("EXCEPTION: {0}", e);
                return Polygon.Empty;
            }            
        }

        private static IPolygon MultinaryOperation(IEnumerable<IPolygon> polygons, ClipType operationType)
        {
            var boundingBox = Box2.Hull(polygons.Select(p => p.BoundingBox()));

            if (boundingBox.IsEmpty)
            {
                return Polygon.Empty;
            }

            var first = polygons.First();
            var rest = polygons.Skip(1);

            if (!rest.Any())
            {
                return first;
            }  
            
            var fixedAnswer = ConvertToFixedPoint(first, boundingBox);

            foreach (var p in rest)
            {
                var fixedP = ConvertToFixedPoint(p, boundingBox);                

                try
                {
                    var clipper = new Clipper();
                    clipper.AddPaths(fixedAnswer, PolyType.ptSubject, closed: true);
                    clipper.AddPaths(fixedP, PolyType.ptClip, closed: true);

                    var tempAnswer = new ClipperPolygon();

                    clipper.Execute(operationType, tempAnswer);

                    fixedAnswer = tempAnswer;
                }
                catch(Exception e)
                {
                    Console.WriteLine("EXCEPTION: {0}", e);
                }                
            }

            return ConvertToFloatingPoint(fixedAnswer, boundingBox);
        }

        public static IPolygon Offset(this IPolygon p, double offset)
        {
            if (p.IsEmpty)
            {
                return Polygon.Empty;
            }

            var boundingBox = p.BoundingBox().Offset(Math.Max(offset, 0));            
            double scale = Math.Max(boundingBox.Width, boundingBox.Height);
            var fixedPointRange = new Box2(boundingBox.MinCorner, new Vector2(scale, scale));

            var fixedP = ConvertToFixedPoint(p, fixedPointRange);

            try
            {
                var offsetter = new ClipperOffset();
                offsetter.AddPaths(fixedP, JoinType.jtMiter, EndType.etClosedPolygon);

                var fixedAnswer = new ClipperPolygon();
                offsetter.Execute(ref fixedAnswer, offset * _fixedPointRange / scale);

                return ConvertToFloatingPoint(fixedAnswer, fixedPointRange);
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: {0}", e);
                return p;
            }
        }

        public static IPolygon Closing(this IPolygon p, double offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", offset, "Expected positive offset");
            }

            var boundingBox = p.BoundingBox().Offset(offset);

            return Clopening(p, boundingBox, offset);
        }

        public static IPolygon Opening(this IPolygon p, double offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", offset, "Expected positive offset");
            }

            return Clopening(p, p.BoundingBox(), -offset);
        }

        private static IPolygon Clopening(IPolygon p, Box2 boundingBox, double offset)
        {
            if (p.IsEmpty)
            {
                return Polygon.Empty;
            }
            
            double scale = Math.Max(boundingBox.Width, boundingBox.Height);
            var fixedPointRange = new Box2(boundingBox.MinCorner, new Vector2(scale, scale));

            var fixedP = ConvertToFixedPoint(p, fixedPointRange);
            var fixedScaleOffset = offset * _fixedPointRange / scale;

            try
            {
                var offsetter = new ClipperOffset();
                offsetter.AddPaths(fixedP, JoinType.jtMiter, EndType.etClosedPolygon);

                var fixedIntermediate = new ClipperPolygon();
                offsetter.Execute(ref fixedIntermediate, fixedScaleOffset);

                offsetter.Clear();
                offsetter.AddPaths(fixedIntermediate, JoinType.jtMiter, EndType.etClosedPolygon);

                var fixedAnswer = new ClipperPolygon();
                offsetter.Execute(ref fixedAnswer, -fixedScaleOffset);

                return ConvertToFloatingPoint(fixedAnswer, fixedPointRange);
            }
            catch(Exception e)
            {
                Console.WriteLine("EXCEPTION: {0}", e);
                return p;
            }            
        }

        public static IPolygon RelativeClosing(this IPolygon p, double relativeOffset)
        {
            if (relativeOffset < 0)
            {
                throw new ArgumentOutOfRangeException("relativeOffset", relativeOffset, "Expected positive relativeOffset");
            }

            var boundingBox = p.BoundingBox();
           
            if (boundingBox.IsEmpty)
            {
                return Polygon.Empty;
            }

            double scale = Math.Max(boundingBox.Width, boundingBox.Height);

            return p.Closing(relativeOffset * scale);
        }

        #region fixed point

        // Clipper allows values up to 2^62, we use 2^61 to have a safety margin
        private const double _minFixedPointValue = -long.MaxValue / 4;
        private const double _fixedPointRange = long.MaxValue / 4 - _minFixedPointValue;

        private static ClipperPolygon ConvertToFixedPoint(IPolygon p, Box2 range)
        {
            return (from contour in p.Contours select ConvertToFixedPoint(contour, range)).ToList();
        }

        private static ClipperPolygon MinusConvertToFixedPoint(IPolygon p, Box2 range)
        {
            return (from contour in p.Contours select MinusConvertToFixedPoint(contour, range)).ToList();
        }

        private static ClipperContour ConvertToFixedPoint(IClosedPolyline p, Box2 range)
        {
            return (from vertex in p.Vertices select ConvertToFixedPoint(vertex, range)).ToList();
        }

        private static ClipperContour MinusConvertToFixedPoint(IClosedPolyline p, Box2 range)
        {
            return (from vertex in p.Vertices select MinusConvertToFixedPoint(vertex, range)).ToList();
        }

        private static ClipperLib.IntPoint ConvertToFixedPoint(Vector2 p, Box2 range)
        {
            return new IntPoint
            {
                X = ConvertToFixedPoint(p.X, range.XRange),
                Y = ConvertToFixedPoint(p.Y, range.YRange)
            };
        }

        private static ClipperLib.IntPoint MinusConvertToFixedPoint(Vector2 p, Box2 range)
        {
            return new IntPoint
            {
                X = -ConvertToFixedPoint(p.X, range.XRange),
                Y = -ConvertToFixedPoint(p.Y, range.YRange)
            };
        }

        private static long ConvertToFixedPoint(double a, Interval range)
        {
            return (long)Math.Round(_minFixedPointValue + (a - range.Min) * _fixedPointRange / range.Length);
        }

        private static IPolygon ConvertToFloatingPoint(ClipperPolygon p, Box2 range)
        {
            var answer = new PolygonBuilder();

            foreach (var clipContour in p)
            {
                var contour = clipContour.Select(v => ConvertToFloatingPoint(v, range)).ToList();
                answer.AddContour(contour);
            }

            return answer.ExtractPolygon();
        }

        private static Vector2 ConvertToFloatingPoint(ClipperLib.IntPoint p, Box2 range)
        {
            return new Vector2
            (
                ConvertToFloatingPoint(p.X, range.XRange),
                ConvertToFloatingPoint(p.Y, range.YRange)
            );
        }

        private static double ConvertToFloatingPoint(long i, Interval range)
        {
            return range.Min + (i - _minFixedPointValue) * range.Length / _fixedPointRange;
        }

        #endregion
    }
}
