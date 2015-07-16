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

    // [### VectorN.cs.TEMPLATE> T = float, D = 4
    ﻿

    public struct Vector4f : IEquatable<Vector4f>
    {
    
        public float X
        {
            get { return _x; }
        }
    
        public float Y
        {
            get { return _y; }
        }
    
        public float Z
        {
            get { return _z; }
        }
    
        public float T
        {
            get { return _t; }
        }
    
    
    
        private readonly float _x;
    
        private readonly float _y;
    
        private readonly float _z;
    
        private readonly float _t;
    

        public enum Axis
        {
            X = 0, 
            Y = 1, 
            Z = 2, 
            T = 3, 
            Count = 4 // used in for loops
        }

        public float this[Axis axis]
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

        #region creation

        public Vector4f(float x, float y, float z, float t)
        {
            _x = x;
            _y = y;
            _z = z;
            _t = t;
        }
    
     
        public Vector4f(Vector3f xyz, float t = 0)
        {
            _x = xyz.X;
            _y = xyz.Y;
            _z = xyz.Z;
            _t = t;
        }
    
    

        public Vector4f(Vector4i v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }

        public Vector4f(Vector4L v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }

        public Vector4f(Vector4f v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }

        public Vector4f(Vector4 v)
        {
            _x = ((float)v.X);
            _y = ((float)v.Y);
            _z = ((float)v.Z);
            _t = ((float)v.T);
        }


        public Vector4f(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 4, "4");

            _x = ((float)v[0]);
            _y = ((float)v[1]);
            _z = ((float)v[2]);
            _t = ((float)v[3]);
        }
    
        public float[] ToArray()
        {
            return new float[] { _x, _y, _z, _t };
        }
    
    
    
        public static implicit operator Vector4f(Vector4i v)
        {
            return new Vector4f(v);
        }
    
    
    
        public static implicit operator Vector4f(Vector4L v)
        {
            return new Vector4f(v);
        }
    
    
    
    
    
        public static explicit operator Vector4f(Vector4 v)
        {
            return new Vector4f(v);
        }
    
    

    
        public static explicit operator Vector4f(Vector3f v)
        {
            return new Vector4f(v);
        }
    

        public static explicit operator Vector4f(OVector v)
        {
            return new Vector4f(v);
        }

        public static implicit operator OVector(Vector4f v)
        {
            return new Vector(v._x, v._y, v._z, v._t);
        }
    
        #endregion

        #region subvectors
    
    
        public Vector3f XYZ
        {
            get { return new Vector3f(_x, _y, _z); }
        }
    
    
        #endregion
    
        #region equality

        public bool Equals(Vector4f v)
        {
            return _x == v._x && _y == v._y && _z == v._z && _t == v._t;
        }

        public override bool Equals(object obj)
        {

            var v = obj as Vector4f?;

            if(!v.HasValue)
            {
                return false;
            }

            return Equals(v.Value);

        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode() ^ _t.GetHashCode();
        }
    
        public static bool operator ==(Vector4f v, Vector4f w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector4f v, Vector4f w)
        {
            return !v.Equals(w);
        }

        #endregion

        #region norm

        public float Norm2
        {
            get { return (_x * _x) + (_y * _y) + (_z * _z) + (_t * _t); }
        }
    
        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static float Distance2(Vector4f v, Vector4f w)
        {
            return (v - w).Norm2;
        }
    
        public static double Distance(Vector4f v, Vector4f w)
        {
            return (v - w).Norm;
        }
    

        public Vector4f Normalized
        {
            get
            {
                var norm = this.Norm;

                if (norm < BasicMath.Epsilon)
                {
                    return UnitX;
                }

                return this / ((float)norm);
            }
        }


        #endregion

        #region algebra

        public static Vector4f operator +(Vector4f v)
        {
            return v;
        }

        public static Vector4f operator -(Vector4f v)
        {
            return new Vector4f(-v._x, -v._y, -v._z, -v._t);
        }

        public static Vector4f operator *(Vector4f v, float a)
        {
            return new Vector4f(v._x * a, v._y * a, v._z * a, v._t * a);
        }

        public static Vector4f operator *(float a, Vector4f v)
        {
            return v * a;
        }

        public static Vector4f operator /(Vector4f v, float a)
        {
            return new Vector4f(v._x / a, v._y / a, v._z / a, v._t / a);
        }

        public static Vector4f operator +(Vector4f v, Vector4f w)
        {
            return new Vector4f(v._x + w._x, v._y + w._y, v._z + w._z, v._t + w._t);
        }

        public static Vector4f operator -(Vector4f v, Vector4f w)
        {
            return new Vector4f(v._x - w._x, v._y - w._y, v._z - w._z, v._t - w._t);
        }

        public static float operator *(Vector4f v, Vector4f w)
        {
            return (v._x * w._x) + (v._y * w._y) + (v._z * w._z) + (v._t * w._t);
        }

    
    
        public static Vector4f Mul(Vector4f v, Vector4f w)
        {
            return new Vector4f(v._x * w._x, v._y * w._y, v._z * w._z, v._t * w._t);
        }
    
        public static Vector4f Div(Vector4f v, Vector4f w)
        {
            return new Vector4f(v._x / w._x, v._y / w._y, v._z / w._z, v._t / w._t);
        }

        #endregion

        #region static

        public static readonly Vector4f Zero = new Vector4f(0, 0, 0, 0);
    
        public static readonly Vector4f UnitX = new Vector4f(1, 0, 0, 0);
    
        public static readonly Vector4f UnitY = new Vector4f(0, 1, 0, 0);
    
        public static readonly Vector4f UnitZ = new Vector4f(0, 0, 1, 0);
    
        public static readonly Vector4f UnitT = new Vector4f(0, 0, 0, 1);
    
    
        public static Vector4f Unit(Axis axis)
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

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", _x, _y, _z, _t);
        }
    }
    // ###]
}
