using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry.Polytopes
{
    // immutable
    public sealed class ClosedPolyline3 : IClosedPolyline3
    {
        public ClosedPolyline3(IEnumerable<Vector3> vertices)
        {
           _vertices = vertices.ToList();
        }

        public ClosedPolyline3(IPolysurfaceFace face)
        {
            _vertices = face.Edges.Select(e => e.Source.Point).ToList();
        }

        public static implicit operator ClosedPolyline3(List<Vector3> vertices)
        {
            return new ClosedPolyline3(vertices);
        }

        public IReadOnlyList<Vector3> Vertices
        {
            get { return _vertices; }
        }

        private readonly IReadOnlyList<Vector3> _vertices;
    }
}
