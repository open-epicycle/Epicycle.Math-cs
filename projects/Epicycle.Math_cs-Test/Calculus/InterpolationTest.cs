using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Epicycle.Math.Geometry;

namespace Epicycle.Math.Calculus
{
    [TestFixture]
    public sealed class InterpolationTest : AssertionHelper
    {
        private const double _tolerance = 1e-9;

        [Test]
        public void SphericalLinear_interpolation_is_equivalent_to_motion_with_constant_angular_velocity()
        {
            var r1 = (Rotation3)new Quaternion(1, 2, 3, 4);
            var dr = (Rotation3)new Quaternion(1, 0.02, 0.03, 0.04);

            var r2 = r1 * dr;
            var r4 = r1 * dr * dr * dr;

            var interp = Interpolation.SphericalLinear(t: 1, t1: 0, r1: r1, t2: 3, r2: r4);

            Expect((interp * r2.Inv).Vector.Norm, Is.EqualTo(0).Within(_tolerance));
        }
    }
}
