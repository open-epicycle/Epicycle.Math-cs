using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Math.Geometry.Differential;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Probability
{
    public static class StochasticProcessUtils
    {
        public static StochasticManifoldPoint Apply(this IStochasticProcess process, StochasticManifoldPoint stochasticPoint, double time)
        {
            var diff = process.ExpectationDifferential(stochasticPoint.Expectation, time);
            var prediction = process.Apply(stochasticPoint.Expectation, time);

            return new StochasticManifoldPoint(prediction.Expectation, stochasticPoint.Covariance.Conjugate(diff) + prediction.Covariance);
        }

        public static IStochasticMapping Apply(this IStochasticProcess process, double time)
        {
            return new StochasticProcessTimeSlice(process, time);
        }

        private sealed class StochasticProcessTimeSlice : IStochasticMapping
        {
            public StochasticProcessTimeSlice(IStochasticProcess process, double time)
            {
                _process = process;
                _time = time;
            }

            private readonly IStochasticProcess _process;
            private readonly double _time;

            public IStochasticProcess Process
            {
                get { return _process; }
            }

            public double Time
            {
                get { return _time; }
            }

            public IManifold Domain
            {
                get { return _process.StateSpace; }
            }

            public IManifold Codomain
            {
                get { return _process.StateSpace; }
            }

            public StochasticManifoldPoint Apply(IManifoldPoint point)
            {
                return _process.Apply(point, _time);
            }

            public OMatrix ExpectationDifferential(IManifoldPoint point)
            {
                return _process.ExpectationDifferential(point, _time);
            }
        }
    }
}
