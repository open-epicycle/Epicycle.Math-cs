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
