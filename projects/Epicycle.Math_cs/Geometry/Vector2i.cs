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

namespace Epicycle.Math.Geometry
{
    using System;

    public struct Vector2i : IEquatable<Vector2i>
    {
        public static readonly Vector2i Zero = new Vector2i(0, 0);
        public static readonly Vector2i UnitX = new Vector2i(1, 0);
        public static readonly Vector2i UnitY = new Vector2i(0, 1);

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

        #region constructors

        public Vector2i(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Vector2i(Vector2 v)
        {
            _x = (int)Math.Round(v.X);
            _y = (int)Math.Round(v.Y);
        }

        #endregion

        #region conversion operators

        public static explicit operator Vector2i(Vector2 v)
        {
            return new Vector2i(v);
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
        return X ^ Y;
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

        #region algebra

        public static Vector2i operator +(Vector2i v)
        {
            return v;
        }

        public static Vector2i operator -(Vector2i v)
        {
            return new Vector2i(-v.X, -v.Y);
        }

        public static Vector2i operator +(Vector2i v, Vector2i w)
        {
            return new Vector2i(v.X + w.X, v.Y + w.Y);
        }

        public static Vector2i operator -(Vector2i v, Vector2i w)
        {
            return new Vector2i(v.X - w.X, v.Y - w.Y);
        }

        public static Vector2i operator *(Vector2i v, int a)
        {
            return new Vector2i(v.X * a, v.Y * a);
        }

        public static Vector2i operator *(int a, Vector2i v)
        {
            return v * a;
        }

        public static Vector2i operator /(Vector2i v, int a)
        {
            return new Vector2i(v.X / a, v.Y / a);
        }

        public static int operator *(Vector2i v, Vector2i w)
        {
            return v._x * w._x + v._y * w._y;
        }

        public static Vector2i Mul(Vector2i v, Vector2i w)
        {
            return new Vector2i(v.X * w.X, v.Y * w.Y);
        }

        #endregion

        public int Norm2
        {
            get { return _x * _x + _y * _y; }
        }

        public double Norm
        {
            get { return Math.Sqrt(this.Norm2); }
        }
        
        public static int Distance2(Vector2i a, Vector2i b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;

            return dx * dx + dy * dy;
        }
        
        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}
