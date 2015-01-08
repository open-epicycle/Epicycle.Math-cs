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

using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.TestUtils.Probability;
using NUnit.Framework;

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

            ProbabilityTestUtils.ValidateProcess(process, _point, time: 7.4, eps: 1e-5, tolerance: 1e-7);
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
