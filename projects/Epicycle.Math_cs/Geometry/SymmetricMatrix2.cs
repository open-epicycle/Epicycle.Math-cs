using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry
{
    public sealed class SymmetricMatrix2
    {
        public SymmetricMatrix2(double xx, double yy, double xy)
        {
            _xx = xx;
            _xy = xy;
            _yy = yy;
        }

        private readonly double _xx;
        private readonly double _xy;
        private readonly double _yy;

        public double XX
        {
            get { return _xx; }
        }

        public double XY
        {
            get { return _xy; }
        }

        public double YY
        {
            get { return _yy; }
        }

        public static Vector2 operator *(SymmetricMatrix2 m, Vector2 v)
        {
            return new Vector2(m.XX * v.X + m.XY * v.Y, m.XY * v.X + m.YY * v.Y);
        }

        public static SymmetricMatrix2 operator +(SymmetricMatrix2 a)
        {
            return a;
        }

        public static SymmetricMatrix2 operator -(SymmetricMatrix2 a)
        {
            return new SymmetricMatrix2(-a.XX, -a.YY, -a.XY);
        }

        public static SymmetricMatrix2 operator +(SymmetricMatrix2 a, SymmetricMatrix2 b)
        {
            return new SymmetricMatrix2(a.XX + b.XX, a.YY + b.YY, a.XY + b.XY);
        }

        public static SymmetricMatrix2 operator -(SymmetricMatrix2 a, SymmetricMatrix2 b)
        {
            return new SymmetricMatrix2(a.XX - b.XX, a.YY - b.YY, a.XY - b.XY);
        }        

        public static SymmetricMatrix2 TensorSquare(Vector2 v)
        {
            return new SymmetricMatrix2(v.X * v.X, v.Y * v.Y, v.X * v.Y);
        }

        public static SymmetricMatrix2 Scalar(double value)
        {
            return new SymmetricMatrix2(xx: value, yy: value, xy: 0);
        }

        static SymmetricMatrix2()
        {
            _zero = new SymmetricMatrix2(xx: 0, yy: 0, xy: 0);
            _id =   new SymmetricMatrix2(xx: 1, yy: 1, xy: 0);
        }

        private static readonly SymmetricMatrix2 _zero;
        private static readonly SymmetricMatrix2 _id;

        public static SymmetricMatrix2 Zero
        {
            get { return _zero; }
        }

        public static SymmetricMatrix2 Id
        {
            get { return _id; }
        }
    }
}
