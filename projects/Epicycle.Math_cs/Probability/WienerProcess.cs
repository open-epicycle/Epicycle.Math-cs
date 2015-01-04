using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public sealed class WienerProcess : IStochasticProcess
    {
        public WienerProcess(OSymmetricMatrix diffusionMatrix, OVector? driftVelocity = null)
        {
            _stateSpace = new AffineSpace(diffusionMatrix.Dimension);

            _diffusionMatrix = diffusionMatrix;

            if (driftVelocity == null)
            {
                _driftVelocity = Vector.Zero(diffusionMatrix.Dimension);
            }
            else
            {
                ArgAssert.Equal(driftVelocity.Value.Dimension, "driftVelocity.Value.Dimension", diffusionMatrix.Dimension, "diffusionMatrix.Dimension");

                _driftVelocity = driftVelocity.Value;
            }
        }

        private readonly IManifold _stateSpace;

        private readonly OSymmetricMatrix _diffusionMatrix;
        private readonly OVector _driftVelocity;

        public int Dimension
        {
            get { return _stateSpace.Dimension; }
        }

        public IManifold StateSpace
        {
            get { return _stateSpace; }
        }

        public StochasticManifoldPoint Apply(IManifoldPoint point, double time)
        {
            return new StochasticManifoldPoint(_stateSpace.Translate(point, time * _driftVelocity), time * _diffusionMatrix);
        }

        public OSquareMatrix ExpectationDifferential(IManifoldPoint point, double time)
        {
            return SquareMatrix.Id(Dimension);
        }
    }
}
