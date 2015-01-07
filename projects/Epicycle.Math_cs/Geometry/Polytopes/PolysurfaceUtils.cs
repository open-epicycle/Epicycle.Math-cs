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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry.Polytopes
{
    using System;

    public static class PolysurfaceUtils
    {
        public static IEnumerable<IPolysurfaceVertex> Vertices(this IPolysurfaceFace @this)
        {
            return @this.Edges.Select(e => e.Source);
        }

        public static IEnumerable<IPolysurfaceFace> Faces(this IPolysurfaceVertex @this)
        {
            return @this.Edges.Select(e => e.RightFace);
        }

        public static Segment3 Segment(this IPolysurfaceEdge @this)
        {
            return new Segment3(@this.Source.Point, @this.Target.Point);
        }

        public static Vector3 Vector(this IPolysurfaceEdge @this)
        {
            return @this.Target.Point - @this.Source.Point;
        }

        public static double Area(this IPartialPolysurface @this)
        {
            return @this.Faces.Select(f => f.Polygon.Area()).Sum();
        }

        public static double DistanceTo(this Vector3 @this, IPolysurface surface)
        {
            return Math.Sqrt(surface.Faces.Select(f => Polygon3Utils.Distance2(@this, f.Polygon)).Min());
        }

        public static Box3 BoundingBox(this IPartialPolysurface @this)
        {
            return Box3.Hull(@this.Vertices.Select(v => v.Point));
        }
    }
}