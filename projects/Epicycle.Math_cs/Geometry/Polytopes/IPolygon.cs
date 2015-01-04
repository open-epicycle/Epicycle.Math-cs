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
