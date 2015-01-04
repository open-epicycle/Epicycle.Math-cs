using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public interface IStochasticMapping
    {
        IManifold Domain { get; }
        IManifold Codomain { get; }

        // throws if point doesn't belong to Domain
        StochasticManifoldPoint Apply(IManifoldPoint point);

        // throws if point doesn't belong to Domain
        OMatrix ExpectationDifferential(IManifoldPoint point);
    }
}
