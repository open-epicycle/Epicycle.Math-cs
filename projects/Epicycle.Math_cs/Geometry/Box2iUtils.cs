namespace Epicycle.Math.Geometry
{
    public static class Box2iUtils
    {
        public enum BoxFitMode
        {
            PreserveRatio,
            Stretch,
        }

        public static Box2i FitBox(Vector2i area, Vector2i areaToFit, BoxFitMode fitMode)
        {
            if ((area.X <= 0) || (area.Y <= 0))
            {
                return Box2i.Empty;
            }

            var boxDimensions = FindBoxDimensions(area, areaToFit, fitMode);
            var offset = (area - boxDimensions) / 2;

            return new Box2i(offset, boxDimensions);
        }

        private static Vector2i FindBoxDimensions(Vector2i area, Vector2i areaToFit, BoxFitMode fitMode)
        {
            var boxWidth = area.X;
            var boxHeight = area.Y;

            if ((areaToFit.X > 0) && (areaToFit.Y > 0) && (fitMode != BoxFitMode.Stretch))
            {
                var areaRatio = ((double)area.X) / ((double)area.Y);
                var areaToFitRatio = ((double)areaToFit.X) / ((double)areaToFit.Y);

                if (areaRatio > areaToFitRatio)
                {
                    boxWidth = (int)System.Math.Round(area.Y * areaToFitRatio);
                }
                else
                {
                    boxHeight = (int)System.Math.Round(area.X / areaToFitRatio);
                }
            }

            return new Vector2i(boxWidth, boxHeight);
        }
    }
}
