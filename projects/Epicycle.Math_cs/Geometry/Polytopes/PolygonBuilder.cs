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

using System.Collections.Generic;

namespace Epicycle.Math.Geometry.Polytopes
{
    public sealed class PolygonBuilder
    {
        public PolygonBuilder()
        {
            _contours = new List<IClosedPolyline>();
        }

        public void AddContour(IEnumerable<Vector2> vertices)
        {
            AddContour(new ClosedPolyline(vertices));
        }

        public void AddContour(IReadOnlyList<Vector2> vertices)
        {
            AddContour(new ClosedPolyline(vertices));
        }

        public void AddContour(IClosedPolyline contour)
        {
            _contours.Add(contour);
        }

        // resets state to empty
        public IPolygon ExtractPolygon()
        {
            var answer = new Polygon(this);

            _contours.Clear();

            return answer;
        }

        internal IReadOnlyCollection<IClosedPolyline> Contours
        {
            get { return _contours; }
        }

        private readonly List<IClosedPolyline> _contours;
    }
}
