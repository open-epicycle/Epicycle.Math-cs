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
