using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Math.LinearAlgebra;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Probability
{
    public sealed class AffineStochasticTransformation : IStochasticMapping
    {
        public AffineStochasticTransformation(OMatrix matrix, OVector freeTerm, OSymmetricMatrix covariance)
        {
            _domain = new AffineSpace(matrix.ColumnCount);
            _codomain = new AffineSpace(matrix.RowCount);

            _matrix = matrix;
            _freeTerm = freeTerm;
            _covariance = covariance;
        }

        private readonly IManifold _domain;
        private readonly IManifold _codomain;

        private readonly OMatrix _matrix;
        private readonly OVector _freeTerm;
        private readonly OSymmetricMatrix _covariance;

        public IManifold Domain
        {
            get { return _domain; }
        }

        public IManifold Codomain
        {
            get { return _codomain; }
        }

        public StochasticManifoldPoint Apply(IManifoldPoint point)
        {
            return new StochasticManifoldPoint(_matrix * (OVector)point + _freeTerm, _covariance);
        }

        public OMatrix ExpectationDifferential(IManifoldPoint point)
        {
            return _matrix;
        }
    }
}
