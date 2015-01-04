using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    // a volume of space bounded by a (possibly disconnected) polyhedral surface
    public interface IPolyhedron : IPolysurface
    {
        bool Contains(Vector3 point);

        double Distance2(Vector3 point);
    }
}
