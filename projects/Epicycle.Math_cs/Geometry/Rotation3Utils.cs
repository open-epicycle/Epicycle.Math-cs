using Epicycle.Commons;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry
{
    public static class Rotation3Utils
    {
        public static readonly Rotation3 RotationX180 = (Rotation3)new Quaternion(0, 1, 0, 0);
        public static readonly Rotation3 RotationY180 = (Rotation3)new Quaternion(0, 0, 1, 0);
        public static readonly Rotation3 RotationZ180 = (Rotation3)new Quaternion(0, 0, 0, 1);
        public static readonly Rotation3 RotationX90 = (Rotation3)new Quaternion(BasicMath.Sqrt2, BasicMath.Sqrt2, 0, 0);
        public static readonly Rotation3 RotationY90 = (Rotation3)new Quaternion(BasicMath.Sqrt2, 0, BasicMath.Sqrt2, 0);
        public static readonly Rotation3 RotationZ90 = (Rotation3)new Quaternion(BasicMath.Sqrt2, 0, 0, BasicMath.Sqrt2);
        public static readonly Rotation3 RotationX270 = (Rotation3)new Quaternion(BasicMath.Sqrt2, -BasicMath.Sqrt2, 0, 0);
        public static readonly Rotation3 RotationY270 = (Rotation3)new Quaternion(BasicMath.Sqrt2, 0, -BasicMath.Sqrt2, 0);
        public static readonly Rotation3 RotationZ270 = (Rotation3)new Quaternion(BasicMath.Sqrt2, 0, 0, -BasicMath.Sqrt2);

        public static OSquareMatrix Matrix(this Rotation3 @this)
        {
            return new SquareMatrix(@this.ColX(), @this.ColY(), @this.ColZ());
        }

        public static OSquareMatrix InvMatrix(this Rotation3 @this)
        {
            return new SquareMatrix(@this.RowX(), @this.RowY(), @this.RowZ());
        }

        #region optimization

        public static Vector3 ColX(this Rotation3 @this)
        {
            return QuaternionUtils.ProductIm(@this.Quaternion.TimesI(), @this.Quaternion.Conjugate);
        }

        public static Vector3 ColY(this Rotation3 @this)
        {
            return QuaternionUtils.ProductIm(@this.Quaternion.TimesJ(), @this.Quaternion.Conjugate);
        }

        public static Vector3 ColZ(this Rotation3 @this)
        {
            return QuaternionUtils.ProductIm(@this.Quaternion.TimesK(), @this.Quaternion.Conjugate);
        }

        public static Vector3 RowX(this Rotation3 @this)
        {
            return QuaternionUtils.ProductIm(@this.Quaternion.Conjugate.TimesI(), @this.Quaternion);
        }

        public static Vector3 RowY(this Rotation3 @this)
        {
            return QuaternionUtils.ProductIm(@this.Quaternion.Conjugate.TimesJ(), @this.Quaternion);
        }

        public static Vector3 RowZ(this Rotation3 @this)
        {
            return QuaternionUtils.ProductIm(@this.Quaternion.Conjugate.TimesK(), @this.Quaternion);
        }

        public static double ProductX(Rotation3 r, Vector3 v)
        {
            return QuaternionUtils.ProductB(r.Quaternion * v, r.Quaternion.Conjugate);
        }

        public static double ProductY(Rotation3 r, Vector3 v)
        {
            return QuaternionUtils.ProductC(r.Quaternion * v, r.Quaternion.Conjugate);
        }

        public static double ProductZ(Rotation3 r, Vector3 v)
        {
            return QuaternionUtils.ProductD(r.Quaternion * v, r.Quaternion.Conjugate);
        }

        public static Vector2 ProductXY(Rotation3 r, Vector3 v)
        {
            var q1 = r.Quaternion * v;
            var q2 = r.Quaternion.Conjugate;

            var x = QuaternionUtils.ProductB(q1, q2);
            var y = QuaternionUtils.ProductC(q1, q2);

            return new Vector2(x, y);
        }

        public static double InvProductX(Rotation3 r, Vector3 v)
        {
            return QuaternionUtils.ProductB(r.Quaternion.Conjugate * v, r.Quaternion);
        }

        public static double InvProductY(Rotation3 r, Vector3 v)
        {
            return QuaternionUtils.ProductC(r.Quaternion.Conjugate * v, r.Quaternion);
        }

        public static double InvProductZ(Rotation3 r, Vector3 v)
        {
            return QuaternionUtils.ProductD(r.Quaternion.Conjugate * v, r.Quaternion);
        }

        public static Vector2 InvProductXY(Rotation3 r, Vector3 v)
        {
            var q1 = r.Quaternion.Conjugate * v;
            var q2 = r.Quaternion;

            var x = QuaternionUtils.ProductB(q1, q2);
            var y = QuaternionUtils.ProductC(q1, q2);

            return new Vector2(x, y);
        }

        #endregion

        public static Rotation3 MinimalRotation(Vector3 x, Vector3 y)
        {
            var axis = x.Cross(y);
            var angle = Vector3.Angle(x, y);

            return new Rotation3(angle, axis);
        }

        public static Rotation3 GramSchmidt(Vector3 x, Vector3 y)
        {
            x = x.Normalized;
            y = (y - (x * y) * x).Normalized;
            var z = x.Cross(y);

            var cosAngle = (x.X + y.Y + z.Z - 1) / 2;

            var angle = BasicMath.Acos(cosAngle);

            var axis0 = new Vector3(y.Z - z.Y, z.X - x.Z, x.Y - y.X); // vanishes at angle pi

            if (cosAngle > -0.9) // optimization
            {
                return new Rotation3(angle, axis0);
            }

            var xy = x.Y + y.X;
            var xz = x.Z + z.X;
            var yz = y.Z + z.Y;

            var axis1 = new Vector3(2 * (x.X - cosAngle), xy, xz);
            var axis2 = new Vector3(xy, 2 * (y.Y - cosAngle), yz);
            var axis3 = new Vector3(xz, yz, 2 * (z.Z - cosAngle));

            var norm1 = axis1.Norm2;
            var norm2 = axis2.Norm2;
            var norm3 = axis3.Norm2;

            Vector3 axis;

            if (norm1 > norm2 && norm1 > norm3)
            {
                axis = (axis1 * axis0 > 0 ? axis1 : -axis1);
            }
            else if (norm2 > norm1 && norm2 > norm3)
            {
                axis = (axis2 * axis0 > 0 ? axis2 : -axis2);
            }
            else
            {
                axis = (axis3 * axis0 > 0 ? axis3 : -axis3);
            }

            return new Rotation3(angle, axis);
        }

        public sealed class YamlSerialization
        {
            public Vector3Utils.YamlSerialization Axis { get; set; }
            public double Angle { get; set; }

            public YamlSerialization() { }

            public YamlSerialization(Rotation3 r)
            {
                Axis = new Vector3Utils.YamlSerialization(r.Axis);
                Angle = r.Angle;
            }

            public Rotation3 Deserialize()
            {
                return new Rotation3(Angle, Axis.Deserialize());
            }
        }
    }
}
