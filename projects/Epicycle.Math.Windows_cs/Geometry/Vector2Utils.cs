using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Epicycle.Math.Geometry.Windows
{
    using System;

    public static class Vector2Utils
    {
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static Vector2 ToVector2(this PointF p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static Point ToDrawingPoint(this Vector2 v)
        {
            return new Point((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }

        public static PointF ToDrawingPointF(this Vector2 v)
        {
            return new PointF((float)v.X, (float)v.Y);
        }
    }
}
