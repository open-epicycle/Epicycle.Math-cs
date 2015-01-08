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

using System.Linq;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Box3Utils
    {
        public static Vector3 Corner(this Box3 @this, Interval.EndType end)
        {
            switch (end)
            {
                case Interval.EndType.Min:
                    return @this.MinCorner;

                case Interval.EndType.Max:
                    return @this.MaxCorner;

                default:
                    throw new ArgumentOutOfRangeException("end", end, "Illegal EndType " + end.ToString());
            }
        }

        public static Interval Range(this Box3 @this, Vector3.Axis axis)
        {
            switch (axis)
            {
                case Vector3.Axis.X:
                    return @this.XRange;

                case Vector3.Axis.Y:
                    return @this.YRange;

                case Vector3.Axis.Z:
                    return @this.ZRange;

                default:
                    throw new ArgumentOutOfRangeException("Invalid 3D axis " + axis.ToString());
            }
        }

        public static bool Contains(this Box3 @this, Box3 that)
        {
            return
                @this.MinX <= that.MinX &&
                @this.MinY <= that.MinY &&
                @this.MinZ <= that.MinZ &&
                @this.MaxX >= that.MaxX &&
                @this.MaxY >= that.MaxY &&
                @this.MaxZ >= that.MaxZ;
        }

        public static bool Contains(this Box3 @this, Vector3 point)
        {
            return
                point.X <= @this.MaxX &&
                point.X >= @this.MinX &&
                point.Y <= @this.MaxY &&
                point.Y >= @this.MinY &&
                point.Z <= @this.MaxZ &&
                point.Z >= @this.MinZ;
        }

        public static Vector3 ToBoxCoords(this Vector3 v, Box3 box)
        {
            return new Vector3((v.X - box.MinX) / box.XDimension, (v.Y - box.MinY) / box.YDimension, (v.Z - box.MinZ) / box.ZDimension);
        }

        public static Vector3 FromBoxCoords(this Vector3 v, Box3 box)
        {
            return new Vector3(box.MinX + box.XDimension * v.X, box.MinY + box.YDimension * v.Y, box.MinZ + box.ZDimension * v.Z);
        }

        public static bool IsOnPositiveSide(this Box3 @this, Plane plane)
        {
            return @this.Vertices.All(v => v.IsOnPositiveSide(plane));
        }

        public static bool IsOnNegativeSide(this Box3 @this, Plane plane)
        {
            return @this.Vertices.All(v => !v.IsOnPositiveSide(plane));
        }

        public static double Distance2(this Box3 @this, Vector3 point)
        {
            return @this.XRange.Distance2(point.X) + @this.YRange.Distance2(point.Y) + @this.ZRange.Distance2(point.Z);
        }

        public static double DistanceTo(this Vector3 @this, Box3 box)
        {
            return Math.Sqrt(Distance2(box, @this));
        }
    }
}