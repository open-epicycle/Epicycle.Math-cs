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
using System.Linq;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Ray2Utils
    {
        // returns first intersection point with box boundary or null if there are none
        public static Vector2? MeetingPoint(this Ray2 ray, Box2 box)
        {
            var meetingDistance = double.PositiveInfinity;

            for (Vector2.Axis axis = 0; axis < Vector2.Axis.Count; axis++)
            {
                var otherAxis = 1 - axis;

                for (Interval.EndType end = 0; end < Interval.EndType.Count; end++)
                {
                    var intersectionDistance = (box.Corner(end)[axis] - ray.Origin[axis]) / ray.Direction[axis];

                    if (intersectionDistance > 0)
                    {
                        var intersectionCoord = ray.Origin[otherAxis] + ray.Direction[otherAxis] * intersectionDistance;

                        if (box.Range(otherAxis).Contains(intersectionCoord))
                        {
                            meetingDistance = Math.Min(meetingDistance, intersectionDistance);
                        }
                    }
                }
            }

            if (double.IsPositiveInfinity(meetingDistance))
            {
                return null;
            }

            return ray.PointAt(meetingDistance);
        }

        // returns first intersection point with boundary or null if there are none
        public static Vector2? BoundaryMeetingPoint(this Ray2 ray, IPolygon polygon)
        {
            Vector2? answer = null;
            var distance = double.PositiveInfinity;

            foreach (var contour in polygon.Contours)
            {
                var prevVertex = contour.Vertices.Last();

                foreach (var vertex in contour.Vertices)
                {
                    var displacement = vertex - prevVertex;
                    var coord = (prevVertex - ray.Origin).Cross(displacement) / ray.Direction.Cross(displacement);
                    
                    if (coord > 0)
                    {
                        var intersectionPoint = ray.PointAt(coord);

                        if ((intersectionPoint - vertex) * (intersectionPoint - prevVertex) < 0 && coord < distance)
                        {
                            answer = intersectionPoint;
                            distance = coord;
                        }
                    }                    

                    prevVertex = vertex;
                }
            }           

            return answer;
        }
    }
}
