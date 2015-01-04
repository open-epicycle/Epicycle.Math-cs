using System.Collections.Generic;
using System.Linq;

#if !__ANDROID__
using System.Drawing;
#endif

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

        #if !__ANDROID__
        public Vector2i(Point p)
        {
        _x = p.X;
        _y = p.Y;
        }

        public Vector2i(PointF p)
        {
        _x = (int)Math.Round(p.X);
        _y = (int)Math.Round(p.Y);
        }
        #endif

        #endregion

        #region conversion operators

        public static explicit operator Vector2i(Vector2 v)
        {
            return new Vector2i(v);
        }

        #if !__ANDROID__
        public static implicit operator Vector2i(Point p)
        {
        return new Vector2i(p);
        }

        public static explicit operator Vector2i(PointF p)
        {
        return new Vector2i(p);
        }

        public static implicit operator Point(Vector2i v)
        {
        return new Point(v.X, v.Y);
        }

        public static implicit operator PointF(Vector2i v)
        {
        return new PointF(v.X, v.Y);
        }
        #endif

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
