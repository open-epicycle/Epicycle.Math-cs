using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry
{
    using System;

    // oriented plane in 3D Euclidean space
    public struct Plane
    {
        public Plane(double freeTerm, Vector3 linearTerms)
        {
            var norm = linearTerms.Norm;

            _normal = linearTerms / norm;
            _freeTerm = freeTerm / norm;
        }

        public Plane(Vector3 point, Vector3 normal)
        {
            _normal = normal.Normalized;
            _freeTerm = -point * _normal;
        }

        private readonly Vector3 _normal;
        private readonly double _freeTerm;

        public Vector3 Normal
        {
            get { return _normal; }
        }

        public double FreeTerm
        {
            get { return _freeTerm; }
        }

        public Plane Reverse
        {
            get { return new Plane(-FreeTerm, -Normal); }
        }

        public static Plane Parametric(Vector3 a, Vector3 u, Vector3 v)
        {
            return new Plane(a, u.Cross(v));
        }

        public static Plane Through(Vector3 a, Vector3 b, Vector3 c)
        {
            return Parametric(a, b - a, c - a);
        }

        public static Plane ParallelTo(Plane plane, double signedDistance)
        {
            return new Plane(plane.FreeTerm - signedDistance, plane.Normal);
        }
    }
}
