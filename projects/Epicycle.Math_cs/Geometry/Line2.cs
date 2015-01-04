using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry
{
    // oriented straight line on the Euclidean plane
    public struct Line2
    {
        public Line2(double freeTerm, Vector2 linearTerms)
        {
            var norm = linearTerms.Norm;

            _normal = linearTerms / norm;
            _freeTerm = freeTerm / norm;
        }

        public Line2(Vector2 point, Vector2 normal)
        {
            _normal = normal.Normalized;
            _freeTerm = -point * _normal;
        }

        public Line2(Ray2 ray)
        {
            _normal = Rotation2Utils.Rotate270(ray.Direction);
            _freeTerm = -ray.Origin * _normal;
        }

        private readonly Vector2 _normal;
        private readonly double _freeTerm;

        public static explicit operator Line2(Ray2 ray)
        {
            return new Line2(ray);
        }

        public Vector2 Normal
        {
            get { return _normal; }
        }

        public double FreeTerm
        {
            get { return _freeTerm; }
        }

        public Vector2 Direction
        {
            get { return _normal.Rotate90(); }
        }

        public Line2 Reverse
        {
            get { return new Line2(-FreeTerm, -Normal); }
        }

        public Line2 PositivelyOriented
        {
            get { return FreeTerm < 0 ? this : this.Reverse; }
        }

        public static Line2 Parametric(Vector2 point, Vector2 direction)
        {
            return new Line2(new Ray2(point, direction));
        }

        public static Line2 Through(Vector2 a, Vector2 b)
        {
            return Parametric(a, b - a);
        }

        public static Line2 ParallelTo(Line2 line, double signedDistance)
        {
            return new Line2(line.FreeTerm - signedDistance, line.Normal);
        }
    }
}
