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

    // [### VectorN.cs.TEMPLATE> T = float, D = 2
    ﻿

    public struct Vector2f : IEquatable<Vector2f>
    {
    
        public float X
        {
            get { return _x; }
        }
    
        public float Y
        {
            get { return _y; }
        }
    
    
    
        private readonly float _x;
    
        private readonly float _y;
    

        public enum Axis
        {
            X = 0, 
            Y = 1, 
            Count = 2 // used in for loops
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
                
                    default:
                        throw new IndexOutOfRangeException("Invalid 2D axis " + axis.ToString());
                }
            }
        }

        #region creation

        public Vector2f(float x, float y)
        {
            _x = x;
            _y = y;
        }
    
    
    

        public Vector2f(Vector2i v)
        {
            _x = v.X;
            _y = v.Y;
        }

        public Vector2f(Vector2L v)
        {
            _x = v.X;
            _y = v.Y;
        }

        public Vector2f(Vector2f v)
        {
            _x = v.X;
            _y = v.Y;
        }

        public Vector2f(Vector2 v)
        {
            _x = ((float)v.X);
            _y = ((float)v.Y);
        }


        public Vector2f(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 2, "2");

            _x = ((float)v[0]);
            _y = ((float)v[1]);
        }
    
        public float[] ToArray()
        {
            return new float[] { _x, _y };
        }
    
    
    
        public static implicit operator Vector2f(Vector2i v)
        {
            return new Vector2f(v);
        }
    
    
    
        public static implicit operator Vector2f(Vector2L v)
        {
            return new Vector2f(v);
        }
    
    
    
    
    
        public static explicit operator Vector2f(Vector2 v)
        {
            return new Vector2f(v);
        }
    
    

    

        public static explicit operator Vector2f(OVector v)
        {
            return new Vector2f(v);
        }

        public static implicit operator OVector(Vector2f v)
        {
            return new Vector(v._x, v._y);
        }
    
        #endregion

        #region subvectors
    
    
    
        #endregion
    
        #region equality

        public bool Equals(Vector2f v)
        {
            return _x == v._x && _y == v._y;
        }

        public override bool Equals(object obj)
        {

            var v = obj as Vector2f?;

            if(!v.HasValue)
            {
                return false;
            }

            return Equals(v.Value);

        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode();
        }
    
        public static bool operator ==(Vector2f v, Vector2f w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector2f v, Vector2f w)
        {
            return !v.Equals(w);
        }

        #endregion

        #region norm

        public float Norm2
        {
            get { return (_x * _x) + (_y * _y); }
        }
    
        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static float Distance2(Vector2f v, Vector2f w)
        {
            return (v - w).Norm2;
        }
    
        public static double Distance(Vector2f v, Vector2f w)
        {
            return (v - w).Norm;
        }
    

        public Vector2f Normalized
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

        public static Vector2f operator +(Vector2f v)
        {
            return v;
        }

        public static Vector2f operator -(Vector2f v)
        {
            return new Vector2f(-v._x, -v._y);
        }

        public static Vector2f operator *(Vector2f v, float a)
        {
            return new Vector2f(v._x * a, v._y * a);
        }

        public static Vector2f operator *(float a, Vector2f v)
        {
            return v * a;
        }

        public static Vector2f operator /(Vector2f v, float a)
        {
            return new Vector2f(v._x / a, v._y / a);
        }

        public static Vector2f operator +(Vector2f v, Vector2f w)
        {
            return new Vector2f(v._x + w._x, v._y + w._y);
        }

        public static Vector2f operator -(Vector2f v, Vector2f w)
        {
            return new Vector2f(v._x - w._x, v._y - w._y);
        }

        public static float operator *(Vector2f v, Vector2f w)
        {
            return (v._x * w._x) + (v._y * w._y);
        }

    
        public float Cross(Vector2f v)
        {
            return _x * v._y - _y * v._x;
        }
    
    
        public static Vector2f Mul(Vector2f v, Vector2f w)
        {
            return new Vector2f(v._x * w._x, v._y * w._y);
        }
    
        public static Vector2f Div(Vector2f v, Vector2f w)
        {
            return new Vector2f(v._x / w._x, v._y / w._y);
        }

        #endregion

        #region static

        public static readonly Vector2f Zero = new Vector2f(0, 0);
    
        public static readonly Vector2f UnitX = new Vector2f(1, 0);
    
        public static readonly Vector2f UnitY = new Vector2f(0, 1);
    
    
        public static Vector2f Unit(Axis axis)
        {
            switch (axis)
            {
            
                case Axis.X:
                    return UnitX;
            
                case Axis.Y:
                    return UnitY;
            

                default:
                    throw new IndexOutOfRangeException("Invalid 2D axis " + axis.ToString());
            }
        }

    
        public static double Angle(Vector2f v, Vector2f w)
        {
            var vwcos = v * w;
            var vwsin = v.Cross(w);

            return Math.Atan2(vwsin, vwcos);
        }
    

        #endregion

        public override string ToString()
        {
            return string.Format("({0}, {1})", _x, _y);
        }
    }
    // ###]
}
