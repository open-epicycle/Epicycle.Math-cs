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
using System.Text;
using System.Threading.Tasks;

using Epicycle.Commons;

namespace Epicycle.Math.Geometry.Polytopes
{
    // surface orientation is compatible with orientation of *closed* vertical prismal surfaces being exterior for counterclockwise base
    public class VerticalPrismalSurface : IPolysurface
    {
        public VerticalPrismalSurface(IPolyline @base, Interval zRange, bool isClosed = false)
        {
            if (zRange.IsEmpty)
            {
                throw new ArgumentException("z range should not be empty", "zRange");
            }

            _base = @base;
            _zRange = zRange;
            _isClosed = isClosed;

            if (isClosed)
            {
                if (!@base.IsClosed)
                {
                    throw new ArgumentException("Closed prism has to have closed base");
                }

                _closedBase = (IClosedPolyline)@base;
                _isCounterclockwise = _closedBase.SignedArea() > 0;
            }            

            _vertices = new VertexCollection(this);
            _edges = new EdgeCollection(this);
            _faces = new FaceCollection(this);

            _undirectedComparer = new UndirectedEdgeComparerImpl(this);
        }

        private readonly IPolyline _base;
        private readonly IClosedPolyline _closedBase;
        private readonly Interval _zRange;
        private readonly bool _isClosed;
        protected readonly bool _isCounterclockwise;

        private readonly VertexCollection _vertices;
        private readonly EdgeCollection _edges;
        private readonly FaceCollection _faces;

        private readonly UndirectedEdgeComparerImpl _undirectedComparer;

        public IPolyline Base
        {
            get { return _base; }
        }

        public Interval ZRange
        {
            get { return _zRange; }
        }

        public bool IsClosed
        {
            get { return _isClosed; }
        }

        public int Order
        {
            get { return Base.Edges().Count; }
        }

        public double Height
        {
            get { return ZRange.Length; }
        }

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
            var sourceV = (Vertex)source;
            var targetV = (Vertex)target;

            if (sourceV.Parent != this)
            {
                throw new ArgumentException("Vertices must belong to this surface", "source");
            }

            if (targetV.Parent != this)
            {
                throw new ArgumentException("Vertices must belong to this surface", "target");
            }

            return Vertex.GetEdge(sourceV, targetV);
        }

        public IEqualityComparer<IPolysurfaceEdge> UndirectedEdgeComparer
        {
            get { return _undirectedComparer; }
        }

        private sealed class UndirectedEdgeComparerImpl : IEqualityComparer<IPolysurfaceEdge>
        {
            public UndirectedEdgeComparerImpl(VerticalPrismalSurface parent)
            {
                _parent = parent;
            }

            private readonly VerticalPrismalSurface _parent;

            public bool Equals(IPolysurfaceEdge x, IPolysurfaceEdge y)
            {
                var horX = x as HorizontalEdge;

                if (horX != null)
                {
                    return horX.UndirectedEquals(y);
                }

                var verX = x as VerticalEdge;

                if (verX != null)
                {
                    return verX.UndirectedEquals(y);
                }

                throw new ArgumentException("Expected VerticalPrismalSurface edge", "x");
            }

            public int GetHashCode(IPolysurfaceEdge edge)
            {
                var horX = edge as HorizontalEdge;

                if (horX != null)
                {
                    return horX.UndirectedHashCode;
                }

                var verX = edge as VerticalEdge;

                if (verX != null)
                {
                    return verX.UndirectedHashCode;
                }

                throw new ArgumentException("Expected VerticalPrismalSurface edge", "edge");
            }
        }

        #endregion        

        public enum BaseSide
        {
            Bottom,
            Top
        }

        public IPolysurfaceVertex GetVertex(int index, BaseSide baseSide)
        {
            ArgAssert.ValidEnum(baseSide, "baseFace");

            return new Vertex(this, index, baseSide);
        }        

        public IPolysurfaceFace GetVerticalFace(int index)
        {
            return new VerticalFace(this, index);
        }

        public IPolysurfaceFace GetHorizontalFace(BaseSide baseSide)
        {
            return new HorizontalFace(this, baseSide);
        }

        #region data implementation

        private sealed class VertexCollection : IReadOnlyCollection<IPolysurfaceVertex>
        {
            public VertexCollection(VerticalPrismalSurface parent)
            {
                _parent = parent;
            }

            private readonly VerticalPrismalSurface _parent;

            public int Count
            {
                get { return 2 * _parent.Base.Vertices.Count; }
            }

            public IEnumerator<IPolysurfaceVertex> GetEnumerator()
            {
                for (int i = 0; i < _parent.Base.Vertices.Count; i++)
                {
                    yield return new Vertex(_parent, i, BaseSide.Bottom);
                    yield return new Vertex(_parent, i, BaseSide.Top);
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private sealed class Vertex : IPolysurfaceVertex, IEquatable<Vertex>
        {
            public Vertex(VerticalPrismalSurface parent, int index, BaseSide baseFace)
            {
                _parent = parent;
                _index = index;
                _baseFace = baseFace;
            }

            private readonly VerticalPrismalSurface _parent;
            private readonly int _index;
            private readonly BaseSide _baseFace;

            #region properties

            internal VerticalPrismalSurface Parent
            {
                get { return _parent; }
            }
            
            public Vector3 Point
            {
                get 
                {
                    var z = (_baseFace == BaseSide.Bottom ? _parent.ZRange.Min : _parent.ZRange.Max);

                    return new Vector3(_parent.Base.Vertices[_index], z);
                }
            }

            public int Order
            {
                get 
                {
                    if(_parent.Base.IsClosed || (_index > 0 && _index < _parent.Order))
                    {
                        return 3;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }

            public IEnumerable<IPolysurfaceEdge> Edges
            {
                get 
                { 
                    if(_baseFace == BaseSide.Bottom)
                    {
                        if(_parent.Base.IsClosed || _index < _parent.Order)
                        {
                            yield return ForwardEdge;
                        }

                        yield return new VerticalEdge(_parent, _index, VerticalEdge.Direction.Up);

                        if (_parent.Base.IsClosed || _index > 0)
                        {
                            yield return BackwardEdge;
                        }
                    }
                    else
                    {
                        if (_parent.Base.IsClosed || _index > 0)
                        {
                            yield return BackwardEdge;
                        }

                        yield return new VerticalEdge(_parent, _index, VerticalEdge.Direction.Down);

                        if (_parent.Base.IsClosed || _index < _parent.Order)
                        {
                            yield return ForwardEdge;
                        }
                    }
                }
            }

            private IPolysurfaceEdge BackwardEdge
            {
                get { return new HorizontalEdge(_parent, _index > 0 ? _index - 1 : _parent.Order - 1, _baseFace, HorizontalEdge.Direction.Backward); }
            }

            private IPolysurfaceEdge ForwardEdge
            {
                get { return new HorizontalEdge(_parent, _index, _baseFace, HorizontalEdge.Direction.Forward); }
            }

            #endregion

            #region equality

            public bool Equals(Vertex that)
            {
                return
                    that != null &&
                    this._parent == that._parent &&
                    this._index == that._index &&
                    this._baseFace == that._baseFace;
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
                return _parent.GetHashCode() ^ _index ^ (_baseFace.GetHashCode() << 30);
            }

            #endregion

            internal static IPolysurfaceEdge GetEdge(Vertex source, Vertex target)
            {
                if (source._baseFace == target._baseFace)
                {
                    return GetHorizontalEdge(source, target);
                }
                else 
                {
                    return GetVerticalEdge(source, target);
                }
            }

            private static IPolysurfaceEdge GetHorizontalEdge(Vertex source, Vertex target)
            {
                var surface = source.Parent;

                if (target._index == source._index + 1)
                {
                    return new HorizontalEdge(surface, source._index, source._baseFace, HorizontalEdge.Direction.Forward);
                }

                if (target._index == source._index - 1)
                {
                    return new HorizontalEdge(surface, target._index, source._baseFace, HorizontalEdge.Direction.Backward);
                }

                if (surface.Base.IsClosed && target._index == 0 && source._index == surface.Order - 1)
                {
                    return new HorizontalEdge(surface, source._index, source._baseFace, HorizontalEdge.Direction.Forward);
                }

                if (surface.Base.IsClosed && target._index == surface.Order - 1 && source._index == 0)
                {
                    return new HorizontalEdge(surface, target._index, source._baseFace, HorizontalEdge.Direction.Backward);
                }

                return null;
            }

            private static IPolysurfaceEdge GetVerticalEdge(Vertex source, Vertex target)
            {
                if (target._index != source._index)
                {
                    return null;
                }

                var direction = (source._baseFace == BaseSide.Bottom ? VerticalEdge.Direction.Up : VerticalEdge.Direction.Down);

                return new VerticalEdge(source._parent, source._index, direction);
            }
        }

        private sealed class EdgeCollection : IReadOnlyCollection<IPolysurfaceEdge>
        {
            public EdgeCollection(VerticalPrismalSurface parent)
            {
                _parent = parent;
            }

            private readonly VerticalPrismalSurface _parent;

            public int Count
            {
                get { return _parent.Base.IsClosed ? 3 * _parent.Order : 3 * _parent.Order + 1; }
            }

            public IEnumerator<IPolysurfaceEdge> GetEnumerator()
            {
                for (int i = 0; i < _parent.Order; i++)
                {
                    yield return new HorizontalEdge(_parent, i, BaseSide.Bottom);
                    yield return new HorizontalEdge(_parent, i, BaseSide.Top);
                    yield return new VerticalEdge(_parent, i);
                }

                if (!_parent.Base.IsClosed)
                {
                    yield return new VerticalEdge(_parent, _parent.Order);
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private sealed class VerticalEdge : IPolysurfaceEdge, IEquatable<VerticalEdge>
        {
            public VerticalEdge(VerticalPrismalSurface parent, int index, Direction direction = Direction.Up)
            {
                _parent = parent;
                _index = index;
                _direction = direction;
            }

            public enum Direction
            {
                Down,
                Up
            }

            private readonly VerticalPrismalSurface _parent;
            private readonly int _index;
            private readonly Direction _direction;

            #region properties

            public IPolysurfaceVertex Source
            {
                get { return _direction == Direction.Up ? BottomVertex : TopVertex; }
            }

            public IPolysurfaceVertex Target
            {
                get { return _direction == Direction.Up ? TopVertex : BottomVertex; }
            }

            private IPolysurfaceVertex BottomVertex
            {
                get { return new Vertex(_parent, _index, BaseSide.Bottom); }
            }

            private IPolysurfaceVertex TopVertex
            {
                get { return new Vertex(_parent, _index, BaseSide.Top); }
            }

            public IPolysurfaceFace RightFace
            {
                get { return _direction == Direction.Up ? ForwardFace : BackwardFace; }
            }

            public IPolysurfaceFace LeftFace
            {
                get { return _direction == Direction.Up ? BackwardFace : ForwardFace; }
            }

            private IPolysurfaceFace ForwardFace
            {
                get 
                { 
                    if (_parent.Base.IsClosed || _index < _parent.Order)
                    {
                        return new VerticalFace(_parent, _index);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            private IPolysurfaceFace BackwardFace
            {
                get
                {
                    if (_index > 0)
                    {
                        return new VerticalFace(_parent, _index - 1);
                    }
                    else if (_parent.Base.IsClosed)
                    {
                        return new VerticalFace(_parent, _parent.Order - 1);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public IPolysurfaceEdge Reversed 
            {
                get { return new VerticalEdge(_parent, _index, _direction == Direction.Down ? Direction.Up : Direction.Down); }
            }

            #endregion

            #region equality

            public bool Equals(VerticalEdge that)
            {
                return UndirectedEquals(that) && this._direction == that._direction;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as VerticalEdge);
            }

            public bool Equals(IPolysurfaceEdge other)
            {
                return Equals(other as VerticalEdge);
            }

            public override int GetHashCode()
            {
                return UndirectedHashCode ^ (_direction.GetHashCode() << 30);
            }

            public bool UndirectedEquals(VerticalEdge that)
            {
                return
                    that != null &&
                    this._parent == that._parent &&
                    this._index == that._index;
            }

            public bool UndirectedEquals(IPolysurfaceEdge other)
            {
                return UndirectedEquals(other as VerticalEdge);
            }

            public int UndirectedHashCode
            {
                get { return _parent.GetHashCode() ^ _index; }
            }

            #endregion
        }

        private sealed class HorizontalEdge : IPolysurfaceEdge, IEquatable<HorizontalEdge>
        {
            // for a forward edge index is the source vertex; for a backward edge index is the target vertex
            public HorizontalEdge(VerticalPrismalSurface parent, int index, BaseSide baseSide, Direction direction = Direction.Forward)
            {
                _parent = parent;
                _index = index;
                _baseSide = baseSide;
                _direction = direction;
            }

            public enum Direction
            {
                Backward,
                Forward
            }

            private readonly VerticalPrismalSurface _parent;
            private readonly int _index;
            private readonly BaseSide _baseSide;
            private readonly Direction _direction;

            #region properties

            public IPolysurfaceVertex Source
            {
                get { return _direction == Direction.Forward ? BackwardVertex : ForwardVertex; }
            }

            public IPolysurfaceVertex Target
            {
                get { return _direction == Direction.Forward ? ForwardVertex : BackwardVertex; }
            }

            private IPolysurfaceVertex ForwardVertex
            {
                get { return new Vertex(_parent, (_index + 1) % _parent.Base.Vertices.Count, _baseSide); }
            }

            private IPolysurfaceVertex BackwardVertex
            {
                get { return new Vertex(_parent, _index, _baseSide); }
            }

            public IPolysurfaceFace RightFace
            {
                get { return VerticalFaceIsLeft ? HorizontalFace : VerticalFace; }
            }

            public IPolysurfaceFace LeftFace
            {
                get { return VerticalFaceIsLeft ? VerticalFace : HorizontalFace; }
            }

            private bool VerticalFaceIsLeft
            {
                get { return (_direction == Direction.Forward) == (_baseSide == BaseSide.Bottom); }
            }

            private IPolysurfaceFace VerticalFace
            {
                get { return new VerticalFace(_parent, _index); }
            }

            private IPolysurfaceFace HorizontalFace
            {
                get { return _parent.IsClosed ? new HorizontalFace(_parent, _baseSide) : null; }
            }

            public IPolysurfaceEdge Reversed
            {
                get { return new HorizontalEdge(_parent, _index, _baseSide, _direction == Direction.Backward ? Direction.Forward : Direction.Backward); }
            }

            #endregion

            #region equality

            public bool Equals(HorizontalEdge that)
            {
                return UndirectedEquals(that) && this._direction == that._direction;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as HorizontalEdge);
            }

            public bool Equals(IPolysurfaceEdge other)
            {
                return Equals(other as HorizontalEdge);
            }

            public override int GetHashCode()
            {
                return UndirectedHashCode ^ (_direction.GetHashCode() << 28);
            }

            public bool UndirectedEquals(HorizontalEdge that)
            {
                return
                    that != null &&
                    this._parent == that._parent &&
                    this._index == that._index &&
                    this._baseSide == that._baseSide;
            }

            public bool UndirectedEquals(IPolysurfaceEdge other)
            {
                return UndirectedEquals(other as HorizontalEdge);
            }

            public int UndirectedHashCode
            {
                get { return _parent.GetHashCode() ^ _index ^ (_baseSide.GetHashCode() << 30); }
            }

            #endregion
        }

        private sealed class FaceCollection : IReadOnlyCollection<IPolysurfaceFace>
        {
            public FaceCollection(VerticalPrismalSurface parent)
            {
                _parent = parent;
            }

            private readonly VerticalPrismalSurface _parent;

            public int Count
            {
                get { return _parent.IsClosed ? _parent.Order + 2 : _parent.Order; }
            }

            public IEnumerator<IPolysurfaceFace> GetEnumerator()
            {
                for (int i = 0; i < _parent.Order; i++)
                {
                    yield return new VerticalFace(_parent, i);
                }

                if (_parent.IsClosed)
                {
                    yield return new HorizontalFace(_parent, BaseSide.Bottom);
                    yield return new HorizontalFace(_parent, BaseSide.Top);
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private sealed class VerticalFace : IPolysurfaceFace, IEquatable<VerticalFace>
        {
            public VerticalFace(VerticalPrismalSurface parent, int index)
            {
                _parent = parent;
                _index = index;
            }

            private readonly VerticalPrismalSurface _parent;
            private readonly int _index;

            public Vector3 Normal
            {
                get { return (Vector3)_parent.Base.Normal(_index); }
            }

            public IPolygon3 Polygon
            {
                get 
                {
                    var displacement = _parent.Base.Displacement(_index);

                    var origin = new Vector3(_parent.Base.Vertices[_index], _parent.ZRange.Min);         
                    var rotation = Rotation3Utils.GramSchmidt(new Vector3(displacement, 0), Vector3.UnitZ);
                    var coordinateSystem = new RotoTranslation3(rotation, origin);

                    var polygon = new Box2(0, 0, displacement.Norm, _parent.Height);

                    return new Polygon3(coordinateSystem, polygon);
                }
            }

            public int Order
            {
                get { return 4; }
            }

            public IEnumerable<IPolysurfaceEdge> Edges
            {
                get 
                {
                    yield return new VerticalEdge(_parent, _index, VerticalEdge.Direction.Down);
                    yield return new HorizontalEdge(_parent, _index, BaseSide.Bottom, HorizontalEdge.Direction.Forward);
                    yield return new VerticalEdge(_parent, (_index + 1) % _parent.Base.Vertices.Count, VerticalEdge.Direction.Up);
                    yield return new HorizontalEdge(_parent, _index, BaseSide.Top, HorizontalEdge.Direction.Backward);
                }
            }

            #region equality

            public bool Equals(VerticalFace that)
            {
                return
                    that != null &&
                    this._parent == that._parent &&
                    this._index == that._index;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as VerticalFace);
            }

            public bool Equals(IPolysurfaceFace other)
            {
                return Equals(other as VerticalFace);
            }

            public override int GetHashCode()
            {
                return _parent.GetHashCode() ^ _index;
            }

            #endregion
        }

        private sealed class HorizontalFace : IPolysurfaceFace, IEquatable<HorizontalFace>
        {
            public HorizontalFace(VerticalPrismalSurface parent, BaseSide side)
            {
                _parent = parent;
                _side = side;
            }

            private readonly VerticalPrismalSurface _parent;
            private readonly BaseSide _side;

            public IPolygon3 Polygon
            {
                get 
                {
                    var top = _side == BaseSide.Top;

                    var z = top ? _parent.ZRange.Max : _parent.ZRange.Min;
                    var translation = new Vector3(0, 0, z);

                    var contour = top ? _parent._closedBase : _parent._closedBase.AsReverse();

                    if (_parent._isCounterclockwise == top)
                    {
                        return new Polygon3((RotoTranslation3)translation, new Polygon(contour)); 
                    }
                    else
                    {
                        var polygon = new Polygon(contour.Vertices.Select(v => new Vector2(v.X, -v.Y)));
                        return new Polygon3(new RotoTranslation3(Rotation3Utils.RotationX180, translation), polygon);
                    }
                }
            }

            public Vector3 Normal
            {
                get 
                { 
                    return (_side == BaseSide.Top) == _parent._isCounterclockwise ? new Vector3(0, 0, +1) : new Vector3(0, 0, -1);
                }
            }

            public int Order
            {
                get { return _parent.Order; }
            }

            public IEnumerable<IPolysurfaceEdge> Edges
            {
                get 
                {
                    if (_side == BaseSide.Top)
                    {
                        for (var i = 0; i < _parent.Order; i++)
                        {
                            yield return new HorizontalEdge(_parent, i, _side, HorizontalEdge.Direction.Forward);
                        }
                    }
                    else
                    {
                        for (var i = _parent.Order - 1; i >= 0 ; i--)
                        {
                            yield return new HorizontalEdge(_parent, i, _side, HorizontalEdge.Direction.Backward);
                        }
                    }
                }
            }

            #region equality

            public bool Equals(HorizontalFace that)
            {
                return
                    that != null &&
                    this._parent == that._parent &&
                    this._side == that._side;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as HorizontalFace);
            }

            public bool Equals(IPolysurfaceFace other)
            {
                return Equals(other as HorizontalFace);
            }

            public override int GetHashCode()
            {
                return _parent.GetHashCode() ^ _side.GetHashCode();
            }

            #endregion
        }

        #endregion
    }
}
