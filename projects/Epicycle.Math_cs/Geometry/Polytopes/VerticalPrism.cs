using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    public sealed class VerticalPrism : VerticalPrismalSurface, IPolyhedron
    {
        public VerticalPrism(IClosedPolyline @base, Interval zRange) : base(@base, zRange, isClosed: true)
        {
            if (!_isCounterclockwise)
            {
                throw new ArgumentException("@base has to be counterclockwise", "@base");
            }

            _basePolygon = new Polygon(@base);
        }

        public VerticalPrism(Box3 box) : base(box.XYRange, box.ZRange, isClosed: true)
        {
            _basePolygon = box.XYRange;
        }

        public static implicit operator VerticalPrism(Box3 box)
        {
            return new VerticalPrism(box);
        }

        private readonly IPolygon _basePolygon;

        public bool Contains(Vector3 point)
        {
            return _basePolygon.Contains(point.XY) && ZRange.Contains(point.Z);
        }

        public double Distance2(Vector3 point)
        {
            return _basePolygon.Distance2(point.XY) + ZRange.Distance2(point.Z);
        }
    }
}
