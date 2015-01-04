using System.Collections.Generic;

namespace Epicycle.Math.Geometry.Polytopes
{
    // polyline, might be open (IOpenPolyline) or closed (IClosedPolyline)
    public interface IPolyline
    {
        bool IsClosed { get; }

        IReadOnlyList<Vector2> Vertices { get; }
    }
}
