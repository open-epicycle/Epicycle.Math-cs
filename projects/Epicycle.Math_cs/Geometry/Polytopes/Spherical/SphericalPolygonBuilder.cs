using System.Collections.Generic;

namespace Epicycle.Math.Geometry.Polytopes.Spherical
{
    public sealed class SphericalPolygonBuilder
    {
        public SphericalPolygonBuilder()
        {
            _contours = new List<IClosedSphericalPolyline>();
        }

        public void AddContour(IEnumerable<UnitVector3> vertices)
        {
            AddContour(new ClosedSphericalPolyline(vertices));
        }

        public void AddContour(IReadOnlyList<UnitVector3> vertices)
        {
            AddContour(new ClosedSphericalPolyline(vertices));
        }

        public void AddContour(IClosedSphericalPolyline contour)
        {
            _contours.Add(contour);
        }

        // resets state to empty
        public ISphericalPolygon ExtractPolygon()
        {
            var answer = new SphericalPolygon(this);

            _contours.Clear();

            return answer;
        }

        internal IReadOnlyCollection<IClosedSphericalPolyline> Contours
        {
            get { return _contours; }
        }

        private readonly List<IClosedSphericalPolyline> _contours;
    }
}
