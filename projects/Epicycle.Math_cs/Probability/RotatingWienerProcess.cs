using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public sealed class RotatingWienerProcess : IStochasticProcess
    {
        public RotatingWienerProcess(double diffusionCoefficient, Vector3 angularVelocity)
        {
            _diffusionCoefficient = diffusionCoefficient;
            _angularVelocity = angularVelocity;

            _stateSpace = new AffineSpace(3);
        }

        private readonly double _diffusionCoefficient;
        private readonly Vector3 _angularVelocity;

        private readonly IManifold _stateSpace;

        public IManifold StateSpace
        {
            get { return _stateSpace; }
        }

        public StochasticManifoldPoint Apply(IManifoldPoint point, double time)
        {
            var expectation = new Rotation3(time * _angularVelocity).Apply((Vector3)(OVector)point);
            var covariance = SymmetricMatrix.Scalar(3, time * _diffusionCoefficient);

            return new StochasticManifoldPoint((OVector)expectation, covariance);
        }

        public OSquareMatrix ExpectationDifferential(IManifoldPoint point, double time)
        {
            return new Rotation3(time * _angularVelocity).Matrix();
        }
    }
}
