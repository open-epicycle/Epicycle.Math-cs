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

    // [### VectorN.cs.TEMPLATE> T = long, D = 4
    ﻿
    public sealed class Vector4L : IEquatable<Vector4L>
    {
        #region Constants

        public static readonly Vector4L Zero = new Vector4L(0, 0, 0, 0);
    
        public static readonly Vector4L UnitX = new Vector4L(1, 0, 0, 0);
    
        public static readonly Vector4L UnitY = new Vector4L(0, 1, 0, 0);
    
        public static readonly Vector4L UnitZ = new Vector4L(0, 0, 1, 0);
    
        public static readonly Vector4L UnitT = new Vector4L(0, 0, 0, 1);
    

        public static Vector4L Unit(Axis axis)
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
    
    
        private readonly long _x;
    
        private readonly long _y;
    
        private readonly long _z;
    
        private readonly long _t;
    
    
        #endregion
    
        #region Construction and conversion
    
        public Vector4L(long x, long y, long z, long t)
        {
            _x = x;
            _y = y;
            _z = z;
            _t = t;
        }

     
        public Vector4L(Vector3L xyz, long t = 0)
        {
            _x = xyz.X;
            _y = xyz.Y;
            _z = xyz.Z;
            _t = t;
        }
    

    
        public Vector4L(Vector4i v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }
    
        public Vector4L(Vector4L v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _t = v.T;
        }
    
        public Vector4L(Vector4f v)
        {
            _x = ((long)Math.Round(v.X));
            _y = ((long)Math.Round(v.Y));
            _z = ((long)Math.Round(v.Z));
            _t = ((long)Math.Round(v.T));
        }
    
        public Vector4L(Vector4 v)
        {
            _x = ((long)Math.Round(v.X));
            _y = ((long)Math.Round(v.Y));
            _z = ((long)Math.Round(v.Z));
            _t = ((long)Math.Round(v.T));
        }
    

        public Vector4L(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 4, "4");

            _x = ((long)Math.Round(v[0]));
            _y = ((long)Math.Round(v[1]));
            _z = ((long)Math.Round(v[2]));
            _t = ((long)Math.Round(v[3]));
        }

        public long[] ToArray()
        {
            return new long[] { _x, _y, _z, _t };
        }

    
    
        public static implicit operator Vector4L(Vector4i v)
        {
            return new Vector4L(v);
        }
    
    
    
    
    
        public static explicit operator Vector4L(Vector4f v)
        {
            return new Vector4L(v);
        }
    
    
    
        public static explicit operator Vector4L(Vector4 v)
        {
            return new Vector4L(v);
        }
    
    

    
        public static explicit operator Vector4L(Vector3L v)
        {
            return new Vector4L(v);
        }
    

        public static explicit operator Vector4L(OVector v)
        {
            return new Vector4L(v);
        }

        public static implicit operator OVector(Vector4L v)
        {
            return new Vector(v._x, v._y, v._z, v._t);
        }

        #endregion
    
        #region Properties
    
    
        public long X
        {
            get { return _x; }
        }
    
        public long Y
        {
            get { return _y; }
        }
    
        public long Z
        {
            get { return _z; }
        }
    
        public long T
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
    
    
        public Vector3L XYZ
        {
            get { return new Vector3L(_x, _y, _z); }
        }
    
    
        #endregion
    
        #region Equality & HashCode

        public bool Equals(Vector4L v)
        {
            return _x == v._x && _y == v._y && _z == v._z && _t == v._t;
        }

        public override bool Equals(object obj)
        {
        
            if(obj == null || !(obj is Vector4L))
            {
                return false;
            }

            return Equals((Vector4L) obj);
        
        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode() ^ _t.GetHashCode();
        }

        public static bool operator ==(Vector4L v, Vector4L w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector4L v, Vector4L w)
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

        public long Norm2
        {
            get { return (_x * _x) + (_y * _y) + (_z * _z) + (_t * _t); }
        }

        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static long Distance2(Vector4L v, Vector4L w)
        {
            return (v - w).Norm2;
        }

        public static double Distance(Vector4L v, Vector4L w)
        {
            return (v - w).Norm;
        }

    
    
        #endregion
    
        #region Algebra

        public static Vector4L operator +(Vector4L v)
        {
            return v;
        }

        public static Vector4L operator -(Vector4L v)
        {
            return new Vector4L(-v._x, -v._y, -v._z, -v._t);
        }

        public static Vector4L operator *(Vector4L v, long a)
        {
            return new Vector4L(v._x * a, v._y * a, v._z * a, v._t * a);
        }

        public static Vector4L operator *(long a, Vector4L v)
        {
            return v * a;
        }

        public static Vector4L operator /(Vector4L v, long a)
        {
            return new Vector4L(v._x / a, v._y / a, v._z / a, v._t / a);
        }

        public static Vector4L operator +(Vector4L v, Vector4L w)
        {
            return new Vector4L(v._x + w._x, v._y + w._y, v._z + w._z, v._t + w._t);
        }

        public static Vector4L operator -(Vector4L v, Vector4L w)
        {
            return new Vector4L(v._x - w._x, v._y - w._y, v._z - w._z, v._t - w._t);
        }

        public static long operator *(Vector4L v, Vector4L w)
        {
            return (v._x * w._x) + (v._y * w._y) + (v._z * w._z) + (v._t * w._t);
        }

    

        public static Vector4L Mul(Vector4L v, Vector4L w)
        {
            return new Vector4L(v._x * w._x, v._y * w._y, v._z * w._z, v._t * w._t);
        }

        public static Vector4L Div(Vector4L v, Vector4L w)
        {
            return new Vector4L(v._x / w._x, v._y / w._y, v._z / w._z, v._t / w._t);
        }
    
    

        #endregion
    }
    // ###]
}
