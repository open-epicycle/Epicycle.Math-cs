using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry.Differential
{
    public interface IManifold
    {
        int Dimension { get; }

        // translates point along tangent vector
        // throws if point doesn't belong to manifold
        // throws if tangentVector dimension is different from manifold dimension
        IManifoldPoint Translate(IManifoldPoint point, OVector tangentVector);

        // find translation vector from one point to another
        // throws if either point doesn't belong to manifold
        OVector GetTranslation(IManifoldPoint to, IManifoldPoint from);
    }

    public interface IManifoldPoint
    {
        int Dimension { get; }
    }
}
