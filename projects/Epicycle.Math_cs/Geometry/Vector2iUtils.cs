namespace Epicycle.Math.Geometry
{
    public static class Vector2iUtils
    {
        public sealed class YamlSerialization
        {
            public int X { get; set; }
            public int Y { get; set; }

            public YamlSerialization() { }

            public YamlSerialization(Vector2i v)
            {
                X = v.X;
                Y = v.Y;
            }

            public Vector2i Deserialize()
            {
                return new Vector2i(X, Y);
            }
        }
    }
}
