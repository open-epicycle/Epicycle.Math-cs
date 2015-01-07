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

namespace Epicycle.Math.Probability
{
    using System;

    public sealed class OrnsteinUhlenbeckProcess : IStochasticProcess
    {
        public OrnsteinUhlenbeckProcess(OSymmetricMatrix diffusionMatrix, double relaxationTime, OVector? attractorPoint = null)
        {
            _diffusionMatrix = diffusionMatrix;
            RelaxationTime = relaxationTime;

            if (attractorPoint != null)
            {
                AttractorPoint = attractorPoint.Value;
            }
            else
            {
                AttractorPoint = Vector.Zero(diffusionMatrix.Dimension);
            }

            _stateSpace = new AffineSpace(diffusionMatrix.Dimension);
        }

        private readonly IManifold _stateSpace;

        private OSymmetricMatrix _diffusionMatrix;
        private OVector _attractorPoint;

        public double RelaxationTime { get; set; }

        public OVector AttractorPoint
        {
            get { return _attractorPoint; }

            set
            {
                ArgAssert.Equal(value.Dimension, "value.Dimension", _diffusionMatrix.Dimension, "_diffusionMatrix.Dimension");

                _attractorPoint = value;
            }
        }

        public OSymmetricMatrix DiffusionMatrix
        {
            get { return _diffusionMatrix; }

            set 
            {
                ArgAssert.Equal(value.Dimension, "value.Dimension", _diffusionMatrix.Dimension, "_diffusionMatrix.Dimension");

                _diffusionMatrix = value;
            }
        }

        public OSymmetricMatrix EquilibriumCovariance
        {
            get { return (RelaxationTime / 2) * DiffusionMatrix; }
        }

        public IManifold StateSpace
        {
            get { return _stateSpace; }
        }

        public StochasticManifoldPoint Apply(IManifoldPoint point, double time)
        {
            var relaxationFactor = Math.Exp(-time / RelaxationTime);

            var expectation = relaxationFactor * (OVector)point + (1 - relaxationFactor) * AttractorPoint;
            var covariance = (RelaxationTime * (1 - relaxationFactor * relaxationFactor) / 2) * DiffusionMatrix;

            return new StochasticManifoldPoint(expectation, covariance);
        }

        public OSquareMatrix ExpectationDifferential(IManifoldPoint point, double time)
        {
            return SquareMatrix.Scalar(StateSpace.Dimension, Math.Exp(-time / RelaxationTime));
        }

        public static OrnsteinUhlenbeckProcess GivenByCovariance(OSymmetricMatrix covariance, double relaxationTime, OVector? attractorPoint = null)
        {
            var diffusionMatrix = (2 / relaxationTime) * covariance;

            return new OrnsteinUhlenbeckProcess(diffusionMatrix, relaxationTime, attractorPoint);
        }
    }
}
