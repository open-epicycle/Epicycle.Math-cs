using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry.Differential
{
    public sealed class AffineSpace : IManifold
    {
        public AffineSpace(int dimension)
        {
            _dimension = dimension;
        }

        private readonly int _dimension;

        public int Dimension
        {
            get { return _dimension; }
        }

        public IManifoldPoint Translate(IManifoldPoint point, OVector tangentVector)
        {
            return (OVector)point + tangentVector;
        }

        public OVector GetTranslation(IManifoldPoint to, IManifoldPoint from)
        {
            return (OVector)to - (OVector)from;
        }
    }
}
