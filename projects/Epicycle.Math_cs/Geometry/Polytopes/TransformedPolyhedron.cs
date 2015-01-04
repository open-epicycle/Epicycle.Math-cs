using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry.Polytopes
{
    public sealed class TransformedPolyhedron : TransformedPolysurface, IPolyhedron
    {
        public TransformedPolyhedron(IPolyhedron localPolyhedron, RotoTranslation3 localToGlobal) : base(localPolyhedron, localToGlobal)
        {
            _localPolyhedron = localPolyhedron;
        }

        private readonly IPolyhedron _localPolyhedron;

        public bool Contains(Vector3 point)
        {
            return _localPolyhedron.Contains(LocalToGlobal.ApplyInv(point));
        }

        public double Distance2(Vector3 point)
        {
            return _localPolyhedron.Distance2(LocalToGlobal.ApplyInv(point));
        }
    }
}
