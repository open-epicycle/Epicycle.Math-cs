using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry.Polytopes
{
    // oriented planar polygon in 3D
    // doesn't have to be convex, simply connected or connected
    public interface IPolygon3
    {
        RotoTranslation3 CoordinateSystem { get; }

        IPolygon InPlane { get; }

        bool IsEmpty { get; }
    }
}
