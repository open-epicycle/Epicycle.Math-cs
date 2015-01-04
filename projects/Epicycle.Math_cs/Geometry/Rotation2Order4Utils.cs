using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Rotation2Order4Utils
    {
        public static Rotation2Order4 Inv(this Rotation2Order4 @this)
        {
            switch (@this)
            {
            case Rotation2Order4.Rotation0:
                return Rotation2Order4.Rotation0;

            case Rotation2Order4.Rotation90:
                return Rotation2Order4.Rotation270;

            case Rotation2Order4.Rotation180:
                return Rotation2Order4.Rotation180;

            case Rotation2Order4.Rotation270:
                return Rotation2Order4.Rotation90;

            default:
                throw new InternalException("Illegal rotation!");
            }
        }

        public static Rotation2Order4 Compose(Rotation2Order4 x, Rotation2Order4 y)
        {
            return (Rotation2Order4)(((int)x + (int)y) % 4);
        }

        public static Vector2 Apply(this Rotation2Order4 @this, Vector2 v)
        {
            switch (@this)
            {
                case Rotation2Order4.Rotation0:
                    return v;

                case Rotation2Order4.Rotation90:
                    return new Vector2(-v.Y, v.X);

                case Rotation2Order4.Rotation180:
                    return -v;

                case Rotation2Order4.Rotation270:
                    return new Vector2(v.Y, -v.X);

                default:
                    throw new InternalException("Illegal rotation!");
            }
        }

        public static Vector2 ApplyInv(this Rotation2Order4 @this, Vector2 v)
        {
            switch (@this)
            {
                case Rotation2Order4.Rotation0:
                    return v;

                case Rotation2Order4.Rotation90:
                    return new Vector2(v.Y, -v.X);

                case Rotation2Order4.Rotation180:
                    return -v;

                case Rotation2Order4.Rotation270:
                    return new Vector2(-v.Y, v.X);

                default:
                    throw new InternalException("Illegal rotation!");
            }
        }

        public static double Angle(this Rotation2Order4 @this)
        {
            switch (@this)
            {
                case Rotation2Order4.Rotation0:
                    return 0;

                case Rotation2Order4.Rotation90:
                    return Math.PI / 2;

                case Rotation2Order4.Rotation180:
                    return Math.PI;

                case Rotation2Order4.Rotation270:
                    return -Math.PI / 2;

                default:
                    throw new InternalException("Illegal rotation!");
            }
        }
    }
}
