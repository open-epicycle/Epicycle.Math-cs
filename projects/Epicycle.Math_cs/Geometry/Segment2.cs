using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Math.Geometry.Polytopes;

namespace Epicycle.Math.Geometry
{
    public struct Segment2 : IOpenPolyline
    {
        public Segment2(Vector2 start, Vector2 end)
        {
            _start = start;
            _end = end;
        }

        private readonly Vector2 _start;
        private readonly Vector2 _end;

        public Vector2 Start
        {
            get { return _start; }
        }

        public Vector2 End
        {
            get { return _end; }
        }

        public double Length
        {
            get { return Vector2.Distance(Start, End); }
        }

        public Vector2 Vector
        {
            get { return End - Start; }
        }

        public Vector2 PointAt(double t)
        {
            return t * End + (1 - t) * Start;
        }

        bool IPolyline.IsClosed
        {
            get { return false; }
        }

        public IReadOnlyList<Vector2> Vertices
        {
            get { return new List<Vector2> { Start, End }; }
        }
    }
}
