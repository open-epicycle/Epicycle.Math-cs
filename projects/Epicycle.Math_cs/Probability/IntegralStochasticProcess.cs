using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Math.Geometry.Differential;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Probability
{
    // the fiber process is the integral of a property of the base process computed using the trapezoid rule
    public abstract class IntegralStochasticProcess : ExtensionStochasticProcess
    {
        public IntegralStochasticProcess(IStochasticProcess baseProcess, IManifold fiber) : base(baseProcess, fiber) { }

        protected abstract OVector Velocity(IManifoldPoint basePoint);
        protected abstract OMatrix VelocityDifferential(IManifoldPoint basePoint);

        protected sealed override StochasticFiberPoint FiberEvolution(State state, StochasticManifoldPoint basePoint2, double time)
        {
            var expectation = Fiber.Translate(state.FiberPoint, (time / 2) * (Velocity(state.BasePoint) + Velocity(basePoint2.Expectation)));

            var differential = (time / 2) * VelocityDifferential(basePoint2.Expectation);

            var mixedCovariance = differential * basePoint2.Covariance;

            var covariance = (mixedCovariance * differential.Transposed()).AsSquare().AsSymmetric();

            return new StochasticFiberPoint(new StochasticManifoldPoint(expectation, covariance), mixedCovariance);
        }

        protected sealed override OSquareMatrix FiberDifferential(State state, IManifoldPoint basePoint2, double time)
        {
            return SquareMatrix.Id(Fiber.Dimension);
        }

        protected sealed override OMatrix MixedDifferential(State state, IManifoldPoint basePoint2, OSquareMatrix baseDifferential, double time)
        {
            return (time / 2) * (VelocityDifferential(state.BasePoint) + VelocityDifferential(basePoint2) * baseDifferential);
        }
    }
}
