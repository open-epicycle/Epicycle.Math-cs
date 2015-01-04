using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry.Differential
{
    public sealed class Rotation3Manifold : IManifold
    {
        public int Dimension
        {
            get { return 3; }
        }

        public IManifoldPoint Translate(IManifoldPoint point, OVector tangentVector)
        {
            return (Rotation3)point * new Rotation3((Vector3)tangentVector);
        }

        public OVector GetTranslation(IManifoldPoint to, IManifoldPoint from)
        {
            return (((Rotation3)from).Inv * (Rotation3)to).Vector;
        }
    }
}
