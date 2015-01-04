using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Moq;

using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    [TestFixture]
    public class KalmanFilterTest : AssertionHelper
    {
        private const double _tolerance = 1e-9;

        private void ExpectVectorsAreAlmostEqual(OVector v, OVector w)
        {
            Expect(v.Dimension, Is.EqualTo(w.Dimension));

            for (int i = 0; i < v.Dimension; i++)
            {
                Expect(v[i], Is.EqualTo(w[i]).Within(_tolerance));
            }
        }

        private void ExpectMatricesAreAlmostEqual(OMatrix v, OMatrix w)
        {
            Expect(v.RowCount, Is.EqualTo(w.RowCount));
            Expect(v.ColumnCount, Is.EqualTo(w.ColumnCount));

            for (int i = 0; i < v.RowCount; i++)
            {
                for (int j = 0; j < v.ColumnCount; j++)
                {
                    Expect(v[i, j], Is.EqualTo(w[i, j]).Within(_tolerance));
                }
            }
        }

        private IStochasticProcess StubProcessModel()
        {
            return Mock.Of<IStochasticProcess>(model => model.StateSpace.Dimension == 3);
        }

        [Test]
        public void Estimate_after_construction_coincides_with_argument_provided_to_constructor()
        {
            var procModel = StubProcessModel();

            OVector initExpectation = new Vector(3.141, 2.718, 0.577);

            var initCovar = new SymmetricMatrix(new double[][]
            {
                new double[] {1},
                new double[] {2, 3},
                new double[] {3, 4, 5}
            });

            var filter = new KalmanFilter(procModel, new StochasticManifoldPoint(initExpectation, initCovar), initialTime: 7);

            ExpectVectorsAreAlmostEqual((OVector)filter.Estimate.Expectation, initExpectation);
            ExpectMatricesAreAlmostEqual(filter.Estimate.Covariance, initCovar);
        }

        private class Time
        {
            public double Value { get; set; }
        }

        private IStochasticProcess MockProcessModel()
        {
            var answer = new Mock<IStochasticProcess>();

            answer.Setup(m => m.StateSpace).Returns(new AffineSpace(dimension: 3));

            answer.Setup(m => m.Apply(It.IsAny<OVector>(), It.IsAny<double>())).Returns
                ((OVector v, double t) => new StochasticManifoldPoint((OVector)new Vector(t * v[0], t + v[1], t - v[2]), new SymmetricMatrix(new double[][]
                {
                    new double[] {v[0]},
                    new double[] {t - v[1], t + v[2]},
                    new double[] {v[2],     v[1],     -t}
                })));

            answer.Setup(m => m.ExpectationDifferential(It.IsAny<OVector>(), It.IsAny<double>())).Returns((OVector v, double t) => new SquareMatrix(new double[,]
                {
                    {v[0] + t, v[1] - t, v[2]},
                    {t,        v[0] - t, v[1]},
                    {3 * t,    2 * t,    v[0]}
                }));

            return answer.Object;
        }

        private IStochasticMapping MockMeasurementModel(Time time)
        {
            var answer = new Mock<IStochasticMapping>();

            answer.Setup(m => m.Domain).Returns(new AffineSpace(dimension: 3));
            answer.Setup(m => m.Codomain).Returns(new AffineSpace(dimension: 3));

            answer.Setup(m => m.Apply(It.IsAny<OVector>())).Returns
                ((OVector v) => new StochasticManifoldPoint((OVector)new Vector(time.Value * v[1], time.Value - v[0], time.Value * v[2]), new SymmetricMatrix(new double[][]
                {
                    new double[] {v[2]},
                    new double[] {time.Value + v[1], time.Value - v[2]},
                    new double[] {v[0],        v[2],                    time.Value}
                })));

            answer.Setup(m => m.ExpectationDifferential(It.IsAny<OVector>())).Returns((OVector v) => new Matrix(new double[,]
                {
                    {v[1] + time.Value, v[1] + time.Value, v[0]},
                    {2 * time.Value,    v[2] - time.Value, v[0]},
                    {3 + time.Value,    2 - time.Value,    v[1]}
                }));

            return answer.Object;
        }

        [Test]
        public void When_measurements_coincide_with_expectation_estimates_coincide_with_prediction()
        {
            var time = new Time { Value = 7 };

            var procModel = MockProcessModel();
            var measModel = MockMeasurementModel(time);

            OVector initE = new Vector(3.141, 2.718, 0.577);

            var initCovar = new SymmetricMatrix(new double[][]
            {
                new double[] {1},
                new double[] {2, 3},
                new double[] {3, 4, 5}
            });

            var predictor = new KalmanFilter(procModel, new StochasticManifoldPoint(initE, initCovar), time.Value);
            var filter = new KalmanFilter(procModel, new StochasticManifoldPoint(initE, initCovar), time.Value);

            for (; time.Value < 15; time.Value += 1.1)
            {
                predictor.Predict(time.Value);
                filter.Update(measModel, measModel.Apply(predictor.Estimate.Expectation).Expectation, time.Value);

                ExpectVectorsAreAlmostEqual((OVector)filter.Estimate.Expectation, (OVector)predictor.Estimate.Expectation);
            }
        }

        [Test]
        public void Update_amounts_to_matrix_weighted_average_when_measurement_covariance_commutes_with_estimate_covariance()
        {
            var procVector = new Vector(3.141, 2.718, 0.577);

            var procCovar = SymmetricMatrix.Scalar(dimension: 3, value: 0.321);

            var procModel = new WienerProcess(procCovar, procVector);

            OMatrix measMatrix = new Matrix(new double[,]
            {
                {-1,   2,    -3},
                {0.4,  -0.5, 6},
                {-0.7, 0.8,  -0.9}
            });            

            var measVector = new Vector(3.141, -2.718, 0.577);

            OSymmetricMatrix measCovar = new SymmetricMatrix(new double[][]
            {
                new double[] {1.0},
                new double[] {0.5, 0.8},
                new double[] {0,   0.4, 0.9}
            });

            var measModel = new AffineStochasticTransformation(measMatrix, measVector, measCovar);

            OVector initEstimate = new Vector(0.314, 0.718, 2.577);

            var initCovar = new SymmetricMatrix(new double[][]
            {
                new double[] {7},
                new double[] {0, 7},
                new double[] {0, 0, 7}
            });

            var time = 7.3;

            var filter = new KalmanFilter(procModel, new StochasticManifoldPoint(initEstimate, initCovar), time);

            var invMeasMatrix = measMatrix.AsSquare().Inv();

            var nativeMeasCovar = measCovar.Conjugate(invMeasMatrix);

            for (; time < 15; time += 0.8)
            {
                OVector measurement = new Vector(0.3 * time, 0.2 * (time + 1), 0.1 * (time - 1));

                var nativeMeas = invMeasMatrix * (measurement - measVector);

                var pred = procModel.Apply(filter.Estimate, time - filter.Time);

                var expectedEstimate = (pred.Covariance + nativeMeasCovar).Inv() * (nativeMeasCovar * (OVector)pred.Expectation + pred.Covariance * nativeMeas);
                var expectedCovariance = (pred.Covariance + nativeMeasCovar).Inv() * pred.Covariance * nativeMeasCovar;

                filter.Update(measModel, measurement, time);

                ExpectVectorsAreAlmostEqual((OVector)filter.Estimate.Expectation, (OVector)expectedEstimate);
                ExpectMatricesAreAlmostEqual(filter.Estimate.Covariance, expectedCovariance);
            }
        }
    }
}
