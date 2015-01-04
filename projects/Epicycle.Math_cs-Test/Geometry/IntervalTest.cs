using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    using System;

    [TestFixture]
    public sealed class IntervalTest : AssertionHelper
    {
        private const double _tolerance = 1e-9;

        [Test]
        public void Distance2_between_value_and_Interval_is_correct()
        {
            var interval = new Interval(3.14, 5.77);

            Expect(IntervalUtils.Distance2(interval, 4), Is.EqualTo(0).Within(_tolerance));

            Expect(IntervalUtils.Distance2(interval, -1), Is.EqualTo(4.14 * 4.14).Within(_tolerance));
            Expect(IntervalUtils.Distance2(interval, 6), Is.EqualTo(0.23 * 0.23).Within(_tolerance));
        }
    }
}
