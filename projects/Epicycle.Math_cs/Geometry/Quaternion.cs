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

// Authors: untrots

using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    using System;

    public sealed class Quaternion
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;
        private readonly double _d;

        #region creation

        public Quaternion(double a, double b, double c, double d)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
        }

        public Quaternion(double real)
        {
            _a = real;

            _b = 0;
            _c = 0;
            _d = 0;
        }

        public Quaternion(Vector3 imag) : this(0, imag) { }

        public Quaternion(double real, Vector3 imag)
        {
            _a = real;

            _b = imag.X;
            _c = imag.Y;
            _d = imag.Z;
        }

        public static implicit operator Quaternion(double real)
        {
            return new Quaternion(real);
        }

        public static implicit operator Quaternion(Vector3 imag)
        {
            return new Quaternion(imag);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("({0}+{1}i+{2}j+{3}k)", _a, _b, _c, _d);
        }

        #region static

        public static Quaternion Zero
        {
            get { return _zero; }
        }

        public static Quaternion One
        {
            get { return _one; }
        }

        public static Quaternion I 
        {
            get { return _i; }
        }

        public static Quaternion J
        {
            get { return _j; }
        }

        public static Quaternion K
        {
            get { return _k; }
        }

        private static readonly Quaternion _zero;
        private static readonly Quaternion _one;

        private static readonly Quaternion _i;
        private static readonly Quaternion _j;
        private static readonly Quaternion _k;

        static Quaternion()
        {
            _zero = new Quaternion(0);
            _one = new Quaternion(1);
            
            _i = new Quaternion(0, 1, 0, 0);
            _j = new Quaternion(0, 0, 1, 0);
            _k = new Quaternion(0, 0, 0, 1);
        }
        public static double Distance2(Quaternion x, Quaternion y)
        {
            return (x - y).Norm2;
        }

        public static double Distance(Quaternion x, Quaternion y)
        {
            return (x - y).Norm;
        }

        #endregion

        #region properties

        public double A
        {
            get { return _a; }
        }

        public double B
        {
            get { return _b; }
        }

        public double C
        {
            get { return _c; }
        }

        public double D
        {
            get { return _d; }
        }

        public double Re
        {
            get { return A; }
        }

        public Vector3 Im
        {
            get { return new Vector3(B, C, D); }
        }
        
        public double Norm2
        {
            get { return _a * _a + _b * _b + _c * _c + _d * _d; }
        }

        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public Quaternion Normalized
        {
            get
            {
                var norm = this.Norm;

                if (norm < BasicMath.Epsilon)
                {
                    return One;
                }

                return this / norm;
            }
        }

        public Quaternion Conjugate
        {
            get { return new Quaternion(_a, -_b, -_c, -_d); }
        }

        public Quaternion Inv
        {
            get { return this.Conjugate / this.Norm2; }
        }        

        #endregion

        #region algebra

        public static Quaternion operator +(Quaternion q)
        {
            return q;
        }

        public static Quaternion operator -(Quaternion q)
        {
            return new Quaternion(-q._a, -q._b, -q._c, -q._d);
        }

        public static Quaternion operator +(Quaternion q, double a)
        {
            return new Quaternion(q._a + a, q._b, q._c, q._d);
        }

        public static Quaternion operator +(double a, Quaternion q)
        {
            return q + a;
        }

        public static Quaternion operator -(Quaternion q, double a)
        {
            return new Quaternion(q._a - a, q._b, q._c, q._d);
        }

        public static Quaternion operator -(double a, Quaternion q)
        {
            return -q + a;
        }

        public static Quaternion operator *(Quaternion q, double x)
        {
            return new Quaternion(q._a * x, q._b * x, q._c * x, q._d * x);
        }

        public static Quaternion operator *(double x, Quaternion q)
        {
            return q * x;
        }

        public static Quaternion operator /(Quaternion q, double x)
        {
            return new Quaternion(q._a / x, q._b / x, q._c / x, q._d / x);
        }

        public static Quaternion operator +(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1._a + q2._a, q1._b + q2._b, q1._c + q2._c, q1._d + q2._d);
        }

        public static Quaternion operator -(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1._a - q2._a, q1._b - q2._b, q1._c - q2._c, q1._d - q2._d);
        }

        public static Quaternion operator *(Quaternion q, Vector3 v)
        {
            return new Quaternion(-q.Im * v, q.Re * v + q.Im.Cross(v));
        }

        public static Quaternion operator *(Vector3 v, Quaternion q)
        {
            return new Quaternion(-q.Im * v, q.Re * v - q.Im.Cross(v));
        }

        public static Quaternion operator *(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(
                q1._a * q2._a - q1._b * q2._b - q1._c * q2._c - q1._d * q2._d,
                q1._a * q2._b + q1._b * q2._a + q1._c * q2._d - q1._d * q2._c,
                q1._a * q2._c - q1._b * q2._d + q1._c * q2._a + q1._d * q2._b,
                q1._a * q2._d + q1._b * q2._c - q1._c * q2._b + q1._d * q2._a);
        }

        public static double Dot(Quaternion q1, Quaternion q2)
        {
            return q1._a * q2._a + q1._b * q2._b + q1._c + q2._c + q1._d * q2._d;
        }

        #endregion        
    }
}
