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
    // a finite area in the plane bounded by multiple polygonal contours
    // there is no enforcement of guarantees against self-intersection and wrong contour orientation
    public interface IPolygon
    {
        IReadOnlyCollection<IClosedPolyline> Contours { get; }

        bool IsEmpty { get; }

        bool IsSimple { get; }

        double Area();

        Box2 BoundingBox();

        int CountVertices();

        bool Contains(Vector2 point);

        double Distance2(Vector2 point);
    }
}
