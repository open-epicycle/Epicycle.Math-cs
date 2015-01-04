using System.Collections.Generic;

namespace Epicycle.Math.Geometry.Polytopes
{
    public sealed class PolygonBuilder
    {
        public PolygonBuilder()
        {
            _contours = new List<IClosedPolyline>();
        }

        public void AddContour(IEnumerable<Vector2> vertices)
        {
            AddContour(new ClosedPolyline(vertices));
        }

        public void AddContour(IReadOnlyList<Vector2> vertices)
        {
            AddContour(new ClosedPolyline(vertices));
        }

        public void AddContour(IClosedPolyline contour)
        {
            _contours.Add(contour);
        }

        // resets state to empty
        public IPolygon ExtractPolygon()
        {
            var answer = new Polygon(this);

            _contours.Clear();

            return answer;
        }

        internal IReadOnlyCollection<IClosedPolyline> Contours
        {
            get { return _contours; }
        }

        private readonly List<IClosedPolyline> _contours;
    }
}
