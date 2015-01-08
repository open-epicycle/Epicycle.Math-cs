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

    public static class Box2Utils
    {
        public static Vector2 Corner(this Box2 @this, Interval.EndType end)
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

        public static Interval Range(this Box2 @this, Vector2.Axis axis)
        {
            switch (axis)
            {
                case Vector2.Axis.X:
                    return @this.XRange;

                case Vector2.Axis.Y:
                    return @this.YRange;

                default:
                    throw new ArgumentOutOfRangeException("Invalid 2D axis " + axis.ToString());
            }
        }

        public static bool Contains(this Box2 @this, Box2 that)
        {
            return
                @this.MinX <= that.MinX &&
                @this.MinY <= that.MinY &&
                @this.MaxX >= that.MaxX &&
                @this.MaxY >= that.MaxY;
        }

        public static Vector2 ToBoxCoords(this Vector2 v, Box2 box)
        {
            return new Vector2((v.X - box.MinX) / box.Width, (v.Y - box.MinY) / box.Height);
        }

        public static Vector2 FromBoxCoords(this Vector2 v, Box2 box)
        {
            return new Vector2(box.MinX + box.Width * v.X, box.MinY + box.Height * v.Y);
        }

        public static bool IsOnPositiveSide(this Box2 @this, Line2 line)
        {
            return @this.Vertices.All(v => v.IsOnPositiveSide(line));
        }

        public static bool IsOnNegativeSide(this Box2 @this, Line2 line)
        {
            return @this.Vertices.All(v => !v.IsOnPositiveSide(line));
        }
        
        public static double DistanceTo(this Vector2 @this, Box2 box)
        {
            return Math.Sqrt(box.Distance2(@this));
        }

        public sealed class YamlSerialization
        {
            public Vector2Utils.YamlSerialization MinCorner { get; set; }
            public Vector2Utils.YamlSerialization Dimensions { get; set; }

            public YamlSerialization() { }

            public YamlSerialization(Box2 box2)
            {
                MinCorner = new Vector2Utils.YamlSerialization(box2.MinCorner);
                Dimensions = new Vector2Utils.YamlSerialization(box2.Dimensions);
            }

            public Box2 Deserialize()
            {
                return new Box2(MinCorner.Deserialize(), Dimensions.Deserialize());
            }
        }
    }
}
