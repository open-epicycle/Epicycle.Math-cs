﻿// [[[[INFO>
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
    public struct StochasticManifoldPoint
    {
        public StochasticManifoldPoint(IManifoldPoint expectation, OSymmetricMatrix covariance)
        {
            ArgAssert.Equal(expectation.Dimension, "expectation.Dimension", covariance.Dimension, "covariance.Dimension");

            _expectation = expectation;
            _covariance = covariance;
        }

        private readonly IManifoldPoint _expectation;
        private readonly OSymmetricMatrix _covariance;

        public int Dimension
        {
            get { return _expectation.Dimension; }
        }

        public IManifoldPoint Expectation
        {
            get { return _expectation; }
        }

        public OSymmetricMatrix Covariance
        {
            get { return _covariance; }
        }
    }
}
