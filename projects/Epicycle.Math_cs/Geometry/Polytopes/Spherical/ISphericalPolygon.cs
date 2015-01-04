using System.Collections.Generic;

namespace Epicycle.Math.Geometry.Polytopes.Spherical
{
    public interface ISphericalPolygon
    {
        IReadOnlyCollection<IClosedSphericalPolyline> Contours { get; }

        bool IsEmpty { get; }

        bool IsSimple { get; }

        int CountVertices();
    }
}