using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using Moq;

using Epicycle.Math.Geometry;
using Epicycle.Math.Geometry.Differential;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Probability
{
    [TestFixture]
    public sealed class IntegralStochasticProcessTest : AssertionHelper
    {
        private static readonly OVector _offset = new Vector(3.141, 2.718);

        private static readonly OMatrix _gain = new Matrix(new double[,] 
        {
            {1, 2, 3},
            {2, 1, 0}
        });

        private static readonly double _time = 7.4;
        private static readonly OVector _basePoint = new Vector(0.4, 0.6, 0.3);
        private static readonly OVector _fiberPoint = new Vector(-1.3, 0.8);

        [SetUp]
        public void SetUp()
        {
            _point = ExtensionStochasticProcess.CreateState(_basePoint, _fiberPoint);
        }

        private IManifoldPoint _point;

        private const double _tolerance = 1e-9;

        private ExtensionStochasticProcess CreateIntegralProcess(IStochasticProcess baseProcess)
        {
            return new AffineIntegralStochasticProcess(baseProcess, new AffineSpace(_gain.RowCount), _gain, _offset);
        }
        
        private OVector ExpectedFiberResult(OVector baseResult)
        {
            var velocity1 = _gain * _basePoint + _offset;
            var velocity2 = _gain * baseResult + _offset;

            return _fiberPoint + (_time / 2) * (velocity1 + velocity2);
        }

        [Test]
        public void Common_IStochasticProcess_validation()
        {
            var baseProcess = new RotatingWienerProcess(diffusionCoefficient: 0.577, angularVelocity: new Vector3(-0.8, 0.4, 1.3));
            var integralProcess = CreateIntegralProcess(baseProcess);

            TestUtils.ValidateProcess(integralProcess, _point, _time, eps: 1e-5, tolerance: 1e-7);
        }

        [Test]
        public void Apply_Expectation_has_base_point_as_in_base_process_and_fiber_point_as_original_fiber_point_plus_time_times_average_velocity()
        {
            var baseProcess = new RotatingWienerProcess(diffusionCoefficient: 0.577, angularVelocity: new Vector3(-0.8, 0.4, 1.3));
            var integralProcess = CreateIntegralProcess(baseProcess);

            var result = integralProcess.Apply(_point, _time).Expectation;

            var baseResult = (OVector)ExtensionStochasticProcess.GetBaseState(result);
            var fiberResult = (OVector)ExtensionStochasticProcess.GetFiberState(result);

            var expectedBaseResult = (OVector)baseProcess.Apply(_basePoint, _time).Expectation;
            Expect((baseResult - expectedBaseResult).Norm(), Is.LessThan(_tolerance));

            var exepectedFiberResult = ExpectedFiberResult(baseResult);
            Expect((fiberResult - exepectedFiberResult).Norm(), Is.LessThan(_tolerance));
        }

        [Test]
        public void Apply_Covariance_results_from_base_covariance_by_conjugating_with_numeric_differential()
        {
            const double eps = 1e-5;
            const double tolerance = 1e-7;

            var diffusionMatrix = new SymmetricMatrix(new double[][]
            {
                new double [] {+0.3,},
                new double [] {-0.1, +0.4,},
                new double [] {+0.1, +0.2, +0.4}
            });

            var baseProcess = new WienerProcess(diffusionMatrix);
            var integralProcess = CreateIntegralProcess(baseProcess);

            var evolution = integralProcess.Apply(_point, _time);

            var baseResult = (OVector)ExtensionStochasticProcess.GetBaseState(evolution.Expectation);

            var baseDim = baseProcess.StateSpace.Dimension;

            var differential = new Matrix(integralProcess.StateSpace.Dimension, baseDim);

            differential.SetSubmatrix(integralProcess.BaseCoordinateIndex(), 0, Matrix.Id(baseDim));

            for (int j = 0; j < baseDim; j++)            
            {
                var delta = eps * Vector.Basis(j, baseDim);

                var retarted = baseResult - delta;
                var advanced = baseResult + delta;

                var gradient = (ExpectedFiberResult(advanced) - ExpectedFiberResult(retarted)) / (2 * eps);

                differential.SetSubmatrix(integralProcess.FiberCoordinateIndex(), j, gradient.AsColumn());
            }

            var expectedCovariance = baseProcess.Apply(_basePoint, _time).Covariance.Conjugate(differential);
            Expect((evolution.Covariance - expectedCovariance).FrobeniusNorm(), Is.LessThan(tolerance));
        }        
    }
}
