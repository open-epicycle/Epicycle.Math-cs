using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Segment2Utils
    {
        public static double Distance2(Vector2 point, Segment2 segment)
        {
            var vector = segment.Vector;

            var toStart = segment.Start - point;
            var toEnd = segment.End - point;

            if ((toStart * vector) * (toEnd * vector) > 0)
            {
                return Math.Min(toStart.Norm2, toEnd.Norm2);
            }
            else
            {
                return BasicMath.Sqr(toStart.Cross(toEnd)) / vector.Norm2;
            }
        }

        public static double DistanceTo(this Vector2 @this, Segment2 segment)
        {
            return Math.Sqrt(Distance2(@this, segment));
        }

        public static Segment2? CropBy(this Segment2 @this, Line2 line)
        {
            var signedDist = @this.Start.SignedDistanceTo(line);

            var t = -signedDist / (line.Normal * @this.Vector);

            if (t > 0 && t < 1) // plane intersects @this
            {
                if (signedDist > 0)
                {
                    return new Segment2(@this.PointAt(t), @this.End);
                }
                else
                {
                    return new Segment2(@this.Start, @this.PointAt(t));
                }
            }
            else // plane doesn't intersect @this
            {
                if (signedDist > 0)
                {
                    return null;
                }
                else
                {
                    return @this;
                }
            }
        }
    }
}
