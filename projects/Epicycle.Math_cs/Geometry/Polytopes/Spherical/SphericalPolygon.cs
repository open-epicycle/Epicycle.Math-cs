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

using Epicycle.Commons.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes.Spherical
{
    public sealed class SphericalPolygon : ISphericalPolygon
    {
        #region creation

        private SphericalPolygon()
        {
            _contours = new List<IClosedSphericalPolyline>().AsReadOnlyList();
        }

        public SphericalPolygon(IClosedSphericalPolyline contour)
        {
            _contours = new List<IClosedSphericalPolyline> { contour }.AsReadOnlyList();
        }

        public SphericalPolygon(IEnumerable<UnitVector3> vertices)
            : this(new ClosedSphericalPolyline(vertices)) { }

        public SphericalPolygon(IReadOnlyList<UnitVector3> vertices)
            : this(new ClosedSphericalPolyline(vertices)) { }

        public SphericalPolygon(IEnumerable<IClosedSphericalPolyline> contours)
        {
            _contours = contours.ToList().AsReadOnlyList();
        }

        public SphericalPolygon(IEnumerable<IEnumerable<UnitVector3>> contours) : this(contours.Select(c => new ClosedSphericalPolyline(c))) { }

        internal SphericalPolygon(SphericalPolygonBuilder builder) : this(builder.Contours) { }

        public static explicit operator SphericalPolygon(ClosedSphericalPolyline contour)
        {
            return new SphericalPolygon(contour);
        }

        public static explicit operator SphericalPolygon(List<UnitVector3> vertices)
        {
            return new SphericalPolygon((ClosedSphericalPolyline)vertices);
        }

        #endregion

        #region properties

        public IReadOnlyCollection<IClosedSphericalPolyline> Contours
        {
            get { return _contours; }
        }

        private readonly IReadOnlyCollection<IClosedSphericalPolyline> _contours;

        public bool IsEmpty
        {
            get { return Contours.Count == 0; }
        }

        // Assumes there are no self-intersections
        public bool IsSimple
        {
            get { return Contours.Count == 1; }
        }

        #endregion

        public int CountVertices()
        {
            return Contours.Select(contour => contour.Vertices.Count).Sum();
        }

        public static SphericalPolygon Empty
        {
            get { return _emptyPolygon; }
        }

        private static readonly SphericalPolygon _emptyPolygon = new SphericalPolygon();
    }
}
