using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry.Polytopes
{
    // UNDER CONSTRUCTION, DO NOT USE
    public sealed class PolysurfaceBuilder : IPartialPolysurface
    {
        public PolysurfaceBuilder()
        {
            _vertices = new List<IPolysurfaceVertex>();
            _edges = new List<IPolysurfaceEdge>();

            throw new NotImplementedException();
        }

        private readonly List<IPolysurfaceVertex> _vertices;
        private readonly List<IPolysurfaceEdge> _edges;

        public IPolysurfaceVertex CreateVertex(Vector3 point)
        {
            var vertex = new Vertex(point);

            _vertices.Add(vertex);

            return vertex;
        }

        public IPolysurfaceEdge CreateEdge(IPolysurfaceVertex source, IPolysurfaceVertex target)
        {
            throw new NotImplementedException();
        }

        public IPolysurfaceFace CreateFace(params IPolysurfaceEdge[] edges)
        {
            throw new NotImplementedException();
        }

        // resets state to empty
        public IPolysurface ExtractPolysurface()
        {
            var answer = new Polysurface(this);

            // TODO: clear...

            return answer;
        }

        public IReadOnlyCollection<IPolysurfaceVertex> Vertices 
        {
            get { throw new NotImplementedException(); }
        }

        public IReadOnlyCollection<IPolysurfaceEdge> Edges 
        {
            get { throw new NotImplementedException(); }
        }

        public IReadOnlyCollection<IPolysurfaceFace> Faces 
        {
            get { throw new NotImplementedException(); }
        }

        public IPolysurfaceEdge GetEdge(IPolysurfaceVertex source, IPolysurfaceVertex target)
        {
            throw new NotImplementedException();
        }

        public IEqualityComparer<IPolysurfaceEdge> UndirectedEdgeComparer
        {
            get { throw new NotImplementedException(); }
        }

        private sealed class Vertex : IPolysurfaceVertex
        {
            public Vertex(Vector3 point)
            {
                _point = point;
                _edges = new List<Edge>();
            }

            private readonly Vector3 _point;
            private readonly List<Edge> _edges;

            public IList<Edge> UnorientedEdges
            {
                get { return _edges; }
            }

            public Vector3 Point
            {
                get { return _point; }
            }

            public int Order
            {
                get { return _edges.Count; }
            }

            public IEnumerable<IPolysurfaceEdge> Edges
            {
                get { return _edges.Select(e => e.Source == this ? e : e.CreateReverse()); }
            }

            public bool Equals(IPolysurfaceVertex other)
            {
                return base.Equals(other);
            }
        }

        private sealed class Edge : IPolysurfaceEdge
        {
            public Edge(Vertex source, Vertex target)
            {
                _source = source;
                _target = target;
            }

            private readonly Vertex _source;
            private readonly Vertex _target;

            public Vertex Source
            {
                get { return _source; }
            }

            public Vertex Target
            {
                get { return _target; }
            }

            public Face RightFace { get; set; }
            public Face LeftFace { get; set; }

            public Edge CreateReverse()
            {
                var answer = new Edge(Target, Source);

                answer.LeftFace = RightFace;
                answer.RightFace = LeftFace;

                return answer;
            }

            IPolysurfaceVertex IPolysurfaceEdge.Source
            {
                get { return Source; }
            }

            IPolysurfaceVertex IPolysurfaceEdge.Target
            {
                get { return Target; }
            }

            IPolysurfaceFace IPolysurfaceEdge.RightFace
            {
                get { return RightFace; }
            }

            IPolysurfaceFace IPolysurfaceEdge.LeftFace
            {
                get { return LeftFace; }
            }

            public bool Equals(IPolysurfaceEdge other)
            {
                return base.Equals(other);
            }

            public IPolysurfaceEdge Reversed
            {
                get { return new Edge(_target, _source); }
            }
        }

        private sealed class Face : IPolysurfaceFace
        {
            public Face()
            {
                _edges = new List<Edge>();
            }

            private readonly List<Edge> _edges;

            public IList<Edge> UnorientedEdges
            {
                get { return _edges; }
            }

            public IPolygon3 Polygon
            {
                get { throw new NotImplementedException(); }
            }

            public Vector3 Normal
            {
                get { throw new NotImplementedException(); }
            }

            public int Order
            {
                get { return _edges.Count; }
            }

            public IEnumerable<IPolysurfaceEdge> Edges
            {
                get { return _edges.Select(e => e.LeftFace == this ? e : e.CreateReverse()); }
            }

            public bool Equals(IPolysurfaceFace other)
            {
                return base.Equals(other);
            }            
        }
    }
}
