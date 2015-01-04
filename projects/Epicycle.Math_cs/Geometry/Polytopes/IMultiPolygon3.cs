using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    // a polyhedral surface each face of which is a connected component
    public interface IMultiPolygon3 : IPolysurface
    {
        IReadOnlyCollection<IPolygon3> Components { get; }

        bool IsEmpty { get; }
    }
}
