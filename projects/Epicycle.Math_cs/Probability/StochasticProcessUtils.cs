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
