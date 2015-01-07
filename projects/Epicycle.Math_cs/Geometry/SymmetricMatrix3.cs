// [[[[INFO>
// Copyright 2015 Epicycle (http://epicycle.org, https://github.com/open-epicycle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/open-epicycle/Epicycle.Math-cs
// ]]]]

using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry
{
    public sealed class SymmetricMatrix3
    {
        public SymmetricMatrix3(double xx, double yy, double zz, double xy, double yz, double zx)
        {
            _xx = xx;
            _yy = yy;
            _zz = zz;
            _xy = xy;
            _yz = yz;
            _zx = zx;
        }

        private readonly double _xx;
        private readonly double _yy;
        private readonly double _zz;
        private readonly double _xy;
        private readonly double _yz;
        private readonly double _zx;

        public double XX
        {
            get { return _xx; }
        }

        public double YY
        {
            get { return _yy; }
        }

        public double ZZ
        {
            get { return _zz; }
        }

        public double XY
        {
            get { return _xy; }
        }

        public double YZ
        {
            get { return _yz; }
        }

        public double ZX
        {
            get { return _zx; }
        }

        public static Vector3 operator *(SymmetricMatrix3 m, Vector3 v)
        {
            return new Vector3
            (
                m.XX * v.X + m.XY * v.Y + m.ZX * v.Z,
                m.XY * v.X + m.YY * v.Y + m.YZ * v.Z,
                m.ZX * v.X + m.YZ * v.Y + m.ZZ * v.Z
            );
        }

        public static SymmetricMatrix3 operator +(SymmetricMatrix3 a)
        {
            return a;
        }

        public static SymmetricMatrix3 operator -(SymmetricMatrix3 a)
        {
            return new SymmetricMatrix3(-a.XX, -a.YY, -a.ZZ, -a.XY, -a.YZ, -a.ZX);
        }

        public static SymmetricMatrix3 operator +(SymmetricMatrix3 a, SymmetricMatrix3 b)
        {
            return new SymmetricMatrix3(a.XX + b.XX, a.YY + b.YY, a.ZZ + b.ZZ, a.XY + b.XY, a.YZ + b.YZ, a.ZX + b.ZX);
        }

        public static SymmetricMatrix3 operator -(SymmetricMatrix3 a, SymmetricMatrix3 b)
        {
            return new SymmetricMatrix3(a.XX - b.XX, a.YY - b.YY, a.ZZ - b.ZZ, a.XY - b.XY, a.YZ - b.YZ, a.ZX - b.ZX);
        }

        public static SymmetricMatrix3 TensorSquare(Vector3 v)
        {
            return new SymmetricMatrix3(v.X * v.X, v.Y * v.Y, v.Z * v.Z, v.X * v.Y, v.Y * v.Z, v.Z * v.X);
        }

        public static SymmetricMatrix3 Scalar(double value)
        {
            return new SymmetricMatrix3(xx: value, yy: value, zz: value, xy: 0, yz: 0, zx: 0);
        }

        static SymmetricMatrix3()
        {
            _zero = new SymmetricMatrix3(xx: 0, yy: 0, zz: 0, xy: 0, yz: 0, zx: 0);
            _id =   new SymmetricMatrix3(xx: 1, yy: 1, zz: 1, xy: 0, yz: 0, zx: 0);
        }

        private static readonly SymmetricMatrix3 _zero;
        private static readonly SymmetricMatrix3 _id;

        public static SymmetricMatrix3 Zero
        {
            get { return _zero; }
        }

        public static SymmetricMatrix3 Id
        {
            get { return _id; }
        }
    }
}
