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

    // [### VectorN.cs.TEMPLATE> T = double, D = 4
    ﻿
    public sealed class Vector4 : IEquatable<Vector4>
    {
        #region Constants

        public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);
    
        public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);
    
        public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);
    
        public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);
    
        public static readonly Vector4 UnitT = new Vector4(0, 0, 0, 1);
    

        public static Vector4 Unit(Axis axis)
        {
            switch (axis)
            {
            
                case Axis.X:
                    return UnitX;
            
                case Axis.Y:
                    return UnitY;
            
                case Axis.Z:
                    return UnitZ;
            
                case Axis.T:
                    return UnitT;
            

                default:
                    throw new IndexOutOfRangeException("Invalid 4D axis " + axis.ToString());
            }
        }

        #endregion

        #region Members
    
    
        private readonly double _x;
    
        private readonly double _y;
    
        private readonly double _z;
    
        private readonly double _t;
    
    
        #endregion
    
        #region Construction and conversion
    
        public Vector4(double x, double y, double z, double t)
        {
            _x = x;
            _y = y;
            _z = z;
            _t = t;
        }

     
        public Vector4(Vector3 xyz, double t = 0)
        {
            _x = xyz.X;
            _y = xyz.Y;
            _z = xyz.Z;
            _t = t;
        }
    

    
        public Vector4(Vector4i v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }
    
        public Vector4(Vector4L v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }
    
        public Vector4(Vector4f v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }
    
        public Vector4(Vector4 v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }
    

        public Vector4(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 4, "4");

            _x = v[0];
            _y = v[1];
            _z = v[2];
            _t = v[3];
        }

        public double[] ToArray()
        {
            return new double[] { _x, _y, _z, _t };
        }

    
    
        public static implicit operator Vector4(Vector4i v)
        {
            return new Vector4(v);
        }
    
    
    
        public static implicit operator Vector4(Vector4L v)
        {
            return new Vector4(v);
        }
    
    
    
        public static implicit operator Vector4(Vector4f v)
        {
            return new Vector4(v);
        }
    
    
    
    

    
        public static explicit operator Vector4(Vector3 v)
        {
            return new Vector4(v);
        }
    

        public static explicit operator Vector4(OVector v)
        {
            return new Vector4(v);
        }

        public static implicit operator OVector(Vector4 v)
        {
            return new Vector(v._x, v._y, v._z, v._t);
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
    
        public double T
        {
            get { return _t; }
        }
    
    
        #endregion
    
        #region Axis
    
        public enum Axis
        {
            X = 0, 
            Y = 1, 
            Z = 2, 
            T = 3, 
            Count = 4 // used in for loops
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
                
                    case Axis.T:
                        return _t;
                
                    default:
                        throw new IndexOutOfRangeException("Invalid 4D axis " + axis.ToString());
                }
            }
        }
    
        #endregion
    
        #region Sub-vectors
    
    
        public Vector3 XYZ
        {
            get { return new Vector3(_x, _y, _z); }
        }
    
    
        #endregion
    
        #region Equality & HashCode

        public bool Equals(Vector4 v)
        {
            return _x == v._x && _y == v._y && _z == v._z && _t == v._t;
        }

        public override bool Equals(object obj)
        {
        
            if(obj == null || !(obj is Vector4))
            {
                return false;
            }

            return Equals((Vector4) obj);
        
        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode() ^ _t.GetHashCode();
        }

        public static bool operator ==(Vector4 v, Vector4 w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector4 v, Vector4 w)
        {
            return !v.Equals(w);
        }

        #endregion
    
        #region ToString
    
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", _x, _y, _z, _t);
        }
    
        #endregion
    
        #region Norm & Distance

        public double Norm2
        {
            get { return (_x * _x) + (_y * _y) + (_z * _z) + (_t * _t); }
        }

        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static double Distance2(Vector4 v, Vector4 w)
        {
            return (v - w).Norm2;
        }

        public static double Distance(Vector4 v, Vector4 w)
        {
            return (v - w).Norm;
        }

    
        public Vector4 Normalized
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

        public static Vector4 operator +(Vector4 v)
        {
            return v;
        }

        public static Vector4 operator -(Vector4 v)
        {
            return new Vector4(-v._x, -v._y, -v._z, -v._t);
        }

        public static Vector4 operator *(Vector4 v, double a)
        {
            return new Vector4(v._x * a, v._y * a, v._z * a, v._t * a);
        }

        public static Vector4 operator *(double a, Vector4 v)
        {
            return v * a;
        }

        public static Vector4 operator /(Vector4 v, double a)
        {
            return new Vector4(v._x / a, v._y / a, v._z / a, v._t / a);
        }

        public static Vector4 operator +(Vector4 v, Vector4 w)
        {
            return new Vector4(v._x + w._x, v._y + w._y, v._z + w._z, v._t + w._t);
        }

        public static Vector4 operator -(Vector4 v, Vector4 w)
        {
            return new Vector4(v._x - w._x, v._y - w._y, v._z - w._z, v._t - w._t);
        }

        public static double operator *(Vector4 v, Vector4 w)
        {
            return (v._x * w._x) + (v._y * w._y) + (v._z * w._z) + (v._t * w._t);
        }

    

        public static Vector4 Mul(Vector4 v, Vector4 w)
        {
            return new Vector4(v._x * w._x, v._y * w._y, v._z * w._z, v._t * w._t);
        }

        public static Vector4 Div(Vector4 v, Vector4 w)
        {
            return new Vector4(v._x / w._x, v._y / w._y, v._z / w._z, v._t / w._t);
        }
    
    

        #endregion
    }
    // ###]
}
