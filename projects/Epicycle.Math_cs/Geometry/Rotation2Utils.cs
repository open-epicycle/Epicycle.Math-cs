using System.Linq;

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Polytopes;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Rotation2Utils
    {
        public const double AngleDegrees0 = 0;
        public const double AngleDegrees90 = Math.PI / 2;
        public const double AngleDegrees180 = Math.PI;
        public const double AngleDegrees270 = -Math.PI / 2;

        public static Vector2 Rotate90(this Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }

        public static Vector2 Rotate270(this Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }

        public static Ray2 Rotate90(this Ray2 r)
        {
            return new Ray2(r.Origin.Rotate90(), r.Direction.Rotate90());
        }

        public static Ray2 Rotate270(this Ray2 r)
        {
            return new Ray2(r.Origin.Rotate270(), r.Direction.Rotate270());
        }

        public static Box2 Rotate90(this Box2 b)
        {
            return Box2.Hull(b.MinCorner.Rotate90(), b.MaxCorner.Rotate90());
        }

        public static Box2 Rotate270(this Box2 b)
        {
            return Box2.Hull(b.MinCorner.Rotate270(), b.MaxCorner.Rotate270());
        }

        public static IClosedPolyline Apply(this Rotation2 @this, IClosedPolyline line)
        {
            return new ClosedPolyline(line.Vertices.Select(v => @this.Apply(v)));
        }

        public static IClosedPolyline ApplyInv(this Rotation2 @this, IClosedPolyline line)
        {
            return new ClosedPolyline(line.Vertices.Select(v => @this.ApplyInv(v)));
        }

        public static IOpenPolyline Apply(this Rotation2 @this, IOpenPolyline line)
        {
            return new OpenPolyline(line.Vertices.Select(v => @this.Apply(v)));
        }

        public static IOpenPolyline ApplyInv(this Rotation2 @this, IOpenPolyline line)
        {
            return new OpenPolyline(line.Vertices.Select(v => @this.ApplyInv(v)));
        }

        public static IPolygon Apply(this Rotation2 @this, IPolygon polygon)
        {
            return new Polygon(polygon.Contours.Select(c => @this.Apply(c)));
        }

        public static IPolygon ApplyInv(this Rotation2 @this, IPolygon polygon)
        {
            return new Polygon(polygon.Contours.Select(c => @this.ApplyInv(c)));
        }
    }
}
