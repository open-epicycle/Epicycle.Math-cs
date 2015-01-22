using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;
using Epicycle.Math.Geometry;

namespace Epicycle.Math.Calculus
{
    using System;

    public static class Interpolation
    {
        public static double Linear(double t, double t1, double p1, double t2, double p2)
        {
            return ((t2 - t) * p1 + (t - t1) * p2) / (t2 - t1);
        }

        public static Vector2 Linear(double t, double t1, Vector2 p1, double t2, Vector2 p2)
        {
            return ((t2 - t) * p1 + (t - t1) * p2) / (t2 - t1);
        }

        public static Vector3 Linear(double t, double t1, Vector3 p1, double t2, Vector3 p2)
        {
            return ((t2 - t) * p1 + (t - t1) * p2) / (t2 - t1);
        }

        public static Rotation3 SphericalLinear(double t, double t1, Rotation3 r1, double t2, Rotation3 r2)
        {
            var q1 = r1.Quaternion;
            var q2 = r2.Quaternion;

            var dot = Quaternion.Dot(q1, q2);

            if (Quaternion.Dot(q1, q2) < 0)
            {
                q2 = -q2;
                dot = -dot;
            }

            var dt = t2 - t1;
            var angle = BasicMath.Acos(dot);

            Quaternion q;

            if (angle > BasicMath.Epsilon)
            {
                q = Math.Sin(angle * (t2 - t) / dt) * q1 + Math.Sin(angle * (t - t1) / dt) * q2;                
            }
            else
            {
                q = ((t2 - t) * q1 + (t - t1) * q2) / dt;
            }
            

            return (Rotation3)q;
        }
    }
}
