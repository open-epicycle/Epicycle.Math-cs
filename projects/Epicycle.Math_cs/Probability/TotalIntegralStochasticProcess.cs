using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public sealed class TotalIntegralStochasticProcess : IntegralStochasticProcess
    {
        public TotalIntegralStochasticProcess(IStochasticProcess baseProcess, IManifold fiber) : base(baseProcess, fiber)
        {
            if (!(baseProcess.StateSpace is AffineSpace))
            {
                throw new ArgumentException("Expected process in affine space", "baseProcess");
            }
        }

        protected override OVector Velocity(IManifoldPoint basePoint)
        {
            return (OVector)basePoint;
        }

        protected override OMatrix VelocityDifferential(IManifoldPoint basePoint)
        {
            return Matrix.Id(Fiber.Dimension);
        }
    }
}
