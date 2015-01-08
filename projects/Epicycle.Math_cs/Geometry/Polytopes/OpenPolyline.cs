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
            get { return _vertices.AsReadOnlyList(); }
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
