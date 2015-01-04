using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Commons;
using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.Geometry
{
    using System;

    public struct Rotation3 : IManifoldPoint
    {
        #region construction and conversion

        public Rotation3(Quaternion q)
        {
            _quaternion = q.Normalized;
        }

        public Rotation3(double angle, Vector3 axis)
        {
            _quaternion = new Quaternion(Math.Cos(angle / 2), Math.Sin(angle / 2) * axis.Normalized);
        }

        public Rotation3(Vector3 rotationVector) : this(rotationVector.Norm, rotationVector) { }

        public static explicit operator Rotation3(Quaternion q)
        {
            return new Rotation3(q);
        }

        public static implicit operator Quaternion(Rotation3 r)
        {
            return r.Quaternion;
        }

        #endregion

        private readonly Quaternion _quaternion;

        public static readonly Rotation3 Id = new Rotation3(Quaternion.One);

        #region properties

        public Quaternion Quaternion
        {
            get { return _quaternion; }
        }

        public double Angle
        {
            get 
            { 
                var acos = Math.Acos(Math.Abs(_quaternion.Re));
                return double.IsNaN(acos) ? 0 : 2 * acos;
            }
        }

        public Vector3 Axis
        {
            get { return BasicMath.Sign(_quaternion.Re) * _quaternion.Im.Normalized; }
        }

        public Vector3 Vector
        {
            get { return Angle * Axis; }
        }

        public Rotation3 Inv
        {
            get { return new Rotation3(_quaternion.Conjugate); }
        }

        #endregion

        public Vector3 Apply(Vector3 v)
        {
            return QuaternionUtils.ProductIm(_quaternion * v, _quaternion.Conjugate);
        }

        public Vector3 ApplyInv(Vector3 v)
        {
            return QuaternionUtils.ProductIm(_quaternion.Conjugate * v, _quaternion);
        }        

        public static Rotation3 operator *(Rotation3 r, Rotation3 q)
        {
            return new Rotation3(r.Quaternion * q.Quaternion);
        }

        public static Vector3 operator *(Rotation3 r, Vector3 v)
        {
            return r.Apply(v);
        }

        int IManifoldPoint.Dimension
        {
            get { return 3; }
        }

        public override string ToString()
        {
            return _quaternion.ToString();
        }
    }
}
