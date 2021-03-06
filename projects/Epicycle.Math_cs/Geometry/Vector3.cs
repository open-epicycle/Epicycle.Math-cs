﻿// [[[[INFO>
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

    // [### VectorN.cs.TEMPLATE> T = double, D = 3
    ﻿
    public sealed class Vector3 : IEquatable<Vector3>
    {
        #region Constants

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

        #endregion

        #region Members
    
    
        private readonly double _x;
    
        private readonly double _y;
    
        private readonly double _z;
    
    
        #endregion
    
        #region Construction and conversion
    
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
    

    
        public Vector3(Vector3i v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }
    
        public Vector3(Vector3L v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }
    
        public Vector3(Vector3f v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }
    
        public Vector3(Vector3 v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }
    

        public Vector3(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 3, "3");

            _x = v[0];
            _y = v[1];
            _z = v[2];
        }

        public double[] ToArray()
        {
            return new double[] { _x, _y, _z };
        }

    
    
        public static implicit operator Vector3(Vector3i v)
        {
            return new Vector3(v);
        }
    
    
    
        public static implicit operator Vector3(Vector3L v)
        {
            return new Vector3(v);
        }
    
    
    
        public static implicit operator Vector3(Vector3f v)
        {
            return new Vector3(v);
        }
    
    
    
    

    
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

        #endregion
    
        #region Properties
    
    
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
    
    
        #endregion
    
        #region Axis
    
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
    
        #endregion
    
        #region Sub-vectors
    
    
        public Vector2 XY
        {
            get { return new Vector2(_x, _y); }
        }

        public Vector2 YZ
        {
            get { return new Vector2(_y, _z); }
        }

        public Vector2 ZX
        {
            get { return new Vector2(_z, _x); }
        }
    
    
        #endregion
    
        #region Equality & HashCode

        public bool Equals(Vector3 v)
        {
            return _x == v._x && _y == v._y && _z == v._z;
        }

        public override bool Equals(object obj)
        {
        
            if(obj == null || !(obj is Vector3))
            {
                return false;
            }

            return Equals((Vector3) obj);
        
        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode();
        }

        public static bool operator ==(Vector3 v, Vector3 w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector3 v, Vector3 w)
        {
            return !v.Equals(w);
        }

        #endregion
    
        #region ToString
    
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", _x, _y, _z);
        }
    
        #endregion
    
        #region Norm & Distance

        public double Norm2
        {
            get { return (_x * _x) + (_y * _y) + (_z * _z); }
        }

        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static double Distance2(Vector3 v, Vector3 w)
        {
            return (v - w).Norm2;
        }

        public static double Distance(Vector3 v, Vector3 w)
        {
            return (v - w).Norm;
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
    
    
        #endregion
    
        #region Algebra

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

        public static Vector3 operator +(Vector3 v, Vector3 w)
        {
            return new Vector3(v._x + w._x, v._y + w._y, v._z + w._z);
        }

        public static Vector3 operator -(Vector3 v, Vector3 w)
        {
            return new Vector3(v._x - w._x, v._y - w._y, v._z - w._z);
        }

        public static double operator *(Vector3 v, Vector3 w)
        {
            return (v._x * w._x) + (v._y * w._y) + (v._z * w._z);
        }

    
        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(_y * v._z - v._y * _z, _z * v._x - v._z * _x, _x * v._y - v._x * _y);
        }
    

        public static Vector3 Mul(Vector3 v, Vector3 w)
        {
            return new Vector3(v._x * w._x, v._y * w._y, v._z * w._z);
        }

        public static Vector3 Div(Vector3 v, Vector3 w)
        {
            return new Vector3(v._x / w._x, v._y / w._y, v._z / w._z);
        }
    
    
        public static double Angle(Vector3 v, Vector3 w)
        {
            var normprod = Math.Sqrt(v.Norm2 * w.Norm2);

            return BasicMath.Acos(v * w / normprod);
        }
    

        #endregion
    }
    // ###]
}
