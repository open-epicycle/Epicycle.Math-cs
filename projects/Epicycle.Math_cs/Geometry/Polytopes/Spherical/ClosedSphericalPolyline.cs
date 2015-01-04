using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes.Spherical
{
    public sealed class ClosedSphericalPolyline : IClosedSphericalPolyline
    {
        public ClosedSphericalPolyline(IEnumerable<UnitVector3> vertices)
        {
            _vertices = vertices.ToList();
        }

        public ClosedSphericalPolyline(IReadOnlyList<UnitVector3> vertices)
        {
            _vertices = vertices;
        }

        public ClosedSphericalPolyline(IClosedSphericalPolyline line)
        {
            _vertices = line.Vertices;
        }

        public static implicit operator ClosedSphericalPolyline(List<UnitVector3> vertices)
        {
            return new ClosedSphericalPolyline(vertices);
        }

        public IReadOnlyList<UnitVector3> Vertices
        {
            get { return _vertices; }
        }

        private readonly IReadOnlyList<UnitVector3> _vertices;
    }
}
