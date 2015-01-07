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

using Epicycle.Math.Geometry.Polytopes;
using System.Linq;

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
