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
using NUnit.Framework;

namespace Epicycle.Math.Probability
{
    public static class TestUtils
    {
        public static void ValidateMappingDifferential(IStochasticMapping mapping, IManifoldPoint point, double eps, double tolerance)
        {
            var dim = mapping.Domain.Dimension;
            var codim = mapping.Codomain.Dimension;

            var expectedDifferential = new Matrix(codim, dim);

            for (int i = 0; i < dim; i++)
            {
                var advancedPoint = mapping.Domain.Translate(point, +eps * Vector.Basis(i, dim));
                var retardedPoint = mapping.Domain.Translate(point, -eps * Vector.Basis(i, dim));

                var advancedImage = mapping.Apply(advancedPoint).Expectation;
                var retardedImage = mapping.Apply(retardedPoint).Expectation;

                var gradient = mapping.Codomain.GetTranslation(advancedImage, retardedImage) / (2 * eps);

                expectedDifferential.SetColumn(i, gradient);
            }

            var actualDifferential = mapping.ExpectationDifferential(point);

            Assert.That((expectedDifferential - actualDifferential).FrobeniusNorm(), Is.LessThan(tolerance));
        }

        private const double _tolerance = 1e-9;

        public static void ValidateProcess(IStochasticProcess process, IManifoldPoint point, double time, double eps, double tolerance)
        {
            var trivialEvolution = process.Apply(point, time: 0);

            Assert.That(process.StateSpace.GetTranslation(trivialEvolution.Expectation, point).Norm(), Is.LessThan(_tolerance));
            Assert.That(trivialEvolution.Covariance.FrobeniusNorm(), Is.LessThan(_tolerance));

            ValidateMappingDifferential(process.Apply(time), point, eps, tolerance);
        }
    }
}
