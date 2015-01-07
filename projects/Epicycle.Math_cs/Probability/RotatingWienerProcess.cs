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

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public sealed class RotatingWienerProcess : IStochasticProcess
    {
        public RotatingWienerProcess(double diffusionCoefficient, Vector3 angularVelocity)
        {
            _diffusionCoefficient = diffusionCoefficient;
            _angularVelocity = angularVelocity;

            _stateSpace = new AffineSpace(3);
        }

        private readonly double _diffusionCoefficient;
        private readonly Vector3 _angularVelocity;

        private readonly IManifold _stateSpace;

        public IManifold StateSpace
        {
            get { return _stateSpace; }
        }

        public StochasticManifoldPoint Apply(IManifoldPoint point, double time)
        {
            var expectation = new Rotation3(time * _angularVelocity).Apply((Vector3)(OVector)point);
            var covariance = SymmetricMatrix.Scalar(3, time * _diffusionCoefficient);

            return new StochasticManifoldPoint((OVector)expectation, covariance);
        }

        public OSquareMatrix ExpectationDifferential(IManifoldPoint point, double time)
        {
            return new Rotation3(time * _angularVelocity).Matrix();
        }
    }
}
