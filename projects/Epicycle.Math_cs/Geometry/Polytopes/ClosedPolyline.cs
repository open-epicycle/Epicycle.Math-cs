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
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    // closed polygonal line; immutable
    public sealed class ClosedPolyline : IClosedPolyline
    {
        public ClosedPolyline(IEnumerable<Vector2> vertices)
        {
            _vertices = vertices.ToList().AsReadOnlyList();
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
