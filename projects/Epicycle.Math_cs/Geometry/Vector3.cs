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

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry
{
    using System;

    public sealed class Vector3 : IEquatable<Vector3>
    {
        private readonly double _x;
        private readonly double _y;
        private readonly double _z;

        #region construction

        public Vector3(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector3(Vector2 xy, double z = 0)
        {
            _x = xy.X;
            _y = xy.Y;
            _z = z;
        }

        public Vector3(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 3, "3");

            _x = v[0];
            _y = v[1];
            _z = v[2];
        }

        #endregion

        #region conversion

        public static explicit operator Vector3(Vector2 v)
        {
            return new Vector3(v);
        }

        public static explicit operator Vector3(OVector v)
        {
            return new Vector3(v);
        }

        public static implicit operator OVector(Vector3 v)
        {
            return new Vector(v._x, v._y, v._z);
        }

        public double[] ToArray()
        {
            return new double[] { _x, _y, _z };
        }

        public float[] ToFloatArray()
        {
            return new float[] { (float)_x, (float)_y, (float)_z };
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }

        #endregion

        #region basic properties

        public double X
        {
            get { return _x; }
        }

        public double Y
        {
            get { return _y; }
        }

        public double Z
        {
            get { return _z; }
        }

        public enum Axis
        {
            X = 0,
            Y = 1,
            Z = 2,
            Count = 3 // used in for loops
        }

        public double this[Axis axis]
        {
            get
            {
                switch (axis)
                {
                    case Axis.X:
                        return _x;

                    case Axis.Y:
                        return _y;

                    case Axis.Z:
                        return _z;

                    default:
                        throw new IndexOutOfRangeException("Invalid 3D axis " + axis.ToString());
                }
            }
        }

        public Vector2 XY
        {
            get { return new Vector2(X, Y); }
        }

        public Vector2 YZ
        {
            get { return new Vector2(Y, Z); }
        }

        public Vector2 ZX
        {
            get { return new Vector2(Z, X); }
        }

        #endregion

        #region norm

        public double Norm2
        {
            get { return _x * _x + _y * _y + _z * _z; }
        }

        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }        

        public Vector3 Normalized
        {
            get 
            {
                var norm = this.Norm;

                if (norm < BasicMath.Epsilon)
                {
                    return UnitX;
                }
 
                return this / norm;
            }
        }

        public static double Distance2(Vector3 v, Vector3 w)
        {
            return (v - w).Norm2;
        }

        public static double Distance(Vector3 v, Vector3 w)
        {
            return (v - w).Norm;
        }

        #endregion

        #region algebra

        public static Vector3 operator +(Vector3 v)
        {
            return v;
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v._x, -v._y, -v._z);
        }

        public static Vector3 operator *(Vector3 v, double a)
        {
            return new Vector3(v._x * a, v._y * a, v._z * a);
        }

        public static Vector3 operator *(double a, Vector3 v)
        {
            return v * a;
        }

        public static Vector3 operator /(Vector3 v, double a)
        {
            return new Vector3(v._x / a, v._y / a, v._z / a);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x + v2._x, v1._y + v2._y, v1._z + v2._z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._x - v2._x, v1._y - v2._y, v1._z - v2._z);
        }

        public static double operator *(Vector3 v1, Vector3 v2)
        {
            return v1._x * v2._x + v1._y * v2._y + v1._z * v2._z;
        }

        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(_y * v._z - v._y * _z, _z * v._x - v._z * _x, _x * v._y - v._x * _y);
        }

        #endregion

        #region equality

        public bool Equals(Vector3 that)
        {
            return this.X == that.X && this.Y == that.Y && this.Z == that.Z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
            {
                return false;
            }

            return Equals((Vector3)obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        #endregion

        #region static

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

        public static Vector3 Unit(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return UnitX;

                case Axis.Y:
                    return UnitY;

                case Axis.Z:
                    return UnitZ;

                default:
                    throw new IndexOutOfRangeException("Invalid 3D axis " + axis.ToString());
            }
        }

        public static double Angle(Vector3 v, Vector3 w)
        {
            var normprod = Math.Sqrt(v.Norm2 * w.Norm2);

            return BasicMath.Acos(v * w / normprod);
        }

        #endregion
    }
}
