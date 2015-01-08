// [[[[INFO>
// Copyright 2015 Epicycle (http://epicycle.org, https://github.com/open-epicycle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/open-epicycle/Epicycle.Math-cs
// ]]]]

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
