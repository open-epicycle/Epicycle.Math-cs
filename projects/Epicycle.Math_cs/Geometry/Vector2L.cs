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

// Authors: squark, untrots

using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry
{
    using System;

    // [### Vector2.cs.TEMPLATE> T = long
    ﻿

    public struct Vector2L : IEquatable<Vector2L>
    {
        public long X 
        {
            get { return _x; }
        }

        public long Y 
        {
            get { return _y; }
        }
    
        private readonly long _x;
        private readonly long _y;

        public enum Axis
        {
            X = 0,
            Y = 1,
            Count = 2 // used in for loops
        }

        public long this[Axis axis]
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

        public Vector2L(long x, long y)
        {
            _x = x;
            _y = y;
        }
    

        public Vector2L(Vector2i v)
        {
            _x = v.X;
            _y = v.Y;
        }

        public Vector2L(Vector2L v)
        {
            _x = v.X;
            _y = v.Y;
        }

        public Vector2L(Vector2f v)
        {
            _x = ((long)Math.Round(v.X));
            _y = ((long)Math.Round(v.Y));
        }

        public Vector2L(Vector2 v)
        {
            _x = ((long)Math.Round(v.X));
            _y = ((long)Math.Round(v.Y));
        }


        public Vector2L(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 2, "2");

            _x = ((long)Math.Round(v[0]));
            _y = ((long)Math.Round(v[1]));
        }
    


        public static implicit operator Vector2L(Vector2i v)
        {
            return new Vector2L(v);
        }





        public static explicit operator Vector2L(Vector2f v)
        {
            return new Vector2L(v);
        }



        public static explicit operator Vector2L(Vector2 v)
        {
            return new Vector2L(v);
        }



        public static explicit operator Vector2L(OVector v)
        {
            return new Vector2L(v);
        }

        public static explicit operator OVector(Vector2L v)
        {
            return new Vector(v._x, v._y);
        }
    
        #endregion

        #region equality

        public bool Equals(Vector2L v)
        {
            return X == v.X && Y == v.Y;
        }

        public override bool Equals(object obj)
        {
            var v = obj as Vector2L?;

            if(!v.HasValue)
            {
                return false;
            }

            return Equals(v.Value);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    
        public static bool operator ==(Vector2L v, Vector2L w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector2L v, Vector2L w)
        {
            return !v.Equals(w);
        }

        #endregion

        #region norm

        public long Norm2
        {
            get { return _x * _x + _y * _y; }
        }
    
        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static long Distance2(Vector2L v, Vector2L w)
        {
            return (v - w).Norm2;
        }
    
        public static double Distance(Vector2L v, Vector2L w)
        {
            return (v - w).Norm;
        }
    


        #endregion

        #region algebra

        public static Vector2L operator +(Vector2L v)
        {
            return v;
        }

        public static Vector2L operator -(Vector2L v)
        {
            return new Vector2L(-v._x, -v._y);
        }

        public static Vector2L operator *(Vector2L v, long a)
        {
            return new Vector2L(v._x * a, v._y * a);
        }

        public static Vector2L operator *(long a, Vector2L v)
        {
            return v * a;
        }

        public static Vector2L operator /(Vector2L v, long a)
        {
            return new Vector2L(v._x / a, v._y / a);
        }

        public static Vector2L operator +(Vector2L v, Vector2L w)
        {
            return new Vector2L(v._x + w._x, v._y + w._y);
        }

        public static Vector2L operator -(Vector2L v, Vector2L w)
        {
            return new Vector2L(v._x - w._x, v._y - w._y);
        }

        public static long operator *(Vector2L v, Vector2L w)
        {
            return v._x * w._x + v._y * w._y;
        }

        public long Cross(Vector2L v)
        {
            return _x * v._y - _y * v._x;
        }
    
        public static Vector2L Mul(Vector2L v, Vector2L w)
        {
            return new Vector2L(v.X * w.X, v.Y * w.Y);
        }

        #endregion

        #region static

        public static readonly Vector2L Zero = new Vector2L(0, 0);
        public static readonly Vector2L UnitX = new Vector2L(1, 0);
        public static readonly Vector2L UnitY = new Vector2L(0, 1);

        public static Vector2L Unit(Axis axis)
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

        public static double Angle(Vector2L v, Vector2L w)
        {
            var vwcos = v * w;
            var vwsin = v.Cross(w);

            return Math.Atan2(vwsin, vwcos);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
     // ###]
 }
