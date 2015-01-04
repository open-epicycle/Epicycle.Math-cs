using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Probability
{
    public struct StochasticVector
    {
        public StochasticVector(OVector expectation, OSymmetricMatrix covariance)
        {
            ArgAssert.Equal(expectation.Dimension, "expectation.Dimension", covariance.Dimension, "covariance.Dimension");

            _expectation = expectation;
            _covariance = covariance;
        }

        private readonly OVector _expectation;
        private readonly OSymmetricMatrix _covariance;

        public int Dimension
        {
            get { return _expectation.Dimension; }
        }

        public OVector Expectation
        {
            get { return _expectation; }
        }

        public OSymmetricMatrix Covariance
        {
            get { return _covariance; }
        }
    }
}
