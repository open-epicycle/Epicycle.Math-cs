namespace Epicycle.Math.Geometry
{
    using System;

    public static class Line2Utils
    {
        public static double SignedDistanceTo(this Vector2 @this, Line2 line)
        {
            return @this * line.Normal + line.FreeTerm;
        }

        public static double DistanceTo(this Vector2 @this, Line2 line)
        {
            return Math.Abs(@this.SignedDistanceTo(line));
        }

        public static bool IsOnPositiveSide(this Vector2 @this, Line2 line)
        {
            return @this.SignedDistanceTo(line) > 0;
        }

        public static Vector2 ProjectOrigin(this Line2 @this)
        {
            return -@this.FreeTerm * @this.Normal;
        }

        public static Vector2 Project(this Line2 @this, Vector2 point)
        {
            var direction = @this.Direction;

            return @this.ProjectOrigin() + (direction * point) * direction;
        }

        public static Line2 Translate(this Line2 @this, Vector2 vector)
        {
            return new Line2(@this.FreeTerm - @this.Normal * vector, @this.Normal);
        }
    }
}