using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry.Differential
{
    public sealed class Rotation2Manifold : IManifold
    {
        public int Dimension
        {
            get { return 1; }
        }

        public IManifoldPoint Translate(IManifoldPoint point, OVector tangentVector)
        {
            ArgAssert.Equal(tangentVector.Dimension, "tangentVector.Dimension", 1, "1");

            return (Rotation2)point * new Rotation2(tangentVector[0]);
        }

        public OVector GetTranslation(IManifoldPoint to, IManifoldPoint from)
        {
            return new Vector((((Rotation2)from).Inv * (Rotation2)to).Angle);
        }
    }
}
