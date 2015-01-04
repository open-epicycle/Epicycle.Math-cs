using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Math.Geometry.Differential;
using Epicycle.Math.LinearAlgebra;

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

            return new StochasticManifoldPoint(new ProductManifold.Point(answerFactors), answerCovariance);
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
