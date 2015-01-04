using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes.Spherical
{
    using System;

    public sealed class SphericalPolygon : ISphericalPolygon
    {
        #region creation

        private SphericalPolygon()
        {
            _contours = new List<IClosedSphericalPolyline>();
        }

        public SphericalPolygon(IClosedSphericalPolyline contour)
        {
            _contours = new List<IClosedSphericalPolyline> { contour };
        }

        public SphericalPolygon(IEnumerable<UnitVector3> vertices)
            : this(new ClosedSphericalPolyline(vertices)) { }

        public SphericalPolygon(IReadOnlyList<UnitVector3> vertices)
            : this(new ClosedSphericalPolyline(vertices)) { }

        public SphericalPolygon(IEnumerable<IClosedSphericalPolyline> contours)
        {
            _contours = contours.ToList();
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
