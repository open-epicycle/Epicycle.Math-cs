using Epicycle.Commons;
using Epicycle.Commons.Collections;

namespace Epicycle.Math.Geometry.Polytopes.Spherical
{

    public static class SphericalPolylineUtils
    {
        public static IClosedSphericalPolyline AsReverse(this IClosedSphericalPolyline @this)
        {
            return new ClosedSphericalPolyline(@this.Vertices.AsReverse());
        }
    }
}
