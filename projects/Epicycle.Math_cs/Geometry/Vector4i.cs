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

    // [### VectorN.cs.TEMPLATE> T = int, D = 4
    ﻿
    public struct Vector4i : IEquatable<Vector4i>
    {
        #region Constants

        public static readonly Vector4i Zero = new Vector4i(0, 0, 0, 0);
    
        public static readonly Vector4i UnitX = new Vector4i(1, 0, 0, 0);
    
        public static readonly Vector4i UnitY = new Vector4i(0, 1, 0, 0);
    
        public static readonly Vector4i UnitZ = new Vector4i(0, 0, 1, 0);
    
        public static readonly Vector4i UnitT = new Vector4i(0, 0, 0, 1);
    

        public static Vector4i Unit(Axis axis)
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
    
    
        private readonly int _x;
    
        private readonly int _y;
    
        private readonly int _z;
    
        private readonly int _t;
    
    
        #endregion
    
        #region Construction and conversion
    
        public Vector4i(int x, int y, int z, int t)
        {
            _x = x;
            _y = y;
            _z = z;
            _t = t;
        }

     
        public Vector4i(Vector3i xyz, int t = 0)
        {
            _x = xyz.X;
            _y = xyz.Y;
            _z = xyz.Z;
            _t = t;
        }
    

    
        public Vector4i(Vector4i v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }
    
        public Vector4i(Vector4L v)
        {
            _x = ((int)v.X);
            _y = ((int)v.Y);
            _z = ((int)v.Z);
            _t = ((int)v.T);
        }
    
        public Vector4i(Vector4f v)
        {
            _x = ((int)Math.Round(v.X));
            _y = ((int)Math.Round(v.Y));
            _z = ((int)Math.Round(v.Z));
            _t = ((int)Math.Round(v.T));
        }
    
        public Vector4i(Vector4 v)
        {
            _x = ((int)Math.Round(v.X));
            _y = ((int)Math.Round(v.Y));
            _z = ((int)Math.Round(v.Z));
            _t = ((int)Math.Round(v.T));
        }
    

        public Vector4i(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 4, "4");

            _x = ((int)Math.Round(v[0]));
            _y = ((int)Math.Round(v[1]));
            _z = ((int)Math.Round(v[2]));
            _t = ((int)Math.Round(v[3]));
        }

        public int[] ToArray()
        {
            return new int[] { _x, _y, _z, _t };
        }

    
    
    
    
        public static explicit operator Vector4i(Vector4L v)
        {
            return new Vector4i(v);
        }
    
    
    
        public static explicit operator Vector4i(Vector4f v)
        {
            return new Vector4i(v);
        }
    
    
    
        public static explicit operator Vector4i(Vector4 v)
        {
            return new Vector4i(v);
        }
    
    

    
        public static explicit operator Vector4i(Vector3i v)
        {
            return new Vector4i(v);
        }
    

        public static explicit operator Vector4i(OVector v)
        {
            return new Vector4i(v);
        }

        public static implicit operator OVector(Vector4i v)
        {
            return new Vector(v._x, v._y, v._z, v._t);
        }

        #endregion
    
        #region Properties
    
    
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
    
        public int T
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
                
                    case Axis.T:
                        return _t;
                
                    default:
                        throw new IndexOutOfRangeException("Invalid 4D axis " + axis.ToString());
                }
            }
        }
    
        #endregion
    
        #region Sub-vectors
    
    
        public Vector3i XYZ
        {
            get { return new Vector3i(_x, _y, _z); }
        }
    
    
        #endregion
    
        #region Equality & HashCode

        public bool Equals(Vector4i v)
        {
            return _x == v._x && _y == v._y && _z == v._z && _t == v._t;
        }

        public override bool Equals(object obj)
        {
        
            var v = obj as Vector4i?;

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

        public static bool operator ==(Vector4i v, Vector4i w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector4i v, Vector4i w)
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

        public int Norm2
        {
            get { return (_x * _x) + (_y * _y) + (_z * _z) + (_t * _t); }
        }

        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static int Distance2(Vector4i v, Vector4i w)
        {
            return (v - w).Norm2;
        }

        public static double Distance(Vector4i v, Vector4i w)
        {
            return (v - w).Norm;
        }

    
    
        #endregion
    
        #region Algebra

        public static Vector4i operator +(Vector4i v)
        {
            return v;
        }

        public static Vector4i operator -(Vector4i v)
        {
            return new Vector4i(-v._x, -v._y, -v._z, -v._t);
        }

        public static Vector4i operator *(Vector4i v, int a)
        {
            return new Vector4i(v._x * a, v._y * a, v._z * a, v._t * a);
        }

        public static Vector4i operator *(int a, Vector4i v)
        {
            return v * a;
        }

        public static Vector4i operator /(Vector4i v, int a)
        {
            return new Vector4i(v._x / a, v._y / a, v._z / a, v._t / a);
        }

        public static Vector4i operator +(Vector4i v, Vector4i w)
        {
            return new Vector4i(v._x + w._x, v._y + w._y, v._z + w._z, v._t + w._t);
        }

        public static Vector4i operator -(Vector4i v, Vector4i w)
        {
            return new Vector4i(v._x - w._x, v._y - w._y, v._z - w._z, v._t - w._t);
        }

        public static int operator *(Vector4i v, Vector4i w)
        {
            return (v._x * w._x) + (v._y * w._y) + (v._z * w._z) + (v._t * w._t);
        }

    

        public static Vector4i Mul(Vector4i v, Vector4i w)
        {
            return new Vector4i(v._x * w._x, v._y * w._y, v._z * w._z, v._t * w._t);
        }

        public static Vector4i Div(Vector4i v, Vector4i w)
        {
            return new Vector4i(v._x / w._x, v._y / w._y, v._z / w._z, v._t / w._t);
        }
    
    

        #endregion
    }
    // ###]
}
