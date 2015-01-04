using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    public static class Matrix3Utils
    {
        public static Vector2 ApplyProjective(this Matrix3 @this, Vector2 v)
        {
            var x = @this.XX * v.X + @this.XY * v.Y + @this.XZ;
            var y = @this.YX * v.X + @this.YY * v.Y + @this.YZ;
            var z = @this.ZX * v.X + @this.ZY * v.Y + @this.ZZ;

            return new Vector2(x / z, y / z);
        }

        public static Matrix3 ComputeTranslationMatrix(Vector2 translation)
        {
            return new Matrix3(1, 0, translation.X, 0, 1, translation.Y, 0, 0, 1);
        }

        public static Matrix3 FitProjective(Box2 source, IReadOnlyList<Vector2> target)
        {
            ArgAssert.Equal(target.Count, "target.Count", 4);

            var a = source.Width;
            var b = source.Height;

            var u = target[1] - target[0];
            var v = target[3] - target[0];
            var w = target[2] - target[0];

            var m11 = w.X - u.X;
            var m12 = w.X - v.X;
            var m21 = w.Y - u.Y;
            var m22 = w.Y - v.Y;

            var det = m11 * m22 - m12 * m21;

            var t1 = (+m22 * w.X - m12 * w.Y) / det;
            var t2 = (-m21 * w.X + m11 * w.Y) / det;

            var mat = new Matrix3(t1 * u.X / a, t2 * v.X / b, 0, t1 * u.Y / a, t2 * v.Y / b, 0, (t1 - 1) / a, (t2 - 1) / b, 1);

            return ComputeTranslationMatrix(target[0]) * mat * ComputeTranslationMatrix(-source.MinCorner);
        }
    }
}
