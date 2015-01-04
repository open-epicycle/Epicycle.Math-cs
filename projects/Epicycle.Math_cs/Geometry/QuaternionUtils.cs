using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry
{
    public static class QuaternionUtils
    {
        public static Quaternion TimesI(this Quaternion @this)
        {
            return new Quaternion(-@this.B, @this.A, @this.D, -@this.C);
        }

        public static Quaternion ITimes(this Quaternion @this)
        {
            return new Quaternion(-@this.B, @this.A, -@this.D, @this.C);
        }

        public static Quaternion TimesJ(this Quaternion @this)
        {
            return new Quaternion(-@this.C, -@this.D, @this.A, @this.B);
        }

        public static Quaternion JTimes(this Quaternion @this)
        {
            return new Quaternion(-@this.C, @this.D, @this.A, -@this.B);
        }

        public static Quaternion TimesK(this Quaternion @this)
        {
            return new Quaternion(-@this.D, @this.C, -@this.B, @this.A);
        }

        public static Quaternion KTimes(this Quaternion @this)
        {
            return new Quaternion(-@this.D, -@this.C, @this.B, @this.A);
        }

        public static double ProductA(Quaternion q1, Quaternion q2)
        {
            return q1.A * q2.A - q1.B * q2.B - q1.C * q2.C - q1.D * q2.D;
        }

        public static double ProductB(Quaternion q1, Quaternion q2)
        {
            return q1.A * q2.B + q1.B * q2.A + q1.C * q2.D - q1.D * q2.C;
        }

        public static double ProductC(Quaternion q1, Quaternion q2)
        {
            return q1.A * q2.C - q1.B * q2.D + q1.C * q2.A + q1.D * q2.B;
        }

        public static double ProductD(Quaternion q1, Quaternion q2)
        {
            return q1.A * q2.D + q1.B * q2.C - q1.C * q2.B + q1.D * q2.A;
        }

        public static double ProductRe(Quaternion q1, Quaternion q2)
        {
            return ProductA(q1, q2);
        }

        public static Vector3 ProductIm(Quaternion q1, Quaternion q2)
        {
            return new Vector3(ProductB(q1, q2), ProductC(q1, q2), ProductD(q1, q2));
        }
    }
}
