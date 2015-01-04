using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;
using Epicycle.Math.Geometry.Differential;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Probability
{
    using System;

    public sealed class OrnsteinUhlenbeckProcess : IStochasticProcess
    {
        public OrnsteinUhlenbeckProcess(OSymmetricMatrix diffusionMatrix, double relaxationTime, OVector? attractorPoint = null)
        {
            _diffusionMatrix = diffusionMatrix;
            RelaxationTime = relaxationTime;

            if (attractorPoint != null)
            {
                AttractorPoint = attractorPoint.Value;
            }
            else
            {
                AttractorPoint = Vector.Zero(diffusionMatrix.Dimension);
            }

            _stateSpace = new AffineSpace(diffusionMatrix.Dimension);
        }

        private readonly IManifold _stateSpace;

        private OSymmetricMatrix _diffusionMatrix;
        private OVector _attractorPoint;

        public double RelaxationTime { get; set; }

        public OVector AttractorPoint
        {
            get { return _attractorPoint; }

            set
            {
                ArgAssert.Equal(value.Dimension, "value.Dimension", _diffusionMatrix.Dimension, "_diffusionMatrix.Dimension");

                _attractorPoint = value;
            }
        }

        public OSymmetricMatrix DiffusionMatrix
        {
            get { return _diffusionMatrix; }

            set 
            {
                ArgAssert.Equal(value.Dimension, "value.Dimension", _diffusionMatrix.Dimension, "_diffusionMatrix.Dimension");

                _diffusionMatrix = value;
            }
        }

        public OSymmetricMatrix EquilibriumCovariance
        {
            get { return (RelaxationTime / 2) * DiffusionMatrix; }
        }

        public IManifold StateSpace
        {
            get { return _stateSpace; }
        }

        public StochasticManifoldPoint Apply(IManifoldPoint point, double time)
        {
            var relaxationFactor = Math.Exp(-time / RelaxationTime);

            var expectation = relaxationFactor * (OVector)point + (1 - relaxationFactor) * AttractorPoint;
            var covariance = (RelaxationTime * (1 - relaxationFactor * relaxationFactor) / 2) * DiffusionMatrix;

            return new StochasticManifoldPoint(expectation, covariance);
        }

        public OSquareMatrix ExpectationDifferential(IManifoldPoint point, double time)
        {
            return SquareMatrix.Scalar(StateSpace.Dimension, Math.Exp(-time / RelaxationTime));
        }

        public static OrnsteinUhlenbeckProcess GivenByCovariance(OSymmetricMatrix covariance, double relaxationTime, OVector? attractorPoint = null)
        {
            var diffusionMatrix = (2 / relaxationTime) * covariance;

            return new OrnsteinUhlenbeckProcess(diffusionMatrix, relaxationTime, attractorPoint);
        }
    }
}
