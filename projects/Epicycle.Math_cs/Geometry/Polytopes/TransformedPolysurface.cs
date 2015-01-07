// [[[[INFO>
// Copyright 2015 Epicycle (http://epicycle.org, https://github.com/open-epicycle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/open-epicycle/Epicycle.Math-cs
// ]]]]

using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    public class TransformedPolysurface : IPolysurface
    {
        public TransformedPolysurface(IPolysurface localSurface, RotoTranslation3 localToGlobal)
        {
            _localSurface = localSurface;
            _localToGlobal = localToGlobal;

            _vertexCollection = new VertexCollection(this);
            _edgeCollection = new EdgeCollection(this);
            _faceCollection = new FaceCollection(this);

            _undirectedComparer = new UndirectedEdgeComparerImpl(this);
        }

        private readonly IPolysurface _localSurface;
        private readonly RotoTranslation3 _localToGlobal;

        private readonly VertexCollection _vertexCollection;
        private readonly EdgeCollection _edgeCollection;
        private readonly FaceCollection _faceCollection;

        private readonly UndirectedEdgeComparerImpl _undirectedComparer;

        public IPolysurface LocalSurface
        {
            get { return _localSurface; }
        }

        public RotoTranslation3 LocalToGlobal
        {
            get { return _localToGlobal; }
        }

        public IReadOnlyCollection<IPolysurfaceVertex> Vertices
        {
            get { return _vertexCollection; }
        }

        public IReadOnlyCollection<IPolysurfaceEdge> Edges
        {
            get { return _edgeCollection; }
        }

        public IReadOnlyCollection<IPolysurfaceFace> Faces
        {
            get { return _faceCollection; }
        }

        public IPolysurfaceEdge GetEdge(IPolysurfaceVertex source, IPolysurfaceVertex target)
        {
            var nativeSource = (TransformedVertex)source;
            var nativeTarget = (TransformedVertex)target;

            return new TransformedEdge(_localSurface.GetEdge(nativeSource.LocalVertex, nativeTarget.LocalVertex), this);
        }

        public IEqualityComparer<IPolysurfaceEdge> UndirectedEdgeComparer
        {
            get { return _undirectedComparer; }
        }

        private sealed class UndirectedEdgeComparerImpl : IEqualityComparer<IPolysurfaceEdge>
        {
            public UndirectedEdgeComparerImpl(TransformedPolysurface parent)
            {
                _parent = parent;
            }

            private readonly TransformedPolysurface _parent;

            public bool Equals(IPolysurfaceEdge x, IPolysurfaceEdge y)
            {
                var nativeX = (TransformedEdge)x;
                var nativeY = (TransformedEdge)y;

                return 
                    nativeX.Parent == nativeY.Parent &&
                    LocalComparer.Equals(nativeX.LocalEdge, nativeY.LocalEdge);
            }

            public int GetHashCode(IPolysurfaceEdge edge)
            {
                var nativeEdge = (TransformedEdge)edge;

                return nativeEdge.Parent.GetHashCode() ^ LocalComparer.GetHashCode(nativeEdge.LocalEdge);
            }

            private IEqualityComparer<IPolysurfaceEdge> LocalComparer
            {
                get { return _parent._localSurface.UndirectedEdgeComparer; }
            }
        }

        #region data implementation

        private sealed class VertexCollection : IReadOnlyCollection<IPolysurfaceVertex>
        {
            public VertexCollection(TransformedPolysurface parent)
            {
                _parent = parent;
            }

            private readonly TransformedPolysurface _parent;

            public int Count
            {
                get { return LocalVertices.Count; }
            }

            public IEnumerator<IPolysurfaceVertex> GetEnumerator()
            {
                return LocalVertices.Select(v => new TransformedVertex(v, _parent)).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private IReadOnlyCollection<IPolysurfaceVertex> LocalVertices
            {
                get { return _parent._localSurface.Vertices; }
            }
        }

        private sealed class EdgeCollection : IReadOnlyCollection<IPolysurfaceEdge>
        {
            public EdgeCollection(TransformedPolysurface parent)
            {
                _parent = parent;
            }

            private readonly TransformedPolysurface _parent;

            public int Count
            {
                get { return LocalEdges.Count; }
            }

            public IEnumerator<IPolysurfaceEdge> GetEnumerator()
            {
                return LocalEdges.Select(v => new TransformedEdge(v, _parent)).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private IReadOnlyCollection<IPolysurfaceEdge> LocalEdges
            {
                get { return _parent._localSurface.Edges; }
            }
        }

        private sealed class FaceCollection : IReadOnlyCollection<IPolysurfaceFace>
        {
            public FaceCollection(TransformedPolysurface parent)
            {
                _parent = parent;
            }

            private readonly TransformedPolysurface _parent;

            public int Count
            {
                get { return LocalFaces.Count; }
            }

            public IEnumerator<IPolysurfaceFace> GetEnumerator()
            {
                return LocalFaces.Select(v => new TransformedFace(v, _parent)).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private IReadOnlyCollection<IPolysurfaceFace> LocalFaces
            {
                get { return _parent._localSurface.Faces; }
            }
        }

        private sealed class TransformedVertex : IPolysurfaceVertex, IEquatable<TransformedVertex>
        {
            public TransformedVertex(IPolysurfaceVertex localVertex, TransformedPolysurface parent)
            {
                _localVertex = localVertex;
                _parent = parent;
            }

            private readonly IPolysurfaceVertex _localVertex;
            private readonly TransformedPolysurface _parent;

            public IPolysurfaceVertex LocalVertex
            {
                get { return _localVertex; }
            }

            public Vector3 Point
            {
                get { return _parent._localToGlobal.Apply(_localVertex.Point); }
            }

            public int Order
            {
                get { return _localVertex.Order; }
            }

            public IEnumerable<IPolysurfaceEdge> Edges
            {
                get { return _localVertex.Edges.Select(edge => new TransformedEdge(edge, _parent)); }
            }

            #region equality

            public bool Equals(IPolysurfaceVertex other)
            {
                return Equals(other as TransformedVertex);
            }

            public bool Equals(TransformedVertex that)
            {
                return
                    that != null && 
                    this._localVertex.Equals(that._localVertex) &&
                    this._parent == that._parent;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TransformedVertex);
            }

            public override int GetHashCode()
            {
                return _parent.GetHashCode() ^ _localVertex.GetHashCode();
            }

            #endregion
        }

        private sealed class TransformedEdge : IPolysurfaceEdge, IEquatable<TransformedEdge>
        {
            public TransformedEdge(IPolysurfaceEdge localEdge, TransformedPolysurface parent)
            {
                _localEdge = localEdge;
                _parent = parent;
            }

            private readonly IPolysurfaceEdge _localEdge;
            private readonly TransformedPolysurface _parent;

            #region properties

            public IPolysurfaceEdge LocalEdge
            {
                get { return _localEdge; }
            }

            public TransformedPolysurface Parent
            {
                get { return _parent; }
            }

            public IPolysurfaceVertex Source
            {
                get { return new TransformedVertex(_localEdge.Source, _parent); }
            }

            public IPolysurfaceVertex Target
            {
                get { return new TransformedVertex(_localEdge.Target, _parent); }
            }

            public IPolysurfaceFace RightFace
            {
                get { return new TransformedFace(_localEdge.RightFace, _parent); }
            }

            public IPolysurfaceFace LeftFace
            {
                get { return new TransformedFace(_localEdge.LeftFace, _parent); }
            }

            public IPolysurfaceEdge Reversed
            {
                get { return new TransformedEdge(_localEdge.Reversed, _parent); }
            }

            #endregion

            #region equality

            public bool Equals(IPolysurfaceEdge other)
            {
                return Equals(other as TransformedEdge);
            }

            public bool Equals(TransformedEdge that)
            {
                return
                    that != null && 
                    this._localEdge.Equals(that._localEdge) &&
                    this._parent == that._parent; 
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TransformedEdge);
            }

            public override int GetHashCode()
            {
                return _parent.GetHashCode() ^ _localEdge.GetHashCode();
            }

            #endregion            
        }

        private sealed class TransformedFace : IPolysurfaceFace, IEquatable<TransformedFace>
        {
            public TransformedFace(IPolysurfaceFace localFace, TransformedPolysurface parent)
            {
                _localFace = localFace;
                _parent = parent;
            }

            private readonly IPolysurfaceFace _localFace;
            private readonly TransformedPolysurface _parent;

            public IPolygon3 Polygon
            {
                get { return _parent._localToGlobal.Apply(_localFace.Polygon); }
            }

            public Vector3 Normal
            {
                get { return _parent._localToGlobal.Rotation.Apply(_localFace.Normal); }
            }

            public int Order
            {
                get { return _localFace.Order; }
            }

            public IEnumerable<IPolysurfaceEdge> Edges
            {
                get { return _localFace.Edges.Select(edge => new TransformedEdge(edge, _parent)); }
            }

            #region equality

            public bool Equals(IPolysurfaceFace other)
            {
                return Equals(other as TransformedFace);
            }

            public bool Equals(TransformedFace that)
            {
                return
                    that != null &&
                    this._localFace.Equals(that._localFace) &&
                    this._parent == that._parent;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TransformedFace);
            }

            public override int GetHashCode()
            {
                return _parent.GetHashCode() ^ _localFace.GetHashCode();
            }

            #endregion
        }

        #endregion
    }
}
