using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public struct StochasticManifoldPoint
    {
        public StochasticManifoldPoint(IManifoldPoint expectation, OSymmetricMatrix covariance)
        {
            ArgAssert.Equal(expectation.Dimension, "expectation.Dimension", covariance.Dimension, "covariance.Dimension");

            _expectation = expectation;
            _covariance = covariance;
        }

        private readonly IManifoldPoint _expectation;
        private readonly OSymmetricMatrix _covariance;

        public int Dimension
        {
            get { return _expectation.Dimension; }
        }

        public IManifoldPoint Expectation
        {
            get { return _expectation; }
        }

        public OSymmetricMatrix Covariance
        {
            get { return _covariance; }
        }
    }
}
