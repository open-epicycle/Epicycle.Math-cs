using System.Collections.Generic;
using System.Linq;

using Epicycle.Math.Geometry.Polytopes;

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
