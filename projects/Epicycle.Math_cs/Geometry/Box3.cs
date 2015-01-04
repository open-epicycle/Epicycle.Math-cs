using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    using System;

    // Box3 is an axis-aligned rectangular cuboid
    public struct Box3
    {
        #region creation

        public Box3(Vector3 minCorner, Vector3 dimensions)
            : this(minCorner.X, minCorner.Y, minCorner.Z, dimensions.X, dimensions.Y, dimensions.Z) { }

        public Box3(double minX, double minY, double minZ, double xDimension, double yDimension, double zDimension)
        {
            if (xDimension < 0 || yDimension < 0 || zDimension < 0)
            {
                SetEmpty(out _minCorner, out _maxCorner);
            }
            else
            {
                _minCorner = new Vector3(minX, minY, minZ);
                _maxCorner = new Vector3(minX + xDimension, minY + yDimension, minZ + zDimension);
            }
        }

        public Box3(Interval xRange, Interval yRange, Interval zRange)
        {
            if (xRange.IsEmpty || yRange.IsEmpty || zRange.IsEmpty)
            {
                SetEmpty(out _minCorner, out _maxCorner);
            }
            else
            {
                _minCorner = new Vector3(xRange.Min, yRange.Min, zRange.Min);
                _maxCorner = new Vector3(xRange.Max, yRange.Max, zRange.Max);
            }
        }

        // dummy argument is needed to distinguish from a ctor with the same signature
        private Box3(Vector3 minCorner, Vector3 maxCorner, bool dummy)
        {
            if (maxCorner.X < minCorner.X || maxCorner.Y < minCorner.Y || maxCorner.Z < minCorner.Z)
            {
                SetEmpty(out _minCorner, out _maxCorner);
            }
            else
            {
                _minCorner = minCorner;
                _maxCorner = maxCorner;
            }
        }

        private static void SetEmpty(out Vector3 minCorner, out Vector3 maxCorner)
        {
            minCorner = new Vector3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
            maxCorner = new Vector3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        }

        private readonly Vector3 _minCorner;
        private readonly Vector3 _maxCorner;

        #endregion

        #region properties

        public Vector3 MinCorner
        {
            get { return _minCorner; }
        }

        public Vector3 MaxCorner
        {
            get { return _maxCorner; }
        }

        public Vector3 Center
        {
            get { return new Vector3((MinX + MaxX) / 2, (MinY + MaxY) / 2, (MinZ + MaxZ) / 2); }
        }

        public Interval XRange
        {
            get { return new Interval(MinCorner.X, MaxCorner.X); }
        }

        public Interval YRange
        {
            get { return new Interval(MinCorner.Y, MaxCorner.Y); }
        }

        public Interval ZRange
        {
            get { return new Interval(MinCorner.Z, MaxCorner.Z); }
        }

        public Box2 XYRange
        {
            get { return new Box2(XRange, YRange); }
        }

        public double MinX
        {
            get { return _minCorner.X; }
        }

        public double MinY
        {
            get { return _minCorner.Y; }
        }

        public double MinZ
        {
            get { return _minCorner.Z; }
        }

        public double MaxX
        {
            get { return _maxCorner.X; }
        }

        public double MaxY
        {
            get { return _maxCorner.Y; }
        }

        public double MaxZ
        {
            get { return _maxCorner.Z; }
        }

        public double XDimension
        {
            get { return MaxCorner.X - MinCorner.X; }
        }

        public double YDimension
        {
            get { return MaxCorner.Y - MinCorner.Y; }
        }

        public double ZDimension
        {
            get { return MaxCorner.Z - MinCorner.Z; }
        }

        public double Diameter2
        {
            get { return Dimensions.Norm2; }
        }

        public double Diameter
        {
            get { return Dimensions.Norm; }
        }

        public Vector3 Dimensions
        {
            get { return new Vector3(XDimension, YDimension, ZDimension); }
        }

        public double Volume
        {
            get
            {
                if (this.IsEmpty)
                {
                    return 0;
                }

                return XDimension * YDimension * ZDimension;
            }
        }

        public bool IsEmpty
        {
            get { return MaxCorner.X < MinCorner.X || MaxCorner.Y < MinCorner.Y || MaxCorner.Z < MinCorner.Z; }
        }

        public IEnumerable<Vector3> Vertices
        {
            get
            {
                yield return MinCorner;
                yield return new Vector3(MinCorner.X, MinCorner.Y, MaxCorner.Z);
                yield return new Vector3(MinCorner.X, MaxCorner.Y, MinCorner.Z);
                yield return new Vector3(MinCorner.X, MaxCorner.Y, MaxCorner.Z);
                yield return new Vector3(MaxCorner.X, MinCorner.Y, MinCorner.Z);
                yield return new Vector3(MaxCorner.X, MinCorner.Y, MaxCorner.Z);
                yield return new Vector3(MaxCorner.X, MaxCorner.Y, MinCorner.Z);
                yield return MaxCorner;
            }
        }

        #endregion

        #region methods

        public Box3 Offset(double offset)
        {
            return new Box3
            (
                new Vector3(MinCorner.X - offset, MinCorner.Y - offset, MinCorner.Z - offset),
                new Vector3(MaxCorner.X + offset, MaxCorner.Y + offset, MaxCorner.Z + offset),
                true
            );
        }

        public override string ToString()
        {
            return string.Format("(Min:{0}, Dim:{1})", MinCorner, Dimensions);
        }

        #endregion

        #region static

        public static Box3 Empty
        {
            get { return _emptyBox; }
        }

        private static readonly Box3 _emptyBox;

        public static Box3 EntirePlane
        {
            get { return _entirePlane; }
        }

        private static readonly Box3 _entirePlane;

        static Box3()
        {
            _emptyBox = new Box3(Interval.Empty, Interval.Empty, Interval.Empty);
            _entirePlane = new Box3(Interval.EntireLine, Interval.EntireLine, Interval.EntireLine);
        }

        public static Box3 Hull(Box3 a, Box3 b)
        {
            var xRange = new Interval(Math.Min(a.MinCorner.X, b.MinCorner.X), Math.Max(a.MaxCorner.X, b.MaxCorner.X));
            var yRange = new Interval(Math.Min(a.MinCorner.Y, b.MinCorner.Y), Math.Max(a.MaxCorner.Y, b.MaxCorner.Y));
            var zRange = new Interval(Math.Min(a.MinCorner.Z, b.MinCorner.Z), Math.Max(a.MaxCorner.Z, b.MaxCorner.Z));

            return new Box3(xRange, yRange, zRange);
        }

        public static Box3 Hull(Vector3 v, Vector3 w)
        {
            var xRange = Interval.Hull(v.X, w.X);
            var yRange = Interval.Hull(v.Y, w.Y);
            var zRange = Interval.Hull(v.Z, w.Z);

            return new Box3(xRange, yRange, zRange);
        }

        public static Box3 Hull(IEnumerable<Vector3> points)
        {
            var minX = double.PositiveInfinity;
            var maxX = double.NegativeInfinity;

            var minY = double.PositiveInfinity;
            var maxY = double.NegativeInfinity;

            var minZ = double.PositiveInfinity;
            var maxZ = double.NegativeInfinity;

            foreach (Vector3 point in points)
            {
                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);

                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);

                minZ = Math.Min(minZ, point.Z);
                maxZ = Math.Max(maxZ, point.Z);
            }

            return new Box3(new Interval(minX, maxX), new Interval(minY, maxY), new Interval(minZ, maxZ));
        }

        public static Box3 Hull(IEnumerable<Box3> boxes)
        {
            var minX = double.PositiveInfinity;
            var maxX = double.NegativeInfinity;

            var minY = double.PositiveInfinity;
            var maxY = double.NegativeInfinity;

            var minZ = double.PositiveInfinity;
            var maxZ = double.NegativeInfinity;

            foreach (Box3 box in boxes)
            {
                minX = Math.Min(minX, box.MinCorner.X);
                maxX = Math.Max(maxX, box.MaxCorner.X);

                minY = Math.Min(minY, box.MinCorner.Y);
                maxY = Math.Max(maxY, box.MaxCorner.Y);

                minZ = Math.Min(minZ, box.MinCorner.Z);
                maxZ = Math.Max(maxZ, box.MaxCorner.Z);
            }

            return new Box3(new Interval(minX, maxX), new Interval(minY, maxY), new Interval(minZ, maxZ));
        }

        public static Box3 Intersection(Box3 a, Box3 b)
        {
            var xRange = new Interval(Math.Max(a.MinCorner.X, b.MinCorner.X), Math.Min(a.MaxCorner.X, b.MaxCorner.X));
            var yRange = new Interval(Math.Max(a.MinCorner.Y, b.MinCorner.Y), Math.Min(a.MaxCorner.Y, b.MaxCorner.Y));
            var zRange = new Interval(Math.Max(a.MinCorner.Z, b.MinCorner.Z), Math.Min(a.MaxCorner.Z, b.MaxCorner.Z));

            return new Box3(xRange, yRange, zRange);
        }

        #endregion
    }
}