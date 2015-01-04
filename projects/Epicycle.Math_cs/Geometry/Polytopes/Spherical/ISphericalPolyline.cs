using System.Collections.Generic;

namespace Epicycle.Math.Geometry.Polytopes.Spherical
{
    public interface ISphericalPolyline
    {
        IReadOnlyList<UnitVector3> Vertices { get; }
    }
}