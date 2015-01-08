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

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Segment3Utils
    {
        public static Segment2 ProjectToXY(this Segment3 @this)
        {
            return new Segment2(@this.Start.XY, @this.End.XY);
        }

        public static double Distance2(Vector3 point, Segment3 segment)
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
                return toStart.Cross(toEnd).Norm2 / vector.Norm2;
            }
        }

        public static double DistanceTo(this Vector3 @this, Segment3 segment)
        {
            return Math.Sqrt(Distance2(@this, segment));
        }

        public static Segment3? CropBy(this Segment3 @this, Plane plane)
        {
            var signedDist = @this.Start.SignedDistanceTo(plane);

            var t = -signedDist / (plane.Normal * @this.Vector);

            if (t > 0 && t < 1) // plane intersects @this
            {
                if (signedDist > 0)
                {
                    return new Segment3(@this.PointAt(t), @this.End);
                }
                else
                {
                    return new Segment3(@this.Start, @this.PointAt(t));                    
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

        public static Vector2? IntersectionWithPlaneXY(this Segment3 @this)
        {
            if (@this.Start.Z * @this.End.Z > 0)
            {
                return null;
            }
            else
            {
                var deltaZ = @this.End.Z - @this.Start.Z;

                if (Math.Abs(deltaZ) < BasicMath.Epsilon)
                {
                    return (@this.Start.XY + @this.End.XY) / 2;
                }

                return (@this.End.Z * @this.Start.XY - @this.Start.Z * @this.End.XY) / deltaZ;
            }
        }

        public static Vector2? IntersectionWithPlaneYZ(this Segment3 @this)
        {
            if (@this.Start.X * @this.End.X > 0)
            {
                return null;
            }
            else
            {
                var deltaX = @this.End.X - @this.Start.X;

                if (Math.Abs(deltaX) < BasicMath.Epsilon)
                {
                    return (@this.Start.YZ + @this.End.YZ) / 2;
                }

                return (@this.End.X * @this.Start.YZ - @this.Start.X * @this.End.YZ) / deltaX;
            }
        }

        public static Vector2? IntersectionWithPlaneZX(this Segment3 @this)
        {
            if (@this.Start.Y * @this.End.Y > 0)
            {
                return null;
            }
            else
            {
                var deltaY = @this.End.Y - @this.Start.Y;

                if (Math.Abs(deltaY) < BasicMath.Epsilon)
                {
                    return (@this.Start.ZX + @this.End.ZX) / 2;
                }

                return (@this.End.Y * @this.Start.ZX - @this.Start.Y * @this.End.ZX) / deltaY;
            }
        }
    }
}
