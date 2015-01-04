namespace Epicycle.Math.Geometry
{
    using System;

    public static class PlaneUtils
    {
        public static double SignedDistanceTo(this Vector3 @this, Plane plane)
        {
            return @this * plane.Normal + plane.FreeTerm;
        }

        public static double DistanceTo(this Vector3 @this, Plane plane)
        {
            return Math.Abs(@this.SignedDistanceTo(plane));
        }

        public static bool IsOnPositiveSide(this Vector3 @this, Plane plane)
        {
            return @this.SignedDistanceTo(plane) > 0;
        }

        public static Line2 IntersectWithPlaneXY(this Plane @this)
        {
            return new Line2(@this.FreeTerm, @this.Normal.XY);
        }

        public static Vector3 ProjectOrigin(this Plane @this)
        {
            return -@this.FreeTerm * @this.Normal;
        }
    }
}
