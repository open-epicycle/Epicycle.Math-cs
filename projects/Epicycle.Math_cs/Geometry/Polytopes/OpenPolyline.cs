using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry.Polytopes
{
    // open polygonal line, mutable
    // explicit implementation of IEnumerable is used for collection style initialization
    public sealed class OpenPolyline : IOpenPolylineBuilder, IEnumerable<Vector2>
    {
        public OpenPolyline()
        {
            _vertices = new List<Vector2>();
        }

        public OpenPolyline(IEnumerable<Vector2> vertices)
        {
            _vertices = vertices.ToList();
        }

        public OpenPolyline(List<Vector2> vertices)
        {
            _vertices = vertices;
        }

        public OpenPolyline(IOpenPolyline line)
        {
            _vertices = line.Vertices.ToList();
        }

        public static implicit operator OpenPolyline(List<Vector2> vertices)
        {
            return new OpenPolyline(vertices);
        }

        bool IPolyline.IsClosed
        {
            get { return false; }
        }

        public IReadOnlyList<Vector2> Vertices
        {
            get { return _vertices; }
        }

        private readonly List<Vector2> _vertices;

        public void Add(Vector2 vertex)
        {
            _vertices.Add(vertex);
        }

        public void Join(IOpenPolyline line)
        {
            _vertices.AddRange(line.Vertices);
        }

        IEnumerator<Vector2> IEnumerable<Vector2>.GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Vector2>)this).GetEnumerator();
        }
    }
}
