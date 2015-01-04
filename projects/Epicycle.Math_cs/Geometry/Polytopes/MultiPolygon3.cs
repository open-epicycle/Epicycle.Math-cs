using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;
using Epicycle.Commons.Collections;

namespace Epicycle.Math.Geometry.Polytopes
{
    public sealed class MultiPolygon3 : IMultiPolygon3
    {
        public MultiPolygon3(IEnumerable<IPolygon3> components)
        {
            _components = components.ToList();

            _componentCount = _components.Count();
            _totalVertexCount = _components.Select(p => p.InPlane.CountVertices()).Sum();

            _vertices = new VertexCollection(this);
            _edges = new EdgeColllection(this);
            _faces = new FaceCollection(this);
            _undirectedEdgeComparer = new UndirectedEdgeComparerImpl(this);
        }

        public MultiPolygon3(IPolygon3 uniqueCompoenent)
        {
            _components = uniqueCompoenent.AsSingleton();

            _componentCount = 1;
            _totalVertexCount = uniqueCompoenent.InPlane.CountVertices();

            _vertices = new VertexCollection(this);
            _edges = new EdgeColllection(this);
            _faces = new FaceCollection(this);
            _undirectedEdgeComparer = new UndirectedEdgeComparerImpl(this);
        }

        private readonly IReadOnlyCollection<IPolygon3> _components;

        private readonly IReadOnlyCollection<IPolysurfaceVertex> _vertices;
        private readonly IReadOnlyCollection<IPolysurfaceEdge> _edges;
        private readonly IReadOnlyCollection<IPolysurfaceFace> _faces;
        private readonly IEqualityComparer<IPolysurfaceEdge> _undirectedEdgeComparer;

        private readonly int _componentCount;
        private readonly int _totalVertexCount;

        public IReadOnlyCollection<IPolygon3> Components
        {
            get { return _components; }
        }

        public bool IsEmpty
        {
            get { return !_components.Any(); }
        }

        public static MultiPolygon3 Empty
        {
            get { return _empty; }
        }

        private static readonly MultiPolygon3 _empty = new MultiPolygon3(EmptyList<IPolygon3>.Instance);

        #region IPartialPolysurface

        public IReadOnlyCollection<IPolysurfaceVertex> Vertices
        {
            get { return _vertices; }
        }

        public IReadOnlyCollection<IPolysurfaceEdge> Edges
        {
            get { return _edges; }
        }

        public IReadOnlyCollection<IPolysurfaceFace> Faces
        {
            get { return _faces; }
        }

        public IPolysurfaceEdge GetEdge(IPolysurfaceVertex source, IPolysurfaceVertex target)
        {
            return Vertex.GetEdge((Vertex)source, (Vertex)target);
        }

        public IEqualityComparer<IPolysurfaceEdge> UndirectedEdgeComparer
        {
            get { return _undirectedEdgeComparer; }
        }

        private sealed class UndirectedEdgeComparerImpl : IEqualityComparer<IPolysurfaceEdge>
        {
            public UndirectedEdgeComparerImpl(MultiPolygon3 parent)
            {
                _parent = parent;
            }

            private readonly MultiPolygon3 _parent;

            public bool Equals(IPolysurfaceEdge x, IPolysurfaceEdge y)
            {
                return ((Edge)x).UndirectedEquals((Edge)y);
            }

            public int GetHashCode(IPolysurfaceEdge edge)
            {
                return ((Edge)edge).UndirectedHashCode;
            }
        }

        #endregion

        #region data implementation

        private sealed class VertexCollection : IReadOnlyCollection<IPolysurfaceVertex>
        {
            public VertexCollection(MultiPolygon3 parent)
            {
                _parent = parent;
            }

            private readonly MultiPolygon3 _parent;

            public int Count
            {
                get { return _parent._totalVertexCount; }
            }

            public IEnumerator<IPolysurfaceVertex> GetEnumerator()
            {
                return _parent._components.SelectMany
                    (p => p.Contours().SelectMany
                        (c => Enumerable.Range(0, c.Vertices.Count).Select
                            (i => new Vertex(p, c, i)))).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private sealed class Vertex : IPolysurfaceVertex, IEquatable<Vertex>
        {
            public Vertex(IPolygon3 polygon, IClosedPolyline3 contour, int vertexIdx)
            {
                _polygon = polygon;
                _contour = contour;
                _vertexIdx = vertexIdx;
            }

            private readonly IPolygon3 _polygon;
            private readonly IClosedPolyline3 _contour;
            private readonly int _vertexIdx;

            public Vector3 Point
            {
                get { return _contour.Vertices[_vertexIdx]; }
            }

            public int Order
            {
                get { return 2; }
            }

            public IEnumerable<IPolysurfaceEdge> Edges
            {
                get 
                {
                    yield return new Edge(_polygon, _contour, _vertexIdx, Edge.Direction.Forward);

                    if (_vertexIdx > 0)
                    {
                        yield return new Edge(_polygon, _contour, _vertexIdx - 1, Edge.Direction.Backward);
                    }
                    else
                    {
                        yield return new Edge(_polygon, _contour, _contour.Vertices.Count - 1, Edge.Direction.Backward);
                    }
                }
            }

            public bool Equals(Vertex other)
            {
                return
                    other != null &&
                    _polygon == other._polygon &&
                    _contour == other._contour &&
                    _vertexIdx == other._vertexIdx;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Vertex);
            }

            public bool Equals(IPolysurfaceVertex other)
            {
                return Equals(other as Vertex);
            }

            public override int GetHashCode()
            {
                return _polygon.GetHashCode() ^ _contour.GetHashCode() ^ _vertexIdx;
            }

            internal static IPolysurfaceEdge GetEdge(Vertex source, Vertex target)
            {
                var polygon = source._polygon;
                var contour = source._contour;

                if (polygon != target._polygon || contour != target._contour)
                {
                    return null;
                }

                if (target._vertexIdx == source._vertexIdx + 1)
                {
                    return new Edge(polygon, contour, source._vertexIdx, Edge.Direction.Forward);
                }

                if (target._vertexIdx == source._vertexIdx - 1)
                {
                    return new Edge(polygon, contour, target._vertexIdx, Edge.Direction.Backward);
                }

                if (target._vertexIdx == 0 && source._vertexIdx == contour.Vertices.Count - 1)
                {
                    return new Edge(polygon, contour, source._vertexIdx, Edge.Direction.Forward);
                }

                if (target._vertexIdx == contour.Vertices.Count - 1 && source._vertexIdx == 0)
                {
                    return new Edge(polygon, contour, target._vertexIdx, Edge.Direction.Backward);
                }

                return null;
            }
        }

        private sealed class EdgeColllection : IReadOnlyCollection<IPolysurfaceEdge>
        {
            public EdgeColllection(MultiPolygon3 parent)
            {
                _parent = parent;
            }

            private readonly MultiPolygon3 _parent;

            public int Count
            {
                get { return _parent._totalVertexCount; }
            }

            public IEnumerator<IPolysurfaceEdge> GetEnumerator()
            {
                return _parent._components.SelectMany
                    (p => p.Contours().SelectMany
                        (c => Enumerable.Range(0, c.Vertices.Count).Select
                            (i => new Edge(p, c, i, Edge.Direction.Forward)))).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private sealed class Edge : IPolysurfaceEdge, IEquatable<Edge>
        {
            // for a forward edge index is the source vertex; for a backward edge index is the target vertex
            public Edge(IPolygon3 polygon, IClosedPolyline3 contour, int edgeIdx, Direction direction)
            {
                _polygon = polygon;
                _contour = contour;
                _edgeIdx = edgeIdx;
                _direction = direction;
            }

            public enum Direction
            {
                Backward,
                Forward
            }

            private readonly IPolygon3 _polygon;
            private readonly IClosedPolyline3 _contour;
            private readonly int _edgeIdx;
            private readonly Direction _direction;

            public IPolysurfaceVertex Source
            {
                get { return _direction == Direction.Forward ? BackwardVertex : ForwardVertex; }
            }

            public IPolysurfaceVertex Target
            {
                get { return _direction == Direction.Forward ? ForwardVertex : BackwardVertex; }
            }

            public IPolysurfaceVertex BackwardVertex
            {
                get { return new Vertex(_polygon, _contour, _edgeIdx); }
            }

            public IPolysurfaceVertex ForwardVertex
            {
                get { return new Vertex(_polygon, _contour, (_edgeIdx + 1) % _contour.Vertices.Count); }
            }

            public IPolysurfaceFace RightFace
            {
                get { return null; }
            }

            public IPolysurfaceFace LeftFace
            {
                get { return new Face(_polygon); }
            }

            public IPolysurfaceEdge Reversed
            {
                get { return new Edge(_polygon, _contour, _edgeIdx, _direction == Direction.Backward ? Direction.Forward : Direction.Backward); }
            }

            public bool Equals(Edge that)
            {
                return UndirectedEquals(that) && this._direction == that._direction;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Edge);
            }

            public bool Equals(IPolysurfaceEdge other)
            {
                return Equals(other as Edge);
            }

            public override int GetHashCode()
            {
                return UndirectedHashCode ^ (_direction.GetHashCode() << 30);
            }

            public bool UndirectedEquals(Edge that)
            {
                return
                    that != null &&
                    this._polygon == that._polygon &&
                    this._contour == that._contour &&
                    this._edgeIdx == that._edgeIdx;
            }

            public bool UndirectedEquals(IPolysurfaceEdge other)
            {
                return UndirectedEquals(other as Edge);
            }

            public int UndirectedHashCode
            {
                get { return _contour.GetHashCode() ^ _edgeIdx; }
            }
        }

        private sealed class FaceCollection : IReadOnlyCollection<IPolysurfaceFace>
        {
            public FaceCollection(MultiPolygon3 parent)
            {
                _parent = parent;
            }

            private readonly MultiPolygon3 _parent;            

            public int Count
            {
                get { return _parent._componentCount; }
            }

            public IEnumerator<IPolysurfaceFace> GetEnumerator()
            {
                return _parent._components.Select(p => new Face(p)).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private sealed class Face : IPolysurfaceFace, IEquatable<Face>
        {
            public Face(IPolygon3 polygon)
            {
                _polygon = polygon;
                _order = polygon.InPlane.CountVertices();
            }

            private readonly IPolygon3 _polygon;
            private readonly int _order;

            public IPolygon3 Polygon
            {
                get { return _polygon; }
            }

            public Vector3 Normal
            {
                get { return _polygon.Normal(); }
            }

            public int Order
            {
                get { return _order; }
            }

            public IEnumerable<IPolysurfaceEdge> Edges
            {
                get 
                {
                    return _polygon.Contours().SelectMany
                        (c => Enumerable.Range(0, c.Vertices.Count).Select
                            (i => new Edge(_polygon, c, i, Edge.Direction.Forward)));
                }
            }

            public bool Equals(Face other)
            {
                return
                    other != null &&
                    _polygon == other._polygon;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Face);
            }

            public bool Equals(IPolysurfaceFace other)
            {
                return Equals(other as Face);
            }

            public override int GetHashCode()
            {
                return _polygon.GetHashCode();
            }
        }

        #endregion
    }
}
