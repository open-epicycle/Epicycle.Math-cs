using System;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry
{
    public static class RotoTranslation2Utils
    {
        public static RotoTranslation2 ComputeRotationAboutPoint(Rotation2 rotation, Vector2 point)
        {
            return new RotoTranslation2(rotation, point - rotation.Apply(point));
        }
    }
}
