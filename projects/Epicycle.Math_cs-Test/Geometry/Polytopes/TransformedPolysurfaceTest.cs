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

using Epicycle.Commons.Collections;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    [TestFixture]
    public sealed class TransformedPolysurfaceTest : AssertionHelper
    {
        private int _numIndices = 10;

        private IDictionary<int, IPolysurfaceVertex> _mockVertices = new Dictionary<int, IPolysurfaceVertex>();

        private Vector3 MockVertexPoint(int index)
        {
            return new Vector3(1, 2, 3) + index * new Vector3(4, 5, 6);
        }

        private IPolysurfaceVertex MockVertex(int index)
        {
            if (!_mockVertices.ContainsKey(index))
            {
                var answer = new Mock<IPolysurfaceVertex>();

                _mockVertices.Add(index, answer.Object);

                var edges = new List<IPolysurfaceEdge>
                {
                    MockEdge(index),
                    MockEdge((index + 1) % _numIndices)
                };

                answer.Setup(v => v.Edges).Returns(edges);
                answer.Setup(v => v.Point).Returns(MockVertexPoint(index));
                answer.Setup(v => v.Order).Returns(index);

                answer.Setup(v => v.Equals(answer.Object)).Returns(true);
                answer.Setup(v => v.Equals(It.IsNotIn(answer.Object))).Returns(false);
                answer.Setup(v => v.GetHashCode()).Returns(index);                
            }            

            return _mockVertices[index];
        }

        private IDictionary<int, IPolysurfaceEdge> _mockEdges = new Dictionary<int, IPolysurfaceEdge>();

        private IPolysurfaceEdge MockEdge(int index)
        {
            if (!_mockEdges.ContainsKey(index))
            {
                var answer = new Mock<IPolysurfaceEdge>();

                _mockEdges.Add(index, answer.Object);

                answer.Setup(e => e.Source).Returns(MockVertex(index));
                answer.Setup(e => e.Target).Returns(MockVertex((index + 1) % _numIndices));

                answer.Setup(e => e.LeftFace).Returns(MockFace(index));
                answer.Setup(e => e.RightFace).Returns(MockFace((index + 1) % _numIndices));

                answer.Setup(v => v.Equals(answer.Object)).Returns(true);
                answer.Setup(v => v.Equals(It.IsNotIn(answer.Object))).Returns(false);
                answer.Setup(v => v.GetHashCode()).Returns(index);                
            }

            return _mockEdges[index];
        }

        private IDictionary<int, IPolysurfaceFace> _mockFaces = new Dictionary<int, IPolysurfaceFace>();

        private IPolygon3 MockFacePolygon(int index)
        {
            var lineMock = new Mock<IClosedPolyline>();

            lineMock.Setup(l => l.SignedArea()).Returns(index);
            lineMock.Setup(l => l.Vertices).Returns(new Vector2(0, index).AsSingleton());

            var polygonMock = new Mock<IPolygon>();

            polygonMock.Setup(p => p.Area()).Returns(index);
            polygonMock.Setup(p => p.BoundingBox()).Returns(new Box2(minX: 0, minY: 0, width: index, height: 0));
            polygonMock.Setup(p => p.CountVertices()).Returns(index);
            polygonMock.Setup(p => p.Contours).Returns(lineMock.Object.AsSingleton());
            polygonMock.Setup(p => p.IsEmpty).Returns(true);
            polygonMock.Setup(p => p.IsSimple).Returns(false);

            var polygon3Mock = new Mock<IPolygon3>();

            polygon3Mock.Setup(p => p.CoordinateSystem).Returns(new RotoTranslation3(new Vector3(0, index, 0)));
            polygon3Mock.Setup(p => p.InPlane).Returns(polygonMock.Object);

            return polygon3Mock.Object;
        }

        private Vector3 MockFaceNormal(int index)
        {
            return new Vector3(6, 5, 4) + index * new Vector3(3, 2, 1);
        }

        private IPolysurfaceFace MockFace(int index)
        {
            if (!_mockFaces.ContainsKey(index))
            {
                var answer = new Mock<IPolysurfaceFace>();

                _mockFaces.Add(index, answer.Object);

                var edges = new List<IPolysurfaceEdge>
                {
                    MockEdge(index),
                    MockEdge((index + 1) % _numIndices)
                };                

                answer.Setup(f => f.Edges).Returns(edges);
                answer.Setup(f => f.Normal).Returns(MockFaceNormal(index));
                answer.Setup(f => f.Polygon).Returns(MockFacePolygon(index));
                answer.Setup(f => f.Order).Returns(index);
            }

            return _mockFaces[index];
        }

        private IEqualityComparer<IPolysurfaceEdge> MockUndirectedComparer()
        {
            var answer = new Mock<IEqualityComparer<IPolysurfaceEdge>>();

            answer.Setup(com => com.Equals(MockEdge(0), MockEdge(1))).Returns(true);
            answer.Setup(com => com.Equals(MockEdge(1), MockEdge(2))).Returns(false);

            answer.Setup(com => com.GetHashCode(MockEdge(0))).Returns(7);

            return answer.Object;
        }

        private IPolysurface MockPolysurface()
        {
            var answer = new Mock<IPolysurface>();

            var vertices = new List<IPolysurfaceVertex>
            {
                MockVertex(0),
                MockVertex(1)
            };

            answer.Setup(sur => sur.Vertices).Returns(vertices);

            var edges = new List<IPolysurfaceEdge>
            {
                MockEdge(0),
                MockEdge(1)
            };

            answer.Setup(sur => sur.Edges).Returns(edges);

            var faces = new List<IPolysurfaceFace>
            {
                MockFace(0),
                MockFace(1)
            };

            answer.Setup(sur => sur.Faces).Returns(faces);

            answer.Setup(sur => sur.GetEdge(MockVertex(0), MockVertex(1))).Returns(MockEdge(2));

            answer.Setup(sur => sur.UndirectedEdgeComparer).Returns(MockUndirectedComparer());

            return answer.Object;
        }

        private const double _tolerance = 1e-9;

        [Test]
        public void Traversal_of_TransformedPolysurface_follows_traversal_of_original_IPolysurface()
        {
            var origSurface = MockPolysurface();            

            var transSurface = new TransformedPolysurface(origSurface, RotoTranslation3.Id);

            var vertex = transSurface.Vertices.ElementAt(1).Edges.ElementAt(1).LeftFace.Edges.ElementAt(0).RightFace.Edges.ElementAt(0).Source.Edges.ElementAt(1).Target;

            Expect(vertex.Order, Is.EqualTo(5));
        }

        [Test]
        public void TransformedPolysurface_Vertex_points_result_from_original_IPolysurface_Vertex_points_by_applying_RotoTranslation3()
        {
            var origSurface = MockPolysurface();    
            var transformation = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var transSurface = new TransformedPolysurface(origSurface, transformation);

            var point = transSurface.Vertices.ElementAt(1).Point;

            Expect(Vector3.Distance(point, transformation.Apply(MockVertexPoint(1))), Is.LessThan(_tolerance));
        }

        [Test]
        public void TransformedPolysurface_Face_normals_result_from_original_IPolysurface_Face_normals_by_applying_Rotation3()
        {
            var origSurface = MockPolysurface();
            var transformation = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var transSurface = new TransformedPolysurface(origSurface, transformation);

            var normal = transSurface.Faces.ElementAt(1).Normal;

            Expect(Vector3.Distance(normal, transformation.Rotation * MockFaceNormal(1)), Is.LessThan(_tolerance));
        }

        [Test]
        public void TransformedPolysurface_Face_polygons_result_from_original_IPolysurface_Face_polygons_by_applying_RotoTranslation3()
        {
            var origSurface = MockPolysurface();
            var transformation = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var transSurface = new TransformedPolysurface(origSurface, transformation);

            var polygon = transSurface.Faces.ElementAt(1).Polygon;

            var actualVertex = polygon.Contours().First().Vertices.First();
            var expectedVertex = transformation.Apply(MockFacePolygon(1)).Contours().First().Vertices.First();

            Expect(Vector3.Distance(actualVertex, expectedVertex), Is.LessThan(_tolerance));
        }
    }
}
