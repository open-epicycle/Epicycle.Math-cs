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

using Epicycle.Commons;
using Epicycle.Math.Geometry.Differential;
using Epicycle.Math.LinearAlgebra;
using System;

namespace Epicycle.Math.Probability
{
    public sealed class AffineIntegralStochasticProcess : IntegralStochasticProcess
    {
        public AffineIntegralStochasticProcess(IStochasticProcess baseProcess, IManifold fiber, OMatrix gain, OVector offset)
            : base(baseProcess, fiber)
        {
            if (!(baseProcess.StateSpace is AffineSpace))
            {
                throw new ArgumentException("Expected process in affine space", "baseProcess");
            }

            ArgAssert.Equal(baseProcess.StateSpace.Dimension, "baseProcess.StateSpace.Dimension", gain.ColumnCount, "gain.ColumnCount");
            ArgAssert.Equal(fiber.Dimension, "fiber.Dimension", gain.RowCount, "gain.RowCount");
            ArgAssert.Equal(offset.Dimension, "offset.Dimension", gain.RowCount, "gain.RowCount");

            _gain = gain;
            _offset = offset;
        }

        private readonly OMatrix _gain;
        private readonly OVector _offset;

        protected override OVector Velocity(IManifoldPoint basePoint)
        {
            return _gain * (OVector)basePoint + _offset;
        }

        protected override OMatrix VelocityDifferential(IManifoldPoint basePoint)
        {
            return _gain;
        }
    }
}
