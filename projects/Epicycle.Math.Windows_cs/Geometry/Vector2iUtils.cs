using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Epicycle.Math.Geometry.Windows
{
    using System;

    public static class Vector2iUtils
    {
        public static Vector2i ToVector2i(this Point p)
        {
            return new Vector2i(p.X, p.Y);
        }

        public static Vector2i ToVector2i(this PointF p)
        {
            return new Vector2i((int)Math.Round(p.X), (int)Math.Round(p.Y));
        }

        public static Point ToDrawingPoint(this Vector2i v)
        {
            return new Point(v.X, v.Y);
        }

        public static PointF ToDrawingPointF(this Vector2i v)
        {
            return new PointF(v.X, v.Y);
        }
    }
}
