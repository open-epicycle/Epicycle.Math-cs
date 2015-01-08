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

using Epicycle.Math.Geometry.Polytopes;
using System.Collections.Generic;

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
