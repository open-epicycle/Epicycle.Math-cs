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

namespace Epicycle.Math.Probability
{
    public sealed class KalmanFilter
    {
        public KalmanFilter(IStochasticProcess processModel, StochasticManifoldPoint initialEstimate, double initialTime)
        {
            ArgAssert.NotNull(processModel, "processModel");
            ArgAssert.Equal(processModel.StateSpace.Dimension, "processModel.StateSpace.Dimension", initialEstimate.Dimension, "initialEstimate.Dimension");

            _time = initialTime;
            _estimate = initialEstimate;

            _processModel = processModel;
        }

        private double _time;
        private StochasticManifoldPoint _estimate;

        private readonly IStochasticProcess _processModel;

        public void Predict(double time)
        {
            _estimate = _processModel.Apply(_estimate, time - _time);
            _time = time;
        }

        // predict + update
        public void Update(IStochasticMapping measurementModel, IManifoldPoint measurement, double time)
        {
            ArgAssert.NotNull(measurementModel, "measurementModel");
            ArgAssert.Equal(measurementModel.Domain.Dimension, "measurementModel.Domain.Dimension", _estimate.Dimension, "_estimate.Dimension");
            ArgAssert.Equal(measurementModel.Codomain.Dimension, "measurementModel.Codomain.Dimension", measurement.Dimension, "measurement.Dimension");

            Predict(time);

            var measDiff = measurementModel.ExpectationDifferential(_estimate.Expectation);
            var expectedMeasurement = measurementModel.Apply(_estimate.Expectation);

            var gain = _estimate.Covariance * measDiff.Transposed() * (_estimate.Covariance.Conjugate(measDiff) + expectedMeasurement.Covariance).Inv();

            var error = measurementModel.Codomain.GetTranslation(measurement, expectedMeasurement.Expectation);
            var updatedExpectation = _processModel.StateSpace.Translate(_estimate.Expectation, gain * error);

            var kh = (gain * measDiff).AsSquare();
            var updatedCovariance = ((kh.Id() - kh) * _estimate.Covariance).AsSymmetric();

            _estimate = new StochasticManifoldPoint(updatedExpectation, updatedCovariance);
        }

        public double Time
        {
            get { return _time; }
        }

        public StochasticManifoldPoint Estimate
        {
            get { return _estimate; }
        }
    }
}
