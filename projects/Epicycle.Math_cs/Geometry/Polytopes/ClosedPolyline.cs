using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    // closed polygonal line; immutable
    public sealed class ClosedPolyline : IClosedPolyline
    {
        public ClosedPolyline(IEnumerable<Vector2> vertices)
        {
            _vertices = vertices.ToList();
        }

        public ClosedPolyline(IReadOnlyList<Vector2> vertices)
        {
            _vertices = vertices;
        }

        public ClosedPolyline(IClosedPolyline line)
        {
            _vertices = line.Vertices;
        }

        public static implicit operator ClosedPolyline(List<Vector2> vertices)
        {
            return new ClosedPolyline(vertices);
        }

        bool IPolyline.IsClosed 
        {
            get { return true; }
        }

        public IReadOnlyList<Vector2> Vertices
        {
            get { return _vertices; }
        }

        private readonly IReadOnlyList<Vector2> _vertices;

        public double SignedArea()
        {
            var answer = 0d;

            for (var i = 0; i < Vertices.Count - 1; i++)
            {
                answer += TrapezoidArea(_vertices[i], _vertices[i + 1]);
            }

            answer += TrapezoidArea(_vertices.Last(), _vertices.First());

            return answer;
        }

        private double TrapezoidArea(Vector2 a, Vector2 b)
        {
            return (a.X - b.X) * (a.Y + b.Y) / 2;
        }
    }
}
