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

using Epicycle.Commons.Collections;
using Epicycle.Math.Geometry.Polytopes;
using System.Collections.Generic;

namespace Epicycle.Math.Geometry
{
    using System;

    // a Box2 is an axis-aligned rectangle
    public sealed class Box2 : IClosedPolyline, IPolygon
    {
        #region creation

        public Box2(Vector2 minCorner, Vector2 dimensions)
            : this(minCorner.X, minCorner.Y, dimensions.X, dimensions.Y) { }

        public Box2(double minX, double minY, double width, double height)
        {
            if (width < 0 || height < 0)
            {
                SetEmpty(out _minCorner, out _maxCorner);
            }
            else
            {
                _minCorner = new Vector2(minX, minY);
                _maxCorner = new Vector2(minX + width, minY + height);
            }        
        }

        public Box2(Interval xRange, Interval yRange)
        {
            if (xRange.IsEmpty || yRange.IsEmpty)
            {
                SetEmpty(out _minCorner, out _maxCorner);
            }
            else
            {
                _minCorner = new Vector2(xRange.Min, yRange.Min);
                _maxCorner = new Vector2(xRange.Max, yRange.Max);
            }        
        }

        // dummy argument is needed to distinguish from a ctor with the same signature
        private Box2(Vector2 minCorner, Vector2 maxCorner, bool dummy)
        {
            if(maxCorner.X < minCorner.X || maxCorner.Y < minCorner.Y)
            {
                SetEmpty(out _minCorner, out _maxCorner);
            }
            else
            {
                _minCorner = minCorner;
                _maxCorner = maxCorner;
            }        
        }

        private static void SetEmpty(out Vector2 minCorner, out Vector2 maxCorner)
        {
            minCorner = new Vector2(double.PositiveInfinity, double.PositiveInfinity);
            maxCorner = new Vector2(double.NegativeInfinity, double.NegativeInfinity);
        }

        private readonly Vector2 _minCorner;
        private readonly Vector2 _maxCorner;

        #endregion

        #region properties

        public Vector2 MinCorner 
        {
            get { return _minCorner; }
        }

        public Vector2 MaxCorner 
        {
            get { return _maxCorner; }
        }

        public Vector2 Center
        {
            get { return new Vector2( (MinX + MaxX) / 2, (MinY + MaxY) / 2 ); }
        }

        public Interval XRange
        {
            get { return new Interval(MinCorner.X, MaxCorner.X); }
        }

        public Interval YRange
        {
            get { return new Interval(MinCorner.Y, MaxCorner.Y); }
        }

        public double MinX
        {
            get { return _minCorner.X; }
        }

        public double MinY
        {
            get { return _minCorner.Y; }
        }

        public double MaxX
        {
            get { return _maxCorner.X; }
        }

        public double MaxY
        {
            get { return _maxCorner.Y; }
        }

        public double Width
        {
            get { return MaxCorner.X - MinCorner.X; }
        }

        public double Height
        {
            get { return MaxCorner.Y - MinCorner.Y; }
        }

        public double Diameter2
        {
            get { return Width * Width + Height * Height; }
        }

        public double Diameter
        {
            get { return Math.Sqrt(Diameter2); }
        }

        public Vector2 Dimensions
        {
            get { return new Vector2(Width, Height); }
        }

        public double Area
        {
            get 
            {
                if(this.IsEmpty)
                {
                    return 0;
                }
 
                return Width * Height;
            }
        }

        public bool IsEmpty
        {
            get { return MaxCorner.X < MinCorner.X || MaxCorner.Y < MinCorner.Y; }
        }

        #endregion

        #region methods

        public Box2 Offset(double offset)
        {
            return new Box2
            (
                new Vector2(MinCorner.X - offset, MinCorner.Y - offset),
                new Vector2(MaxCorner.X + offset, MaxCorner.Y + offset),
                true
            );
        }

        public Box2 Dilate(double factor)
        {
            return Box2.AroundPoint(this.Center, factor * this.Dimensions);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", MinCorner.X, MinCorner.Y, Width, Height);
        }

        #endregion

        #region static

        public static Box2 Empty
        {
            get { return _emptyBox; }
        }

        private static readonly Box2 _emptyBox;

        public static Box2 EntirePlane
        {
            get { return _entirePlane; }
        }

        private static readonly Box2 _entirePlane;

        static Box2()
        {
            _emptyBox = new Box2(Interval.Empty, Interval.Empty);
            _entirePlane = new Box2(Interval.EntireLine, Interval.EntireLine);
        }

        public static Box2 AroundPoint(Vector2 point, Vector2 dimensions)
        {
            return new Box2(point - dimensions / 2, dimensions);
        }

        public static Box2 Hull(Box2 a, Box2 b)
        {
            var xRange = new Interval(Math.Min(a.MinCorner.X, b.MinCorner.X), Math.Max(a.MaxCorner.X, b.MaxCorner.X));
            var yRange = new Interval(Math.Min(a.MinCorner.Y, b.MinCorner.Y), Math.Max(a.MaxCorner.Y, b.MaxCorner.Y));

            return new Box2(xRange, yRange);
        }

        public static Box2 Hull(Vector2 v, Vector2 w)
        {
            var xRange = Interval.Hull(v.X, w.X);
            var yRange = Interval.Hull(v.Y, w.Y);

            return new Box2(xRange, yRange);
        }

        public static Box2 Hull(IEnumerable<Vector2> points)
        {
            var minX = double.PositiveInfinity;
            var maxX = double.NegativeInfinity;
            var minY = double.PositiveInfinity;
            var maxY = double.NegativeInfinity;

            foreach (var point in points)
            {
                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);
            }

            return new Box2(new Interval(minX, maxX), new Interval(minY, maxY));
        }

        public static Box2 Hull(IEnumerable<Box2> boxes)
        {
            var minX = double.PositiveInfinity;
            var maxX = double.NegativeInfinity;
            var minY = double.PositiveInfinity;
            var maxY = double.NegativeInfinity;

            foreach (Box2 box in boxes)
            {
                minX = Math.Min(minX, box.MinCorner.X);
                maxX = Math.Max(maxX, box.MaxCorner.X);
                minY = Math.Min(minY, box.MinCorner.Y);
                maxY = Math.Max(maxY, box.MaxCorner.Y);
            }

            return new Box2(new Interval(minX, maxX), new Interval(minY, maxY));
        }

        public static Box2 Intersection(Box2 a, Box2 b)
        {
            var xRange = new Interval(Math.Max(a.MinCorner.X, b.MinCorner.X), Math.Min(a.MaxCorner.X, b.MaxCorner.X));
            var yRange = new Interval(Math.Max(a.MinCorner.Y, b.MinCorner.Y), Math.Min(a.MaxCorner.Y, b.MaxCorner.Y));

            return new Box2(xRange, yRange);
        }

        public static bool DoIntersect(Box2 a, Box2 b)
        {
            return
                a.MaxX > b.MinX &&
                b.MaxX > a.MinX &&
                a.MaxY > b.MinY &&
                b.MaxY > a.MinY;
        }

        #endregion

        #region IClosedPolyline

        bool IPolyline.IsClosed
        {
            get { return true; }
        }

        public IReadOnlyList<Vector2> Vertices
        {
            get 
            {
                if (this.IsEmpty)
                {
                    return new List<Vector2>().AsReadOnlyList();
                }
            
                return new List<Vector2>
                {
                    MinCorner,
                    new Vector2(MaxCorner.X, MinCorner.Y),
                    MaxCorner,
                    new Vector2(MinCorner.X, MaxCorner.Y)
                }.AsReadOnlyList();
            }
        }

        double IClosedPolyline.SignedArea()
        {
            return Area;
        }

        #endregion

        #region IPolygon

        IReadOnlyCollection<IClosedPolyline> IPolygon.Contours
        {
            get { return this.AsSingleton<IClosedPolyline>(); }
        }

        bool IPolygon.IsSimple
        {
            get { return true; }
        }

        Box2 IPolygon.BoundingBox()
        {
            return this;
        }

        int IPolygon.CountVertices()
        {
            return 4;
        }

        double IPolygon.Area()
        {
            return Area;
        }

        public bool Contains(Vector2 point)
        {
            return
                point.X <= MaxX &&
                point.X >= MinX &&
                point.Y <= MaxY &&
                point.Y >= MinY;
        }

        public double Distance2(Vector2 point)
        {
            return XRange.Distance2(point.X) + YRange.Distance2(point.Y);
        }

        #endregion
    }
}