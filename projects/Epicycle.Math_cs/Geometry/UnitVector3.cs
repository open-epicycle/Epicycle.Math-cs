using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    using System;

    public sealed class UnitVector3
    {
        public UnitVector3(Vector3 v)
        {
            _vector = v.Normalized;
        }

        public UnitVector3(Rotation2 phi, double z)
        {
            var xy = Math.Sqrt(1 - z * z);

            if(double.IsNaN(xy))
            {
                if (z > 0)
                {
                    _vector = new Vector3(0, 0, 1);
                }
                else
                {
                    _vector = new Vector3(0, 0, -1);
                }
            }
            else
            {
                _vector = new Vector3(xy * phi.Cos, xy * phi.Sin, z);
            }            
        }

        private readonly Vector3 _vector;

        public static implicit operator Vector3(UnitVector3 unitVector)
        {
            return unitVector._vector;
        }

        public static explicit operator UnitVector3(Vector3 vector)
        {
            return new UnitVector3(vector);
        }

        public Vector3 Vector
        {
            get { return _vector; }
        }

        public double X
        {
            get { return _vector.X; }
        }

        public double Y
        {
            get { return _vector.Y; }
        }

        public double Z
        {
            get { return _vector.Z; }
        }

        public Vector2 XY
        {
            get { return _vector.XY; }
        }

        public double Theta
        {
            get { return BasicMath.Acos(Z); }
        }

        public double Phi
        {
            get { return Math.Atan2(Y, X); }
        }

        public override string ToString()
        {
            return string.Format("{0}°, {1}°", BasicMath.RadToDeg(Theta), BasicMath.RadToDeg(Phi));
        }
    }
}
