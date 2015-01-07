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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public sealed class WienerProcess : IStochasticProcess
    {
        public WienerProcess(OSymmetricMatrix diffusionMatrix, OVector? driftVelocity = null)
        {
            _stateSpace = new AffineSpace(diffusionMatrix.Dimension);

            _diffusionMatrix = diffusionMatrix;

            if (driftVelocity == null)
            {
                _driftVelocity = Vector.Zero(diffusionMatrix.Dimension);
            }
            else
            {
                ArgAssert.Equal(driftVelocity.Value.Dimension, "driftVelocity.Value.Dimension", diffusionMatrix.Dimension, "diffusionMatrix.Dimension");

                _driftVelocity = driftVelocity.Value;
            }
        }

        private readonly IManifold _stateSpace;

        private readonly OSymmetricMatrix _diffusionMatrix;
        private readonly OVector _driftVelocity;

        public int Dimension
        {
            get { return _stateSpace.Dimension; }
        }

        public IManifold StateSpace
        {
            get { return _stateSpace; }
        }

        public StochasticManifoldPoint Apply(IManifoldPoint point, double time)
        {
            return new StochasticManifoldPoint(_stateSpace.Translate(point, time * _driftVelocity), time * _diffusionMatrix);
        }

        public OSquareMatrix ExpectationDifferential(IManifoldPoint point, double time)
        {
            return SquareMatrix.Id(Dimension);
        }
    }
}
