using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Probability
{
    [TestFixture]
    public class OrnsteinUhlenbeckProcessTest : AssertionHelper
    {
        private readonly OSymmetricMatrix _diffusionMatrix = new SymmetricMatrix(new double[][]
        {
            new double [] {+0.3,},
            new double [] {-0.1, +0.4,},
            new double [] {+0.1, +0.2, +0.4}
        });

        private const double _relaxationTime = 3.141;

        private readonly OVector _attractorPoint = new Vector(0.577, 2.718, -0.123);

        private readonly OVector _point = new Vector(1.234, 0.777, 0.256);

        private const double _tolerance = 1e-9;

        [Test]
        public void Common_IStochasticProcess_validation()
        {
            var process = new OrnsteinUhlenbeckProcess(_diffusionMatrix, _relaxationTime, _attractorPoint);

            TestUtils.ValidateProcess(process, _point, time: 7.4, eps: 1e-5, tolerance: 1e-7);
        }

        [Test]
        public void Apply_Expectation_matches_AttractorPoint_for_large_times()
        {
            var tolerance = 1e-7;
            var time = 1e50;

            var process = new OrnsteinUhlenbeckProcess(_diffusionMatrix, _relaxationTime, _attractorPoint);

            Expect(((OVector)process.Apply(_point, time).Expectation - _attractorPoint).Norm(), Is.LessThan(tolerance));
        }

        [Test]
        public void Apply_Covariance_matches_EquilibriumCovariance_for_large_times()
        {
            var tolerance = 1e-7;
            var time = 1e50;

            var process = new OrnsteinUhlenbeckProcess(_diffusionMatrix, _relaxationTime, _attractorPoint);

            Expect((process.Apply(_point, time).Covariance - process.EquilibriumCovariance).FrobeniusNorm(), Is.LessThan(tolerance));
        }

        [Test]
        public void EquilibriumCovariance_of_OrnsteinUhlenbeckProcess_GivenByCovariance_matches_provided_covariance()
        {
            var covariance = new SymmetricMatrix(new double[][]
            {
                new double [] {+0.3,},
                new double [] {-0.1, +0.4,},
                new double [] {+0.1, +0.2, +0.4}
            });

            var process = OrnsteinUhlenbeckProcess.GivenByCovariance(covariance, _relaxationTime, _attractorPoint);

            Expect((process.EquilibriumCovariance - covariance).FrobeniusNorm(), Is.LessThan(_tolerance));
        }
    }
}
