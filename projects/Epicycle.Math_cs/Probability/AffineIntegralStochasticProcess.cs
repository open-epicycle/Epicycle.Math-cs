using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public sealed class AffineIntegralStochasticProcess : IntegralStochasticProcess
    {
        public AffineIntegralStochasticProcess(IStochasticProcess baseProcess, IManifold fiber, OMatrix gain, OVector offset)
            : base(baseProcess, fiber)
        {
            if (!(baseProcess.StateSpace is AffineSpace))
            {
                throw new ArgumentException("Expected process in affine space", "baseProcess");
            }

            ArgAssert.Equal(baseProcess.StateSpace.Dimension, "baseProcess.StateSpace.Dimension", gain.ColumnCount, "gain.ColumnCount");
            ArgAssert.Equal(fiber.Dimension, "fiber.Dimension", gain.RowCount, "gain.RowCount");
            ArgAssert.Equal(offset.Dimension, "offset.Dimension", gain.RowCount, "gain.RowCount");

            _gain = gain;
            _offset = offset;
        }

        private readonly OMatrix _gain;
        private readonly OVector _offset;

        protected override OVector Velocity(IManifoldPoint basePoint)
        {
            return _gain * (OVector)basePoint + _offset;
        }

        protected override OMatrix VelocityDifferential(IManifoldPoint basePoint)
        {
            return _gain;
        }
    }
}
