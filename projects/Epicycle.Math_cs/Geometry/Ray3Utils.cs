using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry
{
    public static class Ray3Utils
    {
        public static Vector2? IntersectWithPlaneXY(this Ray3 @this)
        {
            var coord = -@this.Origin.Z / @this.Direction.Z;

            if (coord < 0)
            {
                return null;
            }

            return @this.Origin.XY + coord * @this.Direction.XY;
        }

        public static Vector3 Intersect(this Ray3 @this, Plane plane)
        {
            var coord = -@this.Origin.SignedDistanceTo(plane) / (plane.Normal * @this.Direction);

            if (coord < 0)
            {
                return null;
            }

            return @this.PointAt(coord);
        }
    }
}
