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

namespace Epicycle.Math.Geometry.Polytopes
{

    public sealed class Polygon : IPolygon
    {
        #region creation

        private Polygon()
        {
            _contours = new List<IClosedPolyline>().AsReadOnlyList();
            _boundingBox = Box2.Empty;
        }

        public Polygon(IClosedPolyline contour)
        {
            _contours = new List<IClosedPolyline> { contour }.AsReadOnlyList();
            _boundingBox = Box2.Hull(contour.Vertices);
        }

        public Polygon(IEnumerable<Vector2> vertices)
            : this(new ClosedPolyline(vertices)) { }

        public Polygon(IReadOnlyList<Vector2> vertices)
            : this(new ClosedPolyline(vertices)) { }

        public Polygon(IEnumerable<IClosedPolyline> contours)
        {
            _contours = contours.ToList().AsReadOnlyList();
            _boundingBox = Box2.Hull(_contours.SelectMany(c => c.Vertices));
        }

        public Polygon(IEnumerable<IEnumerable<Vector2>> contours) : this(contours.Select(c => new ClosedPolyline(c))) { }

        internal Polygon(PolygonBuilder builder) : this(builder.Contours) {}

        public static explicit operator Polygon(ClosedPolyline contour)
        {
            return new Polygon(contour);
        }

        public static explicit operator Polygon(List<Vector2> vertices)
        {
            return new Polygon((ClosedPolyline)vertices);
        }

        #endregion

        #region properties

        public IReadOnlyCollection<IClosedPolyline> Contours
        {
            get { return _contours; }
        }

        private readonly IReadOnlyCollection<IClosedPolyline> _contours;

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

        #region methods

        // Assumes contour orientation is canonic (counterclockwise for outer contours, clockwise for inner contours)
        public double Area()
        {
            return Contours.Select(contour => contour.SignedArea()).Sum();
        }

        public Box2 BoundingBox()
        {
            return _boundingBox;
        }

        private readonly Box2 _boundingBox;

        public int CountVertices()
        {
            return Contours.Select(contour => contour.Vertices.Count).Sum();
        }

        public bool Contains(Vector2 point)
        {
            if (!_boundingBox.Contains(point))
            {
                return false;
            }
            else
            {
                return Contours.Select(c => c.WindingNumber(point)).Sum() > 0;
            }
        }

        public double Distance2(Vector2 point)
        {
            if (this.Contains(point))
            {
                return 0;
            }
            else if (this.IsEmpty)
            {
                return double.PositiveInfinity;
            }
            else
            {
                return Contours.Select(c => PolylineUtils.Distance2(point, c)).Min();
            }
        }

        #endregion

        public static Polygon Empty
        {
            get { return _emptyPolygon; }
        }

        private static readonly Polygon _emptyPolygon = new Polygon();
    }
}
