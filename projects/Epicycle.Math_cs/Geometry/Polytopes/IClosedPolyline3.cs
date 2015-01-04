using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry.Polytopes
{
    // guarantees at least 3 vertices
    public interface IClosedPolyline3
    {
        IReadOnlyList<Vector3> Vertices { get; }
    }
}
