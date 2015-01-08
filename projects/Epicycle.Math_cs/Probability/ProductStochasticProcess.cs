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

using Epicycle.Commons.Collections;
using Epicycle.Math.Geometry.Differential;
using Epicycle.Math.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Probability
{
    public sealed class ProductStochasticProcess : IStochasticProcess
    {
        public ProductStochasticProcess(params IStochasticProcess[] factors)
        {
            _factors = factors;
            _stateSpace = new ProductManifold(factors.Select(process => process.StateSpace).ToArray());
        }

        private readonly IReadOnlyList<IStochasticProcess> _factors;
        private readonly ProductManifold _stateSpace;

        IManifold IStochasticProcess.StateSpace
        {
	        get { return _stateSpace; }
        }

        public ProductManifold StateSpace
        {
            get { return _stateSpace; }
        }

        public IReadOnlyList<IStochasticProcess> Factors
        {
            get { return _factors; }
        }

        public static IManifoldPoint GetSubstate(IManifoldPoint state, int index)
        {
            return ((ProductManifold.Point)state).Factors[index];
        }

        public StochasticManifoldPoint Apply(IManifoldPoint point, double time)
        {
            var productPoint = (ProductManifold.Point)point;

            var answerFactors = new List<IManifoldPoint>(_factors.Count);
            var answerCovariance = new SymmetricMatrix(_stateSpace.Dimension);

            for (int i = 0; i < _factors.Count; i++)
            {
                var stochasticFactor = _factors[i].Apply(productPoint.Factors[i], time);

                answerFactors.Add(stochasticFactor.Expectation);
                answerCovariance.SetSubmatrix(_stateSpace.CoordinateIndex(i), stochasticFactor.Covariance);
            }

            return new StochasticManifoldPoint(new ProductManifold.Point(answerFactors.AsReadOnlyList()), answerCovariance);
        }

        public OSquareMatrix ExpectationDifferential(IManifoldPoint point, double time)
        {
            var productPoint = (ProductManifold.Point)point;

            var answer = new SquareMatrix(_stateSpace.Dimension);

            for (int i = 0; i < _factors.Count; i++)
            {
                var index = _stateSpace.CoordinateIndex(i);
                var factorDifferential = _factors[i].ExpectationDifferential(productPoint.Factors[i], time);

                answer.SetSubmatrix(index, index, factorDifferential);
            }

            return answer;
        }
    }
}
