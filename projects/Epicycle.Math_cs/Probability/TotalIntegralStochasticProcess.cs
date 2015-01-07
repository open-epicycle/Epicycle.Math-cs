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
using System;

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
