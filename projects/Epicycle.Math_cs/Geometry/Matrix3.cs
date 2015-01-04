using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry
{
    public sealed class Matrix3
    {
        public Matrix3(double xx, double xy, double xz, double yx, double yy, double yz, double zx, double zy, double zz)
        {
            _xx = xx;
            _xy = xy;
            _xz = xz;
            _yx = yx;
            _yy = yy;
            _yz = yz;
            _zx = zx;
            _zy = zy;
            _zz = zz;
        }

        private readonly double _xx, _xy, _xz;
        private readonly double _yx, _yy, _yz;
        private readonly double _zx, _zy, _zz;

        public double XX
        {
            get { return _xx; }
        }

        public double XY
        {
            get { return _xy; }
        }

        public double XZ
        {
            get { return _xz; }
        }

        public double YX
        {
            get { return _yx; }
        }

        public double YY
        {
            get { return _yy; }
        }

        public double YZ
        {
            get { return _yz; }
        }

        public double ZX
        {
            get { return _zx; }
        }

        public double ZY
        {
            get { return _zy; }
        }

        public double ZZ
        {
            get { return _zz; }
        }

        public static Matrix3 operator *(Matrix3 a, Matrix3 b)
        {
            var xx = a.XX * b.XX + a.XY * b.YX + a.XZ * b.ZX;
            var xy = a.XX * b.XY + a.XY * b.YY + a.XZ * b.ZY;
            var xz = a.XX * b.XZ + a.XY * b.YZ + a.XZ * b.ZZ;

            var yx = a.YX * b.XX + a.YY * b.YX + a.YZ * b.ZX;
            var yy = a.YX * b.XY + a.YY * b.YY + a.YZ * b.ZY;
            var yz = a.YX * b.XZ + a.YY * b.YZ + a.YZ * b.ZZ;

            var zx = a.ZX * b.XX + a.ZY * b.YX + a.ZZ * b.ZX;
            var zy = a.ZX * b.XY + a.ZY * b.YY + a.ZZ * b.ZY;
            var zz = a.ZX * b.XZ + a.ZY * b.YZ + a.ZZ * b.ZZ;

            return new Matrix3(xx, xy, xz, yx, yy, yz, zx, zy, zz);
        }
    }
}
