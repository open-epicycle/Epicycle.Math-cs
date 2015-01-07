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

using Epicycle.Commons;
using Epicycle.Commons.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    using System;

    public static class PolylineUtils
    {
        public static IReadOnlyList<Segment2> Edges(this IPolyline @this)
        {
            return new PolylineEdgeCollection(@this);
        }

        private sealed class PolylineEdgeCollection : IReadOnlyList<Segment2>
        {
            public PolylineEdgeCollection(IPolyline parent)
            {
                _parent = parent;
            }

            private readonly IPolyline _parent;

            public int Count
            {
                get { return _parent.IsClosed ? _parent.Vertices.Count : _parent.Vertices.Count - 1; }
            }

            public IEnumerator<Segment2> GetEnumerator()
            {
                var count = this.Count;

                for (int i = 0; i < count; i++)
                {
                    yield return this[i];
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public Segment2 this[int index]
            {
                get 
                {
                    var vertices = _parent.Vertices;

                    if (index == vertices.Count && _parent.IsClosed)
                    {
                        return new Segment2(vertices.Last(), vertices.First());
                    }
                    else
                    {
                        return new Segment2(vertices[index], vertices[index + 1]);
                    }                    
                }
            }
        }

        public static Vector2 Displacement(this IPolyline @this, int index)
        {
            var vertices = @this.Vertices;

            if (@this.IsClosed && index == @this.Vertices.Count - 1)
            {
                return vertices.First() - vertices.Last();
            }
            else
            {                
                return vertices[index + 1] - vertices[index];
            }
        }

        public static Vector2 Displacement(this IClosedPolyline @this, int index)
        {
            var vertices = @this.Vertices;
            return vertices.ElementAtCyclic(index + 1) - vertices.ElementAtCyclic(index);
        }

        public static Vector2 Normal(this IPolyline @this, int index)
        {
            return Displacement(@this, index).Rotate270().Normalized;
        }

        public static Vector2 Normal(this IClosedPolyline @this, int index)
        {
            return Displacement(@this, index).Rotate270().Normalized;
        }

        public static Vector2 GetVertex(this IClosedPolyline @this, int index)
        {
            return @this.Vertices.ElementAtCyclic(index);
        }

        public static Segment2 GetEdge(this IClosedPolyline @this, int index)
        {
            var vertices = @this.Vertices;
            return new Segment2(vertices.ElementAtCyclic(index), vertices.ElementAtCyclic(index + 1));
        }

        public static IClosedPolyline AsReverse(this IClosedPolyline @this)
        {
            return new ClosedPolyline(@this.Vertices.AsReverse());
        }

        public static IOpenPolylineBuilder AsReverse(this IOpenPolyline @this)
        {
            return new OpenPolyline(@this.Vertices.AsReverse());
        }

        public static IClosedPolyline3 AsReverse(this IClosedPolyline3 @this)
        {
            return new ClosedPolyline3(@this.Vertices.AsReverse());
        }

        public static IClosedPolyline Close(this IOpenPolyline @this)
        {
            return new ClosedPolyline(@this.Vertices);
        }

        public static IOpenPolylineBuilder Join(IOpenPolyline line1, IOpenPolyline line2)
        {
            return new OpenPolyline(line1.Vertices.Concat(line2.Vertices));
        }

        public static IOpenPolylineBuilder ToBoxCoords(this IOpenPolyline @this, Box2 box)
        {
            return new OpenPolyline(@this.Vertices.Select(v => v.ToBoxCoords(box)));
        }

        public static IOpenPolylineBuilder FromBoxCoords(this IOpenPolyline @this, Box2 box)
        {
            return new OpenPolyline(@this.Vertices.Select(v => v.FromBoxCoords(box)));
        }

        public static IClosedPolyline ToBoxCoords(this IClosedPolyline @this, Box2 box)
        {
            return new ClosedPolyline(@this.Vertices.Select(v => v.ToBoxCoords(box)));
        }

        public static IClosedPolyline FromBoxCoords(this IClosedPolyline @this, Box2 box)
        {
            return new ClosedPolyline(@this.Vertices.Select(v => v.FromBoxCoords(box)));
        }

        public static int WindingNumber(this IClosedPolyline @this, Vector2 point)
        {
            var answer = 0;

            var prevVector = @this.Vertices.Last() - point;

            foreach (var vertex in @this.Vertices)
            {
                var vector = vertex - point;

                if (vector.Y * prevVector.Y < 0 && // edge crosses X axis
                    vector.Y * prevVector.Cross(vector) > 0) // edge crosses positive half of X axis
                {
                    if (vector.Y > 0)
                    {
                        answer++; // counterclockwise crossing
                    }
                    else
                    {
                        answer--; // clockwise crossing
                    }
                }

                prevVector = vector;
            }

            return answer;
        }

        public static double Distance2(Vector2 point, IClosedPolyline line)
        {
            var prevVector = line.Vertices.Last() - point;
            var prevNorm2 = prevVector.Norm2;

            var answer = prevNorm2;

            foreach (var vertex in line.Vertices)
            {
                var vector = vertex - point;
                var norm2 = vector.Norm2;
                var displacement = vector - prevVector;

                if ((prevVector * displacement) * (vector * displacement) > 0)
                {
                    answer = Math.Min(answer, norm2);
                }
                else
                {
                    answer = Math.Min(answer, BasicMath.Sqr(prevVector.Cross(vector)) / displacement.Norm2);
                }                

                prevVector = vector;
                prevNorm2 = norm2;
            }

            return answer;
        }
 
        public static void FindClosestPoint(this IClosedPolyline @this, Vector2 point, out Vector2 closestPoint, out double distance2)
        {
            var prevVector = @this.Vertices.Last() - point;
            var prevNorm2 = prevVector.Norm2;

            closestPoint = @this.Vertices.Last();
            distance2 = prevNorm2;

            foreach (var vertex in @this.Vertices)
            {
                var vector = vertex - point;
                var norm2 = vector.Norm2;
                var displacement = vector - prevVector;
                var vectorDotDisplacement = vector * displacement;

                if ((prevVector * displacement) * vectorDotDisplacement > 0)
                {
                    if (norm2 < distance2)
                    {
                        closestPoint = vertex;
                        distance2 = norm2;
                    }
                }
                else
                {
                    var displacementNorm2 = displacement.Norm2;
                    var perpendicularLength2 = BasicMath.Sqr(prevVector.Cross(vector)) / displacementNorm2;

                    if (perpendicularLength2 < distance2)
                    {
                        closestPoint = vertex - vectorDotDisplacement * displacement / displacementNorm2;
                        distance2 = perpendicularLength2;
                    }
                }

                prevVector = vector;
                prevNorm2 = norm2;
            }
        }

        public static double DistanceTo(this Vector2 @this, IClosedPolyline line)
        {
            return Math.Sqrt(Distance2(@this, line));
        }

        public static IClosedPolyline TranslateBy(this IClosedPolyline @this, Vector2 translation)
        {
            return new ClosedPolyline(@this.Vertices.Select(v => v + translation));
        }
    }
}
