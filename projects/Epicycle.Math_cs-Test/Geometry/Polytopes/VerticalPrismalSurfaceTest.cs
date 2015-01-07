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

using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    using System;

    [TestFixture]
    public sealed class VerticalPrismalSurfaceTest : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        #region construction

        [Test]
        public void Base_is_same_as_received_in_ctor()
        {
            var @base = Mock.Of<IOpenPolyline>();
            var zRange = new Interval(-3.14, 2.88);

            var surface = new VerticalPrismalSurface(@base, zRange);

            Expect(surface.Base, Is.SameAs(@base));
        }

        [Test]
        public void ZRange_is_equal_to_received_in_ctor()
        {
            var @base = Mock.Of<IOpenPolyline>();
            var zRange = new Interval(-3.14, 2.88);

            var surface = new VerticalPrismalSurface(@base, zRange);

            Expect(surface.ZRange, Is.EqualTo(zRange));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_throws_when_zRange_is_empty()
        {
            var @base = Mock.Of<IOpenPolyline>();

            var zRange = new Interval(3.14, -2.88);

            new VerticalPrismalSurface(@base, zRange);
        }

        #endregion

        #region counting

        private VerticalPrismalSurface CreateSurface(int order, bool isClosedBase = false, bool isClosedSurface = false)
        {
            IPolyline @base;

            if (isClosedBase)
            {
                @base = Mock.Of<IClosedPolyline>(line => line.Vertices.Count == order && line.IsClosed);
            }
            else
            {
                @base = Mock.Of<IOpenPolyline>(line => line.Vertices.Count == order + 1 && !line.IsClosed);
            }            

            var zRange = new Interval(-3.14, 2.88);

            return new VerticalPrismalSurface(@base, zRange, isClosedSurface);
        }        

        [Test]
        public void Order_is_number_of_base_edges([Values(false, true)] bool closed)
        {
            var order = 7;

            var surface = CreateSurface(order, closed);

            Expect(surface.Order, Is.EqualTo(order));
        }

        [Test]
        public void Number_of_surface_Vertices_is_correctly_computed_from_number_of_base_Vertices([Values(false, true)] bool closed)
        {
            var order = 7;

            var surface = CreateSurface(order, closed);

            var numVertices = 2 * surface.Base.Vertices.Count;
            Expect(surface.Vertices.Count, Is.EqualTo(numVertices));
            Expect(surface.Vertices.Count(), Is.EqualTo(numVertices));
        }

        [Test]
        public void Number_of_surface_Edges_is_correctly_computed_from_number_of_base_Vertices([Values(false, true)] bool closed)
        {
            var order = 7;

            var surface = CreateSurface(order, closed);

            var numEdges = 3 * order + (closed ? 0 : 1);
            Expect(surface.Edges.Count, Is.EqualTo(numEdges));
            Expect(surface.Edges.Count(), Is.EqualTo(numEdges));
        }

        [Test, Sequential]
        public void Number_of_surface_Faces_is_correctly_computed_from_number_of_base_Vertices
            ([Values(false, true, true)] bool isClosedBase, [Values(false, false, true)] bool isClosedSurface)
        {
            var order = 7;

            var surface = CreateSurface(order, isClosedBase, isClosedSurface);

            var expectedFaceCount = isClosedSurface ? order + 2 : order;

            Expect(surface.Faces.Count, Is.EqualTo(expectedFaceCount));
            Expect(surface.Faces.Count(), Is.EqualTo(expectedFaceCount));
        }

        #endregion

        private VerticalPrismalSurface CreateSurface(bool isClosedBase = false, bool isClosedSurface = false, bool isCounterclockwise = true)
        {
            var baseVertices = new List<Vector2>
            {
                new Vector2(1.1, 2.2),
                new Vector2(3.5, -5.3),
                new Vector2(-7.8, 9.1),
                new Vector2(-1.8, 0.1),
                new Vector2(3.8, 6.1)
            };

            IPolyline @base;

            if (isClosedBase)
            {
                var line = new ClosedPolyline(baseVertices);
                @base = line.SignedArea() > 0 ^ isCounterclockwise ? line : line.AsReverse();
            }
            else
            {
                @base = new OpenPolyline(baseVertices);
            }

            var zRange = new Interval(-3.14, 2.88);

            return new VerticalPrismalSurface(@base, zRange, isClosedSurface);
        }

        #region vertices

        [Test]
        public void Vertex_points_have_xy_coordinates_as_base_vertices_and_z_coordinates_as_zRange_endpoints([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var vertices = surface.Base.Vertices.SelectMany(v => new List<Vector3>
            { 
                new Vector3(v, surface.ZRange.Min),
                new Vector3(v, surface.ZRange.Max)
            });

            Expect(surface.Vertices.Select(v => v.Point), Is.EquivalentTo(vertices));
        }

        [Test]
        public void Top_base_interior_vertices_have_Edges_with_correct_endpoints_in_counterclockwise_order([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var vertex = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);

            var edges = vertex.Edges;

            Expect(edges.Count(), Is.EqualTo(3));
            Expect(edges.Select(e => e.Source), Has.All.EqualTo(vertex));

            var neighbors = new List<IPolysurfaceVertex>
            {
                surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top),
                surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom),
                surface.GetVertex(index: 2, baseSide: VerticalPrismalSurface.BaseSide.Top),
            };

            Expect(edges.Select(e => e.Target), Is.EqualTo(neighbors).AsCollection);
        }

        [Test]
        public void Bottom_base_interior_vertices_have_Edges_with_correct_endpoints_in_counterclockwise_order([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var vertex = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            var edges = vertex.Edges;

            Expect(edges.Count(), Is.EqualTo(3));
            Expect(edges.Select(e => e.Source), Has.All.EqualTo(vertex));

            var neighbors = new List<IPolysurfaceVertex>
            {
                surface.GetVertex(index: 2, baseSide: VerticalPrismalSurface.BaseSide.Bottom),
                surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top),
                surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom),
            };

            Expect(edges.Select(e => e.Target), Is.EqualTo(neighbors).AsCollection);
        }

        [Test]
        public void Top_left_corner_vertex_has_Edges_with_correct_endpoints_in_counterclockwise_order([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var vertex = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);

            var edges = vertex.Edges;

            Expect(edges.Count(), Is.EqualTo(closed ? 3 : 2));
            Expect(edges.Select(e => e.Source), Has.All.EqualTo(vertex));

            var neighbors = new List<IPolysurfaceVertex>
            {
                surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom),
                surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top),
            };

            if (closed)
            {
                neighbors.Insert(0, surface.GetVertex(index: surface.Order - 1, baseSide: VerticalPrismalSurface.BaseSide.Top));
            }

            Expect(edges.Select(e => e.Target), Is.EqualTo(neighbors).AsCollection);
        }

        [Test]
        public void Top_right_corner_vertex_has_Edges_with_correct_endpoints_in_counterclockwise_order([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);
            var lastIdx = surface.Base.Vertices.Count - 1;

            var vertex = surface.GetVertex(index: lastIdx, baseSide: VerticalPrismalSurface.BaseSide.Top);

            var edges = vertex.Edges;

            Expect(edges.Count(), Is.EqualTo(closed ? 3 : 2));
            Expect(edges.Select(e => e.Source), Has.All.EqualTo(vertex));

            var neighbors = new List<IPolysurfaceVertex>
            {                
                surface.GetVertex(index: lastIdx - 1, baseSide: VerticalPrismalSurface.BaseSide.Top),
                surface.GetVertex(index: lastIdx, baseSide: VerticalPrismalSurface.BaseSide.Bottom),
            };

            if (closed)
            {
                neighbors.Add(surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top));
            }

            Expect(edges.Select(e => e.Target), Is.EqualTo(neighbors).AsCollection);
        }

        [Test]
        public void Bottom_left_corner_vertex_has_Edges_with_correct_endpoints_in_counterclockwise_order([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var vertex = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            var edges = vertex.Edges;

            Expect(edges.Count(), Is.EqualTo(closed ? 3 : 2));
            Expect(edges.Select(e => e.Source), Has.All.EqualTo(vertex));

            var neighbors = new List<IPolysurfaceVertex>
            {                
                surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom),
                surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top),
            };

            if (closed)
            {
                neighbors.Add(surface.GetVertex(index: surface.Order - 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom));
            }

            Expect(edges.Select(e => e.Target), Is.EqualTo(neighbors).AsCollection);
        }

        [Test]
        public void Bottom_right_corner_vertex_has_Edges_with_correct_endpoints_in_counterclockwise_order([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);
            var lastIdx = surface.Base.Vertices.Count - 1;

            var vertex = surface.GetVertex(index: lastIdx, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            var edges = vertex.Edges;

            Expect(edges.Count(), Is.EqualTo(closed ? 3 : 2));
            Expect(edges.Select(e => e.Source), Has.All.EqualTo(vertex));

            var neighbors = new List<IPolysurfaceVertex>
            {                
                surface.GetVertex(index: lastIdx, baseSide: VerticalPrismalSurface.BaseSide.Top),
                surface.GetVertex(index: lastIdx - 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom),
            };

            if (closed)
            {
                neighbors.Insert(0, surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom));
            }

            Expect(edges.Select(e => e.Target), Is.EqualTo(neighbors).AsCollection);
        }

        [Test]
        public void Interior_vertex_order_is_3([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var vertex = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            Expect(vertex.Order, Is.EqualTo(3));
        }

        [Test]
        public void Left_corner_vertex_order_is_2_for_open_base_and_3_for_closed_base([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var vertex = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            Expect(vertex.Order, Is.EqualTo(closed ? 3 : 2));
        }

        [Test]
        public void Right_corner_vertex_order_is_2_for_open_base_and_3_for_closed_base([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);
            var lastIdx = surface.Base.Vertices.Count - 1;

            var vertex = surface.GetVertex(index: lastIdx, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            Expect(vertex.Order, Is.EqualTo(closed ? 3 : 2));
        }

        #endregion   

        #region edges

        [Test]
        public void Surface_Edges_consist_of_bottom_base_edges_top_base_edges_and_vertical_edges_corresponding_to_base_Vertices([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var botVertices = Enumerable.Range(0, surface.Base.Vertices.Count).Select(i => surface.GetVertex(i, VerticalPrismalSurface.BaseSide.Bottom)).ToList();
            var topVertices = Enumerable.Range(0, surface.Base.Vertices.Count).Select(i => surface.GetVertex(i, VerticalPrismalSurface.BaseSide.Top)).ToList();

            var expectedEges = Enumerable.Range(0, surface.Base.Vertices.Count - 1).Select(i => surface.GetEdge(botVertices[i], botVertices[i + 1])).ToList();
            expectedEges.AddRange(Enumerable.Range(0, surface.Base.Vertices.Count - 1).Select(i => surface.GetEdge(topVertices[i], topVertices[i + 1])).ToList());
            expectedEges.AddRange(Enumerable.Range(0, surface.Base.Vertices.Count).Select(i => surface.GetEdge(topVertices[i], botVertices[i])).ToList());

            if (closed)
            {
                expectedEges.Add(surface.GetEdge(topVertices.First(), topVertices.Last()));
                expectedEges.Add(surface.GetEdge(botVertices.First(), botVertices.Last()));
            }

            var edges = surface.Edges;

            Expect(edges, Is.EquivalentTo(expectedEges).Using(surface.UndirectedEdgeComparer));
        }

        #region GedEdge

        private void ExpectGetEdgeReturnsCorrectEdge(VerticalPrismalSurface surface, IPolysurfaceVertex v1, IPolysurfaceVertex v2)
        {
            var edge1 = surface.GetEdge(v1, v2);

            Expect(edge1, Is.Not.Null);
            Expect(edge1.Source, Is.EqualTo(v1));
            Expect(edge1.Target, Is.EqualTo(v2));

            var edge2 = surface.GetEdge(v2, v1);

            Expect(edge2, Is.Not.Null);
            Expect(edge2.Source, Is.EqualTo(v2));
            Expect(edge2.Target, Is.EqualTo(v1));
        }

        [Test]
        public void GetEdge_returns_edge_with_source_and_target_as_specified([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var vertex1 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var vertex2 = surface.GetVertex(index: 2, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            ExpectGetEdgeReturnsCorrectEdge(surface, vertex1, vertex2);

            if (closed)
            {
                vertex1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);
                vertex2 = surface.GetVertex(index: surface.Order - 1, baseSide: VerticalPrismalSurface.BaseSide.Top);

                ExpectGetEdgeReturnsCorrectEdge(surface, vertex1, vertex2);
            }           

            vertex1 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            vertex2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);

            ExpectGetEdgeReturnsCorrectEdge(surface, vertex1, vertex2);
        }

        [Test]
        public void GetEdge_returns_null_for_disconnected_vertices([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var vertex1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var vertex2 = surface.GetVertex(index: 2, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            Expect(surface.GetEdge(vertex1, vertex2), Is.Null);

            vertex1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            vertex2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);

            Expect(surface.GetEdge(vertex1, vertex2), Is.Null);

            if (!closed)
            {
                vertex1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);
                vertex2 = surface.GetVertex(index: surface.Order, baseSide: VerticalPrismalSurface.BaseSide.Top);

                Expect(surface.GetEdge(vertex1, vertex2), Is.Null);
                Expect(surface.GetEdge(vertex2, vertex1), Is.Null);
            }            
        }

        [Test]
        public void GetEdge_returns_null_for_coincident_vertices()
        {
            var surface = CreateSurface();

            var vertex1 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            Expect(surface.GetEdge(vertex1, vertex1), Is.Null);
        }

        [Test]
        public void GetEdge_throws_ArgumentException_when_one_of_vertices_doesnt_belong_to_surface()
        {
            var surface1 = CreateSurface();
            var surface2 = CreateSurface();

            var vertex1 = surface1.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var vertex2 = surface1.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var vertex3 = surface2.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            Expect(() => surface1.GetEdge(vertex1, vertex2), Throws.Nothing);
            Expect(() => surface1.GetEdge(vertex1, vertex3), Throws.ArgumentException);
            Expect(() => surface1.GetEdge(vertex3, vertex2), Throws.ArgumentException);
        }

        [Test]
        public void GetEdge_throws_InvalidCastException_when_one_of_vertices_isnt_VerticalPrismalSurface_vertex()
        {
            var surface = CreateSurface();

            var vertex1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var vertex2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var vertex3 = Mock.Of<IPolysurfaceVertex>();

            Expect(() => surface.GetEdge(vertex1, vertex2), Throws.Nothing);
            Expect(() => surface.GetEdge(vertex1, vertex3), Throws.TypeOf<InvalidCastException>());
            Expect(() => surface.GetEdge(vertex3, vertex2), Throws.TypeOf<InvalidCastException>());
        }

        private void ExpectTwoConstructionsOfEdgeCoincide(VerticalPrismalSurface surface, IPolysurfaceVertex v1, IPolysurfaceVertex v2)
        {
            var edge1 = surface.GetEdge(v1, v2);
            var edge2 = v1.Edges.Where(e => e.Target.Equals(v2)).Single();

            Expect(edge1, Is.EqualTo(edge2));
            Expect(edge1, Is.EqualTo(edge2).Using(surface.UndirectedEdgeComparer));
        }

        #endregion

        #region edge equality

        [Test]
        public void Horizontal_edges_are_equal_when_source_and_target_vertices_coincide()
        {
            var surface = CreateSurface();

            var vertex1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var vertex2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            ExpectTwoConstructionsOfEdgeCoincide(surface, vertex1, vertex2);
        }

        [Test]
        public void Vertical_edges_are_equal_when_source_and_target_vertices_coincide()
        {
            var surface = CreateSurface();

            var vertex1 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var vertex2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            ExpectTwoConstructionsOfEdgeCoincide(surface, vertex1, vertex2);
        }

        private void ExpectOppositeEdgesAreDifferent(VerticalPrismalSurface surface, IPolysurfaceVertex v1, IPolysurfaceVertex v2)
        {
            var edge1 = surface.GetEdge(v1, v2);
            var edge2 = surface.GetEdge(v2, v1);

            Expect(edge1, Is.Not.EqualTo(edge2));
        }

        [Test]
        public void Horizontal_edges_are_not_equal_when_direction_is_opposite()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            ExpectOppositeEdgesAreDifferent(surface, v1, v2);
        }

        [Test]
        public void Vertical_edges_are_not_equal_when_direction_is_opposite()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            ExpectOppositeEdgesAreDifferent(surface, v1, v2);
        }

        [Test]
        public void Horizontal_edges_are_not_equal_when_on_different_bases()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v3 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v4 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);

            var e1 = surface.GetEdge(v1, v2);
            var e2 = surface.GetEdge(v3, v4);

            Expect(e1, Is.Not.EqualTo(e2));
            Expect(e1, Is.Not.EqualTo(e2).Using(surface.UndirectedEdgeComparer));
        }

        [Test]
        public void Edges_belonging_to_different_surfaces_are_not_equal()
        {
            var surface1 = CreateSurface();
            var surface2 = CreateSurface();

            var v1 = surface1.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v2 = surface1.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v3 = surface1.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            var w1 = surface2.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var w2 = surface2.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var w3 = surface2.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            var e11 = surface1.GetEdge(v1, v2);
            var e12 = surface1.GetEdge(v1, v3);
            var e21 = surface2.GetEdge(w1, w2);
            var e22 = surface2.GetEdge(w1, w3);

            Expect(e11, Is.Not.EqualTo(e21));
            Expect(e11, Is.Not.EqualTo(e21).Using(surface1.UndirectedEdgeComparer));
            Expect(e11, Is.Not.EqualTo(e21).Using(surface2.UndirectedEdgeComparer));
            Expect(e12, Is.Not.EqualTo(e22));
            Expect(e12, Is.Not.EqualTo(e22).Using(surface1.UndirectedEdgeComparer));
            Expect(e12, Is.Not.EqualTo(e22).Using(surface2.UndirectedEdgeComparer));
        }

        [Test]
        public void Edge_is_not_equal_to_an_edge_of_a_type_other_than_VerticalPrismalSurface()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v3 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            var e1 = surface.GetEdge(v1, v2);
            var e2 = surface.GetEdge(v1, v3);
            var e3 = Mock.Of<IPolysurfaceEdge>();

            Expect(e3, Is.Not.EqualTo(e1));
            Expect(e3, Is.Not.EqualTo(e1).Using(surface.UndirectedEdgeComparer));
            Expect(e3, Is.Not.EqualTo(e2));
            Expect(e3, Is.Not.EqualTo(e2).Using(surface.UndirectedEdgeComparer));
        }

        [Test]
        public void Equally_directed_horizontal_edges_are_not_equal_when_attached_to_different_vertices()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v3 = surface.GetVertex(index: 2, baseSide: VerticalPrismalSurface.BaseSide.Top);

            var e1 = surface.GetEdge(v1, v2);
            var e2 = surface.GetEdge(v2, v3);

            Expect(e1, Is.Not.EqualTo(e2));
            Expect(e1, Is.Not.EqualTo(e2).Using(surface.UndirectedEdgeComparer));
        }

        [Test]
        public void Equally_directed_vertical_edges_are_not_equal_when_attached_to_different_vertices()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v2 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v3 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v4 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            var e1 = surface.GetEdge(v1, v2);
            var e2 = surface.GetEdge(v3, v4);

            Expect(e1, Is.Not.EqualTo(e2));
            Expect(e1, Is.Not.EqualTo(e2).Using(surface.UndirectedEdgeComparer));
        }

        [Test]
        public void Horizontal_edge_is_not_equal_to_vertical_edge()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v3 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);

            var e1 = surface.GetEdge(v1, v2);
            var e2 = surface.GetEdge(v3, v2);

            Expect(e1, Is.Not.EqualTo(e2));
            Expect(e1, Is.Not.EqualTo(e2).Using(surface.UndirectedEdgeComparer));
            Expect(e2, Is.Not.EqualTo(e1));
            Expect(e2, Is.Not.EqualTo(e1).Using(surface.UndirectedEdgeComparer));
        }

        private void ExpectOppositeEdgesAreEqualAsUndirected(VerticalPrismalSurface surface, IPolysurfaceVertex v1, IPolysurfaceVertex v2)
        {
            var edge1 = surface.GetEdge(v1, v2);
            var edge2 = surface.GetEdge(v2, v1);

            Expect(edge1, Is.EqualTo(edge2).Using(surface.UndirectedEdgeComparer));
        }

        [Test]
        public void Horizontal_edges_are_equal_as_undirected_when_opposite()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            ExpectOppositeEdgesAreEqualAsUndirected(surface, v1, v2);
        }

        [Test]
        public void Vertical_edges_are_equal_as_undirected_when_opposite()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);

            ExpectOppositeEdgesAreEqualAsUndirected(surface, v1, v2);
        }

        #endregion

        #region Edge.*Face

        [Test]
        public void Interior_vertical_edge_has_correct_LeftFace_and_RightFace([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var v1 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v2 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);

            var f0 = surface.GetVerticalFace(index: 0);
            var f1 = surface.GetVerticalFace(index: 1);

            var e1 = surface.GetEdge(v1, v2);

            Expect(e1.RightFace, Is.EqualTo(f1));
            Expect(e1.LeftFace,  Is.EqualTo(f0));

            var e2 = surface.GetEdge(v2, v1);

            Expect(e2.RightFace, Is.EqualTo(f0));
            Expect(e2.LeftFace,  Is.EqualTo(f1));
        }

        [Test]
        public void First_vertical_edge_has_correct_LeftFace_and_RightFace([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var v1 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v2 = surface.GetVertex(index: 0, baseSide: VerticalPrismalSurface.BaseSide.Top);

            var f1 = surface.GetVerticalFace(index: 0);            

            var e1 = surface.GetEdge(v1, v2);
            var e2 = surface.GetEdge(v2, v1);

            Expect(e1.RightFace, Is.EqualTo(f1));
            Expect(e2.LeftFace, Is.EqualTo(f1));

            if (closed)
            {
                var f2 = surface.GetVerticalFace(index: surface.Order - 1);

                Expect(e1.LeftFace, Is.EqualTo(f2));
                Expect(e2.RightFace, Is.EqualTo(f2));
            }
            else
            {
                Expect(e1.LeftFace, Is.Null);
                Expect(e2.RightFace, Is.Null);
            }
        }

        [Test]
        public void Last_vertical_edge_has_correct_LeftFace_and_RightFace([Values(false, true)] bool closed)
        {
            var surface = CreateSurface(closed);

            var v1 = surface.GetVertex(surface.Base.Vertices.Count - 1, VerticalPrismalSurface.BaseSide.Bottom);
            var v2 = surface.GetVertex(surface.Base.Vertices.Count - 1, VerticalPrismalSurface.BaseSide.Top);

            var f1 = surface.GetVerticalFace(surface.Base.Vertices.Count - 2);

            var e1 = surface.GetEdge(v1, v2);
            var e2 = surface.GetEdge(v2, v1);

            Expect(e1.LeftFace, Is.EqualTo(f1));
            Expect(e2.RightFace, Is.EqualTo(f1));

            if (closed)
            {
                var f2 = surface.GetVerticalFace(surface.Order - 1);

                Expect(e1.RightFace, Is.EqualTo(f2));
                Expect(e2.LeftFace, Is.EqualTo(f2));
            }
            else
            {
                Expect(e1.RightFace, Is.Null);
                Expect(e2.LeftFace, Is.Null);
            }
        }

        [Test, Combinatorial]
        public void Horizontal_edge_has_correct_LeftFace_and_RightFace
            ([Values(false, true)] bool isClosedSurface,
            [Values(false, true)] bool isForwardEdge,
            [Values(VerticalPrismalSurface.BaseSide.Bottom, VerticalPrismalSurface.BaseSide.Top)] VerticalPrismalSurface.BaseSide side)
        {
            var surface = CreateSurface(isClosedBase: true, isClosedSurface: isClosedSurface);

            var v1 = surface.GetVertex(2, side);
            var v2 = surface.GetVertex(3, side);

            var edge = isForwardEdge ? surface.GetEdge(v1, v2) : surface.GetEdge(v2, v1);

            var leftFace = edge.LeftFace;
            var rightFace = edge.RightFace;

            if (isClosedSurface)
            {
                Expect(leftFace, Is.Not.Null);
                Expect(rightFace, Is.Not.Null);
            }
            else
            {
                Expect(leftFace == null ^ rightFace == null);
            }

            if (leftFace != null)
            {
                Expect(leftFace.Edges, Has.Exactly(1).EqualTo(edge));
            }

            if (rightFace != null)
            {
                Expect(rightFace.Edges, Has.Exactly(1).EqualTo(edge.Reversed));
            }
        }

        #endregion

        #endregion

        #region faces

        private void ExpectListsAreEqualUpToCyclicPermuation<T>(IReadOnlyList<T> list1, IReadOnlyList<T> list2, System.Collections.IEqualityComparer equality = null)
        {
            if (equality == null)
            {
                equality = EqualityComparer<T>.Default;
            }

            Expect(list1.Count, Is.EqualTo(list2.Count));

            var n = list1.Count;

            for (int i = 0; i < n; i++)
            {
                System.Collections.IStructuralEquatable permutedList1 = Enumerable.Range(0, n).Select(j => list1[(j + i) % n]).ToArray();

                if (permutedList1.Equals(list2.ToArray(), equality))
                {
                    return;
                }
            }

            Expect(false);
        }

        [Test]
        public void GetVerticalFace_returns_face_with_correct_Edges_directed_counterclockwise_in_counterclockwise_order()
        {
            var surface = CreateSurface();

            var v1 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v2 = surface.GetVertex(index: 2, baseSide: VerticalPrismalSurface.BaseSide.Bottom);
            var v3 = surface.GetVertex(index: 2, baseSide: VerticalPrismalSurface.BaseSide.Top);
            var v4 = surface.GetVertex(index: 1, baseSide: VerticalPrismalSurface.BaseSide.Top);

            var e1 = surface.GetEdge(v1, v2);
            var e2 = surface.GetEdge(v2, v3);
            var e3 = surface.GetEdge(v3, v4);
            var e4 = surface.GetEdge(v4, v1);

            var edges = new List<IPolysurfaceEdge> { e1, e2, e3, e4 };

            var face = surface.GetVerticalFace(1);

            ExpectListsAreEqualUpToCyclicPermuation(face.Edges.ToList(), edges);
        }

        [Test, Combinatorial]
        public void GetHorizontalFace_returns_face_with_correct_Edges_directed_counterclockwise_in_counterclockwise_order
            ([Values(VerticalPrismalSurface.BaseSide.Bottom, VerticalPrismalSurface.BaseSide.Top)] VerticalPrismalSurface.BaseSide side,
            [Values(false, true)] bool isOrientedOutside)
        {
            var surface = CreateSurface(isClosedBase: true, isClosedSurface: true, isCounterclockwise: isOrientedOutside);

            Expect(surface.Order, Is.EqualTo(5)); // internal sanity

            var v1 = surface.GetVertex(index: 0, baseSide: side);
            var v2 = surface.GetVertex(index: 1, baseSide: side);
            var v3 = surface.GetVertex(index: 2, baseSide: side);
            var v4 = surface.GetVertex(index: 3, baseSide: side);
            var v5 = surface.GetVertex(index: 4, baseSide: side);

            var e1 = surface.GetEdge(v1, v2);
            var e2 = surface.GetEdge(v2, v3);
            var e3 = surface.GetEdge(v3, v4);
            var e4 = surface.GetEdge(v4, v5);
            var e5 = surface.GetEdge(v5, v1);

            var edges = new List<IPolysurfaceEdge> { e1, e2, e3, e4, e5 };

            if (side == VerticalPrismalSurface.BaseSide.Bottom)
            {
                edges = edges.Select(e => e.Reversed).Reverse().ToList();
            }

            var face = surface.GetHorizontalFace(side);

            ExpectListsAreEqualUpToCyclicPermuation(face.Edges.ToList(), edges);
        }

        [Test]
        public void Faces_correspond_to_base_edges([Values(false, true)] bool isClosedSurface)
        {
            var surface = CreateSurface(isClosedBase: true, isClosedSurface: isClosedSurface);
            var order = surface.Order;

            var faces = Enumerable.Range(0, order).Select(i => surface.GetVerticalFace(i)).ToList();

            if (isClosedSurface)
            {
                faces.Add(surface.GetHorizontalFace(VerticalPrismalSurface.BaseSide.Bottom));
                faces.Add(surface.GetHorizontalFace(VerticalPrismalSurface.BaseSide.Top));
            }

            Expect(surface.Faces, Is.EquivalentTo(faces));
        }

        public struct FaceOrientationTestCase
        {
            public bool IsVertical { get; set; }
            public bool IsOrientedOutside { get; set; }
            public VerticalPrismalSurface.BaseSide Side { get; set; }

            public override string ToString()
            {
                return string.Format("IsVertical={0}, IsOrientedOutside={1}, Side={2}", IsVertical, IsOrientedOutside, Side);
            }
        }

        IEnumerable<FaceOrientationTestCase> GetFaceOrientationTestCases()
        {
            yield return new FaceOrientationTestCase
            {
                IsVertical = true,
                IsOrientedOutside = true,
                Side = VerticalPrismalSurface.BaseSide.Bottom
            };

            yield return new FaceOrientationTestCase
            {
                IsVertical = false,
                IsOrientedOutside = true,
                Side = VerticalPrismalSurface.BaseSide.Bottom
            };

            yield return new FaceOrientationTestCase
            {
                IsVertical = false,
                IsOrientedOutside = false,
                Side = VerticalPrismalSurface.BaseSide.Bottom
            };

            yield return new FaceOrientationTestCase
            {
                IsVertical = false,
                IsOrientedOutside = true,
                Side = VerticalPrismalSurface.BaseSide.Top
            };

            yield return new FaceOrientationTestCase
            {
                IsVertical = false,
                IsOrientedOutside = false,
                Side = VerticalPrismalSurface.BaseSide.Top
            };
        }

        [Test]
        public void Face_Polygon_has_the_same_vertices_as_the_Face([ValueSource("GetFaceOrientationTestCases")] FaceOrientationTestCase testCase)
        {
            var surface = CreateSurface(isClosedBase: true, isClosedSurface: true, isCounterclockwise: testCase.IsOrientedOutside);

            IPolysurfaceFace face;
            
            if (testCase.IsVertical)
            {
                face = surface.GetVerticalFace(2);
            }
            else
            {
                face = surface.GetHorizontalFace(testCase.Side);
            }

            var actual = new ClosedPolyline3(face.Polygon.Vertices());
            var expected = new ClosedPolyline3(face.Vertices().Select(v => v.Point));

            TestUtils.ExpectClosedPolylinesAreEqual(actual, expected, _tolerance);
        }

        [Test]
        public void Face_Normal_is_consistent_with_edge_order_for_convex_base([ValueSource("GetFaceOrientationTestCases")] FaceOrientationTestCase testCase)
        {
            var baseVertices = new List<Vector2>
            {
                new Vector2(-1, +2),
                new Vector2(+1, +2),
                new Vector2(+2, +1),
                new Vector2(+2, -1),
                new Vector2(+1, -2),
                new Vector2(-1, -2),
                new Vector2(-2, -1),
                new Vector2(-2, +1)
            };

            var line = new ClosedPolyline(baseVertices);
            var @base = testCase.IsOrientedOutside ? line.AsReverse() : line;

            var zRange = new Interval(-3.14, 2.88);

            var surface = new VerticalPrismalSurface(@base, zRange, isClosed: true);

            IPolysurfaceFace face;

            if (testCase.IsVertical)
            {
                face = surface.GetVerticalFace(2);
            }
            else
            {
                face = surface.GetHorizontalFace(testCase.Side);
            }

            var edge1 = face.Edges.ElementAt(0);
            var edge2 = face.Edges.ElementAt(1);

            var expected = edge1.Vector().Cross(edge2.Vector());

            Expect(face.Normal * expected, Is.Positive);
        }

        #endregion
    }
}
