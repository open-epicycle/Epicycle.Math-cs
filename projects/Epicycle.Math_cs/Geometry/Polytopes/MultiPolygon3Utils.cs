using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    public static class MultiPolygon3Utils
    {
        public static IMultiPolygon3 Union(IMultiPolygon3 p, IMultiPolygon3 q)
        {
            var components = p.Components.Concat(q.Components);

            return new MultiPolygon3(components);
        }
    }
}
