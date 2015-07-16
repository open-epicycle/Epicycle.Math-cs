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

    // [### VectorN.cs.TEMPLATE> T = float, D = 3
    ﻿

    public struct Vector3f : IEquatable<Vector3f>
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
    
    
    
        private readonly float _x;
    
        private readonly float _y;
    
        private readonly float _z;
    

        public enum Axis
        {
            X = 0, 
            Y = 1, 
            Z = 2, 
            Count = 3 // used in for loops
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
                
                    default:
                        throw new IndexOutOfRangeException("Invalid 3D axis " + axis.ToString());
                }
            }
        }

        #region creation

        public Vector3f(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
    
     
        public Vector3f(Vector2f xy, float z = 0)
        {
            _x = xy.X;
            _y = xy.Y;
            _z = z;
        }
    
    

        public Vector3f(Vector3i v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }

        public Vector3f(Vector3L v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }

        public Vector3f(Vector3f v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }

        public Vector3f(Vector3 v)
        {
            _x = ((float)v.X);
            _y = ((float)v.Y);
            _z = ((float)v.Z);
        }


        public Vector3f(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 3, "3");

            _x = ((float)v[0]);
            _y = ((float)v[1]);
            _z = ((float)v[2]);
        }
    
        public float[] ToArray()
        {
            return new float[] { _x, _y, _z };
        }
    
    
    
        public static implicit operator Vector3f(Vector3i v)
        {
            return new Vector3f(v);
        }
    
    
    
        public static implicit operator Vector3f(Vector3L v)
        {
            return new Vector3f(v);
        }
    
    
    
    
    
        public static explicit operator Vector3f(Vector3 v)
        {
            return new Vector3f(v);
        }
    
    

    
        public static explicit operator Vector3f(Vector2f v)
        {
            return new Vector3f(v);
        }
    

        public static explicit operator Vector3f(OVector v)
        {
            return new Vector3f(v);
        }

        public static implicit operator OVector(Vector3f v)
        {
            return new Vector(v._x, v._y, v._z);
        }
    
        #endregion

        #region subvectors
    
    
        public Vector2f XY
        {
            get { return new Vector2f(X, Y); }
        }

        public Vector2f YZ
        {
            get { return new Vector2f(Y, Z); }
        }

        public Vector2f ZX
        {
            get { return new Vector2f(Z, X); }
        }
    
    
        #endregion
    
        #region equality

        public bool Equals(Vector3f v)
        {
            return _x == v._x && _y == v._y && _z == v._z;
        }

        public override bool Equals(object obj)
        {

            var v = obj as Vector3f?;

            if(!v.HasValue)
            {
                return false;
            }

            return Equals(v.Value);

        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode();
        }
    
        public static bool operator ==(Vector3f v, Vector3f w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector3f v, Vector3f w)
        {
            return !v.Equals(w);
        }

        #endregion

        #region norm

        public float Norm2
        {
            get { return (_x * _x) + (_y * _y) + (_z * _z); }
        }
    
        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static float Distance2(Vector3f v, Vector3f w)
        {
            return (v - w).Norm2;
        }
    
        public static double Distance(Vector3f v, Vector3f w)
        {
            return (v - w).Norm;
        }
    

        public Vector3f Normalized
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

        public static Vector3f operator +(Vector3f v)
        {
            return v;
        }

        public static Vector3f operator -(Vector3f v)
        {
            return new Vector3f(-v._x, -v._y, -v._z);
        }

        public static Vector3f operator *(Vector3f v, float a)
        {
            return new Vector3f(v._x * a, v._y * a, v._z * a);
        }

        public static Vector3f operator *(float a, Vector3f v)
        {
            return v * a;
        }

        public static Vector3f operator /(Vector3f v, float a)
        {
            return new Vector3f(v._x / a, v._y / a, v._z / a);
        }

        public static Vector3f operator +(Vector3f v, Vector3f w)
        {
            return new Vector3f(v._x + w._x, v._y + w._y, v._z + w._z);
        }

        public static Vector3f operator -(Vector3f v, Vector3f w)
        {
            return new Vector3f(v._x - w._x, v._y - w._y, v._z - w._z);
        }

        public static float operator *(Vector3f v, Vector3f w)
        {
            return (v._x * w._x) + (v._y * w._y) + (v._z * w._z);
        }

    
        public Vector3f Cross(Vector3f v)
        {
            return new Vector3f(_y * v._z - v._y * _z, _z * v._x - v._z * _x, _x * v._y - v._x * _y);
        }
    
    
        public static Vector3f Mul(Vector3f v, Vector3f w)
        {
            return new Vector3f(v._x * w._x, v._y * w._y, v._z * w._z);
        }
    
        public static Vector3f Div(Vector3f v, Vector3f w)
        {
            return new Vector3f(v._x / w._x, v._y / w._y, v._z / w._z);
        }

        #endregion

        #region static

        public static readonly Vector3f Zero = new Vector3f(0, 0, 0);
    
        public static readonly Vector3f UnitX = new Vector3f(1, 0, 0);
    
        public static readonly Vector3f UnitY = new Vector3f(0, 1, 0);
    
        public static readonly Vector3f UnitZ = new Vector3f(0, 0, 1);
    
    
        public static Vector3f Unit(Axis axis)
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

    
        public static double Angle(Vector3f v, Vector3f w)
        {
            var normprod = Math.Sqrt(v.Norm2 * w.Norm2);

            return BasicMath.Acos(v * w / normprod);
        }
    

        #endregion

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", _x, _y, _z);
        }
    }
    // ###]
}
