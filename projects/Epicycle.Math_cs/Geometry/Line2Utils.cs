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

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Line2Utils
    {
        public static double SignedDistanceTo(this Vector2 @this, Line2 line)
        {
            return @this * line.Normal + line.FreeTerm;
        }

        public static double DistanceTo(this Vector2 @this, Line2 line)
        {
            return Math.Abs(@this.SignedDistanceTo(line));
        }

        public static bool IsOnPositiveSide(this Vector2 @this, Line2 line)
        {
            return @this.SignedDistanceTo(line) > 0;
        }

        public static Vector2 ProjectOrigin(this Line2 @this)
        {
            return -@this.FreeTerm * @this.Normal;
        }

        public static Vector2 Project(this Line2 @this, Vector2 point)
        {
            var direction = @this.Direction;

            return @this.ProjectOrigin() + (direction * point) * direction;
        }

        public static Line2 Translate(this Line2 @this, Vector2 vector)
        {
            return new Line2(@this.FreeTerm - @this.Normal * vector, @this.Normal);
        }
    }
}