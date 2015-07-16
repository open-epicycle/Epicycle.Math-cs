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

    // [### Vector2.cs.TEMPLATE> T = int, D = 2
    ﻿

    public struct Vector2i : IEquatable<Vector2i>
    {
        public int X 
        {
            get { return _x; }
        }

        public int Y 
        {
            get { return _y; }
        }
    
        private readonly int _x;
        private readonly int _y;

        public enum Axis
        {
            X = 0,
            Y = 1,
            Count = 2 // used in for loops
        }

        public int this[Axis axis]
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

        public Vector2i(int x, int y)
        {
            _x = x;
            _y = y;
        }
    

        public Vector2i(Vector2i v)
        {
            _x = v.X;
            _y = v.Y;
        }

        public Vector2i(Vector2L v)
        {
            _x = ((int)v.X);
            _y = ((int)v.Y);
        }

        public Vector2i(Vector2f v)
        {
            _x = ((int)Math.Round(v.X));
            _y = ((int)Math.Round(v.Y));
        }

        public Vector2i(Vector2 v)
        {
            _x = ((int)Math.Round(v.X));
            _y = ((int)Math.Round(v.Y));
        }


        public Vector2i(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 2, "2");

            _x = ((int)Math.Round(v[0]));
            _y = ((int)Math.Round(v[1]));
        }
    




        public static explicit operator Vector2i(Vector2L v)
        {
            return new Vector2i(v);
        }



        public static explicit operator Vector2i(Vector2f v)
        {
            return new Vector2i(v);
        }



        public static explicit operator Vector2i(Vector2 v)
        {
            return new Vector2i(v);
        }



        public static explicit operator Vector2i(OVector v)
        {
            return new Vector2i(v);
        }

        public static explicit operator OVector(Vector2i v)
        {
            return new Vector(v._x, v._y);
        }
    
        #endregion

        #region equality

        public bool Equals(Vector2i v)
        {
            return X == v.X && Y == v.Y;
        }

        public override bool Equals(object obj)
        {
            var v = obj as Vector2i?;

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
    
        public static bool operator ==(Vector2i v, Vector2i w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector2i v, Vector2i w)
        {
            return !v.Equals(w);
        }

        #endregion

        #region norm

        public int Norm2
        {
            get { return _x * _x + _y * _y; }
        }
    
        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static int Distance2(Vector2i v, Vector2i w)
        {
            return (v - w).Norm2;
        }
    
        public static double Distance(Vector2i v, Vector2i w)
        {
            return (v - w).Norm;
        }
    


        #endregion

        #region algebra

        public static Vector2i operator +(Vector2i v)
        {
            return v;
        }

        public static Vector2i operator -(Vector2i v)
        {
            return new Vector2i(-v._x, -v._y);
        }

        public static Vector2i operator *(Vector2i v, int a)
        {
            return new Vector2i(v._x * a, v._y * a);
        }

        public static Vector2i operator *(int a, Vector2i v)
        {
            return v * a;
        }

        public static Vector2i operator /(Vector2i v, int a)
        {
            return new Vector2i(v._x / a, v._y / a);
        }

        public static Vector2i operator +(Vector2i v, Vector2i w)
        {
            return new Vector2i(v._x + w._x, v._y + w._y);
        }

        public static Vector2i operator -(Vector2i v, Vector2i w)
        {
            return new Vector2i(v._x - w._x, v._y - w._y);
        }

        public static int operator *(Vector2i v, Vector2i w)
        {
            return v._x * w._x + v._y * w._y;
        }

        public int Cross(Vector2i v)
        {
            return _x * v._y - _y * v._x;
        }
    
        public static Vector2i Mul(Vector2i v, Vector2i w)
        {
            return new Vector2i(v.X * w.X, v.Y * w.Y);
        }

        #endregion

        #region static

        public static readonly Vector2i Zero = new Vector2i(0, 0);
        public static readonly Vector2i UnitX = new Vector2i(1, 0);
        public static readonly Vector2i UnitY = new Vector2i(0, 1);

        public static Vector2i Unit(Axis axis)
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

        public static double Angle(Vector2i v, Vector2i w)
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
