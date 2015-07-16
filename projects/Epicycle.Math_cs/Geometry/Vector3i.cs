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

    // [### VectorN.cs.TEMPLATE> T = int, D = 3
    ﻿

    public struct Vector3i : IEquatable<Vector3i>
    {
    
        public int X
        {
            get { return _x; }
        }
    
        public int Y
        {
            get { return _y; }
        }
    
        public int Z
        {
            get { return _z; }
        }
    
    
    
        private readonly int _x;
    
        private readonly int _y;
    
        private readonly int _z;
    

        public enum Axis
        {
            X = 0, 
            Y = 1, 
            Z = 2, 
            Count = 3 // used in for loops
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
                
                    case Axis.Z:
                        return _z;
                
                    default:
                        throw new IndexOutOfRangeException("Invalid 3D axis " + axis.ToString());
                }
            }
        }

        #region creation

        public Vector3i(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
    
     
        public Vector3i(Vector2i xy, int z = 0)
        {
            _x = xy.X;
            _y = xy.Y;
            _z = z;
        }
    
    

        public Vector3i(Vector3i v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }

        public Vector3i(Vector3L v)
        {
            _x = ((int)v.X);
            _y = ((int)v.Y);
            _z = ((int)v.Z);
        }

        public Vector3i(Vector3f v)
        {
            _x = ((int)Math.Round(v.X));
            _y = ((int)Math.Round(v.Y));
            _z = ((int)Math.Round(v.Z));
        }

        public Vector3i(Vector3 v)
        {
            _x = ((int)Math.Round(v.X));
            _y = ((int)Math.Round(v.Y));
            _z = ((int)Math.Round(v.Z));
        }


        public Vector3i(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 3, "3");

            _x = ((int)Math.Round(v[0]));
            _y = ((int)Math.Round(v[1]));
            _z = ((int)Math.Round(v[2]));
        }
    
        public int[] ToArray()
        {
            return new int[] { _x, _y, _z };
        }
    
    
    
    
    
        public static explicit operator Vector3i(Vector3L v)
        {
            return new Vector3i(v);
        }
    
    
    
        public static explicit operator Vector3i(Vector3f v)
        {
            return new Vector3i(v);
        }
    
    
    
        public static explicit operator Vector3i(Vector3 v)
        {
            return new Vector3i(v);
        }
    
    

    
        public static explicit operator Vector3i(Vector2i v)
        {
            return new Vector3i(v);
        }
    

        public static explicit operator Vector3i(OVector v)
        {
            return new Vector3i(v);
        }

        public static implicit operator OVector(Vector3i v)
        {
            return new Vector(v._x, v._y, v._z);
        }
    
        #endregion

        #region subvectors
    
    
        public Vector2i XY
        {
            get { return new Vector2i(X, Y); }
        }

        public Vector2i YZ
        {
            get { return new Vector2i(Y, Z); }
        }

        public Vector2i ZX
        {
            get { return new Vector2i(Z, X); }
        }
    
    
        #endregion
    
        #region equality

        public bool Equals(Vector3i v)
        {
            return _x == v._x && _y == v._y && _z == v._z;
        }

        public override bool Equals(object obj)
        {

            var v = obj as Vector3i?;

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
    
        public static bool operator ==(Vector3i v, Vector3i w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector3i v, Vector3i w)
        {
            return !v.Equals(w);
        }

        #endregion

        #region norm

        public int Norm2
        {
            get { return (_x * _x) + (_y * _y) + (_z * _z); }
        }
    
        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static int Distance2(Vector3i v, Vector3i w)
        {
            return (v - w).Norm2;
        }
    
        public static double Distance(Vector3i v, Vector3i w)
        {
            return (v - w).Norm;
        }
    


        #endregion

        #region algebra

        public static Vector3i operator +(Vector3i v)
        {
            return v;
        }

        public static Vector3i operator -(Vector3i v)
        {
            return new Vector3i(-v._x, -v._y, -v._z);
        }

        public static Vector3i operator *(Vector3i v, int a)
        {
            return new Vector3i(v._x * a, v._y * a, v._z * a);
        }

        public static Vector3i operator *(int a, Vector3i v)
        {
            return v * a;
        }

        public static Vector3i operator /(Vector3i v, int a)
        {
            return new Vector3i(v._x / a, v._y / a, v._z / a);
        }

        public static Vector3i operator +(Vector3i v, Vector3i w)
        {
            return new Vector3i(v._x + w._x, v._y + w._y, v._z + w._z);
        }

        public static Vector3i operator -(Vector3i v, Vector3i w)
        {
            return new Vector3i(v._x - w._x, v._y - w._y, v._z - w._z);
        }

        public static int operator *(Vector3i v, Vector3i w)
        {
            return (v._x * w._x) + (v._y * w._y) + (v._z * w._z);
        }

    
        public Vector3i Cross(Vector3i v)
        {
            return new Vector3i(_y * v._z - v._y * _z, _z * v._x - v._z * _x, _x * v._y - v._x * _y);
        }
    
    
        public static Vector3i Mul(Vector3i v, Vector3i w)
        {
            return new Vector3i(v._x * w._x, v._y * w._y, v._z * w._z);
        }
    
        public static Vector3i Div(Vector3i v, Vector3i w)
        {
            return new Vector3i(v._x / w._x, v._y / w._y, v._z / w._z);
        }

        #endregion

        #region static

        public static readonly Vector3i Zero = new Vector3i(0, 0, 0);
    
        public static readonly Vector3i UnitX = new Vector3i(1, 0, 0);
    
        public static readonly Vector3i UnitY = new Vector3i(0, 1, 0);
    
        public static readonly Vector3i UnitZ = new Vector3i(0, 0, 1);
    
    
        public static Vector3i Unit(Axis axis)
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

    
        public static double Angle(Vector3i v, Vector3i w)
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
