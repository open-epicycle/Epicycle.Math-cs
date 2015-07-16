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

    // [### VectorN.cs.TEMPLATE> T = long, D = 3
    ﻿

    public sealed class Vector3L : IEquatable<Vector3L>
    {
    
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
    
    
    
        private readonly long _x;
    
        private readonly long _y;
    
        private readonly long _z;
    

        public enum Axis
        {
            X = 0, 
            Y = 1, 
            Z = 2, 
            Count = 3 // used in for loops
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
                
                    default:
                        throw new IndexOutOfRangeException("Invalid 3D axis " + axis.ToString());
                }
            }
        }

        #region creation

        public Vector3L(long x, long y, long z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
    
     
        public Vector3L(Vector2L xy, long z = 0)
        {
            _x = xy.X;
            _y = xy.Y;
            _z = z;
        }
    
    

        public Vector3L(Vector3i v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }

        public Vector3L(Vector3L v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }

        public Vector3L(Vector3f v)
        {
            _x = ((long)Math.Round(v.X));
            _y = ((long)Math.Round(v.Y));
            _z = ((long)Math.Round(v.Z));
        }

        public Vector3L(Vector3 v)
        {
            _x = ((long)Math.Round(v.X));
            _y = ((long)Math.Round(v.Y));
            _z = ((long)Math.Round(v.Z));
        }


        public Vector3L(OVector v)
        {
            ArgAssert.Equal(v.Dimension, "v.Dimension", 3, "3");

            _x = ((long)Math.Round(v[0]));
            _y = ((long)Math.Round(v[1]));
            _z = ((long)Math.Round(v[2]));
        }
    
        public long[] ToArray()
        {
            return new long[] { _x, _y, _z };
        }
    
    
    
        public static implicit operator Vector3L(Vector3i v)
        {
            return new Vector3L(v);
        }
    
    
    
    
    
        public static explicit operator Vector3L(Vector3f v)
        {
            return new Vector3L(v);
        }
    
    
    
        public static explicit operator Vector3L(Vector3 v)
        {
            return new Vector3L(v);
        }
    
    

    
        public static explicit operator Vector3L(Vector2L v)
        {
            return new Vector3L(v);
        }
    

        public static explicit operator Vector3L(OVector v)
        {
            return new Vector3L(v);
        }

        public static implicit operator OVector(Vector3L v)
        {
            return new Vector(v._x, v._y, v._z);
        }
    
        #endregion

        #region subvectors
    
    
        public Vector2L XY
        {
            get { return new Vector2L(X, Y); }
        }

        public Vector2L YZ
        {
            get { return new Vector2L(Y, Z); }
        }

        public Vector2L ZX
        {
            get { return new Vector2L(Z, X); }
        }
    
    
        #endregion
    
        #region equality

        public bool Equals(Vector3L v)
        {
            return _x == v._x && _y == v._y && _z == v._z;
        }

        public override bool Equals(object obj)
        {

            if(obj == null || !(obj is Vector3L))
            {
                return false;
            }
        
            return Equals((Vector3L) obj);

        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode();
        }
    
        public static bool operator ==(Vector3L v, Vector3L w)
        {
            return v.Equals(w);
        }

        public static bool operator !=(Vector3L v, Vector3L w)
        {
            return !v.Equals(w);
        }

        #endregion

        #region norm

        public long Norm2
        {
            get { return (_x * _x) + (_y * _y) + (_z * _z); }
        }
    
        public double Norm
        {
            get { return Math.Sqrt(Norm2); }
        }

        public static long Distance2(Vector3L v, Vector3L w)
        {
            return (v - w).Norm2;
        }
    
        public static double Distance(Vector3L v, Vector3L w)
        {
            return (v - w).Norm;
        }
    


        #endregion

        #region algebra

        public static Vector3L operator +(Vector3L v)
        {
            return v;
        }

        public static Vector3L operator -(Vector3L v)
        {
            return new Vector3L(-v._x, -v._y, -v._z);
        }

        public static Vector3L operator *(Vector3L v, long a)
        {
            return new Vector3L(v._x * a, v._y * a, v._z * a);
        }

        public static Vector3L operator *(long a, Vector3L v)
        {
            return v * a;
        }

        public static Vector3L operator /(Vector3L v, long a)
        {
            return new Vector3L(v._x / a, v._y / a, v._z / a);
        }

        public static Vector3L operator +(Vector3L v, Vector3L w)
        {
            return new Vector3L(v._x + w._x, v._y + w._y, v._z + w._z);
        }

        public static Vector3L operator -(Vector3L v, Vector3L w)
        {
            return new Vector3L(v._x - w._x, v._y - w._y, v._z - w._z);
        }

        public static long operator *(Vector3L v, Vector3L w)
        {
            return (v._x * w._x) + (v._y * w._y) + (v._z * w._z);
        }

    
        public Vector3L Cross(Vector3L v)
        {
            return new Vector3L(_y * v._z - v._y * _z, _z * v._x - v._z * _x, _x * v._y - v._x * _y);
        }
    
    
        public static Vector3L Mul(Vector3L v, Vector3L w)
        {
            return new Vector3L(v._x * w._x, v._y * w._y, v._z * w._z);
        }
    
        public static Vector3L Div(Vector3L v, Vector3L w)
        {
            return new Vector3L(v._x / w._x, v._y / w._y, v._z / w._z);
        }

        #endregion

        #region static

        public static readonly Vector3L Zero = new Vector3L(0, 0, 0);
    
        public static readonly Vector3L UnitX = new Vector3L(1, 0, 0);
    
        public static readonly Vector3L UnitY = new Vector3L(0, 1, 0);
    
        public static readonly Vector3L UnitZ = new Vector3L(0, 0, 1);
    
    
        public static Vector3L Unit(Axis axis)
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

    
        public static double Angle(Vector3L v, Vector3L w)
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
