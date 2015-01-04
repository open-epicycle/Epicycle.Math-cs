using Epicycle.Math.Geometry.Polytopes;

namespace Epicycle.Math.Geometry
{
    public static class RotoTranslation3Utils
    {
        public static Ray3 Apply(this RotoTranslation3 @this, Ray3 ray)
        {
            return new Ray3(@this.Apply(ray.Origin), @this.Rotation.Apply(ray.Direction));
        }

        public static Ray3 ApplyInv(this RotoTranslation3 @this, Ray3 ray)
        {
            return new Ray3(@this.ApplyInv(ray.Origin), @this.Rotation.ApplyInv(ray.Direction));
        }

        public static Segment3 Apply(this RotoTranslation3 @this, Segment3 segment)
        {
            return new Segment3(@this.Apply(segment.Start), @this.Apply(segment.End));
        }

        public static Segment3 ApplyInv(this RotoTranslation3 @this, Segment3 segment)
        {
            return new Segment3(@this.ApplyInv(segment.Start), @this.ApplyInv(segment.End));
        }

        public static Plane Apply(this RotoTranslation3 @this, Plane plane)
        {
            var normal = @this.Rotation.Apply(plane.Normal);
            var freeTerm = plane.FreeTerm - normal * @this.Translation;

            return new Plane(freeTerm, normal);
        }

        public static Plane ApplyInv(this RotoTranslation3 @this, Plane plane)
        {
            var normal = @this.Rotation.ApplyInv(plane.Normal);
            var freeTerm = plane.FreeTerm + plane.Normal * @this.Translation;

            return new Plane(freeTerm, normal);
        }

        public static Plane ApplyToPlaneXY(this RotoTranslation3 @this)
        {
            var normal = @this.Rotation.ColZ();
            var freeTerm = -normal * @this.Translation;

            return new Plane(freeTerm, normal);
        }

        public static Plane ApplyInvToPlaneXY(this RotoTranslation3 @this)
        {
            var normal = @this.Rotation.RowZ();
            var freeTerm = @this.Translation.Z;

            return new Plane(freeTerm, normal);
        }

        public static IPolygon3 Apply(this RotoTranslation3 @this, IPolygon3 polygon)
        {
            return new Polygon3(@this * polygon.CoordinateSystem, polygon.InPlane);
        }

        public static IPolygon3 ApplyInv(this RotoTranslation3 @this, IPolygon3 polygon)
        {
            return new Polygon3(@this.Inv * polygon.CoordinateSystem, polygon.InPlane);
        }

        // creates an arbitrary Caresian coordinate system with given plane xy
        public static RotoTranslation3 CoordinateSystemWithPlaneXY(Plane plane)
        {
            var x = Vector3Utils.VectorOrthogonalTo(plane.Normal);
            var y = plane.Normal.Cross(x);

            return new RotoTranslation3(Rotation3Utils.GramSchmidt(x, y), plane.ProjectOrigin());
        }

        public sealed class YamlSerialization
        {
            public Rotation3Utils.YamlSerialization Rotation { get; set; }
            public Vector3Utils.YamlSerialization Translation { get; set; }

            public YamlSerialization() { }

            public YamlSerialization(RotoTranslation3 rotoTranslation3)
            {
                Rotation = new Rotation3Utils.YamlSerialization(rotoTranslation3.Rotation);
                Translation = new Vector3Utils.YamlSerialization(rotoTranslation3.Translation);
            }

            public RotoTranslation3 Deserialize()
            {
                return new RotoTranslation3(Rotation.Deserialize(), Translation.Deserialize());
            }
        }
    }
}
