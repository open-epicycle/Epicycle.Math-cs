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
    public sealed class AffineStochasticTransformation : IStochasticMapping
    {
        public AffineStochasticTransformation(OMatrix matrix, OVector freeTerm, OSymmetricMatrix covariance)
        {
            _domain = new AffineSpace(matrix.ColumnCount);
            _codomain = new AffineSpace(matrix.RowCount);

            _matrix = matrix;
            _freeTerm = freeTerm;
            _covariance = covariance;
        }

        private readonly IManifold _domain;
        private readonly IManifold _codomain;

        private readonly OMatrix _matrix;
        private readonly OVector _freeTerm;
        private readonly OSymmetricMatrix _covariance;

        public IManifold Domain
        {
            get { return _domain; }
        }

        public IManifold Codomain
        {
            get { return _codomain; }
        }

        public StochasticManifoldPoint Apply(IManifoldPoint point)
        {
            return new StochasticManifoldPoint(_matrix * (OVector)point + _freeTerm, _covariance);
        }

        public OMatrix ExpectationDifferential(IManifoldPoint point)
        {
            return _matrix;
        }
    }
}
