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

using System;

namespace Epicycle.Math.Geometry.Polytopes
{
    public sealed class VerticalPrism : VerticalPrismalSurface, IPolyhedron
    {
        public VerticalPrism(IClosedPolyline @base, Interval zRange) : base(@base, zRange, isClosed: true)
        {
            if (!_isCounterclockwise)
            {
                throw new ArgumentException("@base has to be counterclockwise", "@base");
            }

            _basePolygon = new Polygon(@base);
        }

        public VerticalPrism(Box3 box) : base(box.XYRange, box.ZRange, isClosed: true)
        {
            _basePolygon = box.XYRange;
        }

        public static implicit operator VerticalPrism(Box3 box)
        {
            return new VerticalPrism(box);
        }

        private readonly IPolygon _basePolygon;

        public bool Contains(Vector3 point)
        {
            return _basePolygon.Contains(point.XY) && ZRange.Contains(point.Z);
        }

        public double Distance2(Vector3 point)
        {
            return _basePolygon.Distance2(point.XY) + ZRange.Distance2(point.Z);
        }
    }
}
