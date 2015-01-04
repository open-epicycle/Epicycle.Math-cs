using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry
{
    public struct RotoTranslation2
    {
        public RotoTranslation2(Rotation2 rotation, Vector2 translation)
        {
            _rotation = rotation;
            _translation = translation;
        }

        public RotoTranslation2(Rotation2 rotation) : this(rotation, Vector2.Zero) { }

        public RotoTranslation2(Vector2 translation) : this(Rotation2.Id, translation) { }

        public static implicit operator RotoTranslation2(Rotation2 rotation)
        {
            return new RotoTranslation2(rotation);
        }

        // explicit since although conversion doesn't loose information it is not perfectly intuitive
        public static explicit operator RotoTranslation2(Vector2 translation)
        {
            return new RotoTranslation2(translation);
        }

        private readonly Rotation2 _rotation;
        private readonly Vector2 _translation;

        public static readonly RotoTranslation2 Id = new RotoTranslation2(Rotation2.Id, Vector2.Zero);

        public Rotation2 Rotation
        {
            get { return _rotation; }
        }

        public Vector2 Translation
        {
            get { return _translation; }
        }

        public RotoTranslation2 Inv
        {
            get { return new RotoTranslation2(Rotation.Inv, -Rotation.ApplyInv(Translation)); }
        }

        public Vector2 Apply(Vector2 point)
        {
            return Rotation.Apply(point) + Translation;
        }

        public Vector2 ApplyInv(Vector2 point)
        {
            return Rotation.ApplyInv(point - Translation);
        }

        public static RotoTranslation2 operator *(RotoTranslation2 x, RotoTranslation2 y)
        {
            return new RotoTranslation2(x.Rotation * y.Rotation, x.Apply(y.Translation));
        }
    }
}
