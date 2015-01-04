using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public interface IStochasticProcess
    {
        IManifold StateSpace { get; }

        // throws if point doesn't belong to StateSpace
        StochasticManifoldPoint Apply(IManifoldPoint point, double time);

        // throws if point doesn't belong to StateSpace
        OSquareMatrix ExpectationDifferential(IManifoldPoint point, double time);
    }
}
